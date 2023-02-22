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
    [Route("Disease")]
    public class BUS_DISEASEController : Controller
    {
        private readonly drugdbContext _context;
        string webRoot = string.Empty;
        public BUS_DISEASEController(IHostingEnvironment env, drugdbContext context)
        {
            _context = context;
            webRoot = env.WebRootPath;
        }
        // GET: BUS_DISEASE
        /// <summary>
        /// 绑定页面基本视图
        ///</summary>
        /// <returns>BUS_DISEASE视图模型</returns>
        [HttpGet, Route("Index")]
        public ActionResult Index()
        {
            if (!Permission.check(HttpContext, "OPERATE:JBCK"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_DISEASE viewModel = new VM_BUS_DISEASE(this);
            return View("Index", viewModel);

        }
        

        // GET: BUS_DISEASE/Details/5       
        /// <summary>
        /// 绑定查看详情页面
        ///</summary>
        /// <returns>BUS_ALL视图模型</returns>x
        [HttpGet, Route("Details")]
        public ActionResult Details([FromQuery]string DISEASEID)
        {
            try
            {
                ViewBag.TotalDb = this._context.BUS_DISEASE.Where(m => m.IS_PUB && !m.IS_DELETE).Count();
                DISEASEID = DISEASEID ?? Guid.NewGuid().ToString();
                
                var db = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select * from bus_all where DISEASEID=@DISEASEID",
                    new MySqlParameter[] {
                        new MySqlParameter("DISEASEID", DISEASEID)
                    });
                if (db.Count() == 0)
                {
                    return NotFound();
                }
                else
                {
                    var q = this._context.BUS_DISEASE.Find(DISEASEID);
                    q.HIT = q.HIT + 1;
                    this._context.SaveChanges();
                    return View("Details", db.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "异常", "BUS_DISEASE", ex.ToString());
                return NoContent();
            }
        }
        // GET: BUS_DISEASE/Create
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>BUS_DISEASE视图模型</returns>
        [HttpGet, Route("Create")]
        public ActionResult Create()
        {
            if (!Permission.check(HttpContext, "OPERATE:JBXZ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_DISEASE viewModel = new VM_BUS_DISEASE(this);
            viewModel.DISEASEID = Guid.NewGuid().ToString();
            viewModel.IS_PUB = true;
            viewModel.IS_DELETE = false;
            var d = this._context.BUS_DISEASE.Where(n => n.DISEASECODE.StartsWith("1" + DateTime.Now.ToString("yyyyMMdd"))).OrderByDescending(m => m.DISEASECODE);
            if (d.Count() == 0)
            {
                viewModel.DISEASECODE = "1" + DateTime.Now.ToString("yyyyMMdd") + "001";
            }
            else
            {
                viewModel.DISEASECODE = (long.Parse(d.FirstOrDefault().DISEASECODE) + 1).ToString();
            }
            return View("Create", viewModel);
        }
        // GET: BUS_DISEASE/Select
        /// <summary>
        /// 绑定新建数据表单
        ///</summary>       
        /// <returns>BUS_DISEASE视图模型</returns>
        [HttpGet, Route("Select")]
        public ActionResult Select()
        {
            if (!Permission.check(HttpContext, "OPERATE:JBCK"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_DISEASE vm = new VM_BUS_DISEASE();
            vm.DISEASEID = Guid.NewGuid().ToString();
            return View(vm);
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="WGID"></param>
        /// <param name="LX"></param>
        /// <returns></returns>
        [HttpPost("Upload")]
        public ActionResult Upload(string DISEASECODE = "")
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
                        path = path + "/DISEASE" + DISEASECODE + pf;
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
        // GET: BUS_DISEASE/Edit/5        
        /// <summary>
        /// 绑定修改数据表单
        ///</summary>
        /// <returns>BUS_DISEASE视图模型</returns>
        [HttpGet, Route("Edit/{DID?}")]
        public ActionResult Edit([FromRoute] string DID)
        {
            if (!Permission.check(HttpContext, "OPERATE:JBBJ"))
            {
                return View("/Views/ERROR/permission.cshtml");
            }
            VM_BUS_DISEASE viewModel = new VM_BUS_DISEASE(this)
            {
                DISEASEID = DID
            };
            if (DID == null)
            {
                return NotFound();
            }
            else
            {
                viewModel = AutoMapper.Mapper.Map<BUS_DISEASE, VM_BUS_DISEASE>(this._context.BUS_DISEASE.Find(DID));
                return View("Edit", viewModel);
            }
        }
    }
}
