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
    [Route("api/anno/file")]
    public class API_ANNO_FILE : Controller
    {
        private readonly drugdbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private IMemoryCache _memoryCache;

        public API_ANNO_FILE(drugdbContext context, IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _memoryCache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
        }


        string user_name = Permission.getCurrentUser();

        // 获取当前用户已上传的FILE
        [HttpGet, Route("GetFile")]
        public IActionResult GetFile([FromQuery] int page = 1, int limit = 10)
        {
            var t = this._context.PF_USER.Where(m => m.USERNAME.Equals(User.Identity.Name)).FirstOrDefault();
            var user_id = t.GID;
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_ANNO_SAMPLE>("select * from anno_sample where user_id=@user_id and is_delete=false", new MySqlParameter[] { new MySqlParameter("user_id", user_id) }).AsQueryable();
            var ret = queryResult.ToList();
            List<PF_FILE> Files = new List<PF_FILE>();
            foreach (var i in ret)
            {
                var fileList = _context.PF_FILE.Where(m => m.WGID == i.ANNO_SAMPLEID && m.IS_DELETE == false).ToList();
                fileList.ForEach(x => Files.Add(x));
            }
            var result = Files.OrderByDescending(d => d.CREATE_DATE).Skip((page - 1) * limit).Take(limit).ToList();
            return Ok(new ResultList<VM_PF_FILE>
            {
                TotalCount = Files.Count(),
                Results = Mapper.Map<List<PF_FILE>, List<VM_PF_FILE>>(result)
            });
        }


        // 验证project是否存在
        private bool ANNO_FileExists(string Gid)
        {
            return _context.PF_FILE.Any(e => e.GID == Gid);
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
                if (!ANNO_FileExists(postData.GID))
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


        // DELETE: api/API_ANNO_FILE/5
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



    }
}
