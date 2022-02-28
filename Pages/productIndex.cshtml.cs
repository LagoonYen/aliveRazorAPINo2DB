using System;
using AliveStoreTemplate.Model;
using AliveStoreTemplate.Model.DTOModel;
using AliveStoreTemplate.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net;

namespace AliveStoreTemplate.Pages
{
    [BindProperties]
    public class ProductIndexModel : PageModel
    {
        private readonly ProductService _productService;
        public ProductIndexModel(ProductService productService)
        {
            _productService = productService;
        }

        public List<ProductList> CardList { get; set; }

        public void OnGet()
        {
            //先帶全部卡片 後續修改
            var Category = "";
            var Subcategory = "";
            SearchProductReqModel Req = new SearchProductReqModel
            {
                Category = Category,
                Subcategory = Subcategory
            };
            var baseQueryModel = _productService.SearchProduct(Req);
            if(baseQueryModel.StatusCode != HttpStatusCode.BadRequest)
            {
                if(baseQueryModel.Results != null)
                {
                    CardList = (List<ProductList>)baseQueryModel.Results;
                    return;
                }
            }
            ViewData["Message"] = string.Format(baseQueryModel.Message);
        }

        public void OnPostDeleteProduct(string productId, string ImgUrl)
        {
            DeleteProductReqModel Req = new DeleteProductReqModel
            {
                ProductId = productId,
                ImgUrl = ImgUrl
            };
            var baseResponseModel = _productService.DeleteProduct(Req);
            if(baseResponseModel.StatusCode != HttpStatusCode.BadRequest)
            {
                Response.Redirect("productIndex");
            }
            ViewData["Message"] = string.Format(baseResponseModel.Message);
        }

        public string SearchString { get; set; }

        public void OnPostSearchAbility()
        {
            var baseQueryModel = _productService.SearchProductInfo(SearchString);
            if (baseQueryModel.StatusCode != HttpStatusCode.BadRequest)
            {
                if (baseQueryModel.Results != null)
                {
                    CardList = (List<ProductList>)baseQueryModel.Results;
                    return;
                }
            }
        }
    }
}
