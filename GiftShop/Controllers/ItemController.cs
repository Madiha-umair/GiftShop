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
    public class ItemController : Controller
    {
        private static readonly HttpClient client;
        //to convert json object to string
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        //this constructor is to avoid repetition of code 
        static ItemController()
        {
            client = new HttpClient();    //client is anything that is accessing info from server
            client.BaseAddress = new Uri("https://localhost:44396/api/");      //baseaddress common for all url

        }

        // GET: Item/List
        public ActionResult List()
        {
            //objective: communicate with our Item data api to retrieve a list of items.
            // curl https://localhost:44396/api/itemdata/listitems

            //client  in this case is  our data server is requesting data from web api
            string url = "itemdata/listitems";           //this is part of our base uri
            HttpResponseMessage response = client.GetAsync(url).Result;


            //Nothing to render to the page till now. just Testing if our Http communication is successful
            Debug.WriteLine("The response is:");
            Debug.WriteLine(response.StatusCode);

            //we are getting this from the response of our request
            //ReadAsSync----->we are instructing  csharp , what we want to read this content as IEnumerable of type item
            //()----> this is a method
            //Result ------> access the actual result from the request
            //It should get info from our actual http reponse and read it into type of IEnumerable item
            IEnumerable<ItemDto> Items = response.Content.ReadAsAsync<IEnumerable<ItemDto>>().Result;

            //test this out 
            Debug.WriteLine("No of records of items are:");
            Debug.WriteLine(Items.Count());

            //this is proper channel of communication b/w of our web server in item controller with actual item data controller api
            //now it is confirmed we have recieved the response of our http request we return a view of our items in list.cshtml

            return View(Items);
        }


        // GET: Item/Details/5
        public ActionResult Details(int id)
        {
            DetailsItem ViewModel = new DetailsItem();


            //objective: communicate with our Item data api to retrieve one item
            // curl https://localhost:44396/api/itemdata/finditem/{id}

            string url = "itemdata/finditem/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("The response is:");
            Debug.WriteLine(response.StatusCode);

            ItemDto SelectedItem = response.Content.ReadAsAsync<ItemDto>().Result;

            //test this out 
            Debug.WriteLine("Item recieved:");
            Debug.WriteLine(SelectedItem.ItemName);

            ViewModel.SelectedItem = SelectedItem;

            //show all gifts having this particular item
            url = "giftdata/listgiftsforitem/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<GiftDto> ItemOfGifts = response.Content.ReadAsAsync<IEnumerable<GiftDto>>().Result;

            ViewModel.ItemOfGifts = ItemOfGifts;

            return View(ViewModel);
        }
        // creating a new page to display for error
        public ActionResult Error()
        {
            return View();
        }


        // GET: Item/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Item/Create
        [HttpPost]
        public ActionResult Create(Item Item)
        {
            Debug.WriteLine("Successfully created!");
            //objective: add a new item into our system using the API
            // curl -H "Content-Type: application/json" -d @item.json https://localhost:44396/api/itemdata/additem/
            string url = "itemdata/additem";

            //to convert json object to string
            // JavaScriptSerializer jss = new JavaScriptSerializer();   //Declare it at the top, avoid to rewrite it again for other function
            string jsonpayload = jss.Serialize(Item);

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

        // GET: Item/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "itemdata/finditem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ItemDto selecteditem = response.Content.ReadAsAsync<ItemDto>().Result;
            return View(selecteditem);
        }

        // POST: Item/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Item item)
        {
            string url = "itemdata/updateitem/"+id;
            string jsonpayload = jss.Serialize(item);
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

        // GET: Item/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "itemdata/findgift/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ItemDto selecteditem = response.Content.ReadAsAsync<ItemDto>().Result;
            return View(selecteditem);
        }

        // POST: Item/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "itemdata/deleteitem/"+id;

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
