using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;
using Model.Model;
using System.Linq.Dynamic.Core;
using System.Linq;
using Main.platform;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Main.Controllers
{
    [Route("pf/file")]
    public class PF_FILEController : Controller
    {
        private readonly drugdbContext _context;
        public PF_FILEController(drugdbContext context)
        {
            _context = context;
        }
        VM_PF_FILE viewModel = new VM_PF_FILE();
        // GET: �ͻ�������չID�ֶ�
        public IActionResult Index(string GID, string LX)
        {
            var t = this._context.PF_USER.Where(m => m.USERNAME.Equals(User.Identity.Name)).FirstOrDefault();
            viewModel.GID = GID;
            viewModel.WGID = t.GID;
            viewModel.LX = System.Web.HttpUtility.UrlDecode(LX);
            return View("Index", viewModel);
        }
        [HttpGet, Route("dwtpsc")]
        public IActionResult dwtpsc(string GID, string LX)
        {
            viewModel.GID = null;
            viewModel.WGID = GID;
            viewModel.LX = System.Web.HttpUtility.UrlDecode(LX);
            return View("dwtpsc", viewModel);
        }

        [HttpGet,Route("ListView")]
        public IActionResult ListView(string GID)
        {
            viewModel.GID = null;
            viewModel.WGID = GID;
            return View("ListView", viewModel);
        }
        [HttpGet,Route("ListFile")]
        public IActionResult ListFile(string GID, string LX)
        {
            viewModel.GID = null;
            viewModel.WGID = GID;
            viewModel.LX = LX;
            var queryResult = _context.PF_FILE.Where("WGID==@0 and IS_DELETE==false and LX==@1", GID, LX).ToList();
            ViewBag.ListFile = queryResult;
            return View("ListFile", viewModel);
        }
        /// <summary>
        /// ���ڸ���
        /// </summary>
        /// <param name="id">�����ֶ�</param>
        /// <returns></returns>
        [HttpGet, Route("update")]
        public ActionResult Update(string GID,string WGID)
        {
            viewModel.GID = GID;
            viewModel.WGID = WGID;
            return View("Edit", viewModel);
        }

        /// <summary>
        /// �����½�
        /// </summary>
        /// <returns></returns>
        /// //�����ͻ���չ������GID  Ϊ�ͻ�ID
        [HttpGet, Route("create")]
        public ActionResult Create(string GID)
        {
            viewModel.WGID = GID;
            viewModel.GID = null;
            return View("Create", viewModel);
        }
        /// <summary>
        /// �鿴����
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("detail")]
        public ActionResult Detail(string GID)
        {
            viewModel.GID = GID;
            viewModel.WGID = null;
            return View("Detail", viewModel);
        }
        /// <summary>
        /// Ԥ��PDF�ļ�
        /// </summary>
        /// <param name="file">�ļ�·��</param>
        /// <returns></returns>
        [HttpGet, Route("viewpdf")]
        public ActionResult ViewPDF(string file)
        {
            return View("pdfViewer");
        }
        /// <summary>
        /// Ԥ��Word\PPT\Excel�ļ�
        /// </summary>
        /// <param name="file">�ļ�·��</param>
        /// <returns></returns>
        [HttpGet, Route("viewwpe")]
        public ActionResult ViewWPE(string file)
        {
            ViewBag.file = file;
            return View("wpeViewer");
        }
        /// <summary>
        /// Ԥ��ͼƬ�ļ�
        /// </summary>
        /// <param name="file">�ļ�·��</param>
        /// <returns></returns>
        [HttpGet, Route("viewimg")]
        public ActionResult ViewIMG(string file)
        {
            ViewBag.file = file;
            return View("imgViewer");
        }
    }
}
