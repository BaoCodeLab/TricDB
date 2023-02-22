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
using System.Linq;
namespace Main.Areas.CMS
{
    [Area("cms")]
    [Route("cms/channel")]
    public class CMS_CHANNELController : Controller
    {
        VM_CMS_CHANNEL viewModel = new VM_CMS_CHANNEL();
        private readonly drugdbContext _context;
        public CMS_CHANNELController(drugdbContext context)
        {
            _context = context;
        }

        // GET: CMS/CMS_CHANNEL
       /// <summary>
       /// 绑定页面基本视图
       ///</summary>
       /// <returns>CMS/CMS_CHANNEL视图模型</returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (!Permission.check(HttpContext, "MENU:LMGL"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            return View("Index", viewModel);
    
        }

        // GET: cms/cms/channel/Details/CHANNELID   
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
       /// <returns>CMS/CMS_CHANNEL视图模型</returns>
        [HttpGet("{CHANNELID}"),Route("detail")]
        public ActionResult Details(string CHANNELID)
        {
            CMS_CHANNEL queryResult = _context.CMS_CHANNEL.Find(CHANNELID);
            viewModel = Mapper.Map<CMS_CHANNEL, VM_CMS_CHANNEL>(queryResult);
            if (CHANNELID == null)
            {
                return NotFound();
            }
            else{
                return View("Details",viewModel);                
            }
        }

        // GET: cms/cms/channel/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>CMS/CMS_CHANNEL视图模型</returns>
        [HttpGet,Route("create")]
        public ActionResult Create(string PARENTCHANNELID)
        {
            if (!Permission.check(HttpContext, "OPERATE:LMXZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            var m = (from n in this._context.CMS_CHANNEL where n.PARENTCHANNELID.Equals(PARENTCHANNELID) select n.CHANNELXH);
            int xh = 1;
            if (m.Count() > 0)
            {
                xh = m.Max() + 1;
            }
            viewModel.CHANNELID = Guid.NewGuid().ToString();
            viewModel.PARENTCHANNELID = PARENTCHANNELID;
            viewModel.IS_DELETE = false;
            viewModel.ISPUB = true;
            viewModel.MODIFY_DATE = DateTime.Now;
            viewModel.CREATE_DATE = DateTime.Now;
            viewModel.OPERATOR = Permission.getCurrentUser();
            viewModel.BZ = string.Empty;
            viewModel.CHANNELXH = xh;
            return View("Create",viewModel);
        }

        // GET: cms/cms/channel/Edit/CHANNELID
        /// <summary>
        /// 绑定编辑数据表单
        ///</summary>
        /// <returns>CMS/CMS_CHANNEL视图模型</returns>
        [HttpGet("{CHANNELID?}")]
        public ActionResult Edit([FromRoute]string CHANNELID)
        {
            if (!Permission.check(HttpContext, "OPERATE:LMBJ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            viewModel.CHANNELID=CHANNELID;
            if (CHANNELID == null)
            {
                return NotFound();
            }
            else{
                return View("Edit",viewModel);
            }
        }
    }
}
