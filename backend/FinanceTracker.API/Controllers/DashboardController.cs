// Controllers/DashboardController.cs
using Microsoft.AspNetCore.Mvc;
using FinanceTracker.API.Services;

namespace FinanceTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IFinanceService _financeService;

        public DashboardController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        [HttpGet("summary")]
        public IActionResult GetSummary()
        {
            var summary = _financeService.GetDashboardSummary();
            return Ok(summary);
        }
    }
}