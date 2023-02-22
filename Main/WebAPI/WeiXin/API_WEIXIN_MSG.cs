using System;
using Main.Extensions;
using Main.platform;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.WebAPI.WeiXin
{
    /// <summary>
    /// 接收并处理微信消息
    /// </summary>
    [Route("api/weixin_msg")]
    public class API_WEIXIN_MSG : Controller
    {
        //private readonly drugdbContext _context;
        private static string appId = AppConfigurtaionServices.Configuration["AppSettings:wx_appId"];
        private static readonly string secret = AppConfigurtaionServices.Configuration["AppSettings:wx_secret"];
        private static readonly string EncodingAESKey = AppConfigurtaionServices.Configuration["AppSettings:wx_server_EncodingAESKey"];
        private static readonly string token = AppConfigurtaionServices.Configuration["AppSettings:wx_server_token"];
        [HttpPost("basic")]
        public ActionResult Index([FromQuery]PostModel postModel)
        {
            
            //
            try
            {
                if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, token))
                {
                    return Content("参数错误！");
                }
                postModel.AppId = appId;
                postModel.EncodingAESKey = EncodingAESKey;
                postModel.Token = token;
                var messageHandler = new WeiXinCustomMessageHandler(Request.Body, postModel);//.NetCore中Request.Body为数据流，如果请求方式是表单的话，框架默认会解析一次，导致request.body为null,需在Startup.cs中开启app.UseEnableRequestRewind();
                messageHandler.Execute();//执行微信处理过程
                return  Content(messageHandler.ResponseDocument.ToString());
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "微信消息解析错误", "PF_MSG", ex.ToString());
                return Content(ex.Message);

            }
        }
    }
}
