using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Shopnear.Models
{
	public class OrderUser
	{
		public string orderkey { get; set; }
		public string mem_name { get; set; }
		public string pay_img { get; set; }
		public string pricetotal { get; set; }
	}
	public class OrderShop
	{
		public string orderkey { get; set; }
		public string shop_id { get; set; }
		public string shop_name { get; set; }
		public string pay_img { get; set; }
		public string pay_number { get; set; }
		public string pay_bank { get; set; }
		public string pricetotal { get; set; }
	}
	public class Hisorder
	{
		public string orderkey { get; set; }
		public string status { get; set; }
	}
	public class OrderNonti
	{
		public string orderkey { get; set; }
		public string mem_name { get; set; }
		public string mem_address { get; set; }
		public string mem_phone { get; set; }
	}
	public class ProductNonti
	{
		public string product_name { get; set; }
		public string price { get; set; }
		public string number { get; set; }
	}
}
