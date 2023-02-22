using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;
using Main.platform;
using System;
using Model.Model;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Main.Extensions;
using MySql.Data.MySqlClient;

namespace Main.Controllers
{
    [Route("Assay")]
    public class BUS_ASSAYController : Controller
    {
        private readonly drugdbContext _context;
        string webRoot = string.Empty;
        public BUS_ASSAYController(IHostingEnvironment env, drugdbContext context)
        {
            _context = context;
            webRoot = env.WebRootPath;
        }
        // GET: BUS_ASSAY
        /// <summary>
        /// 绑定页面基本视图
        ///</summary>
        /// <returns>BUS_ASSAY视图模型</returns>
        [HttpGet, Route("Index")]
        public ActionResult Index()
        {
            if (!Permission.check(HttpContext, "OPERATE:PTCK"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_ASSAY viewModel = new VM_BUS_ASSAY(this);
            return View("Index", viewModel);

        }

        // GET: BUS_ASSAY/Details/5       
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
        /// <returns>BUS_ALL视图模型</returns>
        [HttpGet, Route("Details")]
        public ActionResult Details([FromQuery]string ASSAYID)
        {
            try
            {
                ASSAYID = ASSAYID ?? Guid.NewGuid().ToString();
                var db = this._context.BUS_ASSAY.Find(ASSAYID);
                if (db == null)
                {
                    return NotFound();
                }
                else
                {
                    return View("Details", db);
                }
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "异常", "BUS_ASSAY", ex.ToString());
                return NoContent();
            }
        }
        // GET: BUS_ASSAY/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>BUS_ASSAY视图模型</returns>
        [HttpGet, Route("create")]
        public ActionResult Create()
        {
            if (!Permission.check(HttpContext, "OPERATE:PTXZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_ASSAY viewModel = new VM_BUS_ASSAY(this);
            viewModel.SEQ_ASSAY_ID = Guid.NewGuid().ToString();
            viewModel.IS_PUB = true;
            var d = this._context.BUS_ASSAY.Where(n => n.ASSAYCODE.StartsWith("7" + DateTime.Now.ToString("yyyyMMdd"))).OrderByDescending(m => m.ASSAYCODE);
            if (d.Count() == 0)
            {
                viewModel.ASSAYCODE = "7" + DateTime.Now.ToString("yyyyMMdd") + "001";
            }
            else
            {
                viewModel.ASSAYCODE = (long.Parse(d.FirstOrDefault().ASSAYCODE) + 1).ToString();
            }
            return View("Create", viewModel);
        }

        // GET: BUS_ASSAY/Edit/5        
        /// <summary>
        /// 绑定修改数据表单
        ///</summary>
        /// <returns>BUS_ASSAY视图模型</returns>
        [HttpGet, Route("edit/{DID?}")]
        public ActionResult Edit([FromRoute] string DID)
        {
            if (!Permission.check(HttpContext, "OPERATE:PTBJ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_ASSAY viewModel = new VM_BUS_ASSAY(this)
            {
                SEQ_ASSAY_ID = DID
            };
            if (DID == null)
            {
                return NotFound();
            }
            else
            {
                viewModel = AutoMapper.Mapper.Map<BUS_ASSAY, VM_BUS_ASSAY>(this._context.BUS_ASSAY.Find(DID));
                return View("Edit", viewModel);
            }
        }
    }
}
