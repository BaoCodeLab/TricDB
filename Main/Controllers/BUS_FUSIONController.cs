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
    [Route("Fusion")]
    public class BUS_FUSIONController : Controller
    {
        private readonly drugdbContext _context;
        string webRoot = string.Empty;
        public BUS_FUSIONController(IHostingEnvironment env, drugdbContext context)
        {
            _context = context;
            webRoot = env.WebRootPath;
        }
        // GET: BUS_FUSION
        /// <summary>
        /// 绑定页面基本视图
        ///</summary>
        /// <returns>BUS_FUSION视图模型</returns>
        [HttpGet, Route("Index")]
        public ActionResult Index()
        {
            if (!Permission.check(HttpContext, "OPERATE:RHCK"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_FUSION viewModel = new VM_BUS_FUSION(this);
            return View("Index", viewModel);

        }

        // GET: BUS_FUSION/Details/5       
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
        /// <returns>BUS_ALL视图模型</returns>
        [HttpGet, Route("Details")]
        public ActionResult Details([FromQuery]string FUSION_ID)
        {
            try
            {
                FUSION_ID = FUSION_ID ?? Guid.NewGuid().ToString();
                var db = this._context.BUS_FUSION.Find(FUSION_ID);
                if (db == null)
                {
                    return NotFound();
                }
                return View(db);
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "异常", "BUS_FUSION", ex.ToString());
                return NoContent();
            }
        }
        // GET: BUS_FUSION/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>BUS_FUSION视图模型</returns>
        [HttpGet, Route("create")]
        public ActionResult Create()
        {
            if (!Permission.check(HttpContext, "OPERATE:RHXZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_FUSION viewModel = new VM_BUS_FUSION(this);
            viewModel.FUSION_ID = Guid.NewGuid().ToString();
            viewModel.IS_PUB = true;
            var d = this._context.BUS_FUSION.Where(n => n.FUSIONCODE.StartsWith("5" + DateTime.Now.ToString("yyyyMMdd"))).OrderByDescending(m => m.FUSIONCODE);
            if (d.Count() == 0)
            {
                viewModel.FUSIONCODE = "5" + DateTime.Now.ToString("yyyyMMdd") + "001";
            }
            else
            {
                viewModel.FUSIONCODE = (long.Parse(d.FirstOrDefault().FUSIONCODE) + 1).ToString();
            }
            return View("Create", viewModel);
        }

        // GET: BUS_FUSION/Edit/5        
        /// <summary>
        /// 绑定修改数据表单
        ///</summary>
        /// <returns>BUS_FUSION视图模型</returns>
        [HttpGet, Route("edit/{DID?}")]
        public ActionResult Edit([FromRoute] string DID)
        {
            if (!Permission.check(HttpContext, "OPERATE:RHBJ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_FUSION viewModel = new VM_BUS_FUSION(this)
            {
                FUSION_ID = DID
            };
            if (DID == null)
            {
                return NotFound();
            }
            else
            {
                viewModel = AutoMapper.Mapper.Map<BUS_FUSION, VM_BUS_FUSION>(this._context.BUS_FUSION.Find(DID));
                return View("Edit", viewModel);
            }
        }
    }
}
