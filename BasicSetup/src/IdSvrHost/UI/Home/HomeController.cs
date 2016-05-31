using Microsoft.AspNetCore.Mvc;

namespace IdSvrHost.UI.Home
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}