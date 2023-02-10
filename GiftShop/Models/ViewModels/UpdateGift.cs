using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftShop.Models.ViewModels
{
    public class UpdateGift
    {
        //This viewmodel is a class which stores information that we need to present to /Gift/Update/{}

        //the existing gift infromation 

        public GiftDto SelectedGift { get; set; }
        //all orders to choose from when updating this gift
        public IEnumerable<OrderDto> OrderOptions { get;set; }
    }
}