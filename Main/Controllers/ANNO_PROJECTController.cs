using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Model.Model;
using Main.ViewModels.Annotation;
using AutoMapper;
using TDSCoreLib;
using Main.platform;
using Main.Extensions;
using Main.Utils;
using System.Data;
using System.Data.OleDb;
using System.IO;
using ExcelDataReader;
using System.Web;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using X.PagedList;

namespace Main.Controllers
{
    [Route("Project")]
    public class ANNO_PROJECTController : Controller
    {
        private readonly drugdbContext _context;
        string webRoot = string.Empty;
        public ANNO_PROJECTController(IHostingEnvironment env, drugdbContext context)
        {
            _context = context;
            webRoot = env.WebRootPath;
        }

        

        /// <summary>
        /// 绑定页面xx视图
        /// </summary>
        /// <returns>xx视图模型</returns>
        [HttpGet, Route("Index")]
        public IActionResult Index()
        {
            VM_ANNO_PROJECT viewModel = new VM_ANNO_PROJECT();
            var t = this._context.PF_USER.Where(m => m.USERNAME.Equals(User.Identity.Name)).FirstOrDefault();
            viewModel.USER_ID = t.GID;
            return View("Index", viewModel);
        }



        // GET: ANNO_PROJECT/Edit
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>ANNO_PROJECT视图模型</returns>
        [HttpGet, Route("edit")]
        public ActionResult Edit(string project_id)
        {
            if (!Permission.check(HttpContext, "OPERATE:PTXZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_ANNO_PROJECT viewModel = new VM_ANNO_PROJECT();
            var t = this._context.PF_USER.Where(m => m.USERNAME.Equals(User.Identity.Name)).FirstOrDefault();
            viewModel = AutoMapper.Mapper.Map<ANNO_PROJECT, VM_ANNO_PROJECT>(this._context.ANNO_PROJECT.Find(project_id));
            viewModel.USER_ID = t.GID;

            return View("Edit", viewModel);
        }


        // GET: ANNO_PROJECT/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>ANNO_PROJECT视图模型</returns>
        [HttpGet, Route("create")]
        public ActionResult Create()
        {
            if (!Permission.check(HttpContext, "OPERATE:PTXZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_ANNO_PROJECT viewModel = new VM_ANNO_PROJECT();
            var t = this._context.PF_USER.Where(m => m.USERNAME.Equals(User.Identity.Name)).FirstOrDefault();
            viewModel.USER_ID = t.GID;
            viewModel.PROJECT_ID = Guid.NewGuid().ToString();



            return View("Create", viewModel);
        }


        
        // GET: ANNO_PROJECT/View
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>ANNO_PROJECT视图模型</returns>
        [HttpGet, Route("Viewpro")]
        public ActionResult Viewpro(string project_id)
        {
            if (!Permission.check(HttpContext, "OPERATE:PTXZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_ANNO_PROJECT viewModel = new VM_ANNO_PROJECT();
            var t = this._context.PF_USER.Where(m => m.USERNAME.Equals(User.Identity.Name)).FirstOrDefault();
            viewModel = AutoMapper.Mapper.Map<ANNO_PROJECT, VM_ANNO_PROJECT>(this._context.ANNO_PROJECT.Find(project_id));
            viewModel.USER_ID = t.GID;

            return View("Viewpro", viewModel);
        }




    }
}
