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

namespace RemaWareHouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly ILogger<SuppliersController> _logger;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly PostService _postService;
        private readonly PutService<Supplier> _putService;
        private readonly DeleteService<Supplier> _deleteService;
        private readonly GetService<Supplier> _getService;

        public SuppliersController(
            ILogger<SuppliersController> logger,
            WarehouseContext context,
            IExceptionLogger exceptionLogger)
        {
            _logger = logger;
            _exceptionLogger = exceptionLogger;
            _postService = new PostService(context);
            _putService = new PutService<Supplier>(context, context.Suppliers);
            _deleteService = new DeleteService<Supplier>(context, context.Suppliers, nameof(Supplier));
            _getService = new GetService<Supplier>(context.Suppliers, nameof(Supplier));

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> Get(int? supplierId)
        {
            try
            {
                return Ok(await _getService.GetAsync(supplierId));
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

        [HttpPut]
        public async Task<IActionResult> Put(SupplierDto supplierDto, int supplierId)
        {
            try
            {
                Supplier temp = new Supplier(supplierDto);
                bool hasOverwritten =  await _putService.PutAsync(temp, supplierId);

                IEnumerable<Supplier> result = await _getService.GetAsync(supplierId);
                if (hasOverwritten)
                {
                    return Ok(result.FirstOrDefault());
                }

                return CreatedAtAction(
                    nameof(Put), 
                    supplierId,
                    result.FirstOrDefault());
            }
            catch (Exception exception)
            {
                _exceptionLogger.LogException(exception, nameof(SuppliersController), _logger);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(SupplierDto supplierDto)
        {
            try
            {
                Supplier temp = new Supplier(supplierDto);
                Supplier result = await _postService.PostAsync(temp);
                
                return CreatedAtAction(
                    nameof(Post),
                    result.Id,
                    result);
            }
            catch (Exception exception)
            {
                _exceptionLogger.LogException(exception, nameof(SuppliersController), _logger);
                throw;
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int supplierId)
        {
            try
            {
                await _deleteService.DeleteAsync(supplierId);
                return NoContent();
            }
            catch (EntityNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                _exceptionLogger.LogException(exception, nameof(SuppliersController), _logger);
                throw;
            }
        }
    }
}