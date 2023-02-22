using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;
using Microsoft.AspNetCore.Hosting;
using TDSCoreLib;
using System.Collections.Generic;


namespace Main.Controllers
{
    [Route("reminder")]
    public class PF_REMINDERController : Controller
    {
        VM_PF_REMINDER viewModel = new VM_PF_REMINDER();

        // GET: PF_REMINDER
       /// <summary>
       /// 绑定页面基本视图
       ///</summary>
       /// <returns>PF_REMINDER视图模型</returns>
        [HttpGet]
        public ActionResult Index()
        {
    
            return View("Index", viewModel);
    
        }

        // GET: PF_REMINDER/Details/5       
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
       /// <returns>PF_REMINDER视图模型</returns>
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

        // GET: PF_REMINDER/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>PF_REMINDER视图模型</returns>
        [HttpGet,Route("create")]
        public ActionResult Create()
        {
           return View("Create",viewModel);
        }

        // GET: PF_REMINDER/Edit/5        
        /// <summary>
        /// 绑定修改数据表单
        ///</summary>
        /// <returns>PF_REMINDER视图模型</returns>
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

        public PF_REMINDERController(IHostingEnvironment hostingEnvironment)
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
            string apiURI = Url.Action("Get", "API_PF_REMINDER",null, HttpContext.Request.IsHttps?"https":"http")+"/all";
            var dataList= HttpClientHelper.GetResponse<List<VM_PF_REMINDER>>(apiURI);
            XlsGenerator.createXlsFile<VM_PF_REMINDER>(dataList, "数据", _hostingEnvironment.WebRootPath, out string filePath);//此处可修改sheet名称
            return File(filePath, "application/excel");        }
    }
}
