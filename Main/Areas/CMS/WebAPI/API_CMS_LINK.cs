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
namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/cms/link")]
    public class API_CMS_LINK : Controller
    {
        private readonly drugdbContext _context;

        public API_CMS_LINK(drugdbContext context)
        {
            _context = context;
        }
        [HttpGet, Route("all")]
        public List<VM_CMS_LINK> GetAll()
        {
            if (!Permission.check(HttpContext, "OPERATE:QBYQLJ"))
            {
                return new List<VM_CMS_LINK>();
            }
            var mData = (from d in _context.CMS_LINK where d.IS_DELETE == false select d).ToList();
            var vmData = Mapper.Map<List<CMS_LINK>, List<VM_CMS_LINK>>(mData);
            return vmData;
        }
        // GET: api/link
        [HttpGet]
        public ResultList<VM_CMS_LINK> Get([FromQuery]int page = 1, int limit = 10, string searchfield = "", string searchword = "", string field = "CREATE_DATE", string order = "DESC")
        {
            if (!Permission.check(HttpContext, "MENU:YQLJGL"))
            {
                return new ResultList<VM_CMS_LINK> { TotalCount=0,Results=null };
            }
            searchfield = string.IsNullOrEmpty(searchfield) ? "LINKID" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            string searchtype = ".Contains(@0)";
            if (typeof(CMS_LINK).GetProperty(searchfield).PropertyType != typeof(System.String))
            {
                searchtype = "=@0";
            }
            var query = _context.CMS_LINK.Where((searchfield + searchtype + " and is_delete == false"), searchword);
            var result = query.OrderBy(field + " " + order).Skip((page - 1) * limit).Take(limit).ToList();
            return new ResultList<VM_CMS_LINK>
            {
                TotalCount = query.Count(),
                Results = Mapper.Map<List<CMS_LINK>, List<VM_CMS_LINK>>(result)
            };

        }

        // GET: api/link/LINKID
        [HttpGet("{LINKID}")]
        public async Task<IActionResult> Get([FromRoute] string LINKID)
        {
            if (!Permission.check(HttpContext, "OPERATE:YQLJXQ"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CMS_LINK queryResult = await _context.CMS_LINK.SingleOrDefaultAsync(m => m.LINKID == LINKID);

            if (queryResult == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<CMS_LINK, VM_CMS_LINK>(queryResult));
        }

        // PUT: api/link/LINKID
        [HttpPut("{LINKID}")]
        public async Task<IActionResult> Update([FromRoute] string LINKID, [FromForm] VM_CMS_LINK postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:BJYQLJ"))
            {
                return Forbid();
            }
            if (LINKID != postData.LINKID)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            try
            {
                if (string.IsNullOrEmpty(postData.INDEXPIC)) postData.INDEXPIC = string.Empty;
                if (string.IsNullOrEmpty(postData.LINKTEXT)) postData.LINKTEXT = string.Empty;
                if (string.IsNullOrEmpty(postData.LINKURL)) postData.LINKURL = string.Empty;
                if (string.IsNullOrEmpty(postData.PRECSS)) postData.PRECSS = string.Empty;
                if (string.IsNullOrEmpty(postData.BZ)) postData.BZ = string.Empty;
                if (string.IsNullOrEmpty(postData.LINKTYPE)) postData.LINKTYPE = string.Empty;
                postData.MODIFY_DATE = DateTime.Now;
                postData.OPERATOR = Permission.getCurrentUser();
                CMS_LINK entity = Mapper.Map<VM_CMS_LINK, CMS_LINK>(postData);
                _context.Update(entity);
                await _context.SaveChangesAsync<VM_CMS_LINK>();
                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CMS_LINKExists(postData.LINKID))
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

        // POST: api/link
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] VM_CMS_LINK postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:XZYQLJ"))
            {
                return Forbid();
            }
            postData.LINKID = Guid.NewGuid().ToString();
            postData.OPERATOR = Permission.getCurrentUser();
            postData.MODIFY_DATE = DateTime.Now;
            postData.CREATE_DATE = DateTime.Now;
            postData.IS_DELETE = false;
            if (string.IsNullOrEmpty(postData.INDEXPIC)) postData.INDEXPIC = string.Empty;
            if (string.IsNullOrEmpty(postData.LINKTEXT)) postData.LINKTEXT = string.Empty;
            if (string.IsNullOrEmpty(postData.LINKURL)) postData.LINKURL = string.Empty;
            if (string.IsNullOrEmpty(postData.PRECSS)) postData.PRECSS = string.Empty;
            if (string.IsNullOrEmpty(postData.LINKTYPE)) postData.LINKTYPE = string.Empty;
            if (string.IsNullOrEmpty(postData.BZ)) postData.BZ = string.Empty;
            var data = Mapper.Map<VM_CMS_LINK, CMS_LINK>(postData);
            _context.CMS_LINK.Add(data);
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException ex)
            {
                if (CMS_LINKExists(postData.LINKID))
                {
                    return BadRequest(WebAPIErrorMsg.Failure("已存在此编号数据，请检查"));
                }
                else
                {
                    return BadRequest(WebAPIErrorMsg.Failure(ex.Message));
                }
            }
        }

        // DELETE: api/link/LINKID
        [HttpDelete("{LINKID?}")]
        public async Task<IActionResult> Delete([FromForm] string LINKID)
        {
            if (!Permission.check(HttpContext, "OPERATE:SCYQLJ"))
            {
                return Forbid();
            }
            CMS_LINK cms_link = await _context.CMS_LINK.SingleOrDefaultAsync(m => m.LINKID == LINKID);
            cms_link.IS_DELETE = true;
            cms_link.OPERATOR =Permission.getCurrentUser();
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CMS_LINKExists(cms_link.LINKID))
                {
                    return Ok(new { msg = "数据不存在或已删除" });
                }
                else
                {
                    return Ok(new { msg = ex.Message });
                }
            }
        }


        // 批量删除指定LINKID的数据
        [HttpDelete, Route("bulkdelete")]
        public async Task<IActionResult> bulkDelete([FromForm] string LINKIDs)
        {
            if (!Permission.check(HttpContext, "OPERATE:SCYQLJ"))
            {
                return Forbid();
            }
            string[] _LINKIDs = LINKIDs.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var list = _context.CMS_LINK.Where(w => _LINKIDs.Contains(w.LINKID)).ToList();
            foreach (var item in list)
            {
                item.IS_DELETE = true;
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

        private bool CMS_LINKExists(string LINKID)
        {
            return _context.CMS_LINK.Any(e => e.LINKID == LINKID);
        }
    }
}
