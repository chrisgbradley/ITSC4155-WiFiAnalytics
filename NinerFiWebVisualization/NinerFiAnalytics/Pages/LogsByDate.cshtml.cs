using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NinerFiAnalytics.Pages
{
    [BindProperties]
    public class LogsByDateModel : PageModel
    {
        //Bind the posted form values to PageModel properties
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        
        public void OnGet()
        {
        }
        public void OnPost()
        {
            ViewData["dateselected"] = $"- {Day}/{Month}/{Year}";
        }
    }
}
