
using AliveStoreTemplate.Model;
using AliveStoreTemplate.Model.DTOModel;
using AliveStoreTemplate.Model.ResponseModel;
using AliveStoreTemplate.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AliveStoreTemplate.Services
{
    public class OrderServiceImpl : OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly ProductRepository _productRepository;

        public OrderServiceImpl(OrderRepository orderRepositroy, ProductRepository productRepository)
        {
            _orderRepository = orderRepositroy;
            _productRepository = productRepository;
        }

        /// <summary>
        /// 下訂單
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        public BaseResponseModel ToOrder(ToOrderReqModel Req)
        {
            try
            {
                ////建立新的地址
                //AddressUpserConditionModel AddressUpserCondi = new AddressUpserConditionModel
                //{
                //    OrderCity = Req.OrderCity,
                //    OrderName = Req.OrderName,
                //    OrderAddress = Req.OrderAddress,
                //    OrderPhoneNumber = Req.OrderPhoneNumber,
                //    OrderTown = Req.OrderTown,
                //    Uid = Req.Uid,
                //    DateTime = DateTime.Now,
                //};

                //取得地址id
                //var OrderAddressId = _orderRepository.UpsertAddress(Req);

                //取得購物車 及 庫存資料
                //var shopcarDetail = _shopCarRepository.GetUserShopcartList(Req.Uid);
                //if (shopcarDetail == null)
                //{
                //    throw new Exception("購物車內無商品");
                //}
                //foreach (var item in shopcarDetail)
                //{
                //    if (item.inventory < item.num)
                //    {
                //        throw new Exception("庫存不足");
                //    }
                //}

                //訂單編號
                var orderId = "OrderId" + Guid.NewGuid().ToString("N");

                //建立訂單內的
                var totalPrice = 0;
                foreach(var item in Req.ProductList)
                {
                    totalPrice += item.Price * item.BuyInventory;

                    var productInStore = _productRepository.GetProductInfoById(item.ProductId);

                    //修改inventory數量
                    var inventory = productInStore.Inventory - item.BuyInventory;
                    PatchProductInventoryConditionModel productList = new PatchProductInventoryConditionModel
                    {
                        ProductId = item.ProductId,
                        Inventory = inventory,
                    };
                    _productRepository.PatchProductInventory(productList);
                }

                //建立訂單
                OrderList orderList = new OrderList
                {
                    BuyerName = Req.BuyerName,
                    BuyerMobile = Req.BuyerMobile,
                    BuyerAddress = Req.BuyerAddress,
                    BuyerAddressCity = Req.BuyerAddressCity,
                    //BuyerAddressCityCode = Req.BuyerAddressCityCode,
                    BuyerAddressTown = Req.BuyerAddressTown,
                    //BuyerAddressTownCode = Req.BuyerAddressTownCode,
                    BuyerRemark = Req.BuyerRemark,
                    PaymentType = Req.PaymentType,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    ProductList = Req.ProductList,
                    TotalPrice = totalPrice,
                };

                _orderRepository.InsertOrder(orderList, orderId);

                return new BaseResponseModel
                {
                    Message = String.Empty,
                    StatusCode = HttpStatusCode.OK  
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseModel
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        /// <summary>
        /// 取得歷史訂單基本資料
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public BaseQueryModel<OrderList> GetOrderList(string phoneNo)
        {
            try
            {
                if (phoneNo == null)
                {
                    throw new Exception("查無訂單");
                }
                var orderList = _orderRepository.GetOrderList(phoneNo);
                return new BaseQueryModel<OrderList>
                {
                    Results = orderList,
                    Message = String.Empty,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseQueryModel<OrderList>
                {
                    Results = null,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        /// <summary>
        /// 取得單筆訂單詳細資料
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        //public BaseQueryModel<OrderDetailResponseModel> GetOrderDetail(OrderDetailReqModel Req)
        //{
        //    //取得訂單資料
        //    var OrderInfo = _orderRepository.GetOrderInfomation(Req.OrderId);
        //    //取得訂單詳細商品資料
        //    var ProductDetail = _orderRepository.GetOrderDetailList(Req.OrderId);
        //    OrderDetailResponseModel Resp = new OrderDetailResponseModel()
        //    {
        //        OrderNumber = OrderInfo.OrderNumber,
        //        Uid = OrderInfo.Uid,
        //        Recipient = OrderInfo.Recipient,
        //        RecipientPhone = OrderInfo.RecipientPhone,
        //        RecipientCity = OrderInfo.RecipientCity,
        //        RecipientTown = OrderInfo.RecipientTown,
        //        RecipientAddress = OrderInfo.RecipientAddress,
        //        Remark = OrderInfo.Remark,
        //        PayPrice = OrderInfo.PayPrice,
        //        IsPay = OrderInfo.IsPay,
        //        PayTime = OrderInfo.PayTime,
        //        IsShip = OrderInfo.IsShip,
        //        ShipTime =  OrderInfo.ShipTime,
        //        IsReceipt = OrderInfo.IsReceipt,
        //        ReceiptTime = OrderInfo.ReceiptTime,
        //        ShipNumber = OrderInfo.ShipNumber,
        //        CreateTime = OrderInfo.CreateTime,
        //        UpdateTime = OrderInfo.UpdateTime,
        //        Products = ProductDetail
        //    };
        //    return new BaseQueryModel<OrderDetailResponseModel>
        //    {
        //        Message = String.Empty,
        //        Results = new List<OrderDetailResponseModel> { Resp },
        //        StatusCode = HttpStatusCode.OK
        //    };
        //}
    }
}
