using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel.DataAnnotations;
using Main.ViewModels;
using System.Reflection;
using TDSCoreLib;
using System.Text.Encodings.Web;
using System;
namespace Main.Extensions
{

    /// <summary>
    /// 高级检索
    /// </summary>
    [HtmlTargetElement("report")]
    public class TagHelper_AdvancedSearch: TagHelper
    {
        /// <summary>
        /// 获取View对应的ViewModel对象
        /// </summary>
        [HtmlAttributeName("viewmodel")]
        public string viewModelName { get; set; }
        /// <summary>
        /// Model对象
        /// </summary>
        [HtmlAttributeName("model")]
        public string modelName { get; set; }

        /// <summary>
        /// 报表标题（名称）
        /// </summary>
        [HtmlAttributeName("title")]
        public string title { get; set; }
        /// <summary>
        /// 获取View对应的ViewModel对象
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression ME { get; set; }

        /// <summary>
        /// 表单Action
        /// </summary>
        [HtmlAttributeName("action")]
        public string action { get; set; }
        

        /// <summary>
        /// 报表controller调用地址：默认为ReportController
        /// </summary>
        [HtmlAttributeName("exporturl")]
        public string exporturl { get; set; }

        /// <summary>
        /// 远程API地址:Controller
        /// </summary>
        [HtmlAttributeName("c")]
        public string c { get; set; }

        /// <summary>
        /// 远程API地址:Action
        /// </summary>
        [HtmlAttributeName("a")]
        public string a { get; set; }

        /// <summary>
        /// 检索结果加载的容器
        /// </summary>
        [HtmlAttributeName("for")]
        public string tableFilter { get; set; }

        /// <summary>
        /// 视图对象
        /// </summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private IHtmlGenerator Generator { get; }
        private readonly IHtmlHelper<VM_Report> _htmlHelper;

        public TagHelper_AdvancedSearch(IHtmlGenerator generator, IHtmlHelper<VM_Report> htmlHelper)
        {
            Generator = generator;
            _htmlHelper = htmlHelper;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //html部分
            output.TagName = "div";
            output.Attributes.Add("class", "layui-collapse");
            var form_layui_form = new TagBuilder("form");
            form_layui_form.AddCssClass("layui-form");
            form_layui_form.Attributes.Add("lay-filter", "advsearch");
            form_layui_form.Attributes.Add("action", action);
            form_layui_form.Attributes.Add("method", "post");
            form_layui_form.GenerateId("advsearch", "");
            var div_layui_colla_item = new TagBuilder("div"); div_layui_colla_item.AddCssClass("layui-colla-item");
            var h2_layui_colla_title = new TagBuilder("h2"); h2_layui_colla_title.AddCssClass("layui-colla-title"); h2_layui_colla_title.InnerHtml.AppendHtml("高级检索");
            var div_layui_colla_content = new TagBuilder("div"); div_layui_colla_content.AddCssClass("layui-colla-content layui-show");
            var div_layui_row = new TagBuilder("div"); div_layui_row.AddCssClass("layui-row");
            form_layui_form.InnerHtml.AppendHtml(div_layui_colla_item);
            div_layui_colla_item.InnerHtml.AppendHtml(h2_layui_colla_title);
            div_layui_colla_item.InnerHtml.AppendHtml(div_layui_colla_content);
            div_layui_colla_content.InnerHtml.AppendHtml(div_layui_row);
            var hidden_ModelName = new TagBuilder("input");
            var hidden_ViewModelName = new TagBuilder("input");
            var hidden_RemoteController = new TagBuilder("input");
            var hidden_RemoteAction = new TagBuilder("input");
            hidden_ModelName.Attributes.Add("name","model");
            hidden_ModelName.Attributes.Add("value", modelName);
            hidden_ModelName.Attributes.Add("type", "hidden");
            hidden_RemoteController.Attributes.Add("name", "c");
            hidden_RemoteController.Attributes.Add("type", "hidden");
            hidden_RemoteController.Attributes.Add("value", c);
            hidden_RemoteAction.Attributes.Add("name", "a");
            hidden_RemoteAction.Attributes.Add("type", "hidden");
            hidden_RemoteAction.Attributes.Add("value", a);
            hidden_ViewModelName.Attributes.Add("name", "viewmodel");
            hidden_ViewModelName.Attributes.Add("value", viewModelName);
            hidden_ViewModelName.Attributes.Add("type", "hidden");
            form_layui_form.InnerHtml.AppendHtml(hidden_ModelName);
            form_layui_form.InnerHtml.AppendHtml(hidden_ViewModelName);
            form_layui_form.InnerHtml.AppendHtml(hidden_RemoteController);
            form_layui_form.InnerHtml.AppendHtml(hidden_RemoteAction);
            Type t = Assembly.Load(new AssemblyName("Main")).GetType("Main.ViewModels."+viewModelName);
            object ViewModel = Activator.CreateInstance(t);
            PropertyInfo[] pi = t.GetProperties();
            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetGetMethod();
                if (m != null && m.IsPublic)
                {
                    DisplayAttribute display = (DisplayAttribute)p.GetCustomAttribute(typeof(DisplayAttribute));
                    enableSearchAttribute enableSearch = (enableSearchAttribute)p.GetCustomAttribute(typeof(enableSearchAttribute));
                    if (enableSearch != null)
                    {
                        var div1 = new TagBuilder("div"); div1.AddCssClass("layui-col-md3 layui-col-sm12");
                        var div2 = new TagBuilder("div"); div2.AddCssClass("layui-fluid");
                        var ep = ME.ModelExplorer.GetExplorerForProperty(p.Name, ViewModel);
                        var label = Generator.GenerateLabel(ViewContext, ep, null, display.Name, null);
                        label.AddCssClass("layui-form-label");
                        var divINPUT = new TagBuilder("div");
                        divINPUT.AddCssClass("layui-input-block");
                        //根据类型渲染组件
                        UIHintAttribute UIHint = (UIHintAttribute)p.GetCustomAttribute(typeof(UIHintAttribute));
                        if (UIHint != null)
                        {
                            var genericListType = typeof(IHtmlHelper<>);
                            var specificListType = genericListType.MakeGenericType(t);
                            var htmlHelper = specificListType.GetInterfaces()[0];
                            VM_Report CVM = new VM_Report();
                            var writer = new System.IO.StringWriter();
                            ((IViewContextAware)_htmlHelper).Contextualize(ViewContext);
                            if (UIHint.UIHint.ToLower() == "select")
                            {
                                _htmlHelper.ViewContext.ViewData["list"] = new { list = StateHelper.getStates(UIHint.PresentationLayer) };
                                _htmlHelper.EditorFor(model => CVM.select, new { list = StateHelper.getStates(UIHint.PresentationLayer) }).WriteTo(writer, HtmlEncoder.Default);
                            }
                            else if (UIHint.UIHint.ToLower() == "check")
                            {
                                _htmlHelper.ViewContext.ViewData["list"] = new { list = StateHelper.getStates(UIHint.PresentationLayer) };
                                _htmlHelper.EditorFor(model => CVM.check, new { list = StateHelper.getStates(UIHint.PresentationLayer) }).WriteTo(writer, HtmlEncoder.Default);
                                div1.Attributes.Remove("class");
                                div1.AddCssClass("layui-col-md12 layui-col-sm12");
                            }
                            else if (UIHint.UIHint.ToLower() == "radio")
                            {
                                _htmlHelper.ViewContext.ViewData["list"] = new { list = StateHelper.getStates(UIHint.PresentationLayer) };
                                _htmlHelper.EditorFor(model => CVM.radio, new { list = StateHelper.getStates(UIHint.PresentationLayer) }).WriteTo(writer, HtmlEncoder.Default);
                            }
                            else if (UIHint.UIHint.ToLower() == "date")
                            {
                                _htmlHelper.ViewContext.ViewData["range"] = UIHint.PresentationLayer.ToLower();
                                _htmlHelper.EditorFor(model => CVM.date).WriteTo(writer, HtmlEncoder.Default);
                            }
                            else if (UIHint.UIHint.ToLower() == "org")
                            {
                                _htmlHelper.EditorFor(model => CVM.org).WriteTo(writer, HtmlEncoder.Default);
                            }
                            divINPUT.InnerHtml.AppendHtml(writer.ToString().Replace("CVM." + UIHint.UIHint.ToLower(), p.Name));
                        }
                        else
                        {
                            var input = Generator.GenerateTextBox(ViewContext, ep, p.Name, null, null, new { @class = "layui-input", id = p.Name });
                            divINPUT.InnerHtml.AppendHtml(input);
                        }
                        div2.InnerHtml.AppendHtml(divINPUT);
                        div1.InnerHtml.AppendHtml(label);
                        div1.InnerHtml.AppendHtml(div2);
                        div_layui_row.InnerHtml.AppendHtml(div1);
                    }
                }
            }
            var form_btn_div = new TagBuilder("div"); form_btn_div.AddCssClass("layui-col-md3 layui-col-sm12 layui-btn-container"); form_btn_div.Attributes.Add("style", "padding-top:15px;");
            var form_submit_button = new TagBuilder("a"); form_submit_button.AddCssClass("layui-btn layui-btn-sm layui-bg-green"); form_submit_button.GenerateId("dosearch", ""); form_submit_button.Attributes.Add("data-type", "search"); form_submit_button.InnerHtml.AppendHtml("检索");
            var form_export_button = new TagBuilder("a"); form_export_button.AddCssClass("layui-btn layui-btn-sm layui-bg-blue"); form_export_button.GenerateId("doexport", ""); form_export_button.Attributes.Add("data-type", "export"); form_export_button.InnerHtml.AppendHtml("下载");
            var form_refresh_button = new TagBuilder("a"); form_refresh_button.AddCssClass("layui-btn layui-btn-sm layui-bg-orange"); form_refresh_button.GenerateId("dorefresh", ""); form_refresh_button.Attributes.Add("data-type", "refresh"); form_refresh_button.InnerHtml.AppendHtml("刷新");
            var form_clear_button = new TagBuilder("a"); form_clear_button.AddCssClass("layui-btn layui-btn-sm layui-bg-red"); form_clear_button.GenerateId("doclear", ""); form_clear_button.Attributes.Add("data-type", "clear"); form_clear_button.InnerHtml.AppendHtml("清空");
            form_btn_div.InnerHtml.AppendHtml(form_submit_button);
            form_btn_div.InnerHtml.AppendHtml(form_export_button);
            form_btn_div.InnerHtml.AppendHtml(form_refresh_button);
            form_btn_div.InnerHtml.AppendHtml(form_clear_button);
            div_layui_row.InnerHtml.AppendHtml(form_btn_div);
            //表单：导出文件
            var form_export_form = new TagBuilder("form");
            form_export_form.Attributes.Add("action", exporturl);
            form_export_form.Attributes.Add("method", "post");
            form_export_form.GenerateId("form_export", "");
            var hidden_action = new TagBuilder("input");
            hidden_action.Attributes.Add("name", "action");
            hidden_action.Attributes.Add("value", action);
            hidden_action.Attributes.Add("type", "hidden");
            hidden_action.Attributes.Add("id", "export_action");
            var hidden_viewmodel = new TagBuilder("input");
            hidden_viewmodel.Attributes.Add("name", "viewmodel");
            hidden_viewmodel.Attributes.Add("value", viewModelName);
            hidden_viewmodel.Attributes.Add("type", "hidden");
            var hidden_params = new TagBuilder("input");
            hidden_params.Attributes.Add("name", "params");
            hidden_params.Attributes.Add("value", "");
            hidden_params.Attributes.Add("type", "hidden");
            hidden_params.Attributes.Add("id", "export_params");
           
            var hidden_title = new TagBuilder("input");
            hidden_title.Attributes.Add("name", "title");
            hidden_title.Attributes.Add("value", title);
            hidden_title.Attributes.Add("type", "hidden");
            form_export_form.InnerHtml.AppendHtml(hidden_action);
            form_export_form.InnerHtml.AppendHtml(hidden_params);
            form_export_form.InnerHtml.AppendHtml(hidden_title);
            form_export_form.InnerHtml.AppendHtml(hidden_ViewModelName);
            //
            output.Content.AppendHtml(form_layui_form);
            output.Content.AppendHtml(form_export_form);

            //脚本部分
            var script = new TagBuilder("script");
            script.Attributes.Add("type", "text/javascript");
            string str_script = string.Format(@"layui.use(['form', 'element', 'table'], function () {{
                            var form = layui.form,  table = layui.table;
                            form.render(null, 'advsearch');
                            var tableFilter='{0}';

                            table.on('sort({0})', function (obj) {{
                                table.reload(tableFilter, {{
                                    initSort: obj
                                    , where: {{
                                        field: obj.field
                                      , order: obj.type
                                    }}
                                }});
                            }});

                            var active = {{
                             refresh: function () {{
                                            table.reload(tableFilter, {{ where: [] }});
                                        }},
                             search: function () {{
                                            if ($('#advsearch').valid()) {{
                                                table.reload(tableFilter, {{
                                                url:'{1}',
                                                loading:true,
                                                method:'post',
                                                where: $('#advsearch').serializeJSON()
                                                    , page: {{ curr: 1 }}
                                                }});
                                            }}
                                        }},
                            export: function () {{
                                            $('#export_params').val(encodeURIComponent($('#advsearch').serialize()));
                                            $('#form_export').submit();
                                        }},
                            clear:function(){{
                                            $('#advsearch input:not([type=hidden],[type=button],[type=submit],[type=reset])').val('').prop('checked',false).removeAttr('checked').remove('selected');
                                        }}
                            }};
                             $('a[data-type]').click(function () {{
                                        var type = $(this).data('type');
                                        active[type] && active[type].call(this);
                                    }});
                            }});", tableFilter, action);
            script.InnerHtml.AppendHtml(str_script);
            output.PostElement.AppendHtml(script);
        }
    }
}
