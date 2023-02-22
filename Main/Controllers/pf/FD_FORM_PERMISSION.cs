using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;
using Microsoft.AspNetCore.Hosting;
using TDSCoreLib;
using System.Collections.Generic;
using Main.platform;

namespace Main.Controllers
{
    [Route("form_permission")]
    public class FD_FORM_PERMISSION : Controller
    {
        VM_FD_FORM_PERMISSION viewModel = new VM_FD_FORM_PERMISSION();

        // GET: FD_FORM_PERMISSION
       /// <summary>
       /// 绑定页面基本视图
       ///</summary>
       /// <returns>FD_FORM_PERMISSION视图模型</returns>
        [HttpGet]
        public ActionResult Index()
        {
    
            return View("Index", viewModel);
    
        }

        // GET: FD_FORM_PERMISSION/Details/5       
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
       /// <returns>FD_FORM_PERMISSION视图模型</returns>
        [HttpGet("{GID}")]
        public ActionResult Details([FromRoute]string GID)
        {
            viewModel.GID=GID;
            if (GID == null)
            {
                return NotFound();
            }
            else{
                return View("Details",viewModel);
                
            }
        }

        // GET: FD_FORM_PERMISSION/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>FD_FORM_PERMISSION视图模型</returns>
        [HttpGet,Route("create")]
        public ActionResult Create()
        {
           return View("Create",viewModel);
        }

        // GET: FD_FORM_PERMISSION/Edit/5        
        /// <summary>
        /// 绑定修改数据表单
        ///</summary>
        /// <returns>FD_FORM_PERMISSION视图模型</returns>
        [HttpGet("{GID?}")]
        public ActionResult Edit([FromRoute]string GID)
        {
            viewModel.GID=GID;
            if (GID == null)
            {
                return NotFound();
            }
            else{
                return View("Edit",viewModel);
            }
        }

        private IHostingEnvironment _hostingEnvironment;

        public FD_FORM_PERMISSION(IHostingEnvironment hostingEnvironment)
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
            string apiURI = Url.Action("Get", "API_FD_FORM_PERMISSION",null, HttpContext.Request.IsHttps?"https":"http")+"/all";
            var dataList= HttpClientHelper.GetResponse<List<VM_FD_FORM_PERMISSION>>(apiURI);
            XlsGenerator.createXlsFile<VM_FD_FORM_PERMISSION>(dataList, "数据", _hostingEnvironment.WebRootPath, out string filePath);//此处可修改sheet名称
            return File(filePath, "application/excel");
        }

        /// <summary>
        /// 读取表单模板
        /// </summary>
        [HttpGet,Route("formlist")]
        public ActionResult Formlist()
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:FORM_PERMISSION"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_FD_FORM viewModel = new VM_FD_FORM();
            return View("Formlist", viewModel);
        }
        /// <summary>
        /// 读取角色清单
        /// </summary>
        [HttpGet, Route("rolelist")]
        public ActionResult Rolelist()
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:FORM_PERMISSION"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_ROLE viewModel = new VM_PF_ROLE();
            return View("Rolelist", viewModel);
        }
        /// <summary>
        /// 读取组织清单
        /// </summary>
        [HttpGet, Route("orglist")]
        public ActionResult Orglist([FromQuery]string formkey,string role_code)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:FORM_PERMISSION"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_ORG viewModel = new VM_PF_ORG();
            ViewBag.formkey = formkey;
            ViewBag.role_code = role_code;
            return View("Orglist", viewModel);
        }
    }
}
