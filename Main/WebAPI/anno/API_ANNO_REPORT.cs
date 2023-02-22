using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ICSharpCode.SharpZipLib.Zip;
using Main.Extensions;
using Main.platform;
using Main.Utils;
using Main.ViewModels;
using Main.ViewModels.Annotation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Model.Model;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.IO;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using TDSCoreLib;
using System.Web;
using MySql.Data.MySqlClient;
using System.Resources;
using System.Collections;
using System.Resources.NetStandard;
using Newtonsoft.Json.Linq;
using Json.Net;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net;
using System.Collections.Concurrent;
using System.Threading;
using MiniSoftware;

namespace Main.WebAPI.anno
{
    [Produces("application/json")]
    [Route("api/anno/report")]
    public class API_ANNO_REPORT : Controller
    {
        private readonly drugdbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private IMemoryCache _memoryCache;

        public API_ANNO_REPORT(drugdbContext context, IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _memoryCache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
        }


        string user_name = Permission.getCurrentUser();

        // 获取当前用户已生成的Report
        [HttpGet, Route("GetReport")]
        public IActionResult GetReport([FromQuery] int page = 1, int limit = 10)
        {
            var t = this._context.PF_USER.Where(m => m.USERNAME.Equals(User.Identity.Name)).FirstOrDefault();
            var user_id = t.GID;
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_ANNO_ALL>("SELECT p.project_id,p.project_name,p.project_descrip,s.*,r.report_id,r.report_name,r.report_description,f.* FROM anno_prosamp ps LEFT JOIN anno_project p ON ps.project_id=p.project_id LEFT JOIN anno_sample s ON ps.anno_sampleid=s.anno_sampleid LEFT JOIN anno_report r ON s.anno_sampleid=r.anno_sampleid LEFT JOIN pf_file f ON r.anno_sampleid=f.LX WHERE p.user_id=@user_id AND p.IS_DELETE=FALSE AND ps.IS_DELETE=FALSE AND s.IS_DELETE=FALSE AND r.IS_DELETE=FALSE AND f.IS_DELETE=FALSE", new MySqlParameter[] { new MySqlParameter("user_id", user_id) }).AsQueryable();
            
            var result = queryResult.Where(m => !string.IsNullOrEmpty(m.REPORT_ID)).DistinctBy(n => new {n.ANNO_SAMPLEID,n.REPORT_ID }).OrderByDescending(d => d.CREATE_DATE).Skip((page - 1) * limit).Take(limit).ToList();
            return Ok(new ResultList<VM_ANNO_ALL>
            {
                TotalCount = result.Count(),
                Results = result
            });
        }


        // 验证project是否存在
        private bool ANNO_ReportExists(string Gid)
        {
            return _context.ANNO_REPORT.Any(e => e.REPORT_ID == Gid);
        }


        /// <summary>
        /// 更新选中的PROJECT数据
        ///</summary>
        /// <returns>执行结果反馈</returns>
        [HttpPost, Route("Update")]
        public async Task<IActionResult> Update([FromForm] VM_PF_FILE postData)
        {

            try
            {
                // 更新project表关于name和description数据
                postData.GID = postData.GID;
                PF_FILE fileData = Mapper.Map<VM_PF_FILE, PF_FILE>(postData);
                // fileData.ORDER = fileData.ORDER;
                fileData.WGID = postData.WGID;
                fileData.FILENAME = postData.FILENAME;
                fileData.IS_DELETE = false;
                fileData.FILEURI = postData.FILEURI;
                fileData.TYPE = postData.TYPE;
                fileData.MD5 = postData.MD5;
                fileData.IP = postData.IP;
                fileData.MODIFY_DATE = DateTime.Now;
                fileData.CREATE_DATE = postData.CREATE_DATE;
                fileData.OPERATOR = user_name;

                _context.PF_FILE.Update(fileData);
                await _context.SaveChangesAsync();
                // 写入日志
                Log.Write(this.GetType(), "File", "Update File:" + fileData.GID + ", Operator is" + user_name + ", Edit base information");


                return Ok(new { result = true, msg = "The File information has been saved successful." });

            }

            catch (DbUpdateConcurrencyException ex)
            {
                if (!ANNO_ReportExists(postData.GID))
                {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
                else
                {
                    return Ok(new { success = "false", msg = ex.Message });
                    //日志记录
                }
            }


        }


        // DELETE: api/API_ANNO_REPORT/5
        /// <summary>
        /// 删除所选中的File
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpDelete("{GID?}")]
        public async Task<IActionResult> Delete([FromForm] string GID)
        {
            if (!Permission.check(HttpContext, "OPERATE:YWSC"))
            {
                return Forbid();
            }
            // 获取要删除的Project对应数据对象
            PF_FILE file = await _context.PF_FILE.SingleOrDefaultAsync(m => m.GID == GID);

            if (file == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            // 选择假性删除
            file.IS_DELETE = true;
            Log.Write(_context, GetType(), "delete", "PF_FILE", "Delete File,ID is" + file.GID + ". Operator is" + Permission.getCurrentUser());
            await _context.SaveChangesAsync();


            return Ok(new { success = "true" });
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
                        ANNO_REPORT report = new ANNO_REPORT();
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
                        tempTitle = pf_file.FILENAME.Replace(" ", "_");
                        //文件路径
                        string GID = Guid.NewGuid().ToString();
                        pf_file.GID = GID;
                        string xdlj = WebPath.FILE_REALTE + GID;//相对路径
                        tempGID = GID;
                        string jdlj = WebPath.FILE_ABSOLUTE;//绝对路径
                        string path = jdlj + xdlj;
                        pf_file.FILEURI = xdlj;
                        pf_file.PX = 0;
                        pf_file.BZ = (string.IsNullOrEmpty(BZ) ? "false" : BZ);
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

                        report.REPORT_ID = GID;
                        report.ANNO_SAMPLEID = LX;
                        report.REPORT_NAME = pf_file.FILENAME;
                        report.CREATE_DATE = DateTime.Now;
                        report.MODIFY_DATE = DateTime.Now;
                        report.OPERATOR = Permission.getCurrentUser();
                        report.IS_PUB = true;
                        report.IS_DELETE = false;
                        report.VERSION = "en-US";

                        _context.PF_FILE.Add(pf_file);
                        _context.ANNO_REPORT.Add(report);
                        Log.Write(GetType(), "PF_FILE", GID + "文件保存");
                        Log.Write(GetType(), "ANNO_REPORT", GID + "文件保存");
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return Ok(new { code = "403", msg = "不允许上传" + formFile.FileName + "类型文件！" });
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



        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="GID"></param>
        /// <returns></returns>
        [HttpGet,Route("Download")]
        public IActionResult Download(string GID)
        {
            PF_FILE PF_FILE = _context.PF_FILE.SingleOrDefault(m => m.GID == GID && m.IS_DELETE == false);
            if (PF_FILE == null)
            {
                return NotFound(new { msg = GID });
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


    }
}
