using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;

namespace Main.Controllers
{
    [Route("view/pf/msg/state")]
    public class PF_MSG_STATEController : Controller
    {
        // GET: 客户档案扩展ID字段
        public IActionResult Index(string GID, string KHMC)
        {
            return View("Index", null);
        }
        
        [HttpGet("create")]
        public IActionResult Create(string GID, string KHMC)
        {
            return View("Create", null);
        }

    }
}
