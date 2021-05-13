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
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IExceptionLogger _exceptionLogger;

        private readonly PostService _postService;
        private readonly PutService<Category> _putService;
        private readonly GetService<Category> _getService;
        private readonly DeleteService<Category> _deleteService;

        public CategoriesController(
            ILogger<CategoriesController> logger,
            WarehouseContext context,
            IExceptionLogger exceptionLogger)
        {
            _logger = logger;
            _exceptionLogger = exceptionLogger;

            _postService = new PostService(context);
            _putService = new PutService<Category>(context, context.Categories);
            _getService = new GetService<Category>(context.Categories, nameof(Category));
            _deleteService = new DeleteService<Category>(context, context.Categories, nameof(Supplier));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get(int? categoryId)
        {
            try
            {
                return Ok(await _getService.GetAsync(categoryId));
            }
            catch (EntityNotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (Exception exception)
            {
                _exceptionLogger.LogException(exception, nameof(CategoriesController), _logger);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Post(CategoryDto categoryDto)
        {
            try
            {
                Category temp = new Category(categoryDto);
                Category category = await _postService.PostAsync(temp);
                
                return CreatedAtAction(
                    nameof(Post),
                    category.Id,
                    category);
            }
            catch (Exception exception)
            {
                _exceptionLogger.LogException(exception, nameof(CategoriesController), _logger);
                throw;
            }
        }

        [HttpPut]
        public async Task<ActionResult<Category>> Put(CategoryDto categoryDto, int categoryId)
        {
            try
            {
                Category temp = new Category(categoryDto);
                bool hasOverwritten = await _putService.PutAsync(temp, categoryId);
                IEnumerable<Category> result = await _getService.GetAsync(categoryId);
                
                if (hasOverwritten)
                {
                    return Ok(result.FirstOrDefault());
                }

                return CreatedAtAction(nameof(Put), categoryId, result.FirstOrDefault());
            }
            catch (Exception exception)
            {
                _exceptionLogger.LogException(exception, nameof(CategoriesController), _logger);
                throw;
            }
        }

        [HttpDelete]
        public async Task<ActionResult<Category>> Delete(int categoryId)
        {
            try
            {
                await _deleteService.DeleteAsync(categoryId);
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