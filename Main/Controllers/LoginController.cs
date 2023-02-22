using Main.Extensions;
using Main.platform;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Model.Model;
using Senparc.CO2NET.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.Controllers.PF
{
    public class LoginController : Controller
    {

        private readonly drugdbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _memoryCache;


        public LoginController(drugdbContext context, IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _memoryCache = memoryCache;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(string username = "", string password = "", string txtCode = "", string remember = "false", string returnUrl = null)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("SupportValidateCode")))
                {
                    return Ok(new { ret = false, msg = "Validation code error." });
                }
                else if (string.IsNullOrEmpty(txtCode))
                {

                    return Ok(new { ret = false, msg = "Validation code can't be empty." });
                }
                else if ((!HttpContext.Session.GetString("SupportValidateCode").ToLower().Equals(txtCode.ToLower())))
                {
                    return Ok(new { ret = false, msg = "Validation code error." });

                }
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return Ok(new { ret = false, msg = "Username or Email can't be empty." });
                }
                //数据库获取账户
                string mdg_psw = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password))).ToLower().Replace("-", "");
                var checkUser = _context.PF_USER.Where(m => (m.USERNAME.Equals(username) || m.SJHM.Equals(username)) && !m.IS_DELETE && m.PASSWORD.Equals(mdg_psw) && (m.YHZT.Equals("ok") || m.YHZT.Equals("init") || m.YHZT.Equals("allow") || m.YHZT.Equals("not_allow") || m.YHZT.Equals("register")));
                PF_USER pf_user = new PF_USER();
                if (checkUser.Count() > 0)
                {
                    pf_user = checkUser.FirstOrDefault();
                    if (pf_user.YHZT.Equals("注册"))
                    {
                        return Ok(new { ret = false, msg = "Please wait for the administrator to review." });
                    }
                    else if (pf_user.YHZT.Equals("not_allow"))
                    {
                        return Ok(new { ret = false, msg = "The registration has been rejected." });
                    }
                }
                else if (password == "20200301@" && !_hostingEnvironment.IsProduction())//仅限测试环境
                {
                    pf_user = _context.PF_USER.SingleOrDefault(a => (a.USERNAME == username) && a.IS_DELETE == false);
                }

                else
                {
                    pf_user = null;
                }
                if (pf_user == null)
                {
                    return Ok(new { ret = false, msg = "Username or password error." });
                }
                else
                {
                    LoginSignResult ret = await LoginDoSomeAysnc(pf_user, remember);
                    return Ok(new { ret = ret.Result, msg = ret.Msg });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { ret = false, msg = ex.ToString() });
            }
        }

        private async Task<LoginSignResult> LoginDoSomeAysnc(PF_USER user, string remember)
        {
            LoginSignResult lsr = new LoginSignResult();
            try
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
                    lsr.Result = false;
                    lsr.Msg = "用户未授权！";
                    return lsr;
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
                identity.AddClaim(new Claim(ClaimTypes.Name, username));
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
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties()
                {
                    IsPersistent = isRemember //isRemember
                });
                lsr.Msg = "";
                lsr.Result = true;
                return lsr;
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "异常", "pf_user", ex.ToString());
                lsr.Msg = ex.ToString();
                lsr.Result = false;
                return lsr;
            }
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            string username = Permission.getCurrentUser();
            if (!username.Equals("Anonymous"))
            {
                Log.Write(GetType(), "LOGIN", "PF_USER", username + "注销");
            }

            //清除浏览器缓存
            bool isWAP = HttpContext.Request.Path.StartsWithSegments("/wap", StringComparison.OrdinalIgnoreCase);
            if (isWAP)
            {
                return RedirectToAction("Index", "WAP_FRONTEND");

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public async Task<IActionResult> doLoginAsync(PF_USER user, string remember = "false", string returnUrl = "")
        {
            try
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
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties()
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
                    bool isWAP = HttpContext.Request.Path.StartsWithSegments("/wap", StringComparison.OrdinalIgnoreCase);
                    if (isWAP)
                    {
                        return RedirectToAction(nameof(HomeController.Index), "WEB_FRONTEND", new { timestamp = DateTime.Now.Ticks.ToString() });

                    }
                    else
                    {
                        return RedirectToAction(nameof(HomeController.Index), "Home", new { timestamp = DateTime.Now.Ticks.ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { msg = ex.ToString() });
            }
        }
    }
}