using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
namespace Main.Extensions
{
    /// <summary>
    /// 按模板导出打印，PF_PRINT_TMPL - Upload
    /// </summary>
    [HtmlTargetElement("print")]
    public class TagHelper_PrintTmpl : TagHelper
    {
        /// <summary>
        /// 打印模板code，PF_PRINT_TMPL.CODE
        /// </summary>
        [HtmlAttributeName("code")]
        public string code { get; set; }

        /// <summary>
        /// 模板抬头数据使用的ViewModel
        /// </summary>
        [HtmlAttributeName("viewmodel_head")]
        public string viewmodel_head { get; set; }
        /// <summary>
        /// 模板列表数据使用的ViewModel
        /// </summary>
        [HtmlAttributeName("viewmodel_body")]
        public string viewmodel_body { get; set; }
        /// <summary>
        /// 模板抬头数据API接口调用地址（Get方式）
        /// </summary>
        [HtmlAttributeName("head_data_url")]
        public string head_data_url { get; set; }

        /// <summary>
        /// 模板列表数据API接口调用地址（Get方式）
        /// </summary>
        [HtmlAttributeName("body_data_url")]
        public string body_data_url { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.Add("class", "layui-btn layui-btn-sm");
            output.Attributes.Add("id", "do_print");
            var layui_icon = new TagBuilder("i");
            layui_icon.AddCssClass("layui-icon layui-icon-file-b");
            var hd_code = new TagBuilder("input");
            var hd_viewmodel_head = new TagBuilder("input");
            var hd_viewmodel_body = new TagBuilder("input");
            var hd_head_data_url = new TagBuilder("input");
            var hd_body_data_url = new TagBuilder("input");
            hd_code.Attributes.Add("type","hidden");
            hd_viewmodel_head.Attributes.Add("type", "hidden");
            hd_viewmodel_body.Attributes.Add("type", "hidden");
            hd_head_data_url.Attributes.Add("type", "hidden");
            hd_body_data_url.Attributes.Add("type", "hidden");
            hd_code.Attributes.Add("id", "p_code");
            hd_viewmodel_head.Attributes.Add("id", "p_viewmodel_head");
            hd_viewmodel_body.Attributes.Add("id", "p_viewmodel_body");
            hd_head_data_url.Attributes.Add("id", "p_head_data_url");
            hd_body_data_url.Attributes.Add("id", "p_body_data_url");
            hd_code.Attributes.Add("value", code);
            hd_viewmodel_head.Attributes.Add("value", viewmodel_head);
            hd_viewmodel_body.Attributes.Add("value", viewmodel_body);
            hd_head_data_url.Attributes.Add("value", head_data_url);
            hd_body_data_url.Attributes.Add("value", body_data_url);
            
            //组装html
            output.Content.AppendHtml(layui_icon);
            output.Content.AppendHtml("生成文档");
            output.Content.AppendHtml(hd_code);
            output.Content.AppendHtml(hd_viewmodel_head);
            output.Content.AppendHtml(hd_viewmodel_body);
            output.Content.AppendHtml(hd_head_data_url);
            output.Content.AppendHtml(hd_body_data_url);
            //脚本部分
            var script = new TagBuilder("script");
            script.Attributes.Add("type", "text/javascript");
            string str_script = @"
                $(""#do_print"").on('click', function () {

                       $.post('/print_tmpl/showexport',
                         {
                            code: $(""#p_code"").val(),
                            viewmodel_head: $(""#p_viewmodel_head"").val(),
                            viewmodel_body: $(""#p_viewmodel_body"").val(),
                            head_data_url: $(""#p_head_data_url"").val(),
                            body_data_url: $(""#p_body_data_url"").val(),
                          },
                        layerIndex = function(str) {
                            layer.open({
                                type: 1,
                                title: '导出数据',
                                content: str,
                                area: ['100%', '100%'],
                                cancel: function(index){
                                    layer.close(index);
                                }
                                 });
                            });
                });";
            script.InnerHtml.AppendHtml(str_script);
            output.PostElement.AppendHtml(script);
        }
    }
}
