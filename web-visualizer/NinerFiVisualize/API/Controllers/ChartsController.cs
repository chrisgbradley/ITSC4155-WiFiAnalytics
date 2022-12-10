using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NinerFiVisualize.API.Data.Services;

namespace NinerFiVisualize.Controllers
{
    [Route("api/[controller]")]
    public class ChartsController : Controller
    {
        private ChartsService _chartService;
        private readonly IDistributedCache _cache;
        private ILogger<ChartsController> _logger;

        public ChartsController(ChartsService chartService,
                                IDistributedCache cache, 
                                ILogger<ChartsController> logger)
        {
            _chartService = chartService ?? throw new ArgumentNullException(nameof(chartService));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("get-error-tracking")]
        public IActionResult GetErrorTracking(params int[] filter)
        {
            var errorTracking = _chartService.GetErrorTrackingView();
            return Ok(errorTracking);
        }


        /* Possible to filter items in URL. Might be useful. Delete later if deemed unnecessary.*/

        //[HttpGet("get-error-tracking/{startyear}-{endyear}/{startmonth}-{endmonth}/{startday}-{endday}")]
        //public IActionResult GetErrorTracking(params int[] filter)        {
        //    var errorTracking = _chartService.GetErrorTrackingView(filter);
        //    return Ok(errorTracking);
        //}

        [HttpGet("get-log-count")]
        public IActionResult GetLogCount()
        {
            var logCount = _chartService.GetLogCountView();
            return Ok(logCount);
        }
    }
}
