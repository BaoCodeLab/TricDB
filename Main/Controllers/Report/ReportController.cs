using System;
using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;
using TDSCoreLib;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Main.platform;
using Newtonsoft.Json.Linq;

namespace Main.Controllers
{
    /// <summary>
    /// ͨ�ñ���
    /// </summary>
    public class ReportController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        public ReportController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// ����ͨ�ñ���
        /// </summary>
        /// <param name="viewmodel"></param>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Report([FromQuery]string viewmodel, [FromQuery]string model, [FromQuery]string name)
        {
            ViewBag.ViewModel = viewmodel;
            ViewBag.Model = model;
            ViewBag.Name = name;
            ViewBag.UserGID = HttpContext.User.Identity.Name;
            VM_Report report = new VM_Report();
            return View("Report", report);
        }

        /// <summary>
        /// ���ر������������
        /// </summary>
        /// <param name="form_key"></param>
        /// <returns></returns>
        [HttpGet,Route("form")]
        public ActionResult formReport([FromQuery]string form_key, [FromQuery]string name)
        {
            VM_FormCase_Report viewmodel = new VM_FormCase_Report();
            ViewBag.Name = name;
            ViewBag.FormKey = form_key;
            return View("formReport", viewmodel);
        }

        /// <summary>
        /// ���ⶨ���ѯ�ı���
        /// </summary>
        /// <param name="viewmodel">��ͼģ��</param>
        /// <param name="action">UrlAction:action</param>
        /// <param name="controller">UrlAction:controller</param>
        /// <param name="name">����</param>
        /// <returns></returns>
        [HttpGet, Route("remote")]
        public ActionResult customReport([FromQuery]string viewmodel, [FromQuery]string action, [FromQuery]string controller, [FromQuery]string name)
        {
            ViewBag.ViewModel = viewmodel;
            ViewBag.Action = action;
            ViewBag.Controller = controller;
            ViewBag.Name = name;
            ViewBag.UserName = Permission.getCurrentUser();
            VM_Report report = new VM_Report();
            return View("remoteReport", report);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="params">���ݲ�ѯ����</param>
        /// <param name="viewmodel">��ͼģ��</param>
        /// <param name="title">�������</param>
        /// <returns></returns>
        [HttpPost]
        public FileResult Export([FromForm]string @params,[FromForm]string viewmodel,[FromForm]string title)
        {
            string API_REPORT = Url.Action("AdvSearch", "API_REPORT", null, "http");
            @params = WebUtility.UrlDecode(@params);
            List<dynamic> list = new List<dynamic>();

            string jsonData = HttpClientHelper.PostResponse(API_REPORT, @params, HttpContext);
            JArray data = (JArray)JsonConvert.DeserializeObject<ResultList<dynamic>>(jsonData).Results;
            list = data.ToObject<List<dynamic>>();

            XlsGenerator.createXlsFile(list, viewmodel, title, _hostingEnvironment.WebRootPath, out string filePath);
            string fileName = DateTime.Now.ToString("yyMMddHms") + ".xls";
            filePath = _hostingEnvironment.ContentRootPath + "\\wwwroot" + filePath;
            Response.Headers.Add("content-disposition", "attachment;filename=" + fileName);
            return File(new FileStream(filePath, FileMode.Open), "application/excel", fileName);
        }
        /// <summary>
        /// ���������������
        /// </summary>
        /// <param name="params"></param>
        /// <param name="header">��ͷ���飬|�ָ�</param>
        /// <param name="field">�ֶΣ�|�ָ�</param>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpPost,Route("form")]
        public FileResult ExportForm([FromForm]string @params, [FromForm]string header, [FromForm]string field, [FromForm]string title)
        {
            string API_REPORT = Url.Action("FormSearch", "API_REPORT", null, "http");
            @params = WebUtility.UrlDecode(@params);
            header = WebUtility.UrlDecode(header);
            List<dynamic> list = new List<dynamic>();
            string[] headers = header.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            string[] fields = field.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            string jsonData = HttpClientHelper.PostResponse(API_REPORT, @params, HttpContext);
            JArray data = (JArray)JsonConvert.DeserializeObject<ResultList<dynamic>>(jsonData).Results;
            list = data.ToObject<List<dynamic>>();

            XlsGenerator.createXlsFile(list, headers, fields, title, _hostingEnvironment.WebRootPath, out string filePath);
            string fileName = DateTime.Now.ToString("yyMMddHms") + ".xls";
            filePath = _hostingEnvironment.ContentRootPath + "\\wwwroot" + filePath;
            Response.Headers.Add("content-disposition", "attachment;filename=" + fileName);
            return File(new FileStream(filePath, FileMode.Open), "application/excel", fileName);
        }
    }
}