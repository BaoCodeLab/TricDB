using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
namespace Main.Extensions
{
    /// <summary>
    /// 文件上传，接口详见API_PF_FILE - Upload
    /// </summary>
    [HtmlTargetElement("fileupload")]
    public class TagHelper_FileUpload : TagHelper
    {
        /// <summary>
        /// 外键GID，即要挂载文件的主文档GID，如合同、订单
        /// </summary>
        [HtmlAttributeName("WGID")]
        public string WGID { get; set; }

        /// <summary>
        /// 附件类型，如：合同、模板等，可在PF_STATE中维护
        /// </summary>
        [HtmlAttributeName("TYPE")]
        public string TYPE { get; set; }
        /// <summary>
        /// 远程API地址
        /// </summary>
        [HtmlAttributeName("API")]
        public string API { get; set; }
        /// <summary>
        /// 是否支持多文件：true/false
        /// </summary>
        [HtmlAttributeName("MULTIPLE")]
        public string MULTIPLE { get; set; }

        /// <summary>
        /// 文件上传后产生的GID赋值给哪个元素，如要赋值给<input id='FGID'></input>，则填FGID
        /// </summary>
        [HtmlAttributeName("FOR")]
        public string FOR { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "layui-card");
            output.Attributes.Add("style", "margin-bottom:0");
            var layui_card_body = new TagBuilder("div");
            layui_card_body.AddCssClass("layui-card-body");
            var layui_upload = new TagBuilder("div");
            layui_upload.AddCssClass("layui-upload");
            var layui_btn_selectfile = new TagBuilder("button");
            layui_btn_selectfile.AddCssClass("layui-btn layui-btn-sm layui-btn-normal");
            layui_btn_selectfile.Attributes.Add("id", "testList");
            layui_btn_selectfile.Attributes.Add("type", "button");
            layui_btn_selectfile.InnerHtml.Append("选择文件");
            var layui_upload_list = new TagBuilder("div");
            layui_upload_list.AddCssClass("layui-upload-list");
            var layui_table = new TagBuilder("table");
            layui_table.AddCssClass("layui-table");
            layui_table.Attributes.Add("lay-size", "sm");
            var thead = new TagBuilder("thead");
            var tr = new TagBuilder("tr");
            var th_1 = new TagBuilder("th");
            th_1.InnerHtml.AppendHtml("文件名");
            var th_2 = new TagBuilder("th");
            th_2.InnerHtml.AppendHtml("大小");
            var th_3 = new TagBuilder("th");
            th_3.InnerHtml.AppendHtml("状态");
            var th_4 = new TagBuilder("th");
            th_4.InnerHtml.AppendHtml("操作");
            var tbody = new TagBuilder("tbody");
            tbody.Attributes.Add("id", "demoList");
            var layui_btn_upload = new TagBuilder("button");
            layui_btn_upload.AddCssClass("layui-btn layui-btn-sm layui-btn-normal");
            layui_btn_upload.Attributes.Add("id", "testListAction");
            layui_btn_upload.Attributes.Add("type", "button");
            layui_btn_upload.InnerHtml.Append("开始上传");
            var layui_btn_container = new TagBuilder("div");
            layui_btn_container.AddCssClass("layui-btn-container");
            //组装html
            tr.InnerHtml.AppendHtml(th_1);
            tr.InnerHtml.AppendHtml(th_2);
            tr.InnerHtml.AppendHtml(th_3);
            tr.InnerHtml.AppendHtml(th_4);
            thead.InnerHtml.AppendHtml(tr);
            layui_table.InnerHtml.AppendHtml(thead);
            layui_table.InnerHtml.AppendHtml(tbody);
            layui_upload_list.InnerHtml.AppendHtml(layui_table);
            layui_btn_container.InnerHtml.AppendHtml(layui_btn_selectfile);
            layui_btn_container.InnerHtml.AppendHtml(layui_btn_upload);
            layui_upload.InnerHtml.AppendHtml(layui_btn_container);
            layui_upload.InnerHtml.AppendHtml(layui_upload_list);
            layui_card_body.InnerHtml.AppendHtml(layui_upload);
            output.Content.AppendHtml(layui_card_body);
            //配置
            string acceptFileMime = AppConfigurtaionServices.Configuration["AppSettings:acceptFileMime"];
            string acceptFileExt = AppConfigurtaionServices.Configuration["AppSettings:acceptFileExt"];
            //脚本部分
            var script = new TagBuilder("script");
            script.Attributes.Add("type", "text/javascript");
            string str_script = string.Format(@"
        layui.use(['upload'], function () {{    
        var upload = layui.upload;
        var demoListView = $('#demoList');
        upload.render({{
            elem: '#testList', //绑定元素
            method: 'POST',
            url: '{0}',
            auto: false,
            data: {{
            WGID: '{1}',
            LX:'{2}'
            }},
            accept: 'file',
            acceptMime: '{3}',
            exts: '{4}',
            multiple: {5},
            bindAction: '#testListAction',
            choose: function(obj) {{
                var files = this.files = obj.pushFile(); //将每次选择的文件追加到文件队列
                //读取本地文件
                obj.preview(function(index, file, result) {{
                    var tr = $(['<tr id=""upload-' + index + '"">'
                     , '<td>' + file.name + '</td>'
                     , '<td>' + (file.size / 1014).toFixed(1) + 'kb</td>'
                     , '<td>等待上传</td>'
                     , '<td>'
                       , '<button class=""layui-btn layui-btn-xs demo-reload layui-hide"">重传</button>'
                       , '<button class=""layui-btn layui-btn-xs layui-btn-danger demo-delete"">删除</button>'
                     , '</td>'
                   , '</tr>'].join(''));
            //单个重传
            tr.find('.demo-reload').on('click', function() {{
                obj.upload(index, file);
            }});
            //删除
            tr.find('.demo-delete').on('click', function() {{
                delete files[index]; //删除对应的文件
                tr.remove();
                uploadListIns.config.elem.next()[0].value = ''; //清空 input file 值，以免删除后出现同名文件不可选
            }});

            demoListView.append(tr);
        }});
            }},
            //单文件上传不会执行allDone方法
            allDone: function(obj)
            {{ //当文件全部被提交后，才触发
                layer.msg('文件全部上传成功');
                if (fileflag)
                {{
                    resetFormButton();
                }}
            }},
                    done: function(res, index, upload)
            {{
                layer.msg('文件上传成功');
                if (res.code == 0)
                {{ //上传成功
                    var tr = demoListView.find('tr#upload-' + index)
                    , tds = tr.children();
                    tds.eq(2).html(""<span style='color: #5FB878;'>上传成功</span>"");
                    tds.eq(3).html(''); //清空操作
                    fileflag = true;
                    $(""#{6}"").val(res.data.gid);
                    resetFormButton();//单文件上传成功后重新设置按钮的状态
                                      //checkFileNumber();
                    delete this.files[index]; //删除文件队列已经上传成功的文件
                    return;
                }}
                this.error(index, upload);
            }},
                    error: function(index, upload)
            {{
                var tr = demoListView.find('tr#upload-' + index), tds = tr.children();
                tds.eq(2).html(""<span style='color: #FF5722;'>上传失败</span>"");
                tds.eq(3).find('.demo-reload').removeClass('layui-hide'); //显示重传
                fileflag = false;
            }}
        }})
    }})", API, WGID,TYPE, acceptFileMime, acceptFileExt,MULTIPLE,FOR);
            script.InnerHtml.AppendHtml(str_script);
            output.PostElement.AppendHtml(script);
        }
    }
}
