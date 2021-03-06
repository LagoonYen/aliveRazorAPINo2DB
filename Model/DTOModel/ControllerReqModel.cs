using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace AliveStoreTemplate.Model.DTOModel
{
    /// <summary>
    /// Member相關
    /// </summary>
    public class LoginReqModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "電子郵件帳號")]
        public string Account { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "{0}必需擁有至少{1}個字元,最多{2}個字元", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }
    }

    public class PatchMemberInfoReqModel
    {
        [EmailAddress]
        [Display(Name = "電子郵件帳號")]
        public string Account { get; set; }

        [Display(Name = "UID")]
        public string UID { get; set; }

        [Display(Name = "暱稱")]
        public string NickName { get; set; }

        [Display(Name = "電話號碼")]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Display(Name = "信箱")]
        public string Email { get; set; }
    }

    /// <summary>
    /// Order相關
    /// </summary>
    public class ToOrderReqModel
    {
        public List<Items> ProductList { get; set; }
        public string BuyerName { get; set; }
        public string BuyerMobile { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerRemark { get; set; }
        //public int BuyerAddressCityCode { get; set; }
        //public int BuyerAddressTownCode { get; set; }
        public string BuyerAddressCity { get; set; }
        public string BuyerAddressTown { get; set; }
        public string PaymentType { get; set; }
    }

    public class OrderDetailReqModel
    {
        public int OrderId { get; set; }
    }

    public class GetOrderListReqModel
    {
        public string PhoneNo { get; set; }
    }

    //public class OrderDetailResponseModel
    //{
    //    public string OrderNumber { get; set; }
    //    public int Uid { get; set; }
    //    public string Recipient { get; set; }
    //    public string RecipientPhone { get; set; }
    //    public string RecipientCity { get; set; }
    //    public string RecipientTown { get; set; }
    //    public string RecipientAddress { get; set; }
    //    public string Remark { get; set; }
    //    public int? PayPrice { get; set; }
    //    public byte? IsPay { get; set; }
    //    public int? PayTime { get; set; }
    //    public byte? IsShip { get; set; }
    //    public int? ShipTime { get; set; }
    //    public byte? IsReceipt { get; set; }
    //    public int? ReceiptTime { get; set; }
    //    public string ShipNumber { get; set; }
    //    public byte? Status { get; set; }
    //    public DateTime CreateTime { get; set; }
    //    public DateTime UpdateTime { get; set; }
    //    public List<OrderProduct> Products { get; set; }
    //}

    /// <summary>
    /// Product相關
    /// </summary>
    public class SearchProductReqModel
    {
        public string Category { get; set; }
        public string Subcategory { get; set; }
    }

    public class ProductInfoReqModel
    {
        public string ProductId { get; set; }
    }

    public class DeleteProductReqModel
    {
        public string ProductId { get; set; }
        public string ImgUrl { get; set; }
    }

    /// <summary>
    /// Shopcar相關
    /// </summary>
    public class AddToCarReqModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSpecId { get; set; }
        public string ProductSpecName { get; set; }

        public int OrderNum { get; set; }
        public int Price { get; set; }
    }

    public class DelFromCarReqModel
    {
        public string ProductId { get; set; }
    }

    public class PatchFromCarReqModel : DelFromCarReqModel
    {
        public string Symbol { get; set; }

    }

    //public class UpsertCartReqModel
    //{
    //    [Required]
    //    public int uid { get; set; }

    //    [Required]
    //    public int product_id { get; set; }

    //    [Required]
    //    public int num { get; set; }

    //    public DateTime UpdateTime { get; set; }
    //}

    //public class UIDReqModel
    //{
    //    public int UID { get; set; }
    //}

    public class ProductReqModel
    {
        public string Id { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Subcategory { get; set; }
        [Required]
        public string CardName { get; set; }
        public List<Ability> Abilities { get; set; }
        public List<Move> Moves { get; set; }
        public int HP { get; set; }
        public int Price { get; set; }
        public int Inventory { get; set; }
        public string ImgUrl { get; set; }
        public IFormFile CardImg { get; set; }
    }
}
