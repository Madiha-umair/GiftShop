using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using GiftShop.Models;
using GiftShop.Models.ViewModels;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using GiftShop.Migrations;


namespace GiftShop.Controllers
{
    public class GiftController : Controller
    {

        private static readonly HttpClient client;
        //to convert json object to string
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        //this constructor is to avoid repetition of code 
        static GiftController()
        {
            client = new HttpClient();    //client is anything that is accessing info from server
            client.BaseAddress = new Uri("https://localhost:44396/api/");      //baseaddress common for all url

        }

        // GET: Gift/List
        public ActionResult List()
        {
            //objective: communicate with our gift basket data api to retrieve a list of gifts.
            // curl https://localhost:44396/api/giftdata/listgifts

            //client  in this case is  our data server is requesting data from web api
            string url = "giftdata/listgifts";           //this is part of our base uri
            HttpResponseMessage response = client.GetAsync(url).Result;


            //Nothing to render to the page till noe. just Testing if our Http communication is successful
            Debug.WriteLine("The response is:");
            Debug.WriteLine(response.StatusCode);

            //we are getting this from the response of our request
            //ReadAsSync----->we are instructing  csharp , what we want to read this content as IEnumerable of type gift
            //()----> this is a method
            //Result ------> access thactual result from the request
            //It should get info from our actual http reponse and read it into type of IEnumerable gift
            IEnumerable<GiftDto> Gifts = response.Content.ReadAsAsync<IEnumerable<GiftDto>>().Result;

            //test this out 
            Debug.WriteLine("No of records of gifts are:");
            Debug.WriteLine(Gifts.Count());

            //this is proper channel of communication b/w of our web server in gift controller with actual gift data controller api
            //now it is confirmed we have recieved the response of our http request we return a view of our gifts in list.cshtml

            return View(Gifts);
        }

        // GET: Gift/Details/5
        public ActionResult Details(int id)
        {
            DetailsGift ViewModel = new DetailsGift();


            //objective: communicate with our gift basket data api to retrieve one gift
            // curl https://localhost:44396/api/giftdata/findgift/{id}

            string url = "giftdata/findgift/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("The response is:");
            Debug.WriteLine(response.StatusCode);

            GiftDto SelectedGift = response.Content.ReadAsAsync<GiftDto>().Result;

            //test this out 
            Debug.WriteLine("Gift recieved:");
            Debug.WriteLine(SelectedGift.GiftBasketSize);

            ViewModel.SelectedGift= SelectedGift;

            //show associated Items with this gift
            url = "itemdata/listitemsforgift/"+id;
            response = client.GetAsync(url).Result;

            IEnumerable<ItemDto> AvailableItems = response.Content.ReadAsAsync<IEnumerable<ItemDto>>().Result;
            ViewModel.AvailableItems = AvailableItems;

            url = "itemdata/listitemsnotingiftbasket/" + id;
            response = client.GetAsync(url).Result;

            IEnumerable<ItemDto> OtherItems = response.Content.ReadAsAsync<IEnumerable<ItemDto>>().Result;
            ViewModel.OtherItems = OtherItems;

            return View(ViewModel);
        }

        //POST: Gift/Associate/{giftid}
        [HttpPost]
        public ActionResult Associate(int id, int ItemId)
        {
            Debug.WriteLine("Attempting to associate gift :" +id+ "with item" + ItemId);
            
            //call our api to associate gift with item
            string url = "giftdata/associategiftwithitem/"+id+ "/" +ItemId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;


            return RedirectToAction("Details/" + id);
        }

        //GET: Gift/UnAssociate/{id}?ItemId={itemId}
        [HttpGet]
        public ActionResult UnAssociate(int id, int ItemId)
        {
            Debug.WriteLine("Attempting to associate gift :" + id + "with item" + ItemId);

            //call our api to associate gift with item
            string url = "giftdata/unassociategiftwithitem/"+ id + "/" +ItemId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        // creating a new page to display for error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Gift/New
        public ActionResult New()
        {
            //information about all orders in the system
            //Get api/orderdata/listorder -------> interesting challenge as we are in inside gift controller and calling data from order api endpoint

            string url = "orderdata/listorder";           //this is part of our base uri
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<OrderDto> OrdersOptions = response.Content.ReadAsAsync<IEnumerable<OrderDto>>().Result;

            return View(OrdersOptions);
        }

        // POST: Gift/Create
        [HttpPost]
        public ActionResult Create(Gift gift)
        {
            Debug.WriteLine("Successfully created!");
            //objective: add a new gift into our system using the API
            // curl -H "Content-Type: application/json" -d @gift.json https://localhost:44396/api/giftdata/addgift/
            string url = "giftdata/addgift";

            //to convert json object to string
            // JavaScriptSerializer jss = new JavaScriptSerializer();   //Declare it at the top, avoid to rewrite it again for other function
            string jsonpayload = jss.Serialize(gift);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Gift/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateGift ViewModel = new UpdateGift(); // -----> this one class contain 2 pieces of info 1 is selected gift and other is 
                                                     // orders available options

            //the existing gift informtion
            string url = "giftdata/findgift/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            GiftDto Selectedgift = response.Content.ReadAsAsync<GiftDto>().Result;
            ViewModel.SelectedGift = Selectedgift;


            //also like to include all orders to choose from when updating this gift
            url = "orderdata/listorder/";
            response = client.GetAsync(url).Result;
            IEnumerable<OrderDto> OrderOptions = response.Content.ReadAsAsync<IEnumerable<OrderDto>>().Result;
            ViewModel.OrderOptions = OrderOptions;

            return View(ViewModel);
        }

        // POST: Gift/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Gift gift)
        {
            string url = "giftdata/updategift/"+id;
            string jsonpayload = jss.Serialize(gift);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Gift/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "giftdata/findgift/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            GiftDto selectedgift = response.Content.ReadAsAsync<GiftDto>().Result;
            return View(selectedgift);
        }

        // POST: Gift/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "giftdata/deletegift/"+id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }
    }
}
