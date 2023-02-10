using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GiftShop.Models;

namespace GiftShop.Controllers
{
    public class GiftDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/GiftData/ListGifts
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

        // GET: api/GiftData/ListGiftsForOrders
        [HttpGet]
        public IEnumerable<GiftDto> ListGiftsForOrders(int id)
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

        // GET: api/GiftData/ListGiftsForItem
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


        // GET: api/GiftData/FindGift/5
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

        // POST: api/GiftData/UpdateGift/5
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

        // POST: api/GiftData/AddGift
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

        // POST: api/GiftData/DeleteGift/5
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