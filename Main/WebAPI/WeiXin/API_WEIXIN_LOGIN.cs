using System;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP.AdvancedAPIs;
using Main.Extensions;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin;
using Model.Model;
using AutoMapper;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Main.platform;
using Microsoft.AspNetCore.Http;

namespace Main.WebAPI.WeiXin
{
    /// <summary>
    /// 微信登录相关
    /// </summary>
    [Produces("application/json")]
    [Route("api/weixin")]
    public class API_WEIXIN_LOGIN : Controller
    {
        private readonly drugdbContext _context;
        private static string appId = AppConfigurtaionServices.Configuration["AppSettings:wx_appId"];
        private static readonly string secret = AppConfigurtaionServices.Configuration["AppSettings:wx_secret"];
        private IMemoryCache _memoryCache;
        public API_WEIXIN_LOGIN(drugdbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// 微信用户授权回调，获取用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state">传递给微信服务器的参数</param>
        /// <param name="proxy_state">从回调代理服务器传来的参数</param>
        /// <returns></returns>
        [HttpGet("userinfocallback")]
        public ActionResult UserInfoCallback(string code, string state, string proxy_state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Ok(new { success = false, msg = "您拒绝了授权" });
            }
            if (_memoryCache.Get(state) == null)
            {
                return Ok(new { success = false, msg = "验证失败！请从正规途径访问" });
            }
            OAuthAccessTokenResult result = null;
            try
            {
                result = OAuthApi.GetAccessToken(appId, secret, code);
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, msg = "出错：" + ex.Message });
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                return Ok(new { success = false, msg = "请求错误：" + result.errmsg });
            }
            try
            {
                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                bool hasReg = _context.WEIXIN_USER.Where(w => w.IS_DELETE == false && w.OPENID == userInfo.openid).Count() > 0 ? true : false;
                if (hasReg)
                {

                }
                else
                {
                    //创建微信用户信息
                    WEIXIN_USER wx_user = new WEIXIN_USER
                    {
                        GID = Guid.NewGuid().ToString(),
                        UNIONID = string.IsNullOrEmpty(userInfo.unionid) ? string.Empty : userInfo.unionid,
                        SEX = userInfo.sex == 1 ? "男" : (userInfo.sex == 2 ? "女" : "未知"),
                        PROVINCE = string.IsNullOrEmpty(userInfo.province) ? string.Empty : userInfo.province,
                        OPENID = string.IsNullOrEmpty(userInfo.openid) ? string.Empty : userInfo.openid,
                        NICKNAME = string.IsNullOrEmpty(userInfo.nickname) ? string.Empty : userInfo.nickname,
                        MODIFY_DATE = DateTime.Now,
                        CREATE_DATE = DateTime.Now,
                        IS_DELETE = false,
                        HEADIMGURL = string.IsNullOrEmpty(userInfo.headimgurl) ? string.Empty : userInfo.headimgurl,
                        COUNTRY = string.IsNullOrEmpty(userInfo.country) ? string.Empty : userInfo.country,
                        CITY = string.IsNullOrEmpty(userInfo.city) ? string.Empty : userInfo.city,
                        BZ1 = appId,
                        BZ2 = string.Empty
                    };
                    _context.WEIXIN_USER.Add(wx_user);
                    _context.SaveChanges();
                }
                //将用户openid写入缓存
                _memoryCache.Set(state, userInfo.openid);
                return Ok(new { success = true, userInfo.openid });
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "微信登陆", "WEIXIN_USER", ex.ToString());
                return Ok(new { success = false, msg = "获取用户信息错误：" + ex.Message });
            }

        }
    }
}