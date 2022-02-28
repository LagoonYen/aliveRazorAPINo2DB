using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AliveStoreTemplate.Common;
using AliveStoreTemplate.Model;
using AliveStoreTemplate.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AliveStoreTemplate.Pages
{
    public class productModel : PageModel
    {
        private readonly ProductService _productService;
        public productModel(ProductService productService)
        {
            _productService = productService;
        }

        //商品資訊
        [BindProperty]
        public ProductList CardInfo { get; set; }

        public SelectList Options { get; set; }

        //下訂數量
        [BindProperty]
        public int OrderCount { get; set; }

        public void OnGet(string productId)
        {
            var result = _productService.GetProductInfoById(productId);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                if (result.Results != null)
                {
                    CardInfo = result.Results.FirstOrDefault();
                    return;
                };
            }
            ViewData["Message"] = string.Format("Card Error");
        }

        public void OnPostAddToCartBySession()
        {
            //資料存放於session
            //<ShopcarSession來自Common>
            var result = _productService.GetProductInfoById(CardInfo.ProductId);

            //建立下訂商品
            CartItem item = new CartItem
            {
                //商品ID
                ProductId = result.Results.FirstOrDefault().ProductId,
                //下訂數量
                Amount = OrderCount,
                //小計
                SubTotal = result.Results.FirstOrDefault().Price * OrderCount,
                //單價
                Price = result.Results.FirstOrDefault().Price
            };

            //判斷是否有購物車
            if (Common.CommonUtil.SessionGetObject<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                //沒有就新增
                List<CartItem> cart = new List<CartItem>();
                cart.Add(item);
                Common.CommonUtil.SessionSetObject(HttpContext.Session, "cart", cart);
            }
            else
            {
                //如果購物車存在
                List<CartItem> cart = Common.CommonUtil.SessionGetObject<List<CartItem>>(HttpContext.Session, "cart");
                //檢查購物車中是否包含同樣商品
                int index = cart.FindIndex(x => x.ProductId.Equals(CardInfo.ProductId));
                if (index != -1)
                {
                    cart[index].Amount += item.Amount;
                    cart[index].SubTotal += item.SubTotal;
                }
                else
                {
                    cart.Add(item);
                }
                Common.CommonUtil.SessionSetObject(HttpContext.Session, "cart", cart);
            }

            //Test
            List<CartItem> testObj = Common.CommonUtil.SessionGetObject<List<CartItem>>(HttpContext.Session, "cart");

            Response.Redirect("product?productId=" + CardInfo.ProductId);
        }


    }
}