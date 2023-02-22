using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;
using Main.platform;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.Controllers
{
    [Route("profile")]
    public class PF_PROFILEController : Controller
    {
        /// <summary>
        /// 绑定页面基本视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:XSRYDAGL"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_PROFILE viewModel = new VM_PF_PROFILE(this);
            return View("index", viewModel);
        }
        /// <summary>
        /// 用于更新
        /// </summary>
        /// <param name="gid">主键字段</param>
        /// <returns></returns>
        /// 

        [HttpGet, Route("update")]
        public ActionResult Update([FromQuery]string gid)
        {
            if (!Permission.check(HttpContext, "XSRYDAGL:EDIT"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_PROFILE viewModel = new VM_PF_PROFILE(this)
            {
                GID = gid
            };
            return View("edit", viewModel);
        }

        [HttpGet, Route("updateme/{gid}")]
        public ActionResult UpdateForMe([FromRoute]string gid)
        {
            VM_PF_PROFILE viewModel = new VM_PF_PROFILE(this)
            {
                GID = gid
            };
            return View("edit_self", viewModel);
        }

        /// <summary>
        /// 用于新建
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("create")]
        public ActionResult Create()
        {
            if (!Permission.check(HttpContext, "XSRYDAGL:ADD"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_PROFILE viewModel = new VM_PF_PROFILE();
            return View("create",viewModel);
        }

    }
}
