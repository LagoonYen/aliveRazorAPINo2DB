using AliveStoreTemplate.Model;
using AliveStoreTemplate.Model.DTOModel;
using AliveStoreTemplate.Model.ResponseModel;

namespace AliveStoreTemplate.Services
{
    public interface OrderService
    {
        public BaseResponseModel ToOrder(ToOrderReqModel Req);

        public BaseQueryModel<OrderList> GetOrderList(string phoneNo);

        //public BaseQueryModel<OrderDetailResponseModel> GetOrderDetail(OrderDetailReqModel Req);
    }
}
