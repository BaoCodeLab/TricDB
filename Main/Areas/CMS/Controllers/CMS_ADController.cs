using System;
using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;
using Microsoft.AspNetCore.Hosting;
using TDSCoreLib;
using System.Collections.Generic;
using System.IO;
using Main.platform;
using Model.Model;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Main.Controllers
{
    [Area("cms")]
    [Route("cms/ad")]
    public class CMS_ADController : Controller
    {
        private readonly drugdbContext _context;

        public CMS_ADController(drugdbContext context)
        {
            _context = context;
        }
        VM_CMS_AD viewModel = new VM_CMS_AD();

        // GET: CMS/CMS_AD
        /// <summary>
        /// 绑定页面基本视图
        ///</summary>
        /// <returns>CMS/CMS_AD视图模型</returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (!Permission.check(HttpContext, "MENU:WBGL"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            return View("Index", viewModel);
        }

        // GET: cms/cms/ad/Details/ADID   
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
        /// <returns>CMS/CMS_AD视图模型</returns>
        [HttpGet,Route("detail")]
        public ActionResult Details(string ADID)
        {
            if (!Permission.check(HttpContext, "OPERATE:WBXQ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            CMS_AD queryResult = _context.CMS_AD.Find(ADID);
            viewModel = Mapper.Map<CMS_AD, VM_CMS_AD>(queryResult);
            if (ADID == null)
            {
                return NotFound();
            }
            else
            {
                return View("Details", viewModel);
            }
        }

        // GET: cms/cms/ad/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>CMS/CMS_AD视图模型</returns>
        [HttpGet, Route("create")]
        public ActionResult Create()
        {
            if (!Permission.check(HttpContext, "OPERATE:XZWB"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            viewModel.ADID = Guid.NewGuid().ToString();
            viewModel.CREATE_DATE = DateTime.Now;
            viewModel.MODIFY_DATE = DateTime.Now;
            viewModel.IS_DELETE = false;
            viewModel.OPERATOR = Permission.getCurrentUser();
            return View("Create", viewModel);
        }

        // GET: cms/cms/ad/Edit/ADID
        /// <summary>
        /// 绑定编辑数据表单
        ///</summary>
        /// <returns>CMS/CMS_AD视图模型</returns>
        [HttpGet("{ADID?}")]
        public ActionResult Edit([FromRoute]string ADID)
        {
            if (!Permission.check(HttpContext, "OPERATE:BJWB"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            viewModel.ADID = ADID;
            viewModel.OPERATOR = Permission.getCurrentUser();
            if (ADID == null)
            {
                return NotFound();
            }
            else
            {
                return View("Edit", viewModel);
            }
        }

    }
}
