using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftShop.Models.ViewModels
{
    public class DetailsOrder
    {
        public OrderDto SelectedOrder { get; set; }
        
        public IEnumerable<GiftDto> RelatedGifts { get; set; }

    }
}