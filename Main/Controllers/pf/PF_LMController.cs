using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;
using Main.platform;

namespace Main.Controllers
{
    [Route("lm")]
    public class PF_LMController : Controller
    {
        

        // GET: PF_LM
       /// <summary>
       /// 绑定页面基本视图
       ///</summary>
       /// <returns>PF_LM视图模型</returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:LMSZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_LM viewModel = new VM_PF_LM(this);
            return View("Index", viewModel);
    
        }

        // GET: PF_LM/Details/5       
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
       /// <returns>PF_LM视图模型</returns>
        [HttpGet("{GID}")]
        public ActionResult Details([FromRoute]string GID)
        {
            if (!Permission.check(HttpContext, "LMSZ:DETAIL"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_LM viewModel = new VM_PF_LM(this)
            {
                GID = GID
            };
            if (GID == null)
            {
                return NotFound();
            }
            else{
                return View("Details",viewModel);
                
            }
        }

        // GET: PF_LM/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>PF_LM视图模型</returns>
        [HttpGet,Route("create")]
        public ActionResult Create()
        {
            if (!Permission.check(HttpContext, "LMSZ:ADD"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_LM viewModel = new VM_PF_LM(this);
            return View("Create",viewModel);
        }

        // GET: PF_LM/Edit/5        
        /// <summary>
        /// 绑定修改数据表单
        ///</summary>
        /// <returns>PF_LM视图模型</returns>
        [HttpGet]
        [Route("edit/{GID?}")]
        public ActionResult Edit([FromRoute] string GID)
        {
            if (!Permission.check(HttpContext, "LMSZ:EDIT"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_LM viewModel = new VM_PF_LM(this)
            {
                GID = GID
            };
            if (GID == null)
            {
                return NotFound();
            }
            else{
                return View("Edit",viewModel);
            }
        }
    }
}
