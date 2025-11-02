// Controllers/CategoriesController.cs
using Microsoft.AspNetCore.Mvc;
using FinanceTracker.API.Services;

namespace FinanceTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IFinanceService _financeService;

        public CategoriesController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _financeService.GetCategories();
            return Ok(categories);
        }
    }
}