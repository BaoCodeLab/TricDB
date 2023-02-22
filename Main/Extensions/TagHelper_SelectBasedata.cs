using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Main.Extensions
{

    /// <summary>
    /// 表单-基础数据选择器
    /// </summary>
    [HtmlTargetElement("basedata")]
    public class TagHelper_SelectBasedata : TagHelper
    {
        /// <summary>
        /// 获取View对应的ViewModel对象
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        /// <summary>
        /// 读取数据的远程地址
        /// </summary>
        [HtmlAttributeName("url")]
        public string Url { get; set; }


        /// <summary>
        /// 默认提示文字
        /// </summary>
        [HtmlAttributeName("placeholder")]
        public string PlaceHolder { get; set; } = "请点击选择";

        /// <summary>
        /// 样式
        /// </summary>
        [HtmlAttributeName("style")]
        public string Style { get; set; } = "";

        /// <summary>
        /// 初始值
        /// </summary>
        [HtmlAttributeName("value")]
        public string Value { get; set; } = "";

        /// <summary>
        /// 下拉框值字段
        /// </summary>
        [HtmlAttributeName("valuefield")]
        public string ValueField { get; set; }

        /// <summary>
        /// 下拉框名字段
        /// </summary>
        [HtmlAttributeName("namefield")]
        public string NameField { get; set; } = "ZH";


        /// <summary>
        /// 选中后的文字赋给的元素ID
        /// </summary>
        [HtmlAttributeName("text-for")]
        public string TextFor { get; set; }



        /// <summary>
        /// 级联表单中的字段
        /// </summary>
        [HtmlAttributeName("relatedfield")]
        public string RelatedField { get; set; }

        /// <summary>
        /// 级联表单中的字段对应本组件数据源中的字段
        /// </summary>
        [HtmlAttributeName("relatedproperty")]
        public string RelatedProperty { get; set; }

        /// <summary>
        /// 是否为远程检索，对于数据量较大的表如果全部到本地渲染后再搜索将导致浏览器崩溃，此时应开启远程检索
        /// </summary>
        [HtmlAttributeName("isremote")]
        public bool IsRemote { get; set; } = false;

        /// <summary>
        /// 远程检索模式下，定义检索数据的主键，将影响选中值
        /// </summary>
        [HtmlAttributeName("remote_checkedkey")]
        public string remoteCheckedKey { get; set; } = "GID";

        /// <summary>
        /// 远程检索模式下，定义检索数据的检索字段
        /// </summary>
        [HtmlAttributeName("remote_searchkey")]
        public string remoteSearchKey { get; set; } = "ZH";

        /// <summary>
        /// 选中数据的主键值赋给的元素ID
        /// </summary>
        [HtmlAttributeName("remote_checkedkey-for")]
        public string remoteCheckedKeyFor { get; set; }

        /// <summary>
        /// 快速选中，row为单击选中，rowDouble为双击选中,off为关闭快速选中，默认为单击选中，多选模式下自动off
        /// </summary>
        [HtmlAttributeName("remote_quickcheckmode")]
        public string remoteQuickCheckMode { get; set; } = "row";
        /// <summary>
        /// 远程数据调用的参数，对应layuiTable中的where属性，用于定义额外的检索参数。示例：HBLX:'供应商'
        /// </summary>
        [HtmlAttributeName("remote_where")]
        public string remoteTableWhere { get; set; }

        /// <summary>
        /// 远程数据多选
        /// </summary>
        [HtmlAttributeName("remote_multicheck")]
        public bool remoteMultiCheck { get; set; } = false;

        /// <summary>
        /// 远程数据模式下，是否显示搜索框，对于仅适用Keyup检索的场景，建议设置为false
        /// </summary>
        [HtmlAttributeName("remote_showSearchInput")]
        public bool showSearchInput { get; set; } = true;
        /// <summary>
        /// 输入框是否可修改，如果为false则只支持下拉选择
        /// </summary>
        [HtmlAttributeName("remote_editable")]
        public bool remoteEditable { get; set; } = true;
        /// <summary>
        /// 选中数据后的回调，为js代码，选中值为data，例如：$("#aaa").val(data.data[0].ZH)
        /// </summary>
        [HtmlAttributeName("remote_donefunc")]
        public string remoteDoneFunc { get; set; } = "";

        /// <summary>
        /// 远程数据在列表中显示的列，默认显示ZH中文、EN英文、JM简码，必须搭配remote_viewmodel属性使用。配置示例：ZH,EN,JM
        /// </summary>
        [HtmlAttributeName("remote_thfields")]
        public string remoteThFields { get; set; } = "ZH,EN,JM";

        /// <summary>
        /// 用于解析远程数据的ViewModel，示例：typeof('VM_DM_YH')
        /// </summary>
        [HtmlAttributeName("remote_viewmodel")]
        public Type remoteViewModel { get; set; }

        /// <summary>
        /// 视图对象
        /// </summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private IHtmlGenerator Generator { get; }
        public TagHelper_SelectBasedata(IHtmlGenerator generator)
        {
            Generator = generator;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string[] remoteTh = remoteThFields.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            if (remoteTh.Length > 0 && remoteViewModel != null)
            {
                StringBuilder sb = new StringBuilder();
                int curIndex = 0;
                foreach (var th in remoteTh)
                {
                    var Field = remoteViewModel.GetProperty(th);
                    DisplayAttribute display = (DisplayAttribute)Field.GetCustomAttribute(typeof(DisplayAttribute));
                    if (display != null)
                    {
                        sb.Append(string.Format("{{ field: \"{0}\", title: \"{1}\" }}", th, display.Name));
                    }
                    else
                    {
                        sb.Append(string.Format("{{ field: \"{0}\", title: \"{1}\" }}", th, "未命名"));

                    }
                    if (curIndex < remoteTh.Length)
                    {
                        sb.Append(",");
                    }
                    curIndex++;
                }
                remoteThFields = sb.ToString();
            }
            else
            {
                remoteThFields = "{ field: 'ZH', title: '中文' },{ field: 'EN', title: '英文' },{ field: 'JM', title: '简码' }";
            }
            string checkType = "radio";
            if (remoteMultiCheck == true)
            {
                checkType = "checkbox";
                remoteQuickCheckMode = "off";
            }
            var field = For.Metadata.ContainerType.GetProperty(For.Name);
            RequiredAttribute required = (RequiredAttribute)field.GetCustomAttribute(typeof(RequiredAttribute));
            DisplayAttribute name = (DisplayAttribute)field.GetCustomAttribute(typeof(DisplayAttribute));
            //html部分
            if (IsRemote == false)
            {
                output.TagName = "div";
                output.Attributes.Add("style", "display:inline");
                output.Attributes.Add("lay-filter", For.Name);
                output.Attributes.Add("class", "layui-form");
                TagBuilder select = new TagBuilder("select");
                select.Attributes.Add("name", For.Name);
                select.Attributes.Add("id", For.Name);
                select.Attributes.Add("lay-search", "");
                select.Attributes.Add("lay-filter", "f_" + For.Name);
                select.Attributes.Add("relatedfield", RelatedField);
                select.Attributes.Add("relatedproperty", RelatedProperty);
                select.Attributes.Add("url", Url);
                select.Attributes.Add("valuefield", ValueField);
                select.Attributes.Add("namefield", NameField);

                if (required != null)
                {
                    select.Attributes.Add("data-val", "true");
                    select.Attributes.Add("data-val-required", name.Name + "为必填字段");
                }
                TagBuilder input = new TagBuilder("input");
                input.Attributes.Add("name", For.Name);
                input.Attributes.Add("id", "val_" + For.Name);
                input.Attributes.Add("type", "hidden");
                TagBuilder defaultOption = new TagBuilder("option");
                defaultOption.Attributes.Add("value", "");
                defaultOption.InnerHtml.AppendHtml("");
                select.InnerHtml.AppendHtml(defaultOption);
                output.Content.AppendHtml(select);
                output.Content.AppendHtml(input);
            }
            else
            {
                output.TagName = "input";
                output.Attributes.Add("id", For.Name);
                output.Attributes.Add("name", For.Name);
                output.Attributes.Add("class", "layui-input valid");
                try
                {
                    //获取Model中的值，优先级低于设定的默认值，一般用于编辑状态
                    if (Value == "")
                    {
                        Value = For.ModelExplorer.Container.Model.GetType().GetProperty(For.Name).GetValue(For.ModelExplorer.Container.Model).ToString();
                    }
                }
                catch { }
                output.Attributes.Add("value", Value);
                output.Attributes.Add("placeholder", PlaceHolder);
                output.Attributes.Add("style", Style);
                output.Attributes.Add("type", "text");
                if (remoteEditable == false)
                {
                    output.Attributes.Add("readonly", "readonly");
                }
                if (required != null)
                {
                    output.Attributes.Add("data-val", "true");
                    output.Attributes.Add("data-val-required", name.Name + "为必填字段");
                }

            }
            //脚本部分
            TagBuilder script = new TagBuilder("script");
            script.Attributes.Add("type", "text/javascript");
            string str_script = "";
            if (IsRemote == false)
            {
                str_script = string.Format(@"
                    layui.use(['form'], function () {{
                           var form = layui.form;
                                $('#{3}').html('<option></option>')
                                if('{0}'==''){{
                                    $.ajax({{
                                        method: 'Get',
                                        data: {{'{0}': $('#{1}').val() }},
                                        url: '{2}',
                                        success: function(data) {{
                                            var eachcount=0;
                                            $.each(data, function(i, n) {{ eachcount++;
                                                var raw=encodeURIComponent(JSON.stringify(n));
                                            if(n.{4}=='{7}'){{
                                                 $('#{3}').append('<option selected data-raw='+raw+' value=' + n.{4} + '>' + n.{5} + '</option>');
                                            }}
                                            else{{
                                                 $('#{3}').append('<option data-raw='+raw+' value=' + n.{4} + '>' + n.{5} + '</option>');
                                            }}
                                               
                                                 if(eachcount>=data.length){{form.render('select','{3}');}}
                                            }});
                                        }},
                                        error: function(data) {{
                                            layer.alert(data.responseJSON.msg)
                                        }}
                                    }});
                                  }}
                                  else{{
                                      var {3}_Internal=setInterval(function(){{
                                        if($('#val_{3}').val()!=''){{
                                            $('#{3}').append('<option selected value=' + $('#val_{3}').val() + '>' + $('#val_{3}').val() + '</option>');
                                            form.render('select','{3}');
                                            clearInterval({3}_Internal);
                                        }}
                                      }},1000);
                                  }}
                            form.on('select(f_{3})', function(data){{
                                $('#{6}').val(data.elem[data.elem.selectedIndex].text);
                                $.each($('[relatedfield=""{3}""]'),function(i,n){{
                                    $.ajax({{
                                    method: 'Get',
                                    data: $(n).attr('relatedproperty')+'='+data.value,
                                    url: $(n).attr('url'),
                                    success: function(data) {{
                                        $(n).html('');
                                        var eachcount=0;
                                        $.each(data, function(i, d) {{
                                            eachcount++;
                                            if(d[$(n).attr('namefield')]=='{7}'){{
                                                $(n).append('<option selected value=' + d[$(n).attr('valuefield')] + '>' + d[$(n).attr('namefield')]  + '</option>');

                                            }}
                                            else{{
                                                $(n).append('<option value=' + d[$(n).attr('valuefield')] + '>' + d[$(n).attr('namefield')]  + '</option>');
                                            }}
                                            if(eachcount>=data.length){{form.render('select',$(n).attr('id'));}}
                                        }});
                                        
                                    }},
                                    error: function(data) {{
                                        layer.alert(data.responseJSON.msg)
                                    }}
                                  }});
                              }})
                            }});
                        }});", RelatedProperty, RelatedField, Url, For.Name, ValueField, NameField, TextFor, Value);
            }
            else
            {
                str_script = string.Format(@"
                    layui.use(['tableSelect','form','table'], function () {{
                         var form=layui.form,tableSelect = layui.tableSelect,table=layui.table;
                         tableSelect.render({{
                            elem: '#{0}'
                            , showSearchInput:{13}
                            , checkedKey: '{1}' //表格的唯一建值，非常重要，影响到选中状态 必填
                            , searchKey: '{2}'	//搜索输入框的name值 默认keyword
                            , searchPlaceholder: '关键词搜索'	//搜索输入框的提示文字 默认关键词搜索
                            , relateField:'{8}'
                            , relateProperty:'{9}'
                            , quickCheckMode:'{14}'
                            , table: {{	//定义表格参数，与LAYUI的TABLE模块一致，只是无需再定义表格elem
                                url: '{3}',
                                size:'sm',
                                where:{{{12}}},
                                height:230,
                                width:620,
                                limit:5,
                                cols: [[
                                    {{ type: '{6}' }},
                                    {7}
                                ]]
                            }}
                            , done: function (elem, data) {{
                                $.each($('[relatedfield=""{0}""]'),function(i,n){{
                                    $.ajax({{
                                    method: 'Get',
                                    data: {{searchfield:$(n).attr('relatedproperty'),searchword:data.data[0].{2}}},
                                    url: $(n).attr('url'),
                                    success: function(rd) {{
                                        $(n).html('');
                                        var eachcount=0;
                                        $.each(rd.data, function(i, d) {{
                                            eachcount++;
                                            $(n).append('<option value=' + d[$(n).attr('valuefield')] + '>' + d[$(n).attr('namefield')]  + '</option>');
                                            if(eachcount>=rd.data.length){{
                                                form.render('select',$(n).attr('id'));
                                            }}
                                        }});
                                        
                                    }},
                                    error: function(data) {{
                                        layer.alert(data.responseJSON.msg)
                                    }}
                                  }});
                                }});
                                if({11}==true){{
                                    var keys=[],values=[];
                                    $.each(data.data,function(i,n){{
                                       keys.push(n.{1});
                                       values.push(n.{10});
                                    }});
                                    $('#{4}').val(keys.join(','));
                                    $('#{0}').val(values.join(','));
                                }}
                                else{{
                                    $('#{4}').val(data.data[0].{1});
                                    $('#{0}').val(data.data[0].{10});
                                }}
                                {5}
                            }}
                        }})
                    }});
                ", For.Name, remoteCheckedKey, remoteSearchKey, Url, remoteCheckedKeyFor, remoteDoneFunc, checkType, remoteThFields, RelatedField, RelatedProperty, NameField, remoteMultiCheck.ToString().ToLower(), remoteTableWhere, showSearchInput.ToString().ToLower(), remoteQuickCheckMode);
            }
            script.InnerHtml.AppendHtml(str_script);
            output.PostElement.AppendHtml(script);
        }
    }
}
