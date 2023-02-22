using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using System.IO;

namespace Main.platform
{
    /// <summary>
    /// Senparc page.171#page.183
    /// </summary>
    public class WeiXinCustomMessageHandler : MessageHandler<WeiXinCustomMessageContext>
    {

        public WeiXinCustomMessageHandler(Stream inputStream, PostModel postModel) : base(inputStream, postModel)
        {
        }
        /// <summary>
        /// 默认返回的消息
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            Log.Write(this.GetType(), "微信消息（默认）", "WEIXIN_LOCATION", requestMessage.FromUserName);
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();//ResponseMessageText也可以是新闻、图片等其他类型
            responseMessage.Content = "默认消息";
            return responseMessage;
        }

        /// <summary>
        /// 处理文本类请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            if (requestMessage.Content == "关于")
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                responseMessage.Articles.Add(new Article() {
                    Title = "武汉启辰合智科技有限公司系统有限公司",
                    Description = "隶属于集团",
                    PicUrl = "/SiteImages/logo_min.png",
                    Url = "/"
                });
                return responseMessage;
            }
            else
            {
                return null;

            }
        }

        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            Log.Write(this.GetType(), "微信消息（定位）", "WEIXIN_LOCATION", requestMessage.Label + "-经度：" + requestMessage.Location_X + "-维度：" + requestMessage.Location_Y);
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "定位成功！"+ requestMessage.Label + "-经度：" + requestMessage.Location_X + "-维度：" + requestMessage.Location_Y;
            return responseMessage;
        }

    }
}
