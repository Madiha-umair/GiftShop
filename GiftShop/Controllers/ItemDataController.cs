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

        // GET: api/ItemData/ListItems
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

        // GET: api/ItemData/ListItemsforGift
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

        // GET: api/ItemData/ListItemsforGift
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

        // GET: api/ItemData/FindItem/5
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

        // POST: api/ItemData/UpdateItem/5
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