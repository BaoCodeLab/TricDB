using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Model;
using Main.ViewModels;
using AutoMapper;
using TDSCoreLib;
using Main.platform;
using Main.Extensions;
using Main.Utils;
using System.Data;
using System.Data.OleDb;
using System.IO;
using ExcelDataReader;
using System.Web;
using MySql.Data.MySqlClient;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/clinical_sample")]
    public class API_BUS_CLINICAL_SAMPLE : Controller
    {
        private readonly drugdbContext _context;

        public API_BUS_CLINICAL_SAMPLE(drugdbContext context)
        {
            _context = context;
        }

        // GET: api/API_BUS_CLINICAL_SAMPLE
        /// <summary>
        /// 获取BUS_CLINICAL_SAMPLE数据列表
        ///</summary>
        /// <returns>api/API_BUS_CLINICAL_SAMPLE视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult Get([FromQuery]int page = 1, int limit = 10, string searchfield = "SAMPLECODE", string searchword = "", string field = "SAMPLECODE", string order = "DESC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchfield = string.IsNullOrEmpty(searchfield) ? "SAMPLECODE" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = _context.BUS_CLINICAL_SAMPLE.Where(m => m.VERSION.Equals(culture))
           .Where(searchfield + ".Contains(@0) and IS_DELETE==false", searchword)
           .OrderBy(field + " " + order)//按条件排序
           .Skip((page - 1) * limit) //跳过前x项
           .Take(limit)//从当前位置开始取前x项
           .ToList();//将结果转为List类型
            return Ok(new ResultList<VM_BUS_CLINICAL_SAMPLE>
            {
                TotalCount = _context.BUS_CLINICAL_SAMPLE.Where(searchfield + ".Contains(@0) and IS_DELETE==false", searchword).Count(),
                Results = Mapper.Map<List<BUS_CLINICAL_SAMPLE>, List<VM_BUS_CLINICAL_SAMPLE>>(queryResult)
            });
        }
        /// <summary>
        /// 获取BUS_CLINICAL_SAMPLE数据列表
        ///</summary>
        /// <returns>api/API_BUS_CLINICAL_SAMPLE视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult SearchAll([FromQuery]int page = 1, int limit = 10, string searchword = "", string field = "SAMPLECODE", string order = "DESC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = this._context.BUS_CLINICAL_SAMPLE.Where(m => m.VERSION.Equals(culture) && m.IS_PUB && !m.IS_DELETE)
           .Where(m => m.CANCER_TYPE_DETAILED.Contains(searchword) || m.SAMPLECODE.Equals(searchword) || m.CANCER_TYPE.Equals(searchword) || m.CANCER_TYPE_DETAILED.Equals(searchword) || m.SAMPLE_TYPE.Contains(searchword) || m.SAMPLE_TYPE_DETAILED.Contains(searchword) || m.SEQ_ASSAY_ID.Equals(searchword)).AsQueryable();
            var ret = queryResult.OrderBy(field + " " + order)//按条件排序
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
            return Ok(new ResultList<BUS_CLINICAL_SAMPLE>
            {
                TotalCount = queryResult.Count(),
                Results = ret
            });
        }

        [HttpGet, Route("Detail")]
        public async Task<IActionResult> Detail([FromQuery] string SAMPLE_ID)
        {
            BUS_CLINICAL_SAMPLE queryResult = await _context.BUS_CLINICAL_SAMPLE.SingleOrDefaultAsync(m => m.SAMPLE_ID == SAMPLE_ID);

            if (queryResult == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<BUS_CLINICAL_SAMPLE, VM_BUS_CLINICAL_SAMPLE>(queryResult));
        }

        // PUT: api/API_BUS_CLINICAL_SAMPLE/5
        /// <summary>
        /// 更新单条BUS_CLINICAL_SAMPLE数据
        ///</summary>
        /// <returns>执行结果反馈</returns>
        [HttpPost, Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(VM_BUS_CLINICAL_SAMPLE postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:YWBJ"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                BUS_CLINICAL_SAMPLE entity = Mapper.Map<VM_BUS_CLINICAL_SAMPLE, BUS_CLINICAL_SAMPLE>(postData);
                entity.MODIFY_DATE = DateTime.Now;
                entity.OPERATOR = Permission.getCurrentUser();
                _context.BUS_CLINICAL_SAMPLE.Update(entity);
                await _context.SaveChangesAsync<VM_BUS_CLINICAL_SAMPLE>();
                Log.Write(GetType(), "update", "BUS_CLINICAL_SAMPLE", "将样本编号为" + postData.SAMPLECODE + "的数据进行更新，操作者为" + Permission.getCurrentUser());

                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!BUS_CLINICAL_SAMPLEExists(postData.SAMPLE_ID))
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

        // POST: api/API_BUS_CLINICAL_SAMPLE
        /// <summary>
        /// 新增单条BUS_CLINICAL_SAMPLE数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BUS_CLINICAL_SAMPLE postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:YWXZ"))
            {
                return Forbid();
            }
            ModelState.Remove("CREATE_DATE");
            ModelState.Remove("MODIFY_DATE");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //postData.DID = Guid.NewGuid().ToString().ToLower();
            postData.PATIENT_ID = postData.PATIENT_ID ?? "";
            postData.AGE_AT_SEQ_REPORT = postData.AGE_AT_SEQ_REPORT ?? "";
            postData.ONCOTREE_CODE = postData.ONCOTREE_CODE ?? "";
            postData.SAMPLE_TYPE = postData.SAMPLE_TYPE ?? "";
            postData.SEQ_ASSAY_ID = postData.SEQ_ASSAY_ID ?? "";
            postData.CANCER_TYPE = postData.CANCER_TYPE ?? "";
            postData.CANCER_TYPE_DETAILED = postData.CANCER_TYPE_DETAILED ?? "";
            postData.SAMPLE_TYPE_DETAILED = postData.SAMPLE_TYPE_DETAILED ?? "";
            postData.CREATE_DATE = DateTime.Now;
            postData.MODIFY_DATE = DateTime.Now;
            postData.OPERATOR = Permission.getCurrentUser();
            postData.IS_PUB = postData.IS_PUB;
            postData.IS_DELETE = false;
            _context.BUS_CLINICAL_SAMPLE.Add(postData);
            try
            {
                await _context.SaveChangesAsync();
                Log.Write(GetType(), "create", "BUS_CLINICAL_SAMPLE", "创建样本编号为" + postData.SAMPLECODE + "，操作者为" + Permission.getCurrentUser());
                return Ok();
            }
            catch (DbUpdateException)
            {
                if (BUS_CLINICAL_SAMPLEExists(postData.SAMPLE_ID))//此处请按照业务要求，将字段更改为需要判断非重复的字段，如用户名、客户编号等
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

        }

        // DELETE: api/API_BUS_CLINICAL_SAMPLE/5
        /// <summary>
        /// 删除单条BUS_CLINICAL_SAMPLE数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpDelete("{SAMPLE_ID?}")]
        public async Task<IActionResult> Delete([FromForm] string SAMPLE_ID)
        {
            if (!Permission.check(HttpContext, "OPERATE:YWSC"))
            {
                return Forbid();
            }
            BUS_CLINICAL_SAMPLE BUS_CLINICAL_SAMPLE = await _context.BUS_CLINICAL_SAMPLE.SingleOrDefaultAsync(m => m.SAMPLE_ID == SAMPLE_ID);
            if (BUS_CLINICAL_SAMPLE == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            if (StateHelper.getCodeByName("删除模式", "直接删除").Equals("1"))
            {
                _context.BUS_CLINICAL_SAMPLE.Remove(BUS_CLINICAL_SAMPLE);
            }
            else
            {
                BUS_CLINICAL_SAMPLE.IS_DELETE = true;
            }
            Log.Write(_context, GetType(), "delete", "BUS_CLINICAL_SAMPLE", "将样本编号" + BUS_CLINICAL_SAMPLE.SAMPLECODE + "的数据的删除，操作者为" + Permission.getCurrentUser());
            await _context.SaveChangesAsync();

            return Ok(new { success = "true" });
        }

        private bool BUS_CLINICAL_SAMPLEExists(string SAMPLE_ID)
        {
            return _context.BUS_CLINICAL_SAMPLE.Any(e => e.SAMPLE_ID == SAMPLE_ID);
        }
    }
}
