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
    [Route("api/assay")]
    public class API_BUS_ASSAY : Controller
    {
        private readonly drugdbContext _context;

        public API_BUS_ASSAY(drugdbContext context)
        {
            _context = context;
        }

        // GET: api/API_BUS_ASSAY
        /// <summary>
        /// 获取BUS_ASSAY数据列表
        ///</summary>
        /// <returns>api/API_BUS_ASSAY视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult Get([FromQuery]int page = 1, int limit = 10, string searchfield = "ASSAYCODE", string searchword = "", string field = "ASSAYCODE", string order = "DESC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchfield = string.IsNullOrEmpty(searchfield) ? "ASSAYCODE" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = _context.BUS_ASSAY.Where(m => m.VERSION.Equals(culture))
           .Where(searchfield + ".Contains(@0)", searchword)
           .OrderBy(field + " " + order)//按条件排序
           .Skip((page - 1) * limit) //跳过前x项
           .Take(limit)//从当前位置开始取前x项
           .ToList();//将结果转为List类型
            return Ok(new ResultList<VM_BUS_ASSAY>
            {
                TotalCount = _context.BUS_ASSAY.Where(m => !m.IS_DELETE && m.IS_PUB && m.VERSION.Equals(culture)).Where(searchfield + ".Contains(@0)", searchword).Count(),
                Results = Mapper.Map<List<BUS_ASSAY>, List<VM_BUS_ASSAY>>(queryResult)
            });
        }
        /// <summary>
        /// 获取BUS_ASSAY数据列表
        ///</summary>
        /// <returns>api/API_BUS_ASSAY视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult SearchAll([FromQuery]int page = 1, int limit = 10, string searchword = "", string field = "ASSAYCODE", string order = "DESC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = this._context.BUS_ASSAY.Where(m => m.IS_PUB && !m.IS_DELETE && m.VERSION.Equals(culture))
           .Where(m => m.ASSAYCODE.Contains(searchword) || m.SEQ_ASSAY_ID.Equals(searchword) || m.CALLING_STRATEGY.Equals(searchword) || m.CENTER.Equals(searchword) || m.COVERAGE.Equals(searchword) || m.INSTRUMENT_MODEL.Equals(searchword) || m.LIBRARY_SELECTION.Equals(searchword) || m.LIBRARY_SELECTION.Equals(searchword) || m.SPECIMEN_TUMOR_CELLULARITY.Equals(searchword) || m.TARGET_CAPTURE_KIT.Equals(searchword)).AsQueryable();
            var ret = queryResult.OrderBy(field + " " + order)//按条件排序
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
            return Ok(new ResultList<BUS_ASSAY>
            {
                TotalCount = queryResult.Count(),
                Results = ret
            });
        }

        [HttpGet, Route("Detail")]
        public async Task<IActionResult> Detail([FromQuery] string SEQ_ASSAY_ID)
        {
            BUS_ASSAY queryResult = await _context.BUS_ASSAY.SingleOrDefaultAsync(m => m.SEQ_ASSAY_ID == SEQ_ASSAY_ID);

            if (queryResult == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<BUS_ASSAY, VM_BUS_ASSAY>(queryResult));
        }

        // PUT: api/API_BUS_ASSAY/5
        /// <summary>
        /// 更新单条BUS_ASSAY数据
        ///</summary>
        /// <returns>执行结果反馈</returns>
        [HttpPost, Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(VM_BUS_ASSAY postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:PTBJ"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                BUS_ASSAY entity = Mapper.Map<VM_BUS_ASSAY, BUS_ASSAY>(postData);
                entity.MODIFY_DATE = DateTime.Now;
                entity.OPERATOR = Permission.getCurrentUser();
                _context.BUS_ASSAY.Update(entity);
                await _context.SaveChangesAsync<VM_BUS_ASSAY>();
                Log.Write(GetType(), "update", "BUS_ASSAY", "将测序平台编号为" + postData.SEQ_ASSAY_ID + "的数据进行更新，操作者为" + Permission.getCurrentUser());

                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!BUS_ASSAYExists(postData.SEQ_ASSAY_ID))
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

        // POST: api/API_BUS_ASSAY
        /// <summary>
        /// 新增单条BUS_ASSAY数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BUS_ASSAY postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:PTXZ"))
            {
                return Forbid();
            }
            ModelState.Remove("CREATE_DATE");
            ModelState.Remove("MODIFY_DATE");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            postData.SEQ_ASSAY_ID = Guid.NewGuid().ToString().ToLower();
            postData.CREATE_DATE = DateTime.Now;
            postData.MODIFY_DATE = DateTime.Now;
            postData.OPERATOR = Permission.getCurrentUser();
            postData.IS_DELETE = false;
            _context.BUS_ASSAY.Add(postData);
            try
            {
                await _context.SaveChangesAsync();
                Log.Write(GetType(), "create", "BUS_ASSAY", "创建测序平台编号为" + postData.SEQ_ASSAY_ID + "，操作者为" + Permission.getCurrentUser());
                return Ok();
            }
            catch (DbUpdateException)
            {
                if (BUS_ASSAYExists(postData.SEQ_ASSAY_ID))//此处请按照业务要求，将字段更改为需要判断非重复的字段，如用户名、客户编号等
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

        }

        // DELETE: api/API_BUS_ASSAY/5
        /// <summary>
        /// 删除单条BUS_ASSAY数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpDelete("{DRUGID?}")]
        public async Task<IActionResult> Delete([FromForm] string SEQ_ASSAY_ID)
        {
            if (!Permission.check(HttpContext, "OPERATE:PTSC"))
            {
                return Forbid();
            }
            BUS_ASSAY BUS_ASSAY = await _context.BUS_ASSAY.SingleOrDefaultAsync(m => m.SEQ_ASSAY_ID == SEQ_ASSAY_ID);
            if (BUS_ASSAY == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            if (StateHelper.getCodeByName("删除模式", "直接删除").Equals("1"))
            {
                _context.BUS_ASSAY.Remove(BUS_ASSAY);
            }
            else
            {
                BUS_ASSAY.IS_DELETE = true;
            }
            Log.Write(_context, GetType(), "delete", "BUS_ASSAY", "将测序平台编号" + BUS_ASSAY.SEQ_ASSAY_ID + "的数据的删除，操作者为" + Permission.getCurrentUser());
            await _context.SaveChangesAsync();

            return Ok(new { success = "true" });
        }

        private bool BUS_ASSAYExists(string SEQ_ASSAY_ID)
        {
            return _context.BUS_ASSAY.Any(e => e.SEQ_ASSAY_ID == SEQ_ASSAY_ID);
        }
    }
}
