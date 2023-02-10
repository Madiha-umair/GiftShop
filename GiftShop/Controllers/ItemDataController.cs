using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GiftShop.Models;

namespace GiftShop.Controllers
{
    public class ItemDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Items in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Items in the database, including their associated orders.
        /// </returns>
        /// <example>
        /// GET: api/ItemData/ListItems
        /// </example>

        [HttpGet]
        public IEnumerable<ItemDto> ListItems()
        {
            List<Item> Items = db.Items.ToList();
            List<ItemDto> ItemDtos = new List<ItemDto>();

            Items.ForEach(a => ItemDtos.Add(new ItemDto()
            {
                ItemId = a.ItemId,
                ItemName = a.ItemName,
                ItemDescription = a.ItemDescription,

            }));
            return ItemDtos;
        }

        /// <summary>
        /// Returns all Items in the system associated with a particular gift.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Items in the database available a particular gift
        /// </returns>
        /// <param name="id">Gift Primary Key</param>
        /// <example>
        /// GET: api/ItemData/ListItemsforGift
        /// </example>

        [HttpGet]
        [ResponseType(typeof(ItemDto))]
        public IEnumerable<ItemDto> ListItemsforGift(int id)
        {
            List<Item> Items = db.Items.Where(
                k=>k.Gifts.Any(
                    a => a.GiftId == id)
                    ).ToList();
            List<ItemDto> ItemDtos = new List<ItemDto>();

            Items.ForEach(a => ItemDtos.Add(new ItemDto()
            {
                ItemId = a.ItemId,
                ItemName = a.ItemName,
                ItemDescription = a.ItemDescription,

            }));
            return ItemDtos;
        }

        /// <summary>
        /// Returns Items in the system not part of the gift.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Items in the database not part of a particular gift
        /// </returns>
        /// <param name="id">Gift Primary Key</param>
        /// <example>
        /// GET: api/ItemData/ListItemsforGift
        /// </example>
        
        [HttpGet]
        [ResponseType(typeof(ItemDto))]
        public IEnumerable<ItemDto> ListItemsNotinGiftBasket(int id)
        {
            List<Item> Items = db.Items.Where(
                k => !k.Gifts.Any(
                    a => a.GiftId == id)
                    ).ToList();
            List<ItemDto> ItemDtos = new List<ItemDto>();

            Items.ForEach(a => ItemDtos.Add(new ItemDto()
            {
                ItemId = a.ItemId,
                ItemName = a.ItemName,
                ItemDescription = a.ItemDescription,

            }));
            return ItemDtos;
        }

        /// <summary>
        /// Returns all Items in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Item in the system matching up to the Item Id primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Item</param>
        /// <example>
        /// GET: api/ItemData/FindItem/5
        /// </example>
         
        [ResponseType(typeof(Item))]
        [HttpGet]
        public IHttpActionResult FindItem(int id)
        {
            Item Item = db.Items.Find(id);
            ItemDto ItemDto = new ItemDto()
            {
                ItemId = Item.ItemId,
                ItemName = Item.ItemName,
                ItemDescription = Item.ItemDescription
            };
            if (Item == null)
            {
                return NotFound();
            }

            return Ok(ItemDto);
        }


        /// <summary>
        /// Updates a particular Item in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Item Id primary key</param>
        /// <param name="Item">JSON FORM DATA of an Item</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/ItemData/UpdateItem/5
        /// FORM DATA: Item JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateItem(int id, Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.ItemId)
            {
                return BadRequest();
            }

            db.Entry(item).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds an Item to the system
        /// </summary>
        /// <param name="Item">JSON FORM DATA of an Item</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Item ID, Item Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ItemData/AddItem
        /// FORM DATA: Item JSON Object
        /// </example>

        // POST: api/ItemData/AddItem
        [ResponseType(typeof(Item))]
        [HttpPost]
        public IHttpActionResult AddItem(Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Items.Add(item);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = item.ItemId }, item);
        }

        /// <summary>
        /// Deletes an Item from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Item</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/ItemData/DeleteItem/5
        /// FORM DATA: (empty)
        /// </example>

        // POST: api/ItemData/DeleteItem/5
        [ResponseType(typeof(Item))]
        [HttpPost]
        public IHttpActionResult DeleteItem(int id)
        {
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            db.Items.Remove(item);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemExists(int id)
        {
            return db.Items.Count(e => e.ItemId == id) > 0;
        }
    }
}