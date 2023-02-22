using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;
using Main.platform;
using System;
using Model.Model;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Main.Extensions;
using MySql.Data.MySqlClient;

namespace Main.Controllers
{

    [Route("Target")]
    public class BUS_TARGETController : Controller
    {
        private readonly drugdbContext _context;
        string webRoot = string.Empty;
        public BUS_TARGETController(IHostingEnvironment env, drugdbContext context)
        {
            _context = context;
            webRoot = env.WebRootPath;
        }
        // GET: BUS_TARGET
        /// <summary>
        /// 绑定页面基本视图
        ///</summary>
        /// <returns>BUS_TARGET视图模型</returns>
        [HttpGet, Route("Index")]
        public ActionResult Index()
        {
            if (!Permission.check(HttpContext, "OPERATE:BDCK"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_TARGET viewModel = new VM_BUS_TARGET(this);
            return View("Index", viewModel);

        }
        // GET: BUS_TARGET/Details/5
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
        /// <returns>BUS_ALL视图模型</returns>
        [HttpGet, Route("Details")]
        public ActionResult Details([FromQuery]string TARGETID)
        {
            try
            {
                ViewBag.TotalDb = this._context.BUS_TARGET.Where(m => m.IS_PUB && !m.IS_DELETE).Count();
                TARGETID = TARGETID ?? Guid.NewGuid().ToString();
                var db = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select * from bus_all where TARGETID=@TARGETID", new MySqlParameter[] { new MySqlParameter("TARGETID", TARGETID) });
                if (db.Count() == 0)
                {
                    return NotFound();
                }
                else
                {
                    var q = this._context.BUS_TARGET.Find(TARGETID);
                    q.HIT = q.HIT + 1;
                    this._context.SaveChanges();
                    return View("Details", db.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "异常", "BUS_TARGET", ex.ToString());
                return NoContent();
            }
        }
        // GET: BUS_TARGET/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>BUS_TARGET视图模型</returns>
        [HttpGet, Route("Create")]
        public ActionResult Create()
        {
            if (!Permission.check(HttpContext, "OPERATE:BDXZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_TARGET viewModel = new VM_BUS_TARGET(this);
            viewModel.TARGETID = Guid.NewGuid().ToString();
            viewModel.IS_PUB = true;
            viewModel.IS_DELETE = false;
            var d = this._context.BUS_TARGET.Where(n => n.TARGETCODE.StartsWith("3" + DateTime.Now.ToString("yyyyMMdd"))).OrderByDescending(m => m.TARGETCODE);
            if (d.Count() == 0)
            {
                viewModel.TARGETCODE = "3" + DateTime.Now.ToString("yyyyMMdd") + "001";
            }
            else
            {
                viewModel.TARGETCODE = (long.Parse(d.FirstOrDefault().TARGETCODE) + 1).ToString();
            }
            return View("Create", viewModel);
        }
        // GET: BUS_TARGET/Select
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>BUS_TARGET视图模型</returns>
        [HttpGet, Route("Select")]
        public ActionResult Select()
        {
            if (!Permission.check(HttpContext, "OPERATE:BDCK"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            return View();
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="WDID"></param>
        /// <param name="LX"></param>
        /// <returns></returns>
        [HttpPost("Upload")]
        public ActionResult Upload(string TARGETCODE = "")
        {
            string[] allowList = { ".jpg", ".png", ".jpeg", ".gif", ".bmp" };
            try
            {
                string path = webRoot + "/relationImg";
                if (!Directory.Exists(path))//判断文件夹是否存在 
                {
                    Directory.CreateDirectory(path);//不存在则创建文件夹 
                }
                List<IFormFile> formFiles = Request.Form.Files.ToList();
                if (formFiles.Count == 0)
                {
                    return Ok(new { code = "404", msg = "无有效文件" });
                }
                foreach (IFormFile formFile in formFiles)
                {
                    var ext = Path.GetExtension(formFile.FileName);
                    if (allowList.Any(y => string.Equals(ext, y, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        string pf = Path.GetExtension(formFile.FileName);
                        path = path + "/" + TARGETCODE + pf;
                        //保存文件
                        using (FileStream fs = System.IO.File.Create(path))
                        {
                            // 复制文件  
                            formFile.CopyTo(fs);
                            // 清空缓冲区数据  
                            fs.Flush();
                            fs.Close();
                        }
                    }
                    else
                    {
                        return Ok(new { code = "403", msg = "不允许上传" + Path.GetExtension(formFile.FileName) + "类型文件！" });
                    }
                }
                return Ok(new
                {
                    code = "0",
                    msg = "上传成功"
                });
            }
            catch (Exception e)
            {
                return Ok(new { code = "404", msg = e.ToString() });
            }
        }

        // GET: BUS_TARGET/Edit/5        
        /// <summary>
        /// 绑定修改数据表单
        ///</summary>
        /// <returns>BUS_TARGET视图模型</returns>
        [HttpGet, Route("Edit/{TID?}")]
        public ActionResult Edit([FromRoute] string TID)
        {
            if (!Permission.check(HttpContext, "OPERATE:BDBJ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_TARGET viewModel = new VM_BUS_TARGET(this)
            {
                TARGETID = TID
            };
            if (TID == null)
            {
                return NotFound();
            }
            else
            {
                viewModel = AutoMapper.Mapper.Map<BUS_TARGET, VM_BUS_TARGET>(this._context.BUS_TARGET.Find(TID));
                return View("Edit", viewModel);
            }
        }
        [HttpGet, Route("Navi/{Id?}")]
        public ActionResult Navi([FromRoute] string Id)
        {
            var q = from n in this._context.BUS_TARGET.Where(m => m.TARGET.ToLower().Equals(Id.ToLower())) select n;
            if (q.Count() > 0)
            {
                return Details(q.FirstOrDefault().TARGETID);
            }
            else
            {
                return Redirect("https://www.ncbi.nlm.nih.gov/gene/?term=" + Id);
            }
        }

        [HttpGet, Route("germline")]
        public ActionResult germline([FromQuery] string FREQ_ID)
        {
            try
            {
                ViewBag.TotalDb = this._context.FREQ_FROM_DATASET.Where(m => m.IS_PUB && !m.IS_DELETE && m.STUDY=="Germline_10389").Count();
                FREQ_ID = FREQ_ID ?? Guid.NewGuid().ToString();
                var db = MySQLDB.GetSimpleTFromQuery<VM_FREQ_ALL>("select * from freq_all where FREQ_ID=@FREQ_ID", new MySqlParameter[] { new MySqlParameter("FREQ_ID", FREQ_ID) });
                if (db.Count() == 0)
                {
                    return NotFound();
                }
                else
                {
                    return View("Germline", db.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "异常", "BUS_TARGET", ex.ToString());
                return NoContent();
            }
        }
    }
}
