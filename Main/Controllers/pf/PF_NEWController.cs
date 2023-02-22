using System;
using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;
using Main.platform;

namespace Main.Controllers
{
    [Route("new")]
    public class PF_NEWController : Controller
    {       

        // GET: PF_NEW
        /// <summary>
        /// 绑定页面基本视图
        ///</summary>
        /// <returns>PF_NEW视图模型</returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:GLWZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_NEW viewModel = new VM_PF_NEW(this);
            return View("Index", viewModel);

        }

        // GET: PF_NEW/Details/5       
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
        /// <returns>PF_NEW视图模型</returns>
        [HttpGet, Route("detail")]
        public ActionResult Details(string GID)
        {
            if (!Permission.check(HttpContext, "GLWZ:DETAIL"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_NEW viewModel = new VM_PF_NEW(this)
            {
                GID = GID
            };
            if (GID == null)
            {
                return NotFound();
            }
            else {
                return View("Detail", viewModel);

            }
        }

        // GET: PF_NEW/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>PF_NEW视图模型</returns>
        [HttpGet, Route("create")]
        public ActionResult Create()
        {
            if (!Permission.check(HttpContext, "GLWZ:ADD"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_NEW viewModel = new VM_PF_NEW(this)
            {
                GID = Guid.NewGuid().ToString()
            };
            return View("Create", viewModel);
        }

        // GET: PF_NEW/Edit/5        
        /// <summary>
        /// 绑定修改数据表单
        ///</summary>
        /// <returns>PF_NEW视图模型</returns>
        [HttpGet("edit/{GID?}")]
        public ActionResult Edit([FromRoute]string GID)
        {
            if (!Permission.check(HttpContext, "GLWZ:EDIT"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_NEW viewModel = new VM_PF_NEW(this)
            {
                GID = GID
            };
            if (GID == null)
            {
                return NotFound();
            }
            else {
                return View("Edit", viewModel);
            }
        }

        // GET: PF_NEW/Read/5       
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
        /// <returns>PF_NEW视图模型</returns>
        [HttpGet]
        [Route("newlist")]
        public ActionResult NewList()
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:YLWZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_NEW viewModel = new VM_PF_NEW(this);
            return View("NewList", viewModel);
        }

        [HttpGet("{GID?}"), Route("read")]
        public ActionResult Read(string GID)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:YLWZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_NEW viewModel = new VM_PF_NEW(this)
            {
                GID = GID
            };
            if (GID == null)
            {
                return NotFound();
            }
            else
            {
                return View("Read", viewModel);
            }
        }

        [HttpGet]
        [Route("draft")]
        public ActionResult Draft(string GID)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:CGX"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_NEW viewModel = new VM_PF_NEW(this);
            return View("Draft", viewModel);

        }

        [HttpGet("readlm/{GID?}")]
        public ActionResult readLM([FromRoute]string GID)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:YLWZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_NEW viewModel = new VM_PF_NEW(this)
            {
                LM_GID = GID
            };
            return View("New_more", viewModel);

        }
    }
}
