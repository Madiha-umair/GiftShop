using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.ComponentModel.DataAnnotations;

namespace OnlineGiftShop.Models
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

        //An Order has many gifts
        
        //public ICollection<Gift> Gifts { get; set; }


    }
}