using CourseWebAPI.DTOs;
using CourseWebAPI.DTOs.ProductDTOs;
using CourseWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.DeleteFlag == false)
                .ToListAsync();

            var result = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CreateDate = p.CreateDate,
                LastUpdateDate = p.LastUpdateDate,
                DeleteFlag = p.DeleteFlag,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name
            }).ToList();

            return Ok(result);
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.DeleteFlag == false);

            if (product == null)
                return NotFound();

            var dto = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CreateDate = product.CreateDate,
                LastUpdateDate = product.LastUpdateDate,
                DeleteFlag = product.DeleteFlag,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name
            };

            return Ok(dto);
        }

        
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Create(CreateProductDTO dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                CreateDate = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                DeleteFlag = false
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductDTO dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;
            product.LastUpdateDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok();
        }

        
        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.DeleteFlag = true;
            await _context.SaveChangesAsync();
            return Ok();
        }

        
        [HttpPost("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null || product.DeleteFlag == false)
                return NotFound();

            product.DeleteFlag = false;
            await _context.SaveChangesAsync();
            return Ok();
        }

       
        [HttpDelete("{id}/hard")]
        public async Task<IActionResult> HardDelete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        
        [HttpGet("by-category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetByCategory(int categoryId)
        {
            var products = await _context.Products
                .Where(p => p.CategoryId == categoryId && p.DeleteFlag == false)
                .Include(p => p.Category)
                .ToListAsync();

            var result = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CreateDate = p.CreateDate,
                LastUpdateDate = p.LastUpdateDate,
                DeleteFlag = p.DeleteFlag,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name
            }).ToList();

            return Ok(result);
        }
    }
}
