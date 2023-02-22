using System;
using Microsoft.AspNetCore.Mvc;
using Main.PF.ViewModels;
using Main.platform;

namespace Main.PF.Controllers
{
    [Route("pf/role/")]
    public class PF_RoleController : Controller
    {


        [HttpGet]
        public IActionResult Index()
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:JSGL"))
            {
                return View("Views/ERROR/401.cshtml");
            }
            RoleViewModel viewmodel = new RoleViewModel(this);
            return View("role", viewmodel);
        }

        [HttpGet,Route("edit")]
        public IActionResult edit(string GID)
        {
            if (!Permission.check(HttpContext, "JSGL:EDIT"))
            {
                return View("Views/ERROR/401.cshtml");
            }
            RoleViewModel viewmodel = new RoleViewModel(this);

            viewmodel = new RoleViewModel
            {
                GID = GID
            };
            return View("role_form", viewmodel);
        }

        [HttpGet, Route("create")]
        public IActionResult create()
        {
            if (!Permission.check(HttpContext, "JSGL:ADD"))
            {
                return View("Views/ERROR/401.cshtml");
            }
            RoleViewModel viewmodel = new RoleViewModel(this);

            viewmodel = new RoleViewModel
            {
                GID = null
            };
            return View("role_form", viewmodel);
        }

        //添加账户权限
        [HttpGet, Route("distribute")]
        public IActionResult distributeu(String username)
        {
            if (!Permission.check(HttpContext, "YHZH:JSFP"))
            {
                return View("Views/ERROR/401.cshtml");
            }
            UserViewModel userviewmodel = new UserViewModel(this)
            {
                USERNAME = username
            };
            return View("role_distribute_u", userviewmodel);
        }

        //添加账户权限
        [HttpGet, Route("distributep")]
        public IActionResult distributep(String CODE)
        {
            PermissionViewModel permissionViewModel = new PermissionViewModel(this);

            if (!Permission.check(HttpContext, "QXGL:JSFP"))
            {
                return View("Views/ERROR/401.cshtml");
            }
            permissionViewModel.CODE = CODE;
            return View("role_distribute_p", permissionViewModel);
        }


    }
}