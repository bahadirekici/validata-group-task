using Microsoft.AspNetCore.Mvc;
using validata_task.Entities;

namespace validata_task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>List of orders.</returns>
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Gets an order by ID.
        /// </summary>
        /// <param name="id">The order ID.</param>
        /// <returns>An order object.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        /// <summary>
        /// Gets orders by customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <returns>List of orders for the customer.</returns>
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomer(int customerId)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            var orders = customer.Orders.OrderBy(o => o.OrderDate).ToList();
            return Ok(orders);
        }

        /// <summary>
        /// Creates a new order for a customer.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <param name="order">The order object.</param>
        /// <returns>The created order.</returns>
        [HttpPost("customer/{customerId}")]
        public async Task<IActionResult> CreateOrder(int customerId, Order order)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            customer.Orders.Add(order);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        /// <summary>
        /// Updates an order by ID.
        /// </summary>
        /// <param name="id">The order ID.</param>
        /// <param name="order">The updated order object.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes an order by ID.
        /// </summary>
        /// <param name="id">The order ID.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _unitOfWork.Orders.Remove(order);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
