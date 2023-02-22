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
    [Route("Patient")]
    public class BUS_PATIENTController : Controller
    {
        private readonly drugdbContext _context;
        string webRoot = string.Empty;
        public BUS_PATIENTController(IHostingEnvironment env, drugdbContext context)
        {
            _context = context;
            webRoot = env.WebRootPath;
        }
        // GET: BUS_PATIENT
        /// <summary>
        /// 绑定页面基本视图
        ///</summary>
        /// <returns>BUS_PATIENT视图模型</returns>
        [HttpGet, Route("Index")]
        public ActionResult Index()
        {
            if (!Permission.check(HttpContext, "OPERATE:BRCK"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_PATIENT viewModel = new VM_BUS_PATIENT(this);
            return View("Index", viewModel);

        }

        // GET: BUS_PATIENT/Details/5       
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
        /// <returns>BUS_ALL视图模型</returns>
        [HttpGet, Route("Details")]
        public ActionResult Details([FromQuery]string PATIENTID)
        {
            try
            {
                PATIENTID = PATIENTID ?? Guid.NewGuid().ToString();
                var q = this._context.BUS_PATIENT.Find(PATIENTID);
                if (q == null)
                {
                    return NotFound();
                }
                else
                {
                    return View("Details", q);
                }
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "异常", "BUS_PATIENT", ex.ToString());
                return NoContent();
            }
        }
        // GET: BUS_PATIENT/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>BUS_PATIENT视图模型</returns>
        [HttpGet, Route("create")]
        public ActionResult Create()
        {
            if (!Permission.check(HttpContext, "OPERATE:BRXZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_PATIENT viewModel = new VM_BUS_PATIENT(this);
            viewModel.PATIENT_ID = Guid.NewGuid().ToString();
            viewModel.IS_PUB = true;
            var d = this._context.BUS_PATIENT.Where(n => n.PATIENTCODE.StartsWith("4" + DateTime.Now.ToString("yyyyMMdd"))).OrderByDescending(m => m.PATIENTCODE);
            if (d.Count() == 0)
            {
                viewModel.PATIENTCODE = "4" + DateTime.Now.ToString("yyyyMMdd") + "001";
            }
            else
            {
                viewModel.PATIENTCODE = (long.Parse(d.FirstOrDefault().PATIENTCODE) + 1).ToString();
            }
            return View("Create", viewModel);
        }
        // GET: BUS_PATIENT/Select
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>BUS_PATIENT视图模型</returns>
        [HttpGet, Route("Select")]
        public ActionResult Select()
        {
            if (!Permission.check(HttpContext, "OPERATE:BRCK"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_PATIENT vm = new VM_BUS_PATIENT();
            vm.PATIENT_ID = Guid.NewGuid().ToString();
            return View(vm);
        }
        // GET: BUS_PATIENT/Edit/5        
        /// <summary>
        /// 绑定修改数据表单
        ///</summary>
        /// <returns>BUS_PATIENT视图模型</returns>
        [HttpGet, Route("edit/{DID?}")]
        public ActionResult Edit([FromRoute] string DID)
        {
            if (!Permission.check(HttpContext, "OPERATE:BRBJ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_PATIENT viewModel = new VM_BUS_PATIENT(this)
            {
                PATIENT_ID = DID
            };
            if (DID == null)
            {
                return NotFound();
            }
            else
            {
                viewModel = AutoMapper.Mapper.Map<BUS_PATIENT, VM_BUS_PATIENT>(this._context.BUS_PATIENT.Find(DID));
                return View("Edit", viewModel);
            }
        }
    }
}
