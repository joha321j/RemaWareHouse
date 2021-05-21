using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RemaWareHouse.DataTransferObjects;
using RemaWareHouse.Exceptions;
using RemaWareHouse.Exceptions.Loggers;
using RemaWareHouse.Models;
using RemaWareHouse.Persistency;
using RemaWareHouse.Services;
using RemaWareHouse.Services.ProductsServices;

namespace RemaWareHouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IExceptionLogger _exceptionLogger;

        private readonly WarehouseContext _context;
        
        private readonly GetProductsService _getService;
        private readonly PostService _postService;
        private readonly PutService<Product> _putService;
        private readonly DeleteService<Product> _deleteService;
        private readonly ValidationService _validationService;

        public ProductsController(
            ILogger<ProductsController> logger,
            WarehouseContext context,
            IExceptionLogger exceptionLogger)
        {
            _logger = logger;
            _context = context;
            _exceptionLogger = exceptionLogger;
            
            _postService = new PostService(context);
            _putService = new PutService<Product>(context, context.Products);
            _getService = new GetProductsService(context);
            _deleteService = new DeleteService<Product>(context, context.Products, nameof(Product));
            _validationService = new ValidationService(context);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get(
            int? id,
            bool withCategory = true,
            bool withSupplier = true,
            bool withUnit = true)
        {
            try
            {
                return Ok(await _getService.GetAsync(id, withCategory, withSupplier, withUnit));
            }
            catch (EntityNotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (Exception exception)
            {
                _exceptionLogger.LogException(exception, nameof(SuppliersController), _logger);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post(ProductDto productDto)
        {
            try
            {
                await _validationService.EnsureValidDependencies(productDto);
                Product temp = new Product(productDto, _context);
                Product result = await _postService.PostAsync(temp);

                return CreatedAtAction(
                    nameof(Post),
                    result.Id,
                    result);
            }
            catch (EntityNotFoundException notFoundException)
            {
                return BadRequest(notFoundException.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(ProductDto productDto, int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("Id cannot be 0");
                }
                await _validationService.EnsureValidDependencies(productDto);
                Product temp = new Product(productDto, _context);

                bool hasOverwritten =  await _putService.PutAsync(temp, id);

                IEnumerable<Product> result = await _getService.GetAsync(id);
                if (hasOverwritten)
                {
                    return Ok(result.FirstOrDefault());
                }

                return CreatedAtAction(
                    nameof(Put), 
                    id,
                    result.FirstOrDefault());
            }
            catch (EntityNotFoundException notFoundException)
            {
                return BadRequest(notFoundException.Message);
            }
            catch (Exception exception)
            {
                _exceptionLogger.LogException(exception, nameof(UnitsController), _logger);
                throw;
            }
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _deleteService.DeleteAsync(id);
                return NoContent();
            }
            catch (EntityNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                _exceptionLogger.LogException(exception, nameof(UnitsController), _logger);
                throw;
            }
        }
    }
}