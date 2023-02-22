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
    [Route("Drug")]
    public class BUS_DRUGController : Controller
    {
        private readonly drugdbContext _context;
        string webRoot = string.Empty;
        public BUS_DRUGController(IHostingEnvironment env, drugdbContext context)
        {
            _context = context;
            webRoot = env.WebRootPath;
        }
        // GET: BUS_DRUG
        /// <summary>
        /// 绑定页面基本视图
        ///</summary>
        /// <returns>BUS_DRUG视图模型</returns>
        [HttpGet, Route("Index")]
        public ActionResult Index()
        {
            if (!Permission.check(HttpContext, "OPERATE:YWCK"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_DRUG viewModel = new VM_BUS_DRUG(this);
            return View("Index", viewModel);

        }

        // GET: BUS_DRUG/Details/5       
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
        /// <returns>BUS_ALL视图模型</returns>
        [HttpGet, Route("Details")]
        public ActionResult Details([FromQuery]string DRUGID, string RELATIONID)
        {
            try
            {
                ViewBag.TotalDb = this._context.BUS_DRUG.Where(m => m.IS_PUB && !m.IS_DELETE).Count();
                DRUGID = DRUGID ?? Guid.NewGuid().ToString();
                var db = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select * from bus_all where DRUGID=@DRUGID and RELATIONID=@RELATIONID", new MySqlParameter[] { new MySqlParameter("DRUGID", DRUGID), new MySqlParameter("RELATIONID", RELATIONID) });
                if (db.Count() == 0)
                {
                    return NotFound();
                }
                else
                {
                    var q = this._context.BUS_DRUG.Find(DRUGID);
                    q.HIT = q.HIT + 1;
                    this._context.SaveChanges();
                    var d = db.FirstOrDefault();
                    if (d.DISEASE_ALIAS == null || d.DISEASE_ALIAS.Equals(string.Empty))
                    {
                        ViewBag.DI = ";" + d.DISEASE_ALIAS + "&disease=&alias=";
                    }
                    else
                    {
                        ViewBag.DI = "&disease=" + d.DISEASE + "&alias=" + d.DISEASE_ALIAS;
                    }
                    return View("Details", db.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "异常", "BUS_DRUG", ex.ToString());
                return NoContent();
            }
        }
        // GET: BUS_DRUG/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>BUS_DRUG视图模型</returns>
        [HttpGet, Route("create")]
        public ActionResult Create()
        {
            if (!Permission.check(HttpContext, "OPERATE:YWXZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_DRUG viewModel = new VM_BUS_DRUG(this);
            viewModel.DRUGID = Guid.NewGuid().ToString();
            viewModel.IS_PUB = true;
            var d = this._context.BUS_DRUG.Where(n => n.DRUGCODE.StartsWith("2" + DateTime.Now.ToString("yyyyMMdd"))).OrderByDescending(m => m.DRUGCODE);
            if (d.Count() == 0)
            {
                viewModel.DRUGCODE = "2" + DateTime.Now.ToString("yyyyMMdd") + "001";
            }
            else
            {
                viewModel.DRUGCODE = (long.Parse(d.FirstOrDefault().DRUGCODE) + 1).ToString();
            }
            return View("Create", viewModel);
        }
        // GET: BUS_DRUG/Select
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>BUS_DRUG视图模型</returns>
        [HttpGet, Route("Select")]
        public ActionResult Select()
        {
            if (!Permission.check(HttpContext, "OPERATE:YWCK"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_DRUG vm = new VM_BUS_DRUG();
            vm.DRUGID = Guid.NewGuid().ToString();
            return View(vm);
        }
        // GET: BUS_DRUG/Edit/5        
        /// <summary>
        /// 绑定修改数据表单
        ///</summary>
        /// <returns>BUS_DRUG视图模型</returns>
        [HttpGet, Route("edit/{DID?}")]
        public ActionResult Edit([FromRoute] string DID)
        {
            if (!Permission.check(HttpContext, "OPERATE:YWBJ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_DRUG viewModel = new VM_BUS_DRUG(this)
            {
                DRUGID = DID
            };
            if (DID == null)
            {
                return NotFound();
            }
            else
            {
                viewModel = AutoMapper.Mapper.Map<BUS_DRUG, VM_BUS_DRUG>(this._context.BUS_DRUG.Find(DID));
                return View("Edit", viewModel);
            }
        }
    }
}
