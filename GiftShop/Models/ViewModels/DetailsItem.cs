using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftShop.Models.ViewModels
{
    public class DetailsItem
    {
        public ItemDto SelectedItem { get; set; }   
        public IEnumerable<GiftDto> ItemOfGifts { get; set; }
    }
}