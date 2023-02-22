using System;
using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;
using Microsoft.AspNetCore.Hosting;
using TDSCoreLib;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Model.Model;
using Main.platform;
using AutoMapper;

namespace Main.Areas.CMS.Controllers
{
    [Area("cms")]
    [Route("cms/article"),Authorize]
    public class CMS_ARTICLEController : Controller
    {
        private readonly drugdbContext _context;

        public CMS_ARTICLEController(drugdbContext context)
        {
            _context = context;
        }
        VM_CMS_ARTICLE viewModel = new VM_CMS_ARTICLE();

        // GET: CMS/CMS_ARTICLE
       /// <summary>
       /// 绑定页面基本视图
       ///</summary>
       /// <returns>CMS/CMS_ARTICLE视图模型</returns>
        [HttpGet]
        public ActionResult Index()
        {    
            return View("Index", viewModel);
        }
        // GET: CMS/CMS_ARTICLE
        /// <summary>
        /// 绑定页面基本视图
        ///</summary>
        /// <returns>CMS/CMS_ARTICLE视图模型</returns>
        [HttpGet,Route("List")]
        public ActionResult List(string title,string channelid)
        {
            ViewBag.XXExpand = "layui-nav-itemed";
            ViewBag.Title = title;
            ViewBag.Roles = Permission.getCurrentUserRoles();
            if (!string.IsNullOrEmpty(channelid))
            {
                ViewBag.ChannelId = channelid;
                return View("List", viewModel);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet,Route("ListImg")]
        public ActionResult ListImg()
        {
            return View();
        }


        [HttpGet, Route("JTYHList")]
        public ActionResult JTYHList(string title, string channelid)
        {
            ViewBag.XXExpand = "layui-nav-itemed";
            ViewBag.Title = title;
            ViewBag.Roles = Permission.getCurrentUserRoles();
            if (!string.IsNullOrEmpty(channelid))
            {
                ViewBag.ChannelId = channelid;
                return View("List", viewModel);
            }
            else
            {
                return NotFound();
            }
        }
  
        // GET: cms/cms/article/Details/ARTICLEID   
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
        /// <returns>CMS/CMS_ARTICLE视图模型</returns>
        [HttpGet,Route("detail")]
        public ActionResult Details(string ARTICLEID)
        {
            CMS_ARTICLE queryResult = _context.CMS_ARTICLE.Find(ARTICLEID);
            viewModel = Mapper.Map<CMS_ARTICLE, VM_CMS_ARTICLE>(queryResult);
            if (viewModel == null)
            {
                return NotFound();
            }
            else{
                return View("Details",viewModel);
                
            }
        }

        // GET: cms/cms/article/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>CMS/CMS_ARTICLE视图模型</returns>
        [HttpGet,Route("create")]
        public ActionResult Create([FromQuery]string channelid)
        {
            if (!Permission.check(HttpContext, "OPERATE:WZXZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            viewModel.CHANNELID = channelid;
            viewModel.ARTICLEID = Guid.NewGuid().ToString();
            viewModel.ISPUB = false;
            viewModel.ARTICLEEDITOR = Permission.getCurrentUser();
            ViewBag.Roles = Permission.getCurrentUserRoles();
            return View("Create",viewModel);
        }

        // GET: cms/cms/article/Edit/ARTICLEID
        /// <summary>
        /// 绑定编辑数据表单
        ///</summary>
        /// <returns>CMS/CMS_ARTICLE视图模型</returns>
        [HttpGet("{ARTICLEID?}")]
        public ActionResult Edit([FromRoute]string ARTICLEID)
        {
            if (!Permission.check(HttpContext, "OPERATE:WZBJ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            viewModel.ARTICLEID=ARTICLEID;
            ViewBag.Roles = Permission.getCurrentUserRoles();
            if (ARTICLEID == null)
            {
                return NotFound();
            }
            else{
                return View("Edit",viewModel);
            }
        }
    }
}
