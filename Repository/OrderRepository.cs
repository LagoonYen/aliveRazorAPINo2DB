
using AliveStoreTemplate.Model;
using AliveStoreTemplate.Model.DTOModel;
using System.Collections.Generic;

namespace AliveStoreTemplate.Repositories
{
    public interface OrderRepository
    {
        //public int UpsertAddress(AddressUpserConditionModel AddressUpserCondi);

        //public void AddOrderDetail(OrderProduct orderProduct);

        public void InsertOrder(OrderList orderList, string OrderId);

        public List<OrderList> GetOrderList(string PhoneNum);

        //public void UpdateTotalPrice(int orderId, int TotalPrice);

        //public OrderList GetOrderInfomation(int orderId);

        //public List<OrderProduct> GetOrderDetailList(int orderIxd);


    }
}
