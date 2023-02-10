using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GiftShop.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAddress { get; set; }

        public string CustomerContactNo { get; set; }

        public DateTime OrderDate { get; set; }

        public int TotalItems { get; set; }

        public int TotalAmount { get; set; }
    }

    public class OrderDto
    {
        public int OrderId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAddress { get; set; }

        public string CustomerContactNo { get; set; }

        public DateTime OrderDate { get; set; }

        public int TotalItems { get; set; }

        public int TotalAmount { get; set; }
    }
}
