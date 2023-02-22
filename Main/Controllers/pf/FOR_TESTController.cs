using System;
using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;
using Microsoft.AspNetCore.Hosting;
using TDSCoreLib;
using System.Collections.Generic;


namespace Main.Controllers
{
    [Route("test")]
    public class FOR_TESTController : Controller
    {
        VM_FOR_TEST viewModel = new VM_FOR_TEST();

        // GET: FOR_TEST
       /// <summary>
       /// 绑定页面基本视图
       ///</summary>
       /// <returns>FOR_TEST视图模型</returns>
        [HttpGet]
        public ActionResult Index()
        {
    
            return View("Index");
    
        }

        // GET: test/Details/5       
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
       /// <returns>FOR_TEST视图模型</returns>
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

        // GET: test/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>FOR_TEST视图模型</returns>
        [HttpGet,Route("create")]
        public ActionResult Create()
        {
           return View("Create",viewModel);
        }

        // GET: test/Edit/5        
        /// <summary>
        /// 绑定修改数据表单
        ///</summary>
        /// <returns>FOR_TEST视图模型</returns>
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

        public FOR_TESTController(IHostingEnvironment hostingEnvironment)
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
            string apiURI = Url.Action("Get", "API_FOR_TEST",null, HttpContext.Request.IsHttps?"https":"http")+"/all";
            var dataList= HttpClientHelper.GetResponse< List<VM_FOR_TEST>>(apiURI);
            string filePath = "";
            XlsGenerator.createXlsFile<VM_FOR_TEST>(dataList, "数据", _hostingEnvironment.WebRootPath, out filePath);//此处可修改sheet名称
            return File(filePath, "application/excel");        }
    }
}
