using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Main.Extensions;
using Main.platform;
using Main.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Model.Model;
using TDSCoreLib;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/front")]
    public class APIFrontController : Controller
    {
        private readonly drugdbContext _context;
        public APIFrontController(drugdbContext context)
        {
            _context = context;
        }
        public static string VerificationCodeCacheFormat = "vcode_cache_{0}";
        [HttpGet, Produces("image/png"), Route("code")]
        public IActionResult ValidateCode()
        {
            try
            {
                VerificationCodeServices _vierificationCodeServices = new VerificationCodeServices();
                string code = "";
                System.IO.MemoryStream ms = _vierificationCodeServices.Create(out code);
                code = code.ToLower();//验证码不分大小写
                HttpContext.Session.SetString("SupportValidateCode", code);
                //Response.Body.Dispose();
                return File(ms.ToArray(), @"image/png");
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "PF_CODE", "生成图片异常，" + ex.ToString());
                return NoContent();
            }
        }
        [HttpGet, Route("checkusername")]
        public IActionResult CheckUserName(string userName)
        {
            try
            {
                userName = System.Web.HttpUtility.UrlDecode(userName);
                Regex rg = new Regex("^[A-Za-z0-9]*$");
                if (!rg.IsMatch(userName))
                {
                    return Ok(new { result = false, msg = "用户名只允许字母和数字组合！" });
                }
                DateTime dtime = DateTime.Now.AddSeconds(-2);
                string lastdtm = HttpContext.Session.GetString("LastCheckDate");
                if (!string.IsNullOrEmpty(lastdtm)) dtime = DateTime.Parse(lastdtm);
                if (dtime.AddSeconds(1) < DateTime.Now)
                {
                    var d = from n in _context.PF_USER where n.USERNAME.Equals(userName) select n.GID;
                    if (d.Count() > 0)
                    {
                        return Ok(new { result = false, msg = "用户名已经存在！" });
                    }
                    else
                    {
                        return Ok(new { result = true, msg = "用户名可以使用" });
                    }
                }
                else
                {
                    return Ok(new { result = false, msg = "查询过于频繁，系统拒绝" });
                }
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "PF_USER", "检测用户名异常" + ex.ToString());
                return Ok(new { result = false, msg = "系统异常，管理员已经记录" });
            }
        }
        /// <summary>
        /// 读取集团列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("getjtlb")]
        public IActionResult GetJTLB()
        {
            var d = from n in this._context.PF_USER_ROLE join m in this._context.PF_ROLE on n.ROLE_GID equals m.GID where m.NAME.Equals("集团用户") select new { n.USER_GID };
            var r = (from m in d join n in this._context.PF_USER on m.USER_GID equals n.GID select new { n.XMBM }).Distinct();
            return Ok(r);
        }
        [HttpGet, Route("getxzqh")]
        public IActionResult GetXZQH([FromQuery]string prefix)
        {
            prefix = System.Web.HttpUtility.UrlDecode(prefix);
            var d = from n in this._context.PF_ORG where n.PATH.StartsWith(prefix) && n.DEPTH == 4 && !n.IS_DELETE orderby n.ORDER select n;
            return Ok(d.ToList());
        }
        [HttpPost, Route("SavePwd")]
        public IActionResult SavePwd([FromForm]string mobile, [FromForm]string pwd)
        {
            try
            {
                if (!HttpContext.Session.GetString("mobile").Equals(mobile))
                {
                    return Ok(new { result = false, msg = "传递的手机号和验证通过的手机号不一致！" });
                }
                var q = this._context.PF_USER.Where(m => m.SJHM.Equals(mobile));
                if (q.Count() == 0)
                {
                    return Ok(new { result = false, msg = "数据库中未检索到对应的手机号码！" });
                }
                var entity = q.FirstOrDefault();
                entity.PASSWORD = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(pwd))).ToLower().Replace("-", "");
                this._context.PF_USER.Update(entity);
                this._context.SaveChanges();
                return Ok(new { result = true, msg = "密码修改成功，请牢记密码" });
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "PF_USER", ex.ToString());
                return Ok(new { result = false, msg = "密码修改异常！" });
            }
        }
    }
}