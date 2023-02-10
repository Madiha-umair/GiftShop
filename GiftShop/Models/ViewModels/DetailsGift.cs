using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftShop.Models.ViewModels
{
    public class DetailsGift
    {
        public GiftDto SelectedGift { get; set; }

        public IEnumerable<ItemDto> AvailableItems { get; set;}


        public IEnumerable<ItemDto> OtherItems { get; set; }
    }
}