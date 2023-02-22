using Senparc.NeuChar.Context;
using Senparc.NeuChar.Entities;

namespace Main.platform
{
    public class WeiXinCustomMessageContext:MessageContext<IRequestMessageBase,IResponseMessageBase>
    {
        public WeiXinCustomMessageContext()
        {
            base.MessageContextRemoved += CustomMessgeContext_MessageContextRemoved;
        }
        void CustomMessgeContext_MessageContextRemoved(object sender,WeixinContextRemovedEventArgs<IRequestMessageBase,IResponseMessageBase> e)
        {
            var messageContext = e.MessageContext as WeiXinCustomMessageContext;
            if (messageContext == null)
            {
                return;//如果是正常调用不会为null
            
            }
            //TODO:这里可根据需要执行消息过期时候的逻辑，如日志、消息通知等
        }
    }
}