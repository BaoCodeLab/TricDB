using AutoMapper;
using Main.platform;
using Main.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TDSCoreLib;
using X.PagedList;

namespace Main.Controllers
{
    [Route("web")]
    public class WebsiteController : Controller
    {
        private readonly drugdbContext _context;
        public WebsiteController(drugdbContext context)
        {
            _context = context;
        }


        // GET: /<controller>/
        [HttpGet("web")]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [Route("login")]
        public IActionResult Register()
        {
            var t = _context.PF_STATE.Where(m => !m.IS_DELETE && m.TYPE.Equals("账号类型")).OrderBy(m => m.ORDERS).ToList();
            ViewBag.AccountType = t;
            return View("register");
        }

        [HttpPost, Route("SaveReg")]
        public IActionResult SaveReg([FromForm]string Email, [FromForm]string FirstName, [FromForm]string LastName, [FromForm]string AccountType, [FromForm]string Password, [FromForm]string ImageCode)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SupportValidateCode")))
            {
                return Ok(new { ret = false, msg = "Validate Code Error." });
            }
            else if (string.IsNullOrEmpty(ImageCode))
            {
                return Ok(new { ret = false, msg = "Please Enter Validate Code." });
            }
            else if ((!HttpContext.Session.GetString("SupportValidateCode").ToLower().Equals(ImageCode.ToLower())))
            {
                return Ok(new { ret = false, msg = "Validate Code Error." });

            }
            var t = this._context.PF_USER.Where(m => m.USERNAME.ToLower().Equals(Email.ToLower()));
            if (t.Count() > 0)
            {
                return Ok(new { ret = false, msg = "Email has Used." });
            }
            PF_USER pf = new PF_USER();
            pf.CREATE_DATE = DateTime.Now;
            pf.FISRTNAME = FirstName;
            pf.GID = Guid.NewGuid().ToString();
            pf.IS_DELETE = false;
            pf.LASTNAME = LastName;
            pf.MODIFY_DATE = DateTime.Now;
            pf.OPERATOR = Email;
            string mdg_psw = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Password))).ToLower().Replace("-", "");
            pf.PASSWORD = mdg_psw;
            pf.RYBM = AccountType;
            pf.SJHM = string.Empty;
            pf.USERNAME = Email;
            pf.XMBM = AccountType;
            pf.YHZT = "注册";
            this._context.PF_USER.Add(pf);
            var r = this._context.PF_ROLE.Where(m => m.NAME.Equals(AccountType) && !m.IS_DELETE);
            if (r.Count() > 0)
            {
                PF_USER_ROLE pur = new PF_USER_ROLE();
                pur.CREATE_DATE = DateTime.Now;
                pur.GID = Guid.NewGuid().ToString();
                pur.IS_DELETE = false;
                pur.MODIFY_DATE = DateTime.Now;
                pur.OPERATOR = Email;
                pur.ROLE_GID = r.FirstOrDefault().GID;
                pur.USER_GID = Email;
                this._context.PF_USER_ROLE.Add(pur);
            }
            this._context.SaveChanges();
            Log.Write(this.GetType(), "PF_User", "Save Register:" + Email + "_" + FirstName + "_" + LastName + "_" + AccountType + "_" + Password + "_" + ImageCode);
            return Ok(new { result = true, msg = "The registration information has been saved, waiting for the administrator to review." });
        }

        [HttpPost, Route("Login")]
        public IActionResult Login([FromForm]string username, [FromForm]string password, [FromForm]string remember, [FromQuery]string returnUrl)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Ok(new { result = false, msg = "请输入用户名和密码！" });
            }
            else
            {
                string mdg_psw = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password))).ToLower().Replace("-", "");
                bool checkUser = _context.PF_USER.Any("username ==@0 and password ==@1 and is_delete == false", username, mdg_psw);
                if (checkUser)
                {
                    PF_USER pf_user = _context.PF_USER.SingleOrDefault("username ==@0 and password ==@1 and is_delete == false", username, mdg_psw);
                    return doLogin(pf_user, remember, returnUrl);
                }
                else
                {
                    return Ok(new { result = false, msg = "用户名或密码错误！" });
                }
            }
        }
        public IActionResult doLogin(PF_USER user, string remember = "false", string returnUrl = null)
        {
            string username = user.USERNAME;
            Log.Write(GetType(), "LOGIN", "PF_USER", username + "登陆");
            //获取用户所属组织机构
            var pf_org = from s in _context.PF_USER_ORG where s.USER_NAME == username && s.IS_DELETE == false select new { orgname = s.ORG_NAME, orgpath = s.ORG_PATH, orgid = s.ORG_GID, bz2 = s.BZ2 };
            //获取角色
            var PF_ROLE = from s in _context.PF_ROLE
                          join c in _context.PF_USER_ROLE on s.GID equals c.ROLE_GID
                          join d in _context.PF_USER on c.USER_GID equals d.GID
                          where s.IS_DELETE == false
                          && c.IS_DELETE == false
                          && d.IS_DELETE == false
                          && d.USERNAME == username
                          select new
                          {
                              code = s.CODE,
                              name = s.NAME,
                          };
            //获取权限字段

            if (PF_ROLE == null)
            {
                const string badUserNameOrPasswordMessage = "用户未授权！";
                return Ok(new { msg = badUserNameOrPasswordMessage });
            }
            //获取权限
            var pf_permissions = from s in _context.PF_PERMISSION
                                 join a in _context.PF_ROLE_PERMISSION on s.GID equals a.PER_GID
                                 join b in _context.PF_ROLE on a.ROLE_GID equals b.GID
                                 join c in _context.PF_USER_ROLE on b.GID equals c.ROLE_GID
                                 join d in _context.PF_USER on c.USER_GID equals d.GID
                                 where s.IS_DELETE == false
                                 && a.IS_DELETE == false
                                 && b.IS_DELETE == false
                                 && c.IS_DELETE == false
                                 && d.IS_DELETE == false
                                 && d.USERNAME == username
                                 select new
                                 {
                                     code = s.CODE,
                                     name = s.NAME,
                                 };
            //获取档案信息
            PF_PROFILE PF_PROFILE = null;
            if (!user.RYBM.Equals(""))
            {
                PF_PROFILE = _context.PF_PROFILE.SingleOrDefault("CODE ==@0 and is_delete == false", user.RYBM);
            }
            //创建用户标识
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Sid, username));
            //创建组织机构标识
            try
            {
                identity.AddClaim(new Claim("ORG_GID", pf_org.FirstOrDefault().orgid));
                identity.AddClaim(new Claim("ORG_PATH", pf_org.FirstOrDefault().orgpath));
                identity.AddClaim(new Claim("ORG_NAME", pf_org.FirstOrDefault().orgname));
                identity.AddClaim(new Claim("BZ2", pf_org.FirstOrDefault().bz2));
            }
            catch
            {
                identity.AddClaim(new Claim("ORG_GID", "--"));
                identity.AddClaim(new Claim("ORG_PATH", "--"));
                identity.AddClaim(new Claim("ORG_NAME", "未分配"));

            }

            if (!Permission.PasswordStrength(user.PASSWORD) && pf_permissions.FirstOrDefault(s => s.code == "DATA:JCMM") != null)
            {
                PF_ROLE = null;
                identity.AddClaim(new Claim("CHECKMM", "false"));
            }
            else
            {
                identity.AddClaim(new Claim("CHECKMM", "true"));
            }
            if (PF_ROLE != null)
            {
                foreach (var role in PF_ROLE)
                {
                    //identity.AddClaim(new Claim(ClaimTypes.Name, role.name));
                    identity.AddClaim(new Claim(ClaimTypes.Role, role.code));
                }
            }
            List<string> claims = new List<string>();
            if (pf_permissions != null)
            {
                foreach (var permission in pf_permissions)
                {
                    claims.Add(permission.code);
                    //identity.AddClaim(new Claim(ClaimTypes.Name, permission.name));
                }
                if (Permission.PDICTIONARY.ContainsKey(username))
                {
                    Permission.PDICTIONARY.Remove(username);
                }
                Permission.PDICTIONARY.Add(username, claims);
            }

            //创建档案标识
            if (PF_PROFILE != null)
            {
                identity.AddClaim(new Claim("DA_CODE", PF_PROFILE.CODE));
                identity.AddClaim(new Claim("DA_ZW", PF_PROFILE.ZW));
                identity.AddClaim(new Claim("DA_NAME", PF_PROFILE.NAME));
                identity.AddClaim(new Claim("DA_SEX", PF_PROFILE.SEX));
                identity.AddClaim(new Claim("DA_AGE", Convert.ToString(PF_PROFILE.AGE)));
                identity.AddClaim(new Claim("DA_PHONE", PF_PROFILE.PHONE));
                identity.AddClaim(new Claim("DA_MAIL", PF_PROFILE.MAIL));
                identity.AddClaim(new Claim("DA_BZ", PF_PROFILE.BZ));
                identity.AddClaim(new Claim("DA_DLZH", PF_PROFILE.DLZH));
                identity.AddClaim(new Claim("DA_TXDZ", PF_PROFILE.TXDZ));
                identity.AddClaim(new Claim("DA_GRAH", PF_PROFILE.GRAH));
                identity.AddClaim(new Claim("DA_SR", PF_PROFILE.SR.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            //注册用户标识
            bool isRemember = false;
            if (remember.Equals("on"))
            {
                isRemember = true;
            }
            AuthenticationProperties pro = new AuthenticationProperties();
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties()
            {
                IsPersistent = isRemember //isRemember
            });

            if (returnUrl == null)
            {
                returnUrl = TempData["returnUrl"]?.ToString();
            }
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "WebSite", new { timestamp = DateTime.Now.Ticks.ToString() });
            }
        }
        /// <summary>
        /// 企业服务
        /// </summary>
        /// <returns></returns>
        [Route("service")]
        public IActionResult CompanyService()
        {
            return View("service");
        }

        [HttpGet("{ADNAME?}"), Route("ad")]
        public IActionResult Ad([FromQuery]string ADNAME)
        {
            CMS_AD vm = new CMS_AD();
            try
            {
                if (!string.IsNullOrEmpty(ADNAME))
                {
                    var query = this._context.CMS_AD.Where("IS_DELETE==false and (ADNAME=@0 or ADID=@0)", ADNAME).Take(1).ToList();
                    vm = query.FirstOrDefault();
                }
                ViewBag.AD = vm;
                return View("ad", vm);
            }
            catch
            {
                return View("ad", vm);
            }
        }
        /// <summary>
        /// 资讯列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("{ChannelId?}"), Route("news")]
        public IActionResult News([FromQuery]string ChannelId, [FromQuery]string auto, [FromQuery]int? Page = 1, [FromQuery]int? Limit = 10)
        {
            try
            {
                var pageIndex = Page ?? 1;
                var limit = Limit ?? 10;
                #region 加载Channel信息 
                if (string.IsNullOrEmpty(ChannelId))
                    ChannelId = Guid.NewGuid().ToString();
                var q = from n in this._context.CMS_CHANNEL where n.CHANNELID.Equals(ChannelId) || n.CHANNELNAME.Equals(ChannelId) select n;
                ViewBag.ChannelId = string.Empty;
                ViewBag.ChannelName = string.Empty;
                ViewBag.HREF = string.Empty;
                if (q.Count() > 0)
                {
                    var c = q.FirstOrDefault();
                    ChannelId = c.CHANNELID;
                    ViewBag.ChannelId = ChannelId;
                    var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
                    ViewBag.ChannelName = c.CHANNELNAME;
                    ViewBag.HREF = string.IsNullOrEmpty(c.CHANNELREDIRECT) ? Url.Action("News", "Website") + "?ChannelId=" + c.CHANNELID : c.CHANNELREDIRECT;
                }
                #endregion
                var queryResult = from n in _context.CMS_ARTICLE where n.ISPUB && !n.IS_DELETE && n.CHANNELID.Equals(ChannelId) orderby n.ARTICLETIME.AddDays(n.ARTICLETOPNUM) descending select new { n.ARTICLEID, n.ARTICLETITLE, n.CHANNELID, n.ARTICLEISTITLE, n.ARTICLEHIT, n.ARTICLEEDITOR, n.ARTICLEINDEXPIC, n.ARTICLEKEYWORDS, n.ARTICLETIME, n.ARTICLETOPNUM, n.CREATE_DATE, n.ARTICLEREDIRECT, n.MODIFY_DATE };
                List<CMS_ARTICLE> cms = new List<CMS_ARTICLE>();
                foreach (var r in queryResult)
                {
                    CMS_ARTICLE c = new CMS_ARTICLE();
                    c.ARTICLECONTENT = string.Empty;
                    c.ARTICLEEDITOR = r.ARTICLEEDITOR;
                    c.ARTICLEHIT = r.ARTICLEHIT;
                    c.ARTICLEID = r.ARTICLEID;
                    c.ARTICLEINDEXPIC = r.ARTICLEINDEXPIC;
                    c.ARTICLEISTITLE = r.ARTICLEISTITLE;
                    c.ARTICLEKEYWORDS = r.ARTICLEKEYWORDS;
                    c.ARTICLEREDIRECT = r.ARTICLEREDIRECT;
                    c.ARTICLETIME = r.ARTICLETIME;
                    c.ARTICLETITLE = r.ARTICLETITLE;
                    c.ARTICLETOPNUM = r.ARTICLETOPNUM;
                    c.CHANNELID = r.CHANNELID;
                    c.CREATE_DATE = r.CREATE_DATE;
                    c.ISPUB = true;
                    c.IS_DELETE = false;
                    c.MODIFY_DATE = r.MODIFY_DATE;
                    cms.Add(c);
                }
                var onePageOfArticle = cms.ToPagedList(pageIndex, limit);
                ViewBag.OnePageOfArticle = onePageOfArticle;
                if (!string.IsNullOrEmpty(auto) && cms.Count == 1)
                {
                    var article = this._context.CMS_ARTICLE.Find(cms.FirstOrDefault().ARTICLEID);
                    ViewBag.Detail = true;
                    ViewBag.Article = article;
                    article.ARTICLEHIT = article.ARTICLEHIT + 1;
                    this._context.SaveChanges();
                }
                else
                {
                    ViewBag.Detail = false;
                    ViewBag.Article = null;
                }
                return View("news");
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "异常", "BUS_ARTICLE", ex.ToString());
                return NoContent();
            }
        }
        /// <summary>
        /// 文章详情
        /// </summary>
        /// <returns></returns>
        [Route("news/article/{ARTICLEID?}")]
        public IActionResult Article([FromRoute]string ARTICLEID)
        {
            VM_CMS_ARTICLE vm = new VM_CMS_ARTICLE();
            try
            {
                ViewBag.LM = string.Empty;
                if (ARTICLEID != null)
                {
                    var query = this._context.CMS_ARTICLE.Where("IS_DELETE==false and ISPUB==true and ARTICLEID=@0", ARTICLEID).Take(1).ToList();
                    vm = Mapper.Map<CMS_ARTICLE, VM_CMS_ARTICLE>(query.FirstOrDefault());
                    var d = from n in this._context.CMS_CHANNEL where n.CHANNELID.Equals(vm.CHANNELID) select n;
                    if (d.Count() > 0)
                    {
                        var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
                        var c = d.FirstOrDefault();
                        ViewBag.LM = c.CHANNELNAME;
                    }
                    query[0].ARTICLEHIT = query[0].ARTICLEHIT + 1;
                    this._context.SaveChanges();
                }
                return View("article", vm);
            }
            catch
            {
                return View("article", vm);
            }
        }
    }
}
