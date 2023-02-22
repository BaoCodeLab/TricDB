using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;

namespace Main.Controllers
{
    [Route("view/pf/menu")]
    public class PF_MENUController : Controller
    {
        VM_PF_MENU viewModel = new VM_PF_MENU();
        // GET: 客户档案扩展ID字段
        [HttpGet]
        public IActionResult Index()
        {
            return View("Index", viewModel);
        }

        [HttpGet("Create")]
        public IActionResult Create(string GID)
        {
            viewModel.GID = GID;
            return View("Create", viewModel);
        }

        [HttpGet("Edit")]
        public IActionResult Edit(string GID)
        {
            viewModel.GID = GID;
            return View("Edit", viewModel);
        }

    }
}
