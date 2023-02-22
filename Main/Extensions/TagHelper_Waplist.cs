using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel.DataAnnotations;
using Main.ViewModels;
using System.Reflection;
using TDSCoreLib;
using System.Text.Encodings.Web;
using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using HtmlAgilityPack;
using System.ComponentModel;

namespace Main.Extensions
{

    /// <summary>
    /// 移动端面板式数据列表，支持滑动加载数据，数据源支持远程API或数据对象。在<waplist></waplist>中填写每项数据的模板即可，数据占位符通过{{item.数据字段}}。详细使用规则参考Layui.LayTpl组件文档
    /// 如果模板有公用头部/尾部的情况（如表格表头），则以<template>内的内容作为模板
    /// </summary>
    [HtmlTargetElement("waplist"), Description("移动端面板式数据列表")]
    public class TagHelper_WapList : TagHelper
    {
        /// <summary>
        /// 列表绑定的视图模型类型
        /// </summary>
        [HtmlAttributeName("ViewModel")]
        public Type ViewModel { get; set; }
        /// <summary>
        /// 调用远程API的地址
        /// </summary>
        [HtmlAttributeName("Url")]
        public string Url { get; set; }

        /// <summary>
        /// 模板外围容器的类型，默认为div
        /// </summary>
        [HtmlAttributeName("OutputTagName")]
        public string OutputTagName { get; set; } = "div";


        /// <summary>
        /// 调用远程API的Action名称
        /// </summary>
        [HtmlAttributeName("Action")]
        public string ActionName { get; set; }


        /// <summary>
        /// IEumerable数据对象，远程API调用模式下无需提供
        /// </summary>
        [HtmlAttributeName("DataObject")]
        public IEnumerable DataObject { get; set; }

        /// <summary>
        /// 调用远程API的Controller名称
        /// </summary>
        [HtmlAttributeName("Controller")]
        public string ControllerName { get; set; }

        /// <summary>
        /// 绑定远程调用返回值中的数据列对象，例如返回值为{code:0,count:100,data:[{},{},...]}，则此处应填d.data
        /// </summary>
        [HtmlAttributeName("BindDataList")]
        public string BindDataList { get; set; } = "d.data";

        /// <summary>
        /// 拖动到底后是自动加载数据（默认），还是显示一个按钮让用户点击。
        /// </summary>
        [HtmlAttributeName("IsAuto")]
        public bool IsAuto { get; set; } = true;


        /// <summary>
        /// 拖到至全部数据加载完毕后显示的内容，默认为“没有更多了”
        /// </summary>
        [HtmlAttributeName("End")]
        public string End { get; set; } = "没有更多了";

        /// <summary>
        /// 每次加载的数据条数（需数据源支持分页）
        /// </summary>
        [HtmlAttributeName("Limit")]
        public int Limit { get; set; } = 10;

        /// <summary>
        /// 是否显示搜索栏和刷新按钮（默认显示）
        /// </summary>
        [HtmlAttributeName("ShowSearchAndReload")]
        public bool ShowSearchAndReload { get; set; } = true;

        /// <summary>
        /// 视图对象
        /// </summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeNotBound]
        public IUrlHelperFactory UrlHelperFactory { get; }
        public TagHelper_WapList(IUrlHelperFactory urlHelperFactory)
        {
            UrlHelperFactory = urlHelperFactory;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            try
            {
                var GenUrl = UrlHelperFactory.GetUrlHelper(ViewContext);
                string genID = "_" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
                string id = genID;
                //html部分
                output.TagName = OutputTagName;
                var layTmpl = new TagBuilder("script");
                layTmpl.Attributes.Add("type", "text/html");
                layTmpl.Attributes.Add("id", $"layTmpl_{genID}");
                string layTmplInnerHtml = output.GetChildContentAsync().Result.GetContent();
                //考虑到模板可能有公用头部/尾部的情况，如果模板中存在<template>标签，则以<template>内的内容作为模板，而非全部
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(layTmplInnerHtml);
                HtmlNode rootNode = doc.DocumentNode;
                HtmlNodeCollection tpls = rootNode.SelectNodes("//template");
                if (tpls != null)
                {
                    string template = tpls.First().InnerHtml;
                    HtmlNode templateParentNode = tpls.First().ParentNode;
                    layTmpl.InnerHtml.AppendHtml(
                       $" {{{{#  layui.each({BindDataList}, function(index, item){{ }}}}{template}{{{{#  }}); }}}}"
                    );
                    tpls.First().Remove();
                    templateParentNode.SetAttributeValue("id", id);
                    output.Content.AppendHtml(rootNode.InnerHtml);
                }
                else
                {
                    layTmpl.InnerHtml.AppendHtml(
                        new HtmlString($" {{{{#  layui.each({BindDataList}, function(index, item){{ }}}}{layTmplInnerHtml}{{{{#  }}); }}}}")
                        );
                    if (output.Attributes["id"] != null)
                    {
                        id = "list" + output.Attributes["id"].Value.ToString();
                    }
                    else
                    {
                        output.Attributes.Add("id", id);
                    }
                    output.Content.Clear();//清除原有内部的模板代码
                }
                output.PostElement.AppendHtml(layTmpl);

                if (string.IsNullOrEmpty(Url))
                {
                    if (!string.IsNullOrEmpty(ActionName) && !string.IsNullOrEmpty(ControllerName))
                    {
                        Url = GenUrl.Action(ActionName, ControllerName);
                    }
                    else
                    {
                        output.Content.Clear();
                        output.Content.AppendHtml(new HtmlString("<h3 style='color:red'>未提供Url参数</h3>"));
                    }
                }
                if (IsAuto)
                {
                    output.PostElement.AppendHtml(new HtmlString("<style type='text/css'>.layui-flow-more{display:none!important}</style>"));
                }
                //脚本部分
                var script = new TagBuilder("script");
                script.Attributes.Add("type", "text/javascript");
                string str_script = string.Format(@"layui.use(['laytpl', 'flow'], function () {{
                            var laytpl = layui.laytpl,  flow = layui.flow;
                             var currentPage=0;
                             window.loadFlowList_{4}=function(isReload,searchField,searchWord){{
                                $(document).off('scroll');
                                flow.load({{
                                    elem: '#{0}'
                                    , end: '{1}'
                                    , mb: 20
                                    , done: function (page, next) {{
                                        if(isReload){{
                                            $('#{0}').html('');
                                            page=1; 
                                        }}
                                        $.get('{2}', {{ page: page, limit: {3},searchfield:searchField,searchword: searchWord}}, function (data) {{
                                            page=page;isReload=false;
                                            if(currentPage!=page){{
                                            laytpl(layTmpl_{4}.innerHTML).render(data, function(html) {{
                                               $('#{0}').append(html);
                                            }});}}
                                            currentPage++;
                                            next('下一页', page<Math.ceil(data.count / {3}));
                                      }});
                                    }}
                                }});
                            }}
                            loadFlowList_{4}(false,'','');
            }});", id, End, Url, Limit, genID);
                script.InnerHtml.AppendHtml(str_script);
                output.PostElement.AppendHtml(script);
                if (ShowSearchAndReload)
                {
                    string searchOptions = string.Join('\n', LayUIHelper.ObjectToLayUISeachField(Activator.CreateInstance(ViewModel)).Select(s => $"<option value='{s.Key}'>{s.Value}</option>").ToArray());
                    //添加刷新和检索框
                    output.PreElement.AppendHtml(new HtmlString(string.Format(@" 
        <div class='layui-row'>
            <div class='layui-col-md7 layui-col-sm12'>
                 <form class='layui-form'  id='searchform{3}'>
                    <div class='layui-row'>
                        <div class='layui-col-md3 layui-col-sm3 layui-col-xs12 layui-mb10'>
                            <select name='searchfield' id='searchfield{3}'>
                               {2}
                            </select>
                        </div>
                        <div class='layui-col-md6 layui-col-sm6 layui-col-xs12 layui-mb10'>
                            <input type='text' id='searchword{3}' class='layui-input' placeholder='输入检索关键词' data-val='true' data-val-required='请填写检索词' name='searchword' />
                        </div>
                        <div class='layui-col-md3 layui-col-sm3 layui-col-xs12 layui-btn-container'>
                            <button type='button' class='layui-btn layui-btn-normal' data-type='search'><i class='layui-icon layui-icon-search'></i>检索</button>
                            <button type='button' class='layui-btn layui-bg-gray' data-type='refresh'><i class='layui-icon layui-icon-refresh'></i>刷新</button>
                        </div>
                    
                    </div>
                 </form>
            </div>
        </div>
        <script type='text/javascript'>
        var active = {{
            refresh: function () {{
                loadFlowList_{3}(true,'','');
            }}
            , search: function () {{
                    if ($('#searchform{3}').valid()) {{
                        loadFlowList_{3}(true,$('#searchfield{3}').val(),$('#searchword{3}').val());
                    }}
                }}
            }};
            $('button[data-type]').click(function () {{
                var type = $(this).data('type');
                active[type] && active[type].call(this);
            }});
        </script>", ActionName, ControllerName, searchOptions, genID)));
                }
            }
            catch (Exception ex)
            {
                output.Content.Clear();
                output.Content.AppendHtml(new HtmlString(ex.ToString()));
            }
        }
    }
}