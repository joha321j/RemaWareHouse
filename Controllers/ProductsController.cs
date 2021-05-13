using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        private readonly PostService _postService;
        private readonly PutService<Product> _putService;
        private readonly GetProductsService _getService;
        private readonly DeleteService<Product> _deleteService;

        public ProductsController(
            ILogger<ProductsController> logger,
            WarehouseContext context,
            IExceptionLogger exceptionLogger)
        {
            _logger = logger;
            _exceptionLogger = exceptionLogger;
            
            _postService = new PostService(context);
            _putService = new PutService<Product>(context, context.Products);
            _getService = new GetProductsService(context);
            _deleteService = new DeleteService<Product>(context, context.Products, nameof(Product));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get(
            int? productId,
            bool withCategory = false,
            bool withSupplier = false,
            bool withUnit = false)
        {
            try
            {
                return Ok(await _getService.GetAsync(productId, withCategory, withSupplier, withUnit));
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
    }
}