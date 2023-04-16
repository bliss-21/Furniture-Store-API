using API.FurnitureStore.Data;
using API.FurnitureStore.Shared;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.FurnitureStore.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly APIFurnitureStoreContext _context;

        public OrdersController(APIFurnitureStoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Include(c => c.Client)
                .ToListAsync();
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetDetail(int Id)
        {
            var order = await _context.Orders
                .Include(x => x.OrderItems)
                .Include(c => c.Client)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Order order)
        {
            if (order == null)
                return NotFound();

            if (order.OrderItems == null)
                return BadRequest("La orden tiene que tener un detalle");

            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == order.ClientId);
            if (client == null)
                return BadRequest("El Cliente no existe");

            order.Client = client;

            await _context.Orders.AddAsync(order);
            /* como Order tiene una lista de detalles usamos el metodo de AddRangeAsync para decirle que agrege todos los detalles
            traiga order, de lo contrario tendriamos que usar un for para recorre los detalles y agregar uno por uno con AddAsync */
            await _context.OrderDetails.AddRangeAsync(order.OrderItems);

            await _context.SaveChangesAsync();

            return CreatedAtAction("Post", order.Id, order);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Order order)
        {

            if (order == null)
                return NotFound();

            if (order.Id <= 0)
                return NotFound();

            if (order.OrderItems == null)
                return BadRequest("La orden tiene que tener un detalle");

            var existingOrder = await _context.Orders
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == order.Id);

            if (existingOrder == null)
                return NotFound();

            existingOrder.OrderDate = order.OrderDate;
            existingOrder.DeliveryDate = order.DeliveryDate;
            existingOrder.ClientId = order.ClientId;
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == order.ClientId);
            if (client == null)
                return BadRequest("El Cliente no existe");
            order.Client = client;

            _context.OrderDetails.RemoveRange(existingOrder.OrderItems);
            _context.Orders.Update(existingOrder);

            await _context.OrderDetails.AddRangeAsync(order.OrderItems);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Order order)
        {
            if (order == null)
                return NotFound();

            if (order.Id <= 0)
                return NotFound();

            var existingOrder = await _context.Orders
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == order.Id);

            if (existingOrder == null)
                return NotFound();

            _context.OrderDetails.RemoveRange(existingOrder.OrderItems);
            _context.Orders.Remove(existingOrder);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
