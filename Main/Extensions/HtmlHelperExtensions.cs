using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;

//扩展HtmlHelper条件：
//1、命名空间为 Microsoft.AspNetCore.Mvc.Rendering
//2、静态类静态方法
//3、静态方法的第一个参数是this IHtmlHelper
namespace Microsoft.AspNetCore.Mvc.Rendering
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// 后端Model对象转前端JSON对象（字符串）
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static HtmlString HtmlConvertToJson(this IHtmlHelper htmlHelper, object model)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };
            return new HtmlString(JsonConvert.SerializeObject(model, settings));
        }
    }

}
