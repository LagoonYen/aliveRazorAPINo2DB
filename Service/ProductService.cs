using AliveStoreTemplate.Model;
using AliveStoreTemplate.Model.DTOModel;
using AliveStoreTemplate.Model.ResponseModel;

namespace AliveStoreTemplate.Services
{
    public interface ProductService
    {
        public BaseQueryModel<ProductList> SearchProduct(SearchProductReqModel Req);

        public BaseQueryModel<ProductResultModel> Product_CategoryList();

        public BaseQueryModel<ProductList> GetProductInfoById(string ProductId);

        public BaseResponseModel PatchProductAllInfo(ProductReqModel productReqModel);

        public BaseResponseModel InsertProduct(ProductReqModel Req);

        public BaseResponseModel DeleteProduct(DeleteProductReqModel Req);

        public BaseQueryModel<ProductList> SearchProductInfo(string SearchString);
    }
}
