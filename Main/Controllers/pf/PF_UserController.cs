using Microsoft.AspNetCore.Mvc;
using Main.PF.ViewModels;
using Main.ViewModels;
using Main.platform;

namespace Main.PF.Controllers
{
    [Route("pf/user/")]
    public class PF_UserController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:YHZH"))
            {
                return View("Views/ERROR/401.cshtml");
            }
            UserViewModel userViewModel = new UserViewModel(this);
            return View("user", userViewModel);
        }

        [HttpGet, Route("pwd")]
        public ActionResult ModifyPsw(string GID)
        {
            if (!Permission.check(HttpContext, "YHZH:XGMM"))
            {
                return View("Views/ERROR/401.cshtml");
            }
            UserViewModel userViewModel = new UserViewModel(this)
            {
                GID = GID
            };
            return View("user_password", userViewModel);
        }

        [HttpGet, Route("mypwd")]
        public ActionResult ModifyMyPsw()
        {
            
            UserViewModel userViewModel = new UserViewModel(this);
            return View("user_mypassword", userViewModel);
        }

        //添加账户
        [HttpGet, Route("add")]
        public ActionResult AddCount()
        {
            if (!Permission.check(HttpContext, "YHZH:ADD"))
            {
                return View("Views/ERROR/401.cshtml");
            }
            UserViewModel userViewModel = new UserViewModel(this);
            return View("user_create", userViewModel);
        }
        //添加账户
        [HttpGet, Route("editzh/{GID?}")]
        public ActionResult EditCount([FromRoute]string GID)
        {
            if (!Permission.check(HttpContext, "YHZH:EDIT"))
            {
                return View("Views/ERROR/401.cshtml");
            }
            UserViewModel userViewModel = new UserViewModel(this);
            userViewModel.GID = GID;
            return View("user_edit", userViewModel);
        }

        //添加账户
        [HttpGet, Route("da")]
        public ActionResult UserDa(string username )
        {
            if (!Permission.check(HttpContext, "YHZH:DAGL"))
            {
                return View("Views/ERROR/401.cshtml");
            }
            VM_PF_PROFILE VM_PF_PROFILE = new VM_PF_PROFILE
            {
                DLZH = username
            };
            return View("user_da", VM_PF_PROFILE);
        }

        //添加账户
        [HttpGet, Route("da_details")]
        public ActionResult UserDaDetails(string username)
        {
            if (!Permission.check(HttpContext, "YHZH:GRDA"))
            {
                return View("Views/ERROR/401.cshtml");
            }
            VM_PF_PROFILE VM_PF_PROFILE = new VM_PF_PROFILE
            {
                DLZH = username
            };
            return View("user_da_details", VM_PF_PROFILE);
        }

        //关联档案
        [HttpGet, Route("glda")]
        public ActionResult UserDaGL(string username)
        {
            VM_PF_PROFILE VM_PF_PROFILE = new VM_PF_PROFILE
            {
                DLZH = username
            };
            return View("user_da_sel", VM_PF_PROFILE);
        }

        //关联档案
        [HttpGet, Route("da_sel")]
        public ActionResult DA_SEL()
        {
            return View("user_da_sel", null);
        }


        //关联用户
        [HttpGet, Route("da_sel_formsg")]
        public ActionResult DA_SEL_MSG()
        {
            return View("user_da_sel_for_msg", null);
        }
        //关联角色
        [HttpGet, Route("role_sel_formsg")]
        public ActionResult ROLE_SEL_MSG(string key)
        {
            return View("user_role_sel_for_msg", key);
        }

    }
}