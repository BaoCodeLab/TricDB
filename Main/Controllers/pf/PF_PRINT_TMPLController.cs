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
        /// ������ǰ�ڵ���¼��ڵ�
        /// </summary>
        /// <param name="GID">��ǰ�ڵ��GID�����������������¼��ڵ�</param>
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
                return Content("����ѡ��ģ��");
            }
        }

        [HttpGet("Edit")]
        public IActionResult Edit(string GID)
        {
            viewModel.GID = GID;
            return View("Edit", viewModel);
        }


        /// <summary>
        /// չ������ģ�����ɽ���
        /// </summary>
        /// <param name="code">ģ����ţ�������¼�ģ�彫һ��չʾ</param>
        /// <param name="viewmodel_head">��ͷ��ͼģ������</param>
        /// <param name="viewmodel_body">������ͼģ������</param>
        /// <param name="head_data">��ͷ��̧ͷ������</param>
        /// <param name="body_data_url">���壨�б�����</param>
        /// <returns></returns>
        [HttpPost("showexport")]
        public IActionResult ShowExport([Required]string code, [Required]string viewmodel_head, [Required]string head_data_url, string viewmodel_body = "", string body_data_url = "")
        {
            //����ViewModel���ɱ�ͷ
            try
            {
                Type ViewModel_Body = Assembly.Load(new AssemblyName("Main")).GetType("Main.ViewModels." + viewmodel_body);
                List<dynamic> Cols = new List<dynamic>();
                List<KeyValuePair<string, string>> viewmodel_body_field = ViewModel_Body.GetProperties().Where(w => ((enableExportAttribute)w.GetCustomAttribute(typeof(enableExportAttribute)) != null)).Select(s => new KeyValuePair<string, string>(s.Name, ((DisplayAttribute)s.GetCustomAttribute(typeof(DisplayAttribute))).Name)).ToList();
                foreach (var kv in viewmodel_body_field)
                {
                    Cols.Add(new { field = kv.Key, title = kv.Value + "��" + kv.Key + "��" });

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
        /// �����ļ�
        /// </summary>
        /// <param name="tmpl_file_gid">ģ���ļ���GID</param>
        /// <param name="head_data"></param>
        /// <param name="body_data"></param>
        /// <param name="file_ext">�������ļ�����</param>
        /// <returns></returns>
        [HttpPost("doexport")]
        public IActionResult DoExport(string tmpl_file_gid, string head_data, string file_ext = "", string body_data = "")
        {
            PF_FILE moban = _context.PF_FILE.Where(w => w.GID == tmpl_file_gid).FirstOrDefault();
            string filePath = WebPath.FILE_ABSOLUTE + moban.FILEURI;
            Document doc = new Document(filePath);
            //��ͷ
            JObject head_data_json = (JObject)JsonConvert.DeserializeObject(head_data);
            DocumentBuilder builder = new DocumentBuilder(doc);
            foreach (var j in head_data_json)
            {
                if (builder.MoveToMergeField(j.Key))
                {
                    builder.Write(j.Value.ToString());
                }
            }
            //�б�
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
