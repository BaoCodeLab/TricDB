using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;
using Microsoft.AspNetCore.Hosting;
using TDSCoreLib;
using System.Collections.Generic;


namespace Main.Controllers
{
    [Route("user_org")]
    public class PF_USER_ORGController : Controller
    {
        VM_PF_USER_ORG viewModel = new VM_PF_USER_ORG();

        // GET: PF_USER_ORG
       /// <summary>
       /// 绑定页面基本视图
       ///</summary>
       /// <returns>PF_USER_ORG视图模型</returns>
        [HttpGet]
        public ActionResult Index([FromQuery]string ORG_GID,string ORG_PATH,string ORG_NAME)
        {
            ViewBag.ORG_GID = ORG_GID;
            ViewBag.ORG_PATH =System.Web.HttpUtility.UrlDecode(ORG_PATH);
            ViewBag.ORG_NAME = System.Web.HttpUtility.UrlDecode(ORG_NAME);
            return View("Index", viewModel);
    
        }

        // GET: PF_USER_ORG/Details/5       
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
       /// <returns>PF_USER_ORG视图模型</returns>
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

        // GET: PF_USER_ORG/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>PF_USER_ORG视图模型</returns>
        [HttpGet,Route("create")]
        public ActionResult Create()
        {
           return View("Create",viewModel);
        }

        // GET: PF_USER_ORG/Edit/5        
        /// <summary>
        /// 绑定修改数据表单
        ///</summary>
        /// <returns>PF_USER_ORG视图模型</returns>
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

        public PF_USER_ORGController(IHostingEnvironment hostingEnvironment)
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
            string apiURI = Url.Action("Get", "API_PF_USER_ORG",null, HttpContext.Request.IsHttps?"https":"http")+"/all";
            var dataList= HttpClientHelper.GetResponse<List<VM_PF_USER_ORG>>(apiURI);
            XlsGenerator.createXlsFile<VM_PF_USER_ORG>(dataList, "数据", _hostingEnvironment.WebRootPath, out string filePath);//此处可修改sheet名称
            return File(filePath, "application/excel");        }
    }
}
