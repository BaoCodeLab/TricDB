using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.platform.weixin
{
    public class TemplateMessage_BusinessStateChangeNotice : TemplateMessageBase
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem keyword1 { get; set; }
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem remark { get; set; }

        /// <summary>
        /// 业务状态变更通知
        /// </summary>
        /// <param name="_first">首</param>
        /// <param name="_remark">尾</param>
        /// <param name="_changeType">内容：变更类型</param>
        /// <param name="_changeResult">内容：变更结果</param>
        /// <param name="templateId">模板编号，详见微信公众号-模板消息</param>
        /// <param name="url">详情链接地址</param>
        public TemplateMessage_BusinessStateChangeNotice(string _first,string _remark,string _changeType,string _changeResult, string url = null,string templateId= "dwgfHP2WKQOUSii_9rRwu-R0kKsv86iQ5bvuO_MVnVU"):base(templateId,url, "业务状态变更通知")
        {
            first = new TemplateDataItem(_first);
            keyword1 = new TemplateDataItem(_changeType);
            keyword2 = new TemplateDataItem(_changeResult,"#ff0000");
            remark = new TemplateDataItem(_remark);
        }
    }
}
