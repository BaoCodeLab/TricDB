using AutoMapper;
using ICSharpCode.SharpZipLib.Zip;
using Main.Extensions;
using Main.platform;
using Main.Utils;
using Main.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Model.Model;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/pf/file")]
    public class API_PF_FILE : Controller
    {
        private readonly drugdbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private IMemoryCache _memoryCache;

        public API_PF_FILE(drugdbContext context, IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _memoryCache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ResultList<VM_PF_FILE> Get([FromQuery]string WGID, int page = 1, int limit = 5, string searchfield = "FILENAME", string searchword = "", string field = "MODIFY_DATE", string order = "DESC")  //PX未用，field改成修改时间MODIFY_DATE进行降序排
        {
            //1、设定检索默认值
            searchfield = string.IsNullOrEmpty(searchfield) ? "FILENAME" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : System.Web.HttpUtility.UrlDecode(searchword);
            //2、执行查询
            var queryResult = _context.PF_FILE
            .Where((searchfield + ".Contains(@0) and WGID ==@1 and is_delete = false"), searchword, WGID) //like查找
            .OrderBy(field + " " + order)//按条件排序
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
            //3、返回结果
            return new ResultList<VM_PF_FILE>
            {
                TotalCount = _context.PF_FILE.Where(searchfield + ".Contains(@0) and WGID ==@1 and is_delete == false", searchword, WGID).Count(),
                Results = Mapper.Map<List<PF_FILE>, List<VM_PF_FILE>>(queryResult)
            };
        }

        [HttpGet, Route("gid")]
        public async Task<IActionResult> GetByID(string GID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PF_FILE PF_FILE = await _context.PF_FILE.SingleOrDefaultAsync(m => m.GID == GID && m.IS_DELETE == false);
            if (PF_FILE == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<PF_FILE, VM_PF_FILE>(PF_FILE));
        }
        [HttpGet, Route("checkexist")]
        public bool CheckExsit(string wgid = "", string lx = "")
        {
            try
            {
                var d = this._context.PF_FILE.Where(m => m.WGID.Equals(wgid) && m.LX.Equals(System.Web.HttpUtility.UrlDecode(lx)) && !m.IS_DELETE).Count() > 0;
                return d;
            }
            catch
            {
                return false;
            }
        }
        [HttpPost("update")]
        public ResultList<Object> Update(PF_FILE PF_FILE)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResultList<object>
                    {
                        StateCode = 5002,
                        Message = "参数不符合！"
                    };
                }
                //查询是否存在
                PF_FILE pf_file = _context.PF_FILE.SingleOrDefault("GID ==@0 and is_delete = false ", PF_FILE.GID);
                if (pf_file == null)
                {
                    return new ResultList<object>
                    {
                        StateCode = 5002,
                        Message = "文件不存在！"
                    };
                }
                pf_file.MODIFY_DATE = DateTime.Now;
                pf_file.FILENAME = PF_FILE.FILENAME;
                pf_file.CREATE_DATE = DateTime.Now;
                pf_file.LX = PF_FILE.LX;
                _context.Update(pf_file);
                _context.SaveChanges();
                return new ResultList<object>
                {
                    StateCode = 5001,
                    Message = "修改成功"
                };
            }
            catch (Exception e)
            {
                return new ResultList<object>
                {
                    StateCode = 0000,
                    Message = e.ToString()
                };
            }
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="WGID"></param>
        /// <param name="LX"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(string WGID, string LX = "", string BZ = "")
        {
            string tempGID = "";
            string tempTitle = "";
            string[] allowList = { ".jpg", ".png", ".jpeg", ".gif", ".bmp", ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx", ".txt", ".pdf", ".zip", ".rar", ".7z", ".vcf", ".maf" };
            try
            {
                if (!Directory.Exists(WebPath.FILE_ABSOLUTE + WebPath.FILE_REALTE))//判断文件夹是否存在 
                {
                    Directory.CreateDirectory(WebPath.FILE_ABSOLUTE + WebPath.FILE_REALTE);//不存在则创建文件夹 
                }
                List<IFormFile> formFiles = Request.Form.Files.ToList();
                if (formFiles.Count == 0)
                {
                    return Ok(new { code = "404", msg = "无有效文件" });
                }
                var t = this._context.PF_USER.Where(m => m.USERNAME.Equals(User.Identity.Name)).FirstOrDefault();
                WGID = t.GID;
                foreach (IFormFile formFile in formFiles)
                {
                    var ext = Path.GetExtension(formFile.FileName);
                    if (allowList.Any(y => string.Equals(ext, y, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        PF_FILE pf_file = new PF_FILE
                        {
                            WGID = WGID,
                            LX = System.Web.HttpUtility.UrlDecode(LX)
                        };
                        if (Path.HasExtension(formFile.FileName))
                        {
                            pf_file.FILENAME = Path.GetFileNameWithoutExtension(formFile.FileName);
                            pf_file.TYPE = Path.GetExtension(formFile.FileName);
                        }
                        else
                        {
                            pf_file.FILENAME = formFile.FileName;
                        }
                        //读取文件MD5码
                        string MD5 = Helper.checkMD5(formFile.OpenReadStream());
                        pf_file.MD5 = MD5;
                        tempTitle = pf_file.FILENAME.Replace(" ","_");
                        //文件路径
                        string GID = Guid.NewGuid().ToString();
                        pf_file.GID = GID;
                        string xdlj = WebPath.FILE_REALTE + tempTitle + "_" + GID;//相对路径
                        tempGID = GID;
                        string jdlj = WebPath.FILE_ABSOLUTE;//绝对路径
                        string path = jdlj + xdlj;
                        pf_file.FILEURI = xdlj;
                        pf_file.PX = 0;
                        pf_file.BZ = (string.IsNullOrEmpty(BZ) ? "" : BZ);
                        pf_file.CREATE_DATE = DateTime.Now;
                        pf_file.MODIFY_DATE = DateTime.Now;
                        pf_file.IP = HttpContext.Connection.RemoteIpAddress.ToString();
                        pf_file.OPERATOR = Permission.getCurrentUser();
                        //保存文件
                        using (FileStream fs = System.IO.File.Create(path))
                        {
                            // 复制文件  
                            formFile.CopyTo(fs);
                            // 清空缓冲区数据  
                            fs.Flush();
                        }
                        _context.Add(pf_file);
                        Log.Write(GetType(), "PF_FILE", GID + "文件保存");
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return Ok(new { code = "403", msg = "不允许上传" + Path.GetExtension(formFile.FileName) + "类型文件！" });
                    }
                }
                return Ok(new
                {
                    code = "0",
                    msg = "上传成功",
                    data = new
                    {
                        gid = tempGID,
                        src = Url.Action("Download", "API_PF_FILE") + "/" + tempGID,
                        title = tempTitle
                    }
                });
            }
            catch (Exception e)
            {
                return Ok(new { code = "404", msg = e.ToString() });
            }
        }
        //删除档案
        [HttpDelete]
        public ResultList<Object> Delete(string GID)
        {
            PF_FILE PF_FILE = _context.PF_FILE.SingleOrDefault(m => m.GID == GID && m.IS_DELETE == false);
            if (PF_FILE == null)
            {
                return new ResultList<object>
                {
                    StateCode = 5002,
                    Message = "无此文件"
                };
            }
            PF_FILE.IS_DELETE = true;
            PF_FILE.MODIFY_DATE = DateTime.Now;
            _context.Update(PF_FILE);
            _context.SaveChanges();
            return new ResultList<object>
            {
                StateCode = 5001,
                Message = "成功删除"
            };
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="GID"></param>
        /// <returns></returns>
        [HttpGet("file/{GID?}")]
        public IActionResult Download([FromRoute]string GID)
        {
            PF_FILE PF_FILE = _context.PF_FILE.SingleOrDefault(m => m.GID == GID && m.IS_DELETE == false);
            if (PF_FILE == null)
            {
                return NotFound(new { msg = "文件1不存在" });
            }
            string xdlj = PF_FILE.FILEURI;
            string jdlj = WebPath.FILE_ABSOLUTE;//绝对路径
            var stream = System.IO.File.OpenRead(jdlj + xdlj);
            if (stream == null)
            {
                return NotFound(new { msg = "文件2不存在" });
            }
            return File(stream, Helper.GetContentType(PF_FILE.TYPE.Replace(".", "")), PF_FILE.FILENAME + PF_FILE.TYPE);
        }
        /// <summary>
        /// 预览文件
        /// </summary>
        /// <param name="GID"></param>
        /// <returns></returns>
        [HttpGet("viewfile/{GID?}")]
        public IActionResult ViewFile([FromRoute]string GID)
        {
            string[] imgArr = { ".jpg", ".png", ".jpeg", ".gif", ".bmp" };
            string[] wpeArr = { ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx" };
            //获取数据库文件
            PF_FILE PF_FILE = _context.PF_FILE.SingleOrDefault(m => m.GID == GID && m.IS_DELETE == false);
            if (PF_FILE == null)
            {
                return NotFound(new { msg = "文件不存在" });
            }
            else if (PF_FILE.TYPE.ToLower() == ".pdf")
            {
                string filepath = Url.Action("Download", "API_PF_FILE", new { GID });
                return RedirectToAction("ViewPDF", "PF_FILE", new { file = filepath });

            }
            else if (imgArr.Contains(PF_FILE.TYPE.ToLower()))
            {
                string filepath = Url.Action("Download", "API_PF_FILE", new { GID });
                return RedirectToAction("ViewIMG", "PF_FILE", new { file = filepath });
            }
            else if (wpeArr.Contains(PF_FILE.TYPE.ToLower()))
            {
                string filepath = Url.Action("Download", "API_PF_FILE", new { GID }, "http");
                return RedirectToAction("ViewWPE", "PF_FILE", new { file = filepath });
            }
            else
            {
                //直接下载
                return RedirectToAction("Download", "API_PF_FILE", new { GID });
            }
        }
    }
}