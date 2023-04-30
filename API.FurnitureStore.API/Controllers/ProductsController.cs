using API.FurnitureStore.Data;
using API.FurnitureStore.Shared;
using API.FurnitureStore.Shared.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
using System.Security.Claims;

namespace API.FurnitureStore.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly APIFurnitureStoreContext _context;

        public ProductsController(APIFurnitureStoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            //var identity = this.HttpContext.User.Identities.FirstOrDefault();
            //var user = this.HttpContext.User;
            //var http = this.HttpContext;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _context.Products
                .Include(p => p.ProductCategory)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return BadRequest();

            return Ok(product);
        }

        [HttpGet("GetByCategory/{productCategoryId}")]
        public async Task<IEnumerable<Product>> GetByCategory(int productCategoryId)
        {
            return await _context.Products
                            .Where(x => x.ProductCategoryId == productCategoryId)
                            .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            //Limpiamos si mandan una orden ya que no es posible
            product.OrderDetails = null;

            var producCategory = await _context.ProductCategories.FirstOrDefaultAsync(x => x.Id == product.ProductCategoryId);
            if (producCategory == null)
                return BadRequest("ProductCategory no existe");

            product.ProductCategory = producCategory;

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Post", product.Id, product);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Product product)
        {
            //Limpiamos si mandan una orden ya que no es posible
            product.OrderDetails = null;
            var producCategory = await _context.ProductCategories.FirstOrDefaultAsync(x => x.Id == product.ProductCategoryId);

            if (producCategory == null)
                return BadRequest("ProductCategory no existe");

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Product product)
        { 
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
