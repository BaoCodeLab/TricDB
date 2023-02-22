using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP;
using Main.Extensions;
using Main.platform;

namespace Main.Controllers
{
    /// <summary>
    /// ΢����Ϣ������
    /// </summary>
    [Route("weixin")]
    public class WeiXinController : Controller
    {
        private static readonly string appId = AppConfigurtaionServices.Configuration["AppSettings:wx_appId"];
        private static readonly string token = AppConfigurtaionServices.Configuration["AppSettings:wx_server_token"];
        private static readonly string EncodingAESKey = AppConfigurtaionServices.Configuration["AppSettings:wx_server_EncodingAESKey"];
        [HttpPost("index")]
        public ActionResult Index(PostModel postModel)
        {
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, postModel.Token))
            {
                return Content("��������");
            }
            postModel.AppId = appId;
            postModel.EncodingAESKey = EncodingAESKey;
            postModel.Token = token;
            var messageHandler = new WeiXinCustomMessageHandler(Request.Body, postModel);//������Ϣ����һ��
            messageHandler.Execute();//ִ��΢�Ŵ������
            return Content(messageHandler.TextResponseMessage);
        }
    }
}