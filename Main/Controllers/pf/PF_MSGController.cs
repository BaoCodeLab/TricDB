using Microsoft.AspNetCore.Mvc;

namespace Main.PF
{
    [Route("/view/pf/msg/")]
    public class PF_MSGController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("Index", null);
        }
    }
}