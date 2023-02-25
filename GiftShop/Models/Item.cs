using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GiftShop.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public string ItemDescription { get; set; }

        //data needed for keeping track of item images uploaded
        //images deposited into /Content/Images/Items/{id}.{extension}
        public bool ItemHasPic { get; set; }
        public string PicExtension { get; set; }

        //An item will be present in many Gifts
        public ICollection<Gift> Gifts { get; set; }
    }
    public class ItemDto
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public string ItemDescription { get; set; }

        //data needed for keeping track of items images uploaded
        //images deposited into /Content/Images/Items/{id}.{extension}
        public bool ItemHasPic { get; set; }
        public string PicExtension { get; set; }
    }
}