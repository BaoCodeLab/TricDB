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
    [Route("api/cna_hg")]
    public class API_BUS_CNA_HG : Controller
    {
        private readonly drugdbContext _context;

        public API_BUS_CNA_HG(drugdbContext context)
        {
            _context = context;
        }

        // GET: api/API_BUS_CNA_HG
        /// <summary>
        /// 获取BUS_CNA_HG数据列表
        ///</summary>
        /// <returns>api/API_BUS_CNA_HG视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult Get([FromQuery]int page = 1, int limit = 10, string searchfield = "CNACODE", string searchword = "", string field = "CNACODE", string order = "DESC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchfield = string.IsNullOrEmpty(searchfield) ? "CNACODE" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = _context.BUS_CNA_HG.Where(m => m.VERSION.Equals(culture))
           .Where(searchfield + ".Contains(@0)", searchword)
           .OrderBy(field + " " + order)//按条件排序
           .Skip((page - 1) * limit) //跳过前x项
           .Take(limit)//从当前位置开始取前x项
           .ToList();//将结果转为List类型
            return Ok(new ResultList<VM_BUS_CNA_HG>
            {
                TotalCount = _context.BUS_CNA_HG.Where(m => !m.IS_DELETE && m.IS_PUB && m.VERSION.Equals(culture)).Where(searchfield + ".Contains(@0)", searchword).Count(),
                Results = Mapper.Map<List<BUS_CNA_HG>, List<VM_BUS_CNA_HG>>(queryResult)
            });
        }
        /// <summary>
        /// 获取BUS_CNA_HG数据列表
        ///</summary>
        /// <returns>api/API_BUS_CNA_HG视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult SearchAll([FromQuery]int page = 1, int limit = 10, string searchword = "", string field = "CNACODE", string order = "DESC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = this._context.BUS_CNA_HG.Where(m => m.IS_PUB && !m.IS_DELETE && m.VERSION.Equals(culture))
           .Where(m => m.CNACODE.Contains(searchword) || m.CNA_ID.Equals(searchword) || m.CHROM.Equals(searchword) || m.LOCASTART.Equals(searchword) || m.LOCEND.Equals(searchword)).AsQueryable();
            var ret = queryResult.OrderBy(field + " " + order)//按条件排序
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
            return Ok(new ResultList<BUS_CNA_HG>
            {
                TotalCount = queryResult.Count(),
                Results = ret
            });
        }

        [HttpGet, Route("Detail")]
        public async Task<IActionResult> Detail([FromQuery] string CNA_ID)
        {
            BUS_CNA_HG queryResult = await _context.BUS_CNA_HG.SingleOrDefaultAsync(m => m.CNA_ID == CNA_ID);

            if (queryResult == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<BUS_CNA_HG, VM_BUS_CNA_HG>(queryResult));
        }

        // PUT: api/API_BUS_CNA_HG/5
        /// <summary>
        /// 更新单条BUS_CNA_HG数据
        ///</summary>
        /// <returns>执行结果反馈</returns>
        [HttpPost, Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(VM_BUS_CNA_HG postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:BRBJ"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                BUS_CNA_HG entity = Mapper.Map<VM_BUS_CNA_HG, BUS_CNA_HG>(postData);
                entity.MODIFY_DATE = DateTime.Now;
                entity.OPERATOR = Permission.getCurrentUser();
                _context.BUS_CNA_HG.Update(entity);
                await _context.SaveChangesAsync<VM_BUS_CNA_HG>();
                Log.Write(GetType(), "update", "BUS_CNA_HG", "将CNA编号为" + postData.CNA_ID + "的数据进行更新，操作者为" + Permission.getCurrentUser());

                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!BUS_CNA_HGExists(postData.CNA_ID))
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

        // POST: api/API_BUS_CNA_HG
        /// <summary>
        /// 新增单条BUS_CNA_HG数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BUS_CNA_HG postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:BRXZ"))
            {
                return Forbid();
            }
            ModelState.Remove("CREATE_DATE");
            ModelState.Remove("MODIFY_DATE");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            postData.CNA_ID = Guid.NewGuid().ToString().ToLower();
            postData.CREATE_DATE = DateTime.Now;
            postData.MODIFY_DATE = DateTime.Now;
            postData.OPERATOR = Permission.getCurrentUser();
            postData.IS_DELETE = false;
            _context.BUS_CNA_HG.Add(postData);
            try
            {
                await _context.SaveChangesAsync();
                Log.Write(GetType(), "create", "BUS_CNA_HG", "创建CNA编号为" + postData.CNA_ID + "，操作者为" + Permission.getCurrentUser());
                return Ok();
            }
            catch (DbUpdateException)
            {
                if (BUS_CNA_HGExists(postData.CNA_ID))//此处请按照业务要求，将字段更改为需要判断非重复的字段，如用户名、客户编号等
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

        }

        // DELETE: api/API_BUS_CNA_HG/5
        /// <summary>
        /// 删除单条BUS_CNA_HG数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpDelete("{DRUGID?}")]
        public async Task<IActionResult> Delete([FromForm] string CNA_ID)
        {
            if (!Permission.check(HttpContext, "OPERATE:BRSC"))
            {
                return Forbid();
            }
            BUS_CNA_HG BUS_CNA_HG = await _context.BUS_CNA_HG.SingleOrDefaultAsync(m => m.CNA_ID == CNA_ID);
            if (BUS_CNA_HG == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            if (StateHelper.getCodeByName("删除模式", "直接删除").Equals("1"))
            {
                _context.BUS_CNA_HG.Remove(BUS_CNA_HG);
            }
            else
            {
                BUS_CNA_HG.IS_DELETE = true;
            }
            Log.Write(_context, GetType(), "delete", "BUS_CNA_HG", "将CNA编号" + BUS_CNA_HG.CNA_ID + "的数据的删除，操作者为" + Permission.getCurrentUser());
            await _context.SaveChangesAsync();

            return Ok(new { success = "true" });
        }

        private bool BUS_CNA_HGExists(string CNA_ID)
        {
            return _context.BUS_CNA_HG.Any(e => e.CNA_ID == CNA_ID);
        }
    }
}
