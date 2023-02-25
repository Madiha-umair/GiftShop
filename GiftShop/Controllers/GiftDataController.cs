using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using GiftShop.Models;

namespace GiftShop.Controllers
{
    public class GiftDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all gifts in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all gifts in the database, including their associated orders.
        /// </returns>
        /// <example>
        /// GET: api/GiftData/ListGifts
        /// </example>
        /// 
        [HttpGet]
        public IEnumerable<GiftDto> ListGifts()
        {
            List<Gift> Gifts = db.Gifts.ToList();
            List<GiftDto> GiftDtos = new List<GiftDto>();

            Gifts.ForEach(a => GiftDtos.Add(new GiftDto()
            {
                GiftId = a.GiftId,
                GiftBasketSize = a.GiftBasketSize,
                GiftBasketQuantity = a.GiftBasketQuantity,
                GiftBasketDetails = a.GiftBasketDetails,
                CustomerName = a.Order.CustomerName

            }));
            return GiftDtos;
        }

        /// <summary>
        /// Gathers information about all gifts related to a particular Order ID
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>
        ///  HEADER: 200 (OK)
        /// CONTENT: all giftss in the database, including their associated orders matched with a particular Order ID
        /// </returns>
        /// <example>
        /// GET: api/GiftData/ListGiftsForOrder
        /// </example>

        [HttpGet]
        public IEnumerable<GiftDto> ListGiftsForOrder(int id)
        {
            List<Gift> Gifts = db.Gifts.Where(a=>a.OrderId==id).ToList();
            List<GiftDto> GiftDtos = new List<GiftDto>();

            Gifts.ForEach(a => GiftDtos.Add(new GiftDto()
            {
                GiftId = a.GiftId,
                GiftBasketSize = a.GiftBasketSize,
                GiftBasketQuantity = a.GiftBasketQuantity,
                GiftBasketDetails = a.GiftBasketDetails,
                CustomerName = a.Order.CustomerName

            }));
            return GiftDtos;
        }

        /// <summary>
        /// Gathers information about gifts related to a particular item
        /// </summary>
        /// <param name="id">Item Id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all gifts in the database, including their associated orders that match to a particular item id
        /// </returns>
        /// <example>
        /// GET: api/GiftData/ListGiftsForItem
        /// </example>
        [HttpGet]
        [ResponseType(typeof(GiftDto))]
        public IEnumerable<GiftDto> ListGiftsForItem(int id)
        {
            List<Gift> Gifts = db.Gifts.Where(
                a => a.Items.Any(
                    k =>k.ItemId==id
                )).ToList();
            List<GiftDto> GiftDtos = new List<GiftDto>();

            Gifts.ForEach(a => GiftDtos.Add(new GiftDto()
            {
                GiftId = a.GiftId,
                GiftBasketSize = a.GiftBasketSize,
                GiftBasketQuantity = a.GiftBasketQuantity,
                GiftBasketDetails = a.GiftBasketDetails,
                CustomerName = a.Order.CustomerName

            }));
            return GiftDtos;
        }
        /// <summary>
        /// Associates a particular item with a particular gift
        /// </summary>
        /// <param name="giftid">The gift ID primary key</param>
        /// <param name="itemid">The item ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/GiftData/AssociateGiftWithItem/5/1
        /// </example>

        [HttpPost]
        [Route("api/GiftData/AssociateGiftWithItem/{giftid}/{itemid}")]
        public IHttpActionResult AssociateGiftWithItem(int giftid, int itemid)
        {
            Gift SelectedGift = db.Gifts.Include(a =>a.Items).Where
                (a =>a.GiftId==giftid).FirstOrDefault();
            Item SelectedItem = db.Items.Find(itemid);

            if(SelectedGift==null || SelectedItem==null)
            {
                return NotFound();
            }

            SelectedGift.Items.Add(SelectedItem);
            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular item and a particular gift
        /// </summary>
        /// <param name="giftid">The gift ID primary key</param>
        /// <param name="itemid">The item ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/AnimalData/AssociateGiftWithItem/9/1
        /// </example>

        [HttpPost]
        [Route("api/GiftData/UnAssociateGiftWithItem/{giftid}/{itemid}")]
        public IHttpActionResult UnAssociateGiftWithItem(int giftid, int itemid)
        {
            Gift SelectedGift = db.Gifts.Include(a => a.Items).Where
                (a => a.GiftId==giftid).FirstOrDefault();
            Item SelectedItem = db.Items.Find(itemid);

            if (SelectedGift==null || SelectedItem==null)
            {
                return NotFound();
            }

            Debug.WriteLine("input animal id is: " + giftid);
            Debug.WriteLine("selected animal name is: " + SelectedGift.GiftId);
            Debug.WriteLine("input animal id is: " + itemid);
            Debug.WriteLine("selected animal name is: " + SelectedItem.ItemName);

            SelectedGift.Items.Remove(SelectedItem);
            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Returns all gifts in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An gift in the system matching up to the gift ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the gift</param>
        /// <example>
        /// GET: api/GiftData/FindGift/5
        /// </example>

        [ResponseType(typeof(Gift))]
        [HttpGet]
        public IHttpActionResult FindGift(int id)
        {
            Gift Gift = db.Gifts.Find(id);
            GiftDto GiftDto = new GiftDto()
            {
                GiftId = Gift.GiftId,
                GiftBasketSize = Gift.GiftBasketSize,
                GiftBasketQuantity = Gift.GiftBasketQuantity,
                GiftBasketDetails = Gift.GiftBasketDetails,
                CustomerName = Gift.Order.CustomerName
            };
            if (Gift == null)
            {
                return NotFound();
            }

            return Ok(GiftDto);
        }

        /// <summary>
        /// Updates a particular gift in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Gift ID primary key</param>
        /// <param name="animal">JSON FORM DATA of an gift</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/GiftData/UpdateGift/5
        /// FORM DATA: Gift JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateGift(int id, Gift gift)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gift.GiftId)
            {
                return BadRequest();
            }

            db.Entry(gift).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GiftExists(id))
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
        /// Adds a gift to the system
        /// </summary>
        /// <param name="gift">JSON FORM DATA of an gift</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Gift ID, Gift Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/GiftData/AddGift
        /// FORM DATA: Gift JSON Object
        /// </example>

        [ResponseType(typeof(Gift))]
        [HttpPost]
        public IHttpActionResult AddGift(Gift gift)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Gifts.Add(gift);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = gift.GiftId }, gift);
        }

        /// <summary>
        /// Deletes an gift from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the gift</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/GiftData/DeleteGift/5
        /// FORM DATA: (empty)
        /// </example>

        [ResponseType(typeof(Gift))]
        [HttpPost]
        public IHttpActionResult DeleteGift(int id)
        {
            Gift gift = db.Gifts.Find(id);
            if (gift == null)
            {
                return NotFound();
            }

            db.Gifts.Remove(gift);
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

        private bool GiftExists(int id)
        {
            return db.Gifts.Count(e => e.GiftId == id) > 0;
        }
    }
}