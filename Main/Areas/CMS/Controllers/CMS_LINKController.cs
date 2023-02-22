using System;
using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;
using Microsoft.AspNetCore.Hosting;
using TDSCoreLib;
using System.Collections.Generic;
using System.IO;
using Main.platform;

namespace Main.Controllers
{
    [Area("cms")]
    [Route("cms/link")]
    public class CMS_LINKController : Controller
    {
        VM_CMS_LINK viewModel = new VM_CMS_LINK();

        // GET: CMS/CMS_LINK
        /// <summary>
        /// 绑定页面基本视图
        ///</summary>
        /// <returns>CMS/CMS_LINK视图模型</returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (!Permission.check(HttpContext, "MENU:YQLJGL"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            return View("Index", viewModel);

        }

        // GET: cms/cms/link/Details/GID   
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
        /// <returns>CMS/CMS_LINK视图模型</returns>
        [HttpGet("{LINKID}")]
        public ActionResult Details([FromRoute]string LINKID)
        {
            viewModel.LINKID = LINKID;
            if (LINKID == null)
            {
                return NotFound();
            }
            else
            {
                return View("Details", viewModel);

            }
        }

        // GET: cms/cms/link/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>CMS/CMS_LINK视图模型</returns>
        [HttpGet, Route("create")]
        public ActionResult Create()
        {
            if (!Permission.check(HttpContext, "OPERATE:XZYQLJ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_CMS_LINK viewModel = new VM_CMS_LINK(this);
            viewModel.OPERATOR = Permission.getCurrentUser();
            return View("Create", viewModel);
        }

        // GET: cms/cms/link/Edit/GID
        /// <summary>
        /// 绑定编辑数据表单
        ///</summary>
        /// <returns>CMS/CMS_LINK视图模型</returns>
        [HttpGet("{LINKID?}")]
        public ActionResult Edit([FromRoute]string LINKID)
        {
            if (!Permission.check(HttpContext, "OPERATE:BJYQLJ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            viewModel.LINKID = LINKID;
            if (LINKID == null)
            {
                return NotFound();
            }
            else
            {
                VM_CMS_LINK viewModel = new VM_CMS_LINK(this);
                viewModel.LINKID = LINKID;
                viewModel.OPERATOR = Permission.getCurrentUser();
                return View("Edit", viewModel);
            }
        }

        private IHostingEnvironment _hostingEnvironment;

        public CMS_LINKController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
    }
}
