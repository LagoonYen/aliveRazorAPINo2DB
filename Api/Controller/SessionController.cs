using AliveStoreTemplate.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using System.Collections.Generic;
using AliveStoreTemplate.Model.DTOModel;

namespace AliveStoreTemplate.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    public class SessionController : ControllerBase
    {
        /// <summary>
        /// 加入到購物車
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("[action]")]
        //public IActionResult AddProductToCarBySession([FromBody]AddToCarReqModel Req)
        //{
        //    try 
        //    {
        //        //帶個大概
        //        //建立下訂商品
        //        CartItem item = new CartItem
        //        {
        //            //商品ID
        //            ProductId = Req.ProductId,
        //            //商品款式ID
        //            ProductSpecId = Req.ProductSpecId,
        //            //下訂數量
        //            Amount = Req.OrderNum,
        //            //小計
        //            SubTotal = Req.Price * Req.OrderNum,
        //            //單價
        //            Price = Req.Price
        //        };

        //        //判斷是否有購物車
        //        if (Common.CommonUtil.SessionGetObject<List<CartItem>>(HttpContext.Session, "cart") == null)
        //        {
        //            //沒有就新增
        //            List<CartItem> cart = new List<CartItem>();
        //            cart.Add(item);
        //            Common.CommonUtil.SessionSetObject(HttpContext.Session, "cart", cart);
        //        }
        //        else
        //        {
        //            //如果購物車存在
        //            List<CartItem> cart = Common.CommonUtil.SessionGetObject<List<CartItem>>(HttpContext.Session, "cart");
        //            //檢查購物車中是否包含同樣商品
        //            int index = cart.FindIndex(x => x.ProductId.Equals(Req.ProductId) && x.ProductSpecId.Equals(Req.ProductSpecId));
        //            if (index != -1)
        //            {
        //                cart[index].Amount += item.Amount;
        //                cart[index].SubTotal += item.SubTotal;
        //            }
        //            else
        //            {
        //                cart.Add(item);
        //            }
        //            Common.CommonUtil.SessionSetObject(HttpContext.Session, "cart", cart);
        //        }
                
        //        //Test
        //        List<CartItem> testObj = Common.CommonUtil.SessionGetObject<List<CartItem>>(HttpContext.Session, "cart");
        //        return Ok(testObj);
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        
        /// <summary>
        /// 取得購物車
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetProductFromCarBySession()
        {
            try
            {
                List<CartItem> cart = Common.CommonUtil.SessionGetObject<List<CartItem>>(HttpContext.Session, "cart");
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 移除自購物車
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public IActionResult DelProductFromCarBySession(DelFromCarReqModel Req)
        {
            try
            {
                List<CartItem> cart = Common.CommonUtil.SessionGetObject<List<CartItem>>(HttpContext.Session, "cart");

                //index定位後移除
                int index = cart.FindIndex(x => x.ProductId.Equals(Req.ProductId));
                cart.RemoveAt(index);

                // 購物車小於1項商品時移除購物車 否則重新寫入session
                if (cart.Count < 1)
                {
                    Common.CommonUtil.Remove(HttpContext.Session, "cart");
                }
                else
                {
                    Common.CommonUtil.SessionSetObject(HttpContext.Session, "cart", cart);
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 修改數量
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public IActionResult PatchProductNumFromCarBySession(PatchFromCarReqModel Req)
        {
            try
            {
                List<CartItem> cart = Common.CommonUtil.SessionGetObject<List<CartItem>>(HttpContext.Session, "cart");

                //index定位後移除
                int index = cart.FindIndex(x => x.ProductId.Equals(Req.ProductId));
                if(Req.Symbol == "plus")
                {
                    cart[index].Amount++;
                    cart[index].SubTotal = cart[index].SubTotal + cart[index].Price;
                }
                else
                {
                    cart[index].Amount--;
                    cart[index].SubTotal = cart[index].SubTotal - cart[index].Price;
                }

                Common.CommonUtil.SessionSetObject(HttpContext.Session, "cart", cart);

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 移除購物車
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("[action]")]
        public IActionResult CleanShopcar()
        {
            try
            {
                Common.CommonUtil.Remove(HttpContext.Session, "cart");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
