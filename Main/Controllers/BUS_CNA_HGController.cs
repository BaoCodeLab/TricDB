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
    [Route("CnaHg")]
    public class BUS_CNA_HGController : Controller
    {
        private readonly drugdbContext _context;
        string webRoot = string.Empty;
        public BUS_CNA_HGController(IHostingEnvironment env, drugdbContext context)
        {
            _context = context;
            webRoot = env.WebRootPath;
        }
        // GET: BUS_CNA_HG
        /// <summary>
        /// 绑定页面基本视图
        ///</summary>
        /// <returns>BUS_CNA_HG视图模型</returns>
        [HttpGet, Route("Index")]
        public ActionResult Index()
        {
            if (!Permission.check(HttpContext, "OPERATE:CNACK"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_CNA_HG viewModel = new VM_BUS_CNA_HG(this);
            return View("Index", viewModel);

        }

        // GET: BUS_CNA_HG/Details/5       
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
        /// <returns>BUS_ALL视图模型</returns>
        [HttpGet, Route("Details")]
        public ActionResult Details([FromQuery]string CNA_ID)
        {
            try
            {
                CNA_ID = CNA_ID ?? Guid.NewGuid().ToString();
                var db = this._context.BUS_CNA_HG.Find(CNA_ID);
                if (db == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(db);
                }
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "异常", "BUS_CNA_HG", ex.ToString());
                return NoContent();
            }
        }
        // GET: BUS_CNA_HG/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>BUS_CNA_HG视图模型</returns>
        [HttpGet, Route("create")]
        public ActionResult Create()
        {
            if (!Permission.check(HttpContext, "OPERATE:CNAXZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_CNA_HG viewModel = new VM_BUS_CNA_HG(this);
            viewModel.CNA_ID = Guid.NewGuid().ToString();
            viewModel.IS_PUB = true;
            var d = this._context.BUS_CNA_HG.Where(n => n.CNACODE.StartsWith("6" + DateTime.Now.ToString("yyyyMMdd"))).OrderByDescending(m => m.CNACODE);
            if (d.Count() == 0)
            {
                viewModel.CNACODE = "6" + DateTime.Now.ToString("yyyyMMdd") + "001";
            }
            else
            {
                viewModel.CNACODE = (long.Parse(d.FirstOrDefault().CNACODE) + 1).ToString();
            }
            return View("Create", viewModel);
        }

        // GET: BUS_CNA_HG/Edit/5        
        /// <summary>
        /// 绑定修改数据表单
        ///</summary>
        /// <returns>BUS_CNA_HG视图模型</returns>
        [HttpGet, Route("edit/{DID?}")]
        public ActionResult Edit([FromRoute] string DID)
        {
            if (!Permission.check(HttpContext, "OPERATE:CNABJ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_CNA_HG viewModel = new VM_BUS_CNA_HG(this)
            {
                CNA_ID = DID
            };
            if (DID == null)
            {
                return NotFound();
            }
            else
            {
                viewModel = AutoMapper.Mapper.Map<BUS_CNA_HG, VM_BUS_CNA_HG>(this._context.BUS_CNA_HG.Find(DID));
                return View("Edit", viewModel);
            }
        }
    }
}
