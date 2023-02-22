using Microsoft.AspNetCore.Mvc;
using Main.PF.ViewModels;
using Main.platform;

namespace Main.PF.Controllers
{
    [Route("pf/permission")]
    public class PF_PERMISSIONController : Controller
    {


        [HttpGet]
        public IActionResult Index()
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:QXGL"))
            {
                return View("Views/ERROR/401.cshtml");
            }
            PermissionViewModel viewmodel = new PermissionViewModel(this);

            return View("permission", viewmodel);
        }

        [HttpGet,Route("edit")]
        public IActionResult edit(string GID)
        {
            if (!Permission.check(HttpContext, "QXGL:EDIT"))
            {
                return View("Views/ERROR/401.cshtml");
            }
            PermissionViewModel_MENU viewmodel = new PermissionViewModel_MENU(this)
            {
                GID = GID
            };
            return View("permission_form", viewmodel);
        }

        [HttpGet, Route("create")]
        public IActionResult create(string MENU)
        {
            if (!Permission.check(HttpContext, "QXGL:ADD"))
            {
                return View("Views/ERROR/401.cshtml");
            }
            PermissionViewModel_MENU viewmodel = new PermissionViewModel_MENU(this)
            {
                GID = null,
                MENUGID = MENU
            };
            return View("permission_form", viewmodel);
        }


        [HttpGet, Route("sel")]
        public IActionResult select(string MENU)
        {
            if (!Permission.check(HttpContext, "QXGL:ADD"))
            {
                return View("Views/ERROR/401.cshtml");
            }
            PermissionViewModel_MENU viewmodel = new PermissionViewModel_MENU(this)
            {
                GID = null,
                MENUGID = MENU
            };
            return View("permission_sel", viewmodel);
        }




    }
}