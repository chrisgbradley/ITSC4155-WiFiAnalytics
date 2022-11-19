using Microsoft.AspNetCore.Mvc;
using NinerFiVisualize.Data.Services;

namespace NinerFiVisualize.Controllers
{
    [Route("api/[controller]")]
    public class ChartsController : Controller
    {
        public ChartsService _chartService;
        public ChartsController(ChartsService chartService)
        {
            _chartService = chartService;
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
