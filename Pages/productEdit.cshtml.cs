using AliveStoreTemplate.Model;
using AliveStoreTemplate.Model.DTOModel;
using AliveStoreTemplate.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Net;

namespace AliveStoreTemplate.Pages
{
    public class productEditModel : PageModel
    {
        private readonly ProductService _productService;

        public productEditModel(ProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public ProductList ProductResult { get; set; }

        public void OnGet(string productId)
        {
            try
            {
                var result = _productService.GetProductInfoById(productId);

                if(result.StatusCode == HttpStatusCode.OK)
                {
                    ProductResult = result.Results.FirstOrDefault();
                    return;
                }
            }
            catch(Exception ex)
            {
                ViewData["Message"] = string.Format(ex.Message);
            }
        }


        [BindProperty]
        public IFormFile CardImg { get; set; }

        public void OnPostPatchProductAllInfo()
        {
            try
            {
                ProductReqModel productList = new ProductReqModel
                {
                    Id = ProductResult.ProductId,
                    Category = ProductResult.Category,
                    Subcategory = ProductResult.Subcategory,
                    CardName = ProductResult.CardName,
                    Abilities = ProductResult.Abilities,
                    Moves = ProductResult.Moves,
                    HP = ProductResult.HP,
                    Price = ProductResult.Price,
                    Inventory = ProductResult.Inventory,
                    ImgUrl = ProductResult.ImgUrl,
                    CardImg = CardImg
                };

                var result = _productService.PatchProductAllInfo(productList);
                if( result.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(result.Message);
                }
                Response.Redirect("productIndex");
                return;
            }
            catch (Exception ex)
            {
                ViewData["Message"] = string.Format(ex.Message);
            }
        }
    }
}
