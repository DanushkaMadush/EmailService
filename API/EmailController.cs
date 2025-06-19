using Microsoft.AspNetCore.Mvc;

namespace EmailService.API
{
    public class EmailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
