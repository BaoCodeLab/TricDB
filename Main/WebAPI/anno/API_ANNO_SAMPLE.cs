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





namespace Main.WebAPI.anno
{
    [Produces("application/json")]
    [Route("api/anno/sample")]
    public class API_ANNO_SAMPLE : Controller
    {
        private readonly drugdbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private IMemoryCache _memoryCache;

        public API_ANNO_SAMPLE(drugdbContext context, IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _memoryCache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
        }



        [HttpPost, Route("SaveSample")]
        // 保存上传的样本数据
        public IActionResult SaveSample([FromForm] VM_ANNO_SAMPLE postSampleData)
        {
            if (!Permission.check(HttpContext, "OPERATE:YWBJ"))
            {
                return Forbid();
            }

            VM_ANNO_SAMPLE anno_sample = new VM_ANNO_SAMPLE();
            var t = this._context.PF_USER.Where(m => m.USERNAME.Equals(User.Identity.Name)).FirstOrDefault();
            anno_sample.ANNO_SAMPLEID = postSampleData.ANNO_SAMPLEID;
            anno_sample.USER_ID = t.GID;
            anno_sample.PATIENT_ID = postSampleData.PATIENT_ID;
            anno_sample.SAMPLE_NAME = postSampleData.SAMPLE_NAME ?? "";
            anno_sample.SAMPLE_SOURCE = postSampleData.SAMPLE_SOURCE ?? "";
            anno_sample.SAMPLE_TYPE = postSampleData.SAMPLE_TYPE ?? "";
            anno_sample.SAMPLE_POSI = postSampleData.SAMPLE_POSI ?? "";
            anno_sample.SAMPLE_METHOD = postSampleData.SAMPLE_METHOD ?? "";
            anno_sample.SAMPLE_DATE = postSampleData.SAMPLE_DATE;
            anno_sample.ACCESSION_DATE = postSampleData.ACCESSION_DATE;
            anno_sample.SAMPLE_DIAG = postSampleData.SAMPLE_DIAG ?? "";
            anno_sample.MUTATION_TYPE = postSampleData.MUTATION_TYPE;
            anno_sample.SEQUENCE_TYPE = postSampleData.SEQUENCE_TYPE;
            anno_sample.MSI = postSampleData.MSI_Type + ":" + postSampleData.MSI;
            anno_sample.CAPTURE_SIZE = postSampleData.CAPTURE_SIZE;
            anno_sample.CREATE_DATE = DateTime.Now;
            anno_sample.MODIFY_DATE = DateTime.Now;
            anno_sample.IS_PUB = true;
            anno_sample.IS_DELETE = false;
            anno_sample.OPERATOR = Permission.getCurrentUser();
            anno_sample.VERSION = "en-us";
            ANNO_SAMPLE entity = Mapper.Map<VM_ANNO_SAMPLE, ANNO_SAMPLE>(anno_sample);
            _context.ANNO_SAMPLE.Add(entity);
            this._context.SaveChanges();
            Log.Write(this.GetType(), "anno_sample", "Summit the sample of code:" + anno_sample.ANNO_SAMPLEID + ", Operater is" + t.USERNAME + ",ID is" + t.GID);
            return Ok(new { result = true, msg = "The sample information has been saved, please check in the sample information page." });

        }



        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="WGID"></param>
        /// <param name="LX"></param>
        /// <returns></returns>
        [HttpPost("upload")] 
        public async Task<IActionResult> Upload(string WGID, List<string> arrayLX, List<string> arrayName, string BZ = "")
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
                            WGID = WGID
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


                        for (int i=0; i< arrayLX[0].Split(',').Length;i++)
                        {
                            if (arrayName[0].Split(',')[i].Contains(pf_file.FILENAME))
                            {
                                pf_file.LX = arrayLX[0].Split(',')[i];
                            }
                        }

                        //读取文件MD5码
                        string MD5 = Helper.checkMD5(formFile.OpenReadStream());
                        pf_file.MD5 = MD5;
                        tempTitle = pf_file.FILENAME.Replace(" ", "_");
                        //文件路径
                        string GID = Guid.NewGuid().ToString();
                        pf_file.GID = GID;
                        string xdlj = WebPath.FILE_REALTE + tempTitle + "_" + GID;//相对路径
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
                        _context.Add(pf_file);
                        Log.Write(GetType(), "PF_FILE", GID + "文件保存");
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

        public class LX
        {
            public string name { get; set; }
            public string lx { get; set; }
        }



    }
}
