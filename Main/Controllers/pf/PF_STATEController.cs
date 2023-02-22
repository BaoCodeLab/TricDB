using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;
using Microsoft.AspNetCore.Hosting;
using TDSCoreLib;
using System.Collections.Generic;
using Main.platform;

namespace Main.Controllers
{
    [Route("state")]
    public class PF_STATEController : Controller
    {
       

        // GET: PF_STATE
       /// <summary>
       /// 绑定页面基本视图
       ///</summary>
       /// <returns>PF_STATE视图模型</returns>
        [HttpGet]
        public ActionResult Index([FromQuery]string type="")
        {

            if (!Permission.check(HttpContext, "MENU:PF_STATE"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_STATE viewModel = new VM_PF_STATE(this);
            ViewBag.TYPE = type;
            ViewBag.Title = type == "" ? "状态管理" : type;
            return View("Index", viewModel);
    
        }

        // GET: PF_STATE/Details/5       
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
       /// <returns>PF_STATE视图模型</returns>
        [HttpGet("{GID}")]
        public ActionResult Details([FromRoute]string GID)
        {
            if (!Permission.check(HttpContext, "MENU:PF_STATE"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_STATE viewModel = new VM_PF_STATE(this)
            {
                GID = GID
            };
            if (GID == null)
            {
                return NotFound();
            }
            else{
                return View("Details",viewModel);
                
            }
        }

        // GET: PF_STATE/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>PF_STATE视图模型</returns>
        [HttpGet,Route("create")]
        public ActionResult Create()
        {
            if (!Permission.check(HttpContext, "OPERATE:DMXZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_STATE viewModel = new VM_PF_STATE(this);
            return View("Create",viewModel);
        }

        // GET: PF_STATE/Edit/5        
        /// <summary>
        /// 绑定修改数据表单
        ///</summary>
        /// <returns>PF_STATE视图模型</returns>
        [HttpGet("{GID?}")]
        public ActionResult Edit([FromRoute]string GID)
        {
            if (!Permission.check(HttpContext, "OPERATE:DMBJ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_STATE viewModel = new VM_PF_STATE(this)
            {
                GID = GID
            };
            if (GID == null)
            {
                return NotFound();
            }
            else{
                return View("Edit",viewModel);
            }
        }

        private IHostingEnvironment _hostingEnvironment;

        public PF_STATEController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("export")]
        public ActionResult ExportXls()
        {
            string apiURI = Url.Action("Get", "API_PF_STATE",null, HttpContext.Request.IsHttps?"https":"http")+"/all";
            var dataList= HttpClientHelper.GetResponse<List<VM_PF_STATE>>(apiURI);
            XlsGenerator.createXlsFile<VM_PF_STATE>(dataList, "数据", _hostingEnvironment.WebRootPath, out string filePath);//此处可修改sheet名称
            return File(filePath, "application/excel");        }
    }
}
