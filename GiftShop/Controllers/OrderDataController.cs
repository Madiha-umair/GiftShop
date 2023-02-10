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


        /// <summary>
        /// Returns all Orders in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Orders in the database, including their associated Order.
        /// </returns>
        /// <example>
        /// GET: api/OrderData/ListOrder
        /// </example>
         
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

        /// <summary>
        /// Returns all Orders in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Order in the system matching up to the Order Id primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Order</param>
        /// <example>
        /// GET: api/OrderData/FindOrder/5
        /// </example>

   
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

        /// <summary>
        /// Updates a particular Order in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Order Id primary key</param>
        /// <param name="Order">JSON FORM DATA of an order</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/OrderData/UpdateOrder/5
        /// FORM DATA: Species JSON Object
        /// </example>

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

        /// <summary>
        /// Adds an Order to the system
        /// </summary>
        /// <param name="Order">JSON FORM DATA of an Order</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Order Id, Order Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/OrderData/AddOrder
        /// FORM DATA: Order JSON Object
        /// </example>

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

        /// <summary>
        /// Deletes an Order  from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Order</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/OrderData/DeleteOrder/5
        /// FORM DATA: (empty)
        /// </example>

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