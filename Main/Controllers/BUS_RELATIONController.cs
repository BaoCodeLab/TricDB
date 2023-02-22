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

namespace Main.Controllers
{
    [Route("Relation")]
    public class BUS_RELATIONController : Controller
    {
        private readonly drugdbContext _context;
        string webRoot = string.Empty;
        public BUS_RELATIONController(IHostingEnvironment env, drugdbContext context)
        {
            _context = context;
            webRoot = env.WebRootPath;
        }
        // GET: BUS_RELATION
        /// <summary>
        /// 绑定页面基本视图
        ///</summary>
        /// <returns>BUS_RELATION视图模型</returns>
        [HttpGet, Route("Index")]
        public ActionResult Index()
        {
            if (!Permission.check(HttpContext, "OPERATE:GXCK"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_RELATION viewModel = new VM_BUS_RELATION(this);
            return View("Index", viewModel);

        }
        // GET: BUS_RELATION/Details/5
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
        /// <returns>BUS_RELATION视图模型</returns>
        [HttpGet, Route("Details")]
        public ActionResult Details([FromRoute]string RID)
        {
            if (!Permission.check(HttpContext, "OPERATE:GXCK"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_RELATION viewModel = new VM_BUS_RELATION(this)
            {
                RELATIONID = RID
            };
            if (RID == null)
            {
                return NotFound();
            }
            else
            {
                return View("Details", viewModel);

            }
        }
        // GET: BUS_RELATION/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>BUS_RELATION视图模型</returns>
        [HttpGet, Route("Create")]
        public ActionResult Create(string TARGETID)
        {
            if (!Permission.check(HttpContext, "OPERATE:GXXZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            if (string.IsNullOrEmpty(TARGETID))
            {
                return NoContent();
            }
            var bus_target = _context.BUS_TARGET.Find(TARGETID);
            if (bus_target == null) return NoContent();
            VM_BUS_RELATION viewModel = new VM_BUS_RELATION(this);
            viewModel.RELATIONID = Guid.NewGuid().ToString();
            viewModel.TARGETID = TARGETID;
            viewModel.TARGET = bus_target.TARGET;
            viewModel.ALTERATION = bus_target.ALTERATION;
            viewModel.IS_PUB = true;
            viewModel.IS_DELETE = false;
            return View("Create", viewModel);
        }
        // GET: BUS_RELATION/Edit/5        
        /// <summary>
        /// 绑定修改数据表单
        ///</summary>
        /// <returns>BUS_RELATION视图模型</returns>
        [HttpGet, Route("Edit/{RID?}")]
        public ActionResult Edit([FromRoute] string RID)
        {
            if (!Permission.check(HttpContext, "OPERATE:GXBJ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_RELATION viewModel = new VM_BUS_RELATION(this)
            {
                RELATIONID = RID
            };
            if (RID == null)
            {
                return NotFound();
            }
            else
            {
                viewModel = AutoMapper.Mapper.Map<BUS_RELATION, VM_BUS_RELATION>(this._context.BUS_RELATION.Find(RID));
                return View("Edit", viewModel);
            }
        }
    }
}
