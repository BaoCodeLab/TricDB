using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Model.Model;
using Main.ViewModels;
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
    [Route("File")]
    public class ANNO_FILEController : Controller
    {
        private readonly drugdbContext _context;
        string webRoot = string.Empty;
        public ANNO_FILEController(IHostingEnvironment env, drugdbContext context)
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
            return View("Index");
        }


        // GET: ANNO_PROJECT/Edit
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>ANNO_PROJECT视图模型</returns>
        [HttpGet, Route("edit")]
        public ActionResult Edit(string GID)
        {
            if (!Permission.check(HttpContext, "OPERATE:PTXZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_PF_FILE viewModel = new VM_PF_FILE();
            viewModel = AutoMapper.Mapper.Map<PF_FILE, VM_PF_FILE>(this._context.PF_FILE.Find(GID));
            viewModel.GID = GID;

            return View("Edit", viewModel);
        }


    }
}
