using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliveStoreTemplate.Model;
using AliveStoreTemplate.Model.DTOModel;
using AliveStoreTemplate.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AliveStoreTemplate.Pages
{
    public class categoryModel : PageModel
    {
        [Obsolete]
        private IHostingEnvironment Environment;
        private readonly ProductService _productService;

        [Obsolete]
        public categoryModel(IHostingEnvironment _environment, ProductService productService)
        {
            Environment = _environment;
            _productService = productService;
        }

        [BindProperty]
        public List<ProductList> CardList { get; set; }

        public void OnGet(string category, string subCategory)
        {
            SearchProductReqModel Req = new SearchProductReqModel();
            if (category != null)
            {
                Req.Category = category;
                Req.Subcategory = "";
                if (subCategory != null)
                {
                    Req.Subcategory = subCategory;
                }
            }

            var result = _productService.SearchProduct(Req);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (result.Results != null)
                {
                    CardList = result.Results.ToList();
                    return;
                };
            }
            ViewData["Message"] = string.Format("Category Error");
        }
    }
}