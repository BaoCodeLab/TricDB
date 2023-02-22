using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Model;
using Main.ViewModels;
using TDSCoreLib;
using System.Linq.Dynamic.Core;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Main.platform;
using Microsoft.AspNetCore.Authorization;
using Main.Extensions;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/cms/ad")]
    public class API_CMS_AD : Controller
    {
        private readonly drugdbContext _context;

        public API_CMS_AD(drugdbContext context)
        {
            _context = context;
        }
        [HttpGet, Route("all")]
        public List<VM_CMS_AD> GetAll()
        {
            if (!Permission.check(HttpContext, "OPERATE:QBWB"))
            {
                return new List<VM_CMS_AD>();
            }
            var mData = (from d in _context.CMS_AD where d.IS_DELETE == false select d).ToList();
            var vmData = Mapper.Map<List<CMS_AD>, List<VM_CMS_AD>>(mData);
            return vmData;
        }
        // GET: api/ad
        [HttpGet]
        public ResultList<VM_CMS_AD> Get([FromQuery]int page = 1, int limit = 10, string searchfield = "", string searchword = "", string field = "CREATE_DATE", string order = "DESC")
        {
            if (!Permission.check(HttpContext, "OPERATE:QBWB"))
            {
                return new ResultList<VM_CMS_AD> { TotalCount = 0, Results = null };
            }
            searchfield = string.IsNullOrEmpty(searchfield) ? "ADID" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            string searchtype = ".Contains(@0)";
            if (typeof(CMS_AD).GetProperty(searchfield).PropertyType != typeof(System.String))
            {
                searchtype = "=@0";
            }
            var query = _context.CMS_AD.Where((searchfield + searchtype + " and is_delete == false"), searchword);
            var result = query.OrderBy(field + " " + order).Skip((page - 1) * limit).Take(limit).ToList();
            return new ResultList<VM_CMS_AD>
            {
                TotalCount = query.Count(),
                Results = Mapper.Map<List<CMS_AD>, List<VM_CMS_AD>>(result)
            };
        }

        // GET: api/ad/ADID
        [HttpGet("{ADID}")]
        public async Task<IActionResult> Get([FromRoute] string ADID)
        {
            if (!Permission.check(HttpContext, "OPERATE:WBXQ"))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CMS_AD queryResult = await _context.CMS_AD.SingleOrDefaultAsync(m => m.ADID == ADID);

            if (queryResult == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<CMS_AD, VM_CMS_AD>(queryResult));
        }

        // PUT: api/ad/ADID
        [HttpPut("{ADID}")]
        public async Task<IActionResult> Update([FromRoute] string ADID, [FromForm] VM_CMS_AD postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:BJWB"))
            {
                return NotFound();
            }

            if (ADID != postData.ADID)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            try
            {
                postData.MODIFY_DATE = DateTime.Now;
                postData.OPERATOR = Permission.getCurrentUser();
                if (string.IsNullOrEmpty(postData.ADNAME)) postData.ADNAME = string.Empty;
                if (string.IsNullOrEmpty(postData.ADTEXT)) postData.ADTEXT = string.Empty;
                if (string.IsNullOrEmpty(postData.BZ)) postData.BZ = string.Empty;
                CMS_AD entity = Mapper.Map<VM_CMS_AD, CMS_AD>(postData);
                _context.Update(entity);
                await _context.SaveChangesAsync<VM_CMS_AD>();
                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CMS_ADExists(postData.ADID))
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

        // POST: api/ad
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] VM_CMS_AD postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:XZWB"))
            {
                return NotFound();
            }
            postData.IS_DELETE = false;
            postData.CREATE_DATE = DateTime.Now;
            postData.MODIFY_DATE = DateTime.Now;
            postData.OPERATOR = Permission.getCurrentUser();
            if (string.IsNullOrEmpty(postData.ADNAME)) postData.ADNAME = string.Empty;
            if (string.IsNullOrEmpty(postData.ADTEXT)) postData.ADTEXT = string.Empty;
            if (string.IsNullOrEmpty(postData.BZ)) postData.BZ = string.Empty;
            var data = Mapper.Map<VM_CMS_AD, CMS_AD>(postData);
            _context.CMS_AD.Add(data);
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException ex)
            {
                if (CMS_ADExists(postData.ADID))
                {
                    return BadRequest(WebAPIErrorMsg.Failure("已存在此编号数据，请检查"));
                }
                else
                {
                    return BadRequest(WebAPIErrorMsg.Failure(ex.Message));
                }
            }
        }

        // DELETE: api/ad/ADID
        [HttpDelete("{ADID?}")]
        public async Task<IActionResult> Delete([FromForm] string ADID)
        {
            if (!Permission.check(HttpContext, "OPERATE:SCWB"))
            {
                return NotFound();
            }
            CMS_AD cms_ad = await _context.CMS_AD.SingleOrDefaultAsync(m => m.ADID == ADID);
            if (StateHelper.getCodeByName("删除模式", "直接删除").Equals("1"))
            {
                _context.CMS_AD.Remove(cms_ad);
            }
            else
            {
                cms_ad.IS_DELETE = true;
            }
            cms_ad.MODIFY_DATE = DateTime.Now;
            cms_ad.OPERATOR = Permission.getCurrentUser();
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CMS_ADExists(cms_ad.ADID))
                {
                    return Ok(new { msg = "数据不存在或已删除" });
                }
                else
                {
                    return Ok(new { msg = ex.Message });
                }
            }
        }


        // 批量删除指定ADID的数据
        [HttpDelete, Route("bulkdelete")]
        public async Task<IActionResult> bulkDelete([FromForm] string gids)
        {
            if (!Permission.check(HttpContext, "OPERATE:SCWB"))
            {
                return NotFound();
            }
            string[] ADIDs = gids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var list = _context.CMS_AD.Where(w => ADIDs.Contains(w.ADID)).ToList();
            foreach (var item in list)
            {
                item.IS_DELETE = true;
                item.MODIFY_DATE = DateTime.Now;
                item.OPERATOR = Permission.getCurrentUser();
            }
            int x = await _context.SaveChangesAsync();
            if (x > -1)
            {
                return Ok(new { success = "true", msg = "成功删除" + x + "条数据" });
            }
            else
            {
                return Ok(new { success = "true", msg = "删除失败" });

            }
        }

        private bool CMS_ADExists(string ADID)
        {
            return _context.CMS_AD.Any(e => e.ADID == ADID);
        }
    }
}
