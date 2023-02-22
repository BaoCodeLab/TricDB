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
    [Route("ClinicalSample")]
    public class BUS_CLINICAL_SAMPLEController : Controller
    {
        private readonly drugdbContext _context;
        string webRoot = string.Empty;
        public BUS_CLINICAL_SAMPLEController(IHostingEnvironment env, drugdbContext context)
        {
            _context = context;
            webRoot = env.WebRootPath;
        }
        // GET: BUS_CLINICAL_SAMPLE
        /// <summary>
        /// 绑定页面基本视图
        ///</summary>
        /// <returns>BUS_CLINICAL_SAMPLE视图模型</returns>
        [HttpGet, Route("Index")]
        public ActionResult Index()
        {
            if (!Permission.check(HttpContext, "OPERATE:YWCK"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_CLINICAL_SAMPLE viewModel = new VM_BUS_CLINICAL_SAMPLE(this);
            return View("Index", viewModel);

        }

        // GET: BUS_CLINICAL_SAMPLE/Details/5       
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
        /// <returns>BUS_ALL视图模型</returns>
        [HttpGet, Route("Details")]
        public ActionResult Details([FromQuery]string SAMPLE_ID)
        {
            try
            {
                SAMPLE_ID = SAMPLE_ID ?? Guid.NewGuid().ToString();
                var q = this._context.BUS_CLINICAL_SAMPLE.Find(SAMPLE_ID);
                if(q==null)
                {
                    return NotFound();
                }
                else { 
                    return View("Details", q);
                }
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "异常", "BUS_CLINICAL_SAMPLE", ex.ToString());
                return NoContent();
            }
        }
        // GET: BUS_CLINICAL_SAMPLE/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>BUS_CLINICAL_SAMPLE视图模型</returns>
        [HttpGet, Route("create")]
        public ActionResult Create()
        {
            if (!Permission.check(HttpContext, "OPERATE:YWXZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_CLINICAL_SAMPLE viewModel = new VM_BUS_CLINICAL_SAMPLE(this);
            viewModel.SAMPLE_ID = Guid.NewGuid().ToString();
            viewModel.IS_PUB = true;
            var d = this._context.BUS_CLINICAL_SAMPLE.Where(n => n.SAMPLECODE.StartsWith("2" + DateTime.Now.ToString("yyyyMMdd"))).OrderByDescending(m => m.SAMPLECODE);
            if (d.Count() == 0)
            {
                viewModel.SAMPLECODE = "2" + DateTime.Now.ToString("yyyyMMdd") + "001";
            }
            else
            {
                viewModel.SAMPLECODE = (long.Parse(d.FirstOrDefault().SAMPLECODE) + 1).ToString();
            }
            return View("Create", viewModel);
        }
        // GET: BUS_CLINICAL_SAMPLE/Select
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>BUS_CLINICAL_SAMPLE视图模型</returns>
        [HttpGet, Route("Select")]
        public ActionResult Select()
        {
            if (!Permission.check(HttpContext, "OPERATE:YWCK"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_CLINICAL_SAMPLE vm = new VM_BUS_CLINICAL_SAMPLE();
            vm.SAMPLE_ID = Guid.NewGuid().ToString();
            return View(vm);
        }
        // GET: BUS_CLINICAL_SAMPLE/Edit/5        
        /// <summary>
        /// 绑定修改数据表单
        ///</summary>
        /// <returns>BUS_CLINICAL_SAMPLE视图模型</returns>
        [HttpGet, Route("edit/{DID?}")]
        public ActionResult Edit([FromRoute] string DID)
        {
            if (!Permission.check(HttpContext, "OPERATE:YWBJ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_CLINICAL_SAMPLE viewModel = new VM_BUS_CLINICAL_SAMPLE(this)
            {
                SAMPLE_ID = DID
            };
            if (DID == null)
            {
                return NotFound();
            }
            else
            {
                viewModel = AutoMapper.Mapper.Map<BUS_CLINICAL_SAMPLE, VM_BUS_CLINICAL_SAMPLE>(this._context.BUS_CLINICAL_SAMPLE.Find(DID));
                return View("Edit", viewModel);
            }
        }
    }
}
