﻿using AliveStoreTemplate.Model;
using AliveStoreTemplate.Model.DTOModel;
using AliveStoreTemplate.Model.ResponseModel;
using AliveStoreTemplate.Repositories;
using AliveStoreTemplate.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace AliveStoreTemplate.Service
{
    public class ProductServiceImpl : ProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductServiceImpl(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public BaseResponseModel DeleteProduct(DeleteProductReqModel Req)
        {
            throw new System.NotImplementedException();
        }

        public BaseQueryModel<ProductList> GetProductInfoById(string ProductId)
        {
            try
            {
                var productInfo = _productRepository.GetProductInfoById(ProductId);
                return new BaseQueryModel<ProductList>
                {
                    Results = new List<ProductList> { productInfo },
                    Message = String.Empty,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseQueryModel<ProductList>
                {
                    Results = null,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public BaseResponseModel InsertProduct(ProductReqModel Req)
        {
            try
            {
                //取出所有的卡片清單
                var category = "";
                var subcategory = "";
                var cardList = _productRepository.SearchProduct(category, subcategory);
                var NewcardPathInDb = "";
                if (Req.CardImg != null)
                {
                    Req.ImgUrl = Req.CardImg.FileName;
                    //比對是否有重複的圖片名稱
                    //var fileName = Req.CardImg.FileName;
                    var fileName = Guid.NewGuid().ToString("N");
                    foreach (var item in cardList)
                    {
                        var dbfileName = item.ImgUrl.Split(".").TakeLast(1).FirstOrDefault();
                        fileName = (fileName == dbfileName) ? Guid.NewGuid().ToString() : fileName;
                    }

                    //建造儲存路徑
                    var fileExtension = Req.CardImg.FileName.Split(".").TakeLast(1).FirstOrDefault();
                    NewcardPathInDb = $"img/{fileName}.{fileExtension}";

                    //感謝Kevin指引 [走丟的] 路徑要加在哪邊www
                    using (var stream = new FileStream($"./wwwroot/" + NewcardPathInDb, FileMode.Create))
                    {
                        Req.CardImg.CopyTo(stream);
                    }
                }

                ProductList product = new ProductList
                {
                    ProductId = "Product" + Guid.NewGuid().ToString("N"),
                    CardName = Req.CardName,
                    Category = Req.Category,
                    Subcategory = Req.Subcategory,
                    Price = Req.Price,
                    Abilities = Req.Abilities,
                    Moves = Req.Moves,
                    Inventory = Req.Inventory,
                    ImgUrl = NewcardPathInDb,
                    RealseTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                _productRepository.InsertProduct(product);
                return new BaseResponseModel
                {
                    Message = "修改完成",
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

        public BaseResponseModel PatchProductAllInfo(ProductReqModel productReqModel)
        {
            throw new System.NotImplementedException();
        }

        public BaseQueryModel<ProductResultModel> Product_CategoryList()
        {
            try
            {
                string category = "";
                string subCategory = "";

                //先取出所有卡片的資料
                var productList = _productRepository.SearchProduct(category, subCategory);
                //製作卡片大小分類
                var productViewModel = productList.Select(x => new
                {
                    Category = x.Category,
                    SubCategory = x.Subcategory
                }).GroupBy(x => x.Category).Select(x => new ProductResultModel
                {
                    Category = x.Key,
                    SubCategory = x.Select(x => x.SubCategory).Distinct().ToList()
                });

                return new BaseQueryModel<ProductResultModel>
                {
                    Results = productViewModel,
                    Message = String.Empty,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseQueryModel<ProductResultModel>
                {
                    Results = null,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public BaseQueryModel<ProductList> SearchProduct(SearchProductReqModel Req)
        {
            try
            {
                //取得Category及SubCategory分類
                var category = Req.Category;
                var subCategory = Req.Subcategory;

                var productList = _productRepository.SearchProduct(category, subCategory);
                return new BaseQueryModel<ProductList>
                {
                    Results = productList,
                    Message = String.Empty,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseQueryModel<ProductList>
                {
                    Results = null,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
    }
}