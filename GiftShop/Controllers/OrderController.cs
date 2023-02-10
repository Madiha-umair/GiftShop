using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using GiftShop.Models;
using GiftShop.Models.ViewModels;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using GiftShop.Migrations;

namespace GiftShop.Controllers
{
    public class OrderController : Controller
    {
        private static readonly HttpClient client;
        //to convert json object to string
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        //this constructor is to avoid repetition of code 
        static OrderController()
        {
            client = new HttpClient();    //client is anything that is accessing info from server
            client.BaseAddress = new Uri("https://localhost:44396/api/");      //baseaddress common for all url

        }
        // GET: Order/List
        public ActionResult List()
        {
            //objective: communicate with our order data api to retrieve a list of orders.
            // curl https://localhost:44396/api/orderdata/listorder

            //client  in this case is  our data server is requesting data from web api
            string url = "orderdata/listorder";           //this is part of our base uri
            HttpResponseMessage response = client.GetAsync(url).Result;


            //Nothing to render to the page till now. just Testing if our Http communication is successful
            Debug.WriteLine("The response is:");
            Debug.WriteLine(response.StatusCode);

            //we are getting this from the response of our request
            //ReadAsSync----->we are instructing  csharp , what we want to read this content as IEnumerable of type order
            //()----> this is a method
            //Result ------> access the actual result from the request
            //It should get info from our actual http reponse and read it into type of IEnumerable order
            IEnumerable<OrderDto> Orders = response.Content.ReadAsAsync<IEnumerable<OrderDto>>().Result;

            //test this out 
            Debug.WriteLine("No of records of orders are:");
            Debug.WriteLine(Orders.Count());

            //this is proper channel of communication b/w of our web server in item controller with actual item data controller api
            //now it is confirmed we have recieved the response of our http request we return a view of our items in list.cshtml

            return View(Orders);
        }

        // GET: Order/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our Order data api to retrieve one order
            // curl https://localhost:44396/api/orderdata/findorder/{id}

            DetailsOrder ViewModel = new DetailsOrder();

            string url = "orderdata/findorder/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("The response is:");
            Debug.WriteLine(response.StatusCode);

            OrderDto SelectedOrder = response.Content.ReadAsAsync<OrderDto>().Result;

            //test this out 
            Debug.WriteLine("Order recieved:");
            Debug.WriteLine(SelectedOrder.CustomerName);

            ViewModel.SelectedOrder = SelectedOrder;
            //showcase information about gifts related to this order
            //send a request to gather information about gifts related to a particular order Id
            url = "giftdata/listgiftsfororders/"+id;
            response= client.GetAsync(url).Result;
            IEnumerable<GiftDto> RelatedGifts = response.Content.ReadAsAsync<IEnumerable<GiftDto>>().Result; 
            ViewModel.RelatedGifts = RelatedGifts;

            return View(ViewModel);
        }
        // creating a new page to display for error
        public ActionResult Error()
        {
            return View();
        }


        // GET: Order/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        public ActionResult Create(OrderDto Order)
        {
            Debug.WriteLine("Successfully created!");
            //objective: add a new order into our system using the API
            // curl -H "Content-Type: application/json" -d @order.json https://localhost:44396/api/orderdata/addorder/
            string url = "orderdata/addorder";

            //to convert json object to string
            // JavaScriptSerializer jss = new JavaScriptSerializer();   //Declare it at the top, avoid to rewrite it again for other function
            string jsonpayload = jss.Serialize(Order);

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

        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "orderdata/findorder/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            OrderDto selectedorder = response.Content.ReadAsAsync<OrderDto>().Result;
            return View(selectedorder); 
        }

        // POST: Order/Update/5
        [HttpPost]
        public ActionResult Update(int id, OrderDto Order)
        {
            string url = "orderdata/updateorder/"+id;
            string jsonpayload = jss.Serialize(Order);
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

        // GET: Order/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "orderdata/findorder/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            OrderDto selectedorder = response.Content.ReadAsAsync<OrderDto>().Result;
            return View(selectedorder);
        }

        // POST: Order/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "orderdata/deleteorder/"+id;

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
