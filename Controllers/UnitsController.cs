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
    public class UnitsController : ControllerBase
    {
        private readonly ILogger<UnitsController> _logger;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly PostService _postService;
        private readonly PutService<Unit> _putService;
        private readonly DeleteService<Unit> _deleteService;
        private readonly GetService<Unit> _getService;

        public UnitsController(
            ILogger<UnitsController> logger,
            IExceptionLogger exceptionLogger,
            WarehouseContext context)
        {
            _logger = logger;
            _exceptionLogger = exceptionLogger;
            _postService = new PostService(context);
            _putService = new PutService<Unit>(context, context.Units);
            _deleteService = new DeleteService<Unit>(context, context.Units, nameof(Unit));
            _getService = new GetService<Unit>(context.Units, nameof(Unit));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Unit>>> Get(int? unitId)
        {
            try
            {
                return Ok(await _getService.GetAsync(unitId));
            }
            catch (EntityNotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (Exception exception)
            {
                _exceptionLogger.LogException(exception, nameof(UnitsController), _logger);
                throw;
            }
        }
        
        [HttpPut]
        public async Task<IActionResult> Put(UnitDto unitDto, int unitId)
        {
            try
            {
                if (unitId == 0)
                {
                    return BadRequest("Id cannot be 0");
                }
                
                Unit temp = new Unit(unitDto);
                bool hasOverwritten =  await _putService.PutAsync(temp, unitId);

                IEnumerable<Unit> result = await _getService.GetAsync(unitId);
                if (hasOverwritten)
                {
                    return Ok(result.FirstOrDefault());
                }

                return CreatedAtAction(
                    nameof(Put), 
                    unitId,
                    result.FirstOrDefault());
            }
            catch (Exception exception)
            {
                _exceptionLogger.LogException(exception, nameof(UnitsController), _logger);
                throw;
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(UnitDto unitDto)
        {
            try
            {
                Unit temp = new Unit(unitDto);
                Unit result = await _postService.PostAsync(temp);
                
                return CreatedAtAction(
                    nameof(Post),
                    result.Id,
                    result);
            }
            catch (Exception exception)
            {
                _exceptionLogger.LogException(exception, nameof(UnitsController), _logger);
                throw;
            }
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(int unitId)
        {
            try
            {
                await _deleteService.DeleteAsync(unitId);
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