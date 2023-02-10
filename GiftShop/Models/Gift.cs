using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GiftShop.Models
{
    public class Gift
    {

        [Key]
        public int GiftId { get; set; }

        public string GiftBasketSize { get; set; }

        public int GiftBasketQuantity { get; set; }

        public string GiftBasketDetails { get; set; }

        //A Gift belongs to one Order
        //An Order can have many Gifts
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        //A Gift can have many items
        public ICollection<Item> Items { get; set; }
    }
    
    public class GiftDto
    {
        public int GiftId { get; set; }

        public string GiftBasketSize { get; set; }

        public int GiftBasketQuantity { get; set; }

        public string GiftBasketDetails { get; set; }

        public string CustomerName { get; set; }

    }
}