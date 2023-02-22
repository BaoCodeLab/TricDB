using Aspose.Words;
using Main.Utils;
using Main.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Model.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using TDSCoreLib;

namespace Main.Controllers
{
    [Route("print_tmpl")]
    public class PF_PRINT_TMPLController : Controller
    {
        VM_PF_PRINT_TMPL viewModel = new VM_PF_PRINT_TMPL();

        private readonly drugdbContext _context;
        public PF_PRINT_TMPLController(drugdbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View("Index", viewModel);
        }

        /// <summary>
        /// 创建当前节点的下级节点
        /// </summary>
        /// <param name="GID">当前节点的GID，即将创建的是其下级节点</param>
        /// <param name="TITLE"></param>
        /// <returns></returns>
        [HttpGet("Create")]
        public IActionResult Create(string GID)
        {
            if (GID == "root")
            {
                VM_PF_PRINT_TMPL v = new VM_PF_PRINT_TMPL();
                v.SUPER = GID;
                v.GID = Guid.NewGuid().ToString();
                return View("Create", v);
            }
            else if (GID != null)
            {
                var currentTMPL = _context.PF_PRINT_TMPL.Where(w => w.IS_DELETE == false && w.GID == GID).SingleOrDefault();
                viewModel.SUPER = GID;
                viewModel.GID = Guid.NewGuid().ToString();
                ViewBag.TITLE = currentTMPL.TITLE;
                viewModel.DEPTH = currentTMPL.DEPTH;
                return View("Create", viewModel);
            }
            else
            {
                return Content("请先选择模板");
            }
        }

        [HttpGet("Edit")]
        public IActionResult Edit(string GID)
        {
            viewModel.GID = GID;
            return View("Edit", viewModel);
        }


        /// <summary>
        /// 展现数据模板生成界面
        /// </summary>
        /// <param name="code">模板代号，如果有下级模板将一并展示</param>
        /// <param name="viewmodel_head">表头视图模型类名</param>
        /// <param name="viewmodel_body">表列视图模型类名</param>
        /// <param name="head_data">表头（抬头）数据</param>
        /// <param name="body_data_url">表体（列表）数据</param>
        /// <returns></returns>
        [HttpPost("showexport")]
        public IActionResult ShowExport([Required]string code, [Required]string viewmodel_head, [Required]string head_data_url, string viewmodel_body = "", string body_data_url = "")
        {
            //根据ViewModel生成表头
            try
            {
                Type ViewModel_Body = Assembly.Load(new AssemblyName("Main")).GetType("Main.ViewModels." + viewmodel_body);
                List<dynamic> Cols = new List<dynamic>();
                List<KeyValuePair<string, string>> viewmodel_body_field = ViewModel_Body.GetProperties().Where(w => ((enableExportAttribute)w.GetCustomAttribute(typeof(enableExportAttribute)) != null)).Select(s => new KeyValuePair<string, string>(s.Name, ((DisplayAttribute)s.GetCustomAttribute(typeof(DisplayAttribute))).Name)).ToList();
                foreach (var kv in viewmodel_body_field)
                {
                    Cols.Add(new { field = kv.Key, title = kv.Value + "（" + kv.Key + "）" });

                }
                ViewBag.BodyCols = "[" + JsonConvert.SerializeObject(Cols) + "]";

            }
            catch
            {

            }
            ViewBag.viewmodel_head = viewmodel_head;
            ViewBag.viewmodel_body = viewmodel_body;
            ViewBag.BodyDataUrl = body_data_url;
            ViewBag.HeadDataUrl = head_data_url;
            ViewBag.Code = code;
            return View("ShowExport");
        }

        /// <summary>
        /// 导出文件
        /// </summary>
        /// <param name="tmpl_file_gid">模板文件的GID</param>
        /// <param name="head_data"></param>
        /// <param name="body_data"></param>
        /// <param name="file_ext">导出的文件类型</param>
        /// <returns></returns>
        [HttpPost("doexport")]
        public IActionResult DoExport(string tmpl_file_gid, string head_data, string file_ext = "", string body_data = "")
        {
            PF_FILE moban = _context.PF_FILE.Where(w => w.GID == tmpl_file_gid).FirstOrDefault();
            string filePath = WebPath.FILE_ABSOLUTE + moban.FILEURI;
            Document doc = new Document(filePath);
            //表头
            JObject head_data_json = (JObject)JsonConvert.DeserializeObject(head_data);
            DocumentBuilder builder = new DocumentBuilder(doc);
            foreach (var j in head_data_json)
            {
                if (builder.MoveToMergeField(j.Key))
                {
                    builder.Write(j.Value.ToString());
                }
            }
            //列表
            if (!string.IsNullOrEmpty(body_data))
            {
                JArray body_data_array = (JArray)JsonConvert.DeserializeObject(body_data);
                DataTable body_data_Table = JsonConvert.DeserializeObject<DataTable>(body_data_array.ToString());
                body_data_Table.TableName = "List";
                doc.MailMerge.ExecuteWithRegions(body_data_Table);
            }
            doc.MailMerge.DeleteFields();
            var docStream = new MemoryStream();
            if (string.IsNullOrEmpty(file_ext))
            {
                SaveFormat format = new SaveFormat();
                switch (moban.TYPE.ToLower())
                {
                    case ".doc": format = SaveFormat.Doc; file_ext = "doc"; break;
                    case ".docx": format = SaveFormat.Docx; file_ext = "docx"; break;
                }
                doc.Save(docStream, format);
            }
            else if (file_ext.ToLower() == "pdf")
            {
                doc.Save(docStream, SaveFormat.Pdf);
            }
            this.Response.ContentLength = docStream.Length;
            docStream.Position = 0;
            return new FileStreamResult(docStream, "application/octet-stream") { FileDownloadName = moban.FILENAME + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + file_ext };
        }
    }
}
