using System;
using System.Collections.Generic;

namespace AliveStoreTemplate.Model
{
    public class OrderList
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
        public int TotalPrice { get; set; }

        public int? PayPrice { get; set; }
        public byte? IsPay { get; set; }
        public int? PayTime { get; set; }
        public byte? IsShip { get; set; }
        public int? ShipTime { get; set; }
        public byte? IsReceipt { get; set; }
        public int? ReceiptTime { get; set; }
        public string ShipNumber { get; set; }
        public byte? Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    public class Items
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public int BuyInventory { get; set; }
        public int TotalPrice { get; set; }
    }
}
