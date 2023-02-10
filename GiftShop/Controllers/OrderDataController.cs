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
    public class OrderDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/OrderData/ListOrder
        [HttpGet]
        public IEnumerable<OrderDto> ListOrder()
        {
            List<Order> Orders = db.Order.ToList();
            List<OrderDto> OrderDtos = new List<OrderDto>();

            Orders.ForEach(a => OrderDtos.Add(new OrderDto()
            {
                OrderId = a.OrderId,
                CustomerName = a.CustomerName,
                CustomerAddress = a.CustomerAddress,
                CustomerContactNo = a.CustomerContactNo,
                OrderDate = a.OrderDate,
                TotalItems = a.TotalItems,
                TotalAmount = a.TotalAmount

            }));
            return OrderDtos;
        }

        // GET: api/OrderData/FindOrder/5
        [ResponseType(typeof(Order))]
        [HttpGet]
        public IHttpActionResult FindOrder(int id)
        {
            Order order = db.Order.Find(id);
            OrderDto OrderDto = new OrderDto()
            {
                OrderId = order.OrderId,
                CustomerName  = order.CustomerName,
                CustomerAddress = order.CustomerAddress,
                CustomerContactNo = order.CustomerContactNo,
                OrderDate = order.OrderDate,
                TotalItems = order.TotalItems,
                TotalAmount = order.TotalAmount
            };
            if (order == null)
            {
                return NotFound();
            }

            return Ok(OrderDto);
        }

        // POST: api/OrderData/UpdateOrder/5

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.OrderId)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/OrderData/AddOrder
        [ResponseType(typeof(Order))]
        [HttpPost]
        public IHttpActionResult AddOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Order.Add(order);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = order.OrderId }, order);
        }

        // POST: api/OrderData/DeleteOrder/5
        [ResponseType(typeof(Order))]
        [HttpPost]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Order.Remove(order);
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

        private bool OrderExists(int id)
        {
            return db.Order.Count(e => e.OrderId == id) > 0;
        }
    }
}