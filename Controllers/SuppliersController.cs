using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RemaWareHouse.Exceptions;
using RemaWareHouse.Models;
using RemaWareHouse.Persistency;
using RemaWareHouse.Services.Suppliers;

namespace RemaWareHouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly ILogger<SuppliersController> _logger;
        private readonly GetSuppliersService _getSuppliersService;
        private readonly PutSuppliersService _putSuppliersService;
        private readonly PostSuppliersService _postSuppliersService;
        private readonly DeleteSuppliersService _deleteSuppliersService;

        public SuppliersController(ILogger<SuppliersController> logger, WarehouseContext context)
        {
            _logger = logger;
            _getSuppliersService = new GetSuppliersService(context);
            _putSuppliersService = new PutSuppliersService(context);
            _postSuppliersService = new PostSuppliersService(context);
            _deleteSuppliersService = new DeleteSuppliersService(context);

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> Get(int? supplierId)
        {
            try
            {
                return await _getSuppliersService.GetSuppliersAsync(supplierId);
            }
            catch (Exception exception)
            {
                LogException(exception);
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(Supplier supplier, int? supplierId)
        {
            try
            {
                bool hasOverWritten =  await _putSuppliersService.PutAsync(supplier, supplierId);

                if (hasOverWritten)
                {
                    return Ok(supplier);
                }

                return CreatedAtAction(nameof(Put), supplierId, supplier);
            }
            catch (Exception exception)
            {
                LogException(exception);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Supplier supplier)
        {
            try
            {
                int id = await _postSuppliersService.PostAsync(supplier);

                return CreatedAtAction(nameof(Post), id, supplier);
            }
            catch (Exception exception)
            {
                LogException(exception);
                throw;
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int supplierId)
        {
            try
            {
                await _deleteSuppliersService.DeleteAsync(supplierId);
                return NoContent();
            }
            catch (EntityNotFoundException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                LogException(exception);
                throw;
            }
        }

        private void LogException(Exception exception)
        {
            _logger.Log(LogLevel.Error, @"Could not return suppliers: {Exception}", exception);
        }
    }
}