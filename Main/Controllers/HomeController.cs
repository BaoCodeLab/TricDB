using AutoMapper;
using Main.Extensions;
using Main.platform;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Model.Model;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web;
using X.PagedList;

namespace Main.Controllers
{
    public class HomeController : Controller
    {
        private readonly drugdbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        public VM_HOME VM_HOME = new VM_HOME();
        private IMemoryCache _memoryCache;
        public HomeController(drugdbContext context, IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _memoryCache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
        }
        //微信登录
        private static readonly string appId = AppConfigurtaionServices.Configuration["AppSettings:wx_appId"];
        private static readonly string secret = AppConfigurtaionServices.Configuration["AppSettings:wx_secret"];
        private static readonly string wx_proxy = AppConfigurtaionServices.Configuration["AppSettings:wx_proxy"];//中间服务器

        /// <summary>
        /// 用户登录界面
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login([FromQuery]string ReturnUrl = "", string err = "")
        {
            if (string.IsNullOrEmpty(ReturnUrl)) ReturnUrl = Url.Action("Index", "Home");
            ViewBag.ReturnUrl = ReturnUrl;
            string SessionID = Guid.NewGuid().ToString(); // 用state作为参数，存入缓存中，并在回调中加以判断，避免在多次握手过程中泄露redirect_url及参数导致的安全问题，以增强系统安全性
            HttpContext.Session.SetString("SessionID", SessionID);
            _memoryCache.Set<string>(SessionID, "", TimeSpan.FromMinutes(15));//15分钟过期
            ViewBag.UrlUserInfo = OAuthApi.GetAuthorizeUrl(appId, wx_proxy, SessionID, Senparc.Weixin.MP.OAuthScope.snsapi_userinfo);
            ViewBag.error = err;
            return View("Login");
        }

        [Authorize]
        public IActionResult SystemM()
        {
            VM_HOME VM_HOME = new VM_HOME(this);

            User user = new User
            {
                username = Permission.getCurrentUser(),
                roles = Permission.getCurrentUserRoles(),
                permissions = Permission.getCurrentUserPermissions()
            };
            VM_HOME.userinfo = JsonConvert.SerializeObject(user);
            var t = this._context.PF_USER.Where(m => m.USERNAME.Equals(User.Identity.Name)).FirstOrDefault();
            ViewBag.UserName = t.USERNAME;
            ViewBag.FirstName = t.FISRTNAME;
            ViewBag.LastName = t.LASTNAME;
            ViewBag.AccountType = t.XMBM;
            ViewBag.CreateTime = t.CREATE_DATE.ToString("yyyy-MM-dd");
            return View(VM_HOME);
        }
        public IActionResult Index(string data)
        {
            return View();
        }
        public IActionResult Drug([FromQuery]string searchKey = "")
        {
            ViewBag.TotalDb = this._context.BUS_DRUG.Where(m => m.IS_PUB && !m.IS_DELETE).Count();
            ViewBag.SearchWord = searchKey;
            return View();
        }
        public IActionResult Disease([FromQuery]string searchKey = "")
        {
            ViewBag.TotalDb = this._context.BUS_DISEASE.Where(m => m.IS_PUB && !m.IS_DELETE).Count();
            ViewBag.SearchWord = searchKey;
            return View();
        }
        public IActionResult Targets([FromQuery]string searchKey = "")
        {
            ViewBag.TotalDb = this._context.BUS_TARGET.Where(m => m.IS_PUB && !m.IS_DELETE).Count();
            ViewBag.SearchWord = searchKey;
            return View();
        }
        public IActionResult All([FromQuery]string searchKey = "")
        {
            ViewBag.TotalDb = this._context.BUS_RELATION.Where(m => m.IS_PUB && !m.IS_DELETE).Count();
            ViewBag.SearchWord = searchKey;
            return View();
        }
        public IActionResult Detail([FromQuery]string DID = "")
        {
            if (string.IsNullOrEmpty(DID)) DID = Guid.NewGuid().ToString();
            var d = this._context.BUS_DRUG.Find(DID);
            if (d != null)
            {
                var m = Mapper.Map<BUS_DRUG, VM_BUS_DRUG>(d);
                if (d.HIT.HasValue)
                {
                    d.HIT = d.HIT.Value + 1;
                }
                else
                {
                    d.HIT = 1;
                }
                this._context.BUS_DRUG.Update(d);
                this._context.SaveChanges();
                string path = _hostingEnvironment.WebRootPath + "/relationImg/" + d.DRUGCODE + ".png";
                bool lap = System.IO.File.Exists(path);
                if (lap)
                {
                    ViewBag.ShowImg = "<img class='imgdetail img-responsive' src=\"/relationImg/" + d.DRUGCODE + ".png\"/>";
                }
                else
                {
                    path = _hostingEnvironment.WebRootPath + "/relationImg/" + d.DRUGCODE + ".jpg";
                    lap = System.IO.File.Exists(path);
                    if (lap)
                    {
                        ViewBag.ShowImg = "<img class='imgdetail img-responsive' src=\"/relationImg/" + d.DRUGCODE + ".jpg\"/>";
                    }
                    else
                    {
                        ViewBag.ShowImg = string.Empty;
                    }
                }
                return View("Detail", m);
            }
            else
            {
                ViewBag.Find = false;
                return View("Detail", new VM_BUS_DRUG());
            }
        }
    }
}
