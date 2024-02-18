using System;
using System.Collections.Generic;
using System.Text;

namespace Shopnear.Models
{
    public class Product
    {
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string shop_id { get; set; }
        public string product_detail { get; set; }
        public string img { get; set; }
        public string price { get; set; }
        public string stock { get; set; }
        public string shop_name { get; set; }
        public string promotion { get; set; }
    }
    public class ImgProduct
    {
        public string img { get; set; }
    }
    public class CartItem
    {
        public string member_id { get; set; }
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string price { get; set; }
        public string number { get; set; }
        public string img { get; set; }
    }
}
