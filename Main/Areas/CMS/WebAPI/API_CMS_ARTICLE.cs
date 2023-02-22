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

namespace Main.Areas.CMS.WebAPI
{
    [Produces("application/json")]
    [Route("api/cms/article")]
    public class API_CMS_ARTICLE : Controller
    {
        private readonly drugdbContext _context;

        public API_CMS_ARTICLE(drugdbContext context)
        {
            _context = context;
        }
        [HttpGet, Route("all")]
        public List<VM_CMS_ARTICLE> GetAll()
        {
            var mData = (from d in _context.CMS_ARTICLE where d.IS_DELETE == false select d).ToList();
            var vmData = Mapper.Map<List<CMS_ARTICLE>, List<VM_CMS_ARTICLE>>(mData);
            return vmData;
        }
        // GET: api/article
        [HttpGet]
        public ResultList<VM_CMS_ARTICLE> Get([FromQuery]int page = 1, int limit = 10, string channelid = "", string searchfield = "", string searchword = "", string field = "CREATE_DATE", string order = "DESC")
        {
            if (string.IsNullOrEmpty(channelid)) channelid = Guid.NewGuid().ToString();
            searchfield = string.IsNullOrEmpty(searchfield) ? "ARTICLEID" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            string searchtype = ".Contains(@0)";
            if (typeof(CMS_ARTICLE).GetProperty(searchfield).PropertyType != typeof(System.String))
            {
                searchtype = "=@0";
            }
            var query = _context.CMS_ARTICLE.Where((searchfield + searchtype + " and is_delete == false and channelid=@1"), searchword, channelid);
            if (!User.IsInRole("admin") && !User.IsInRole("系统管理员"))
            {
                query = query.Where(m => m.OPERATOR.Equals(User.Identity.Name));
            }
            var result = query.OrderBy(field + " " + order).Skip((page - 1) * limit).Take(limit).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                result[i].ARTICLECONTENT = string.Empty;
            }
            return new ResultList<VM_CMS_ARTICLE>
            {
                TotalCount = query.Count(),
                Results = Mapper.Map<List<CMS_ARTICLE>, List<VM_CMS_ARTICLE>>(result)
            };
        }

        // GET: api/article/ARTICLEID
        [HttpGet("{ARTICLEID}")]
        public async Task<IActionResult> Get([FromRoute] string ARTICLEID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CMS_ARTICLE queryResult = await _context.CMS_ARTICLE.SingleOrDefaultAsync(m => m.ARTICLEID == ARTICLEID);

            if (queryResult == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<CMS_ARTICLE, VM_CMS_ARTICLE>
                (queryResult));
        }

        // PUT: api/article/ARTICLEID
        [HttpPut("{ARTICLEID}")]
        public async Task<IActionResult> Update([FromRoute] string ARTICLEID, [FromForm] VM_CMS_ARTICLE postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:WZBJ"))
            {
                return NotFound();
            }
            if (ARTICLEID != postData.ARTICLEID)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            try
            {
                CMS_ARTICLE data = Mapper.Map<VM_CMS_ARTICLE, CMS_ARTICLE>(postData);
                if (string.IsNullOrEmpty(data.ARTICLECONTENT)) data.ARTICLECONTENT = string.Empty;
                if (string.IsNullOrEmpty(data.ARTICLEEDITOR)) data.ARTICLEEDITOR = Permission.getCurrentUser();
                if (string.IsNullOrEmpty(data.ARTICLEINDEXPIC)) data.ARTICLEINDEXPIC = string.Empty;
                if (string.IsNullOrEmpty(data.ARTICLEKEYWORDS)) data.ARTICLEKEYWORDS = string.Empty;
                if (string.IsNullOrEmpty(data.ARTICLEREDIRECT)) data.ARTICLEREDIRECT = string.Empty;
                if (string.IsNullOrEmpty(data.ARTICLETITLE)) data.ARTICLETITLE = string.Empty;
                if (string.IsNullOrEmpty(data.BZ)) data.BZ = string.Empty;
                _context.Update(data);
                await _context.SaveChangesAsync<VM_CMS_ARTICLE>();
                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CMS_ARTICLEExists(postData.ARTICLEID))
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

        // POST: api/article
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] VM_CMS_ARTICLE postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:WZXZ"))
            {
                return NotFound();
            }
            var data = Mapper.Map<VM_CMS_ARTICLE, CMS_ARTICLE>(postData);
            if (string.IsNullOrEmpty(data.ARTICLECONTENT)) data.ARTICLECONTENT = string.Empty;
            if (string.IsNullOrEmpty(data.ARTICLEEDITOR)) data.ARTICLEEDITOR = Permission.getCurrentUser();
            if (string.IsNullOrEmpty(data.ARTICLEINDEXPIC)) data.ARTICLEINDEXPIC = string.Empty;
            if (string.IsNullOrEmpty(data.ARTICLEKEYWORDS)) data.ARTICLEKEYWORDS = string.Empty;
            if (string.IsNullOrEmpty(data.ARTICLEREDIRECT)) data.ARTICLEREDIRECT = string.Empty;
            if (string.IsNullOrEmpty(data.ARTICLETITLE)) data.ARTICLETITLE = string.Empty;
            if (string.IsNullOrEmpty(data.BZ)) data.BZ = string.Empty;
            _context.CMS_ARTICLE.Add(data);
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException ex)
            {
                if (CMS_ARTICLEExists(postData.ARTICLEID))
                {
                    return BadRequest(WebAPIErrorMsg.Failure("已存在此编号数据，请检查"));
                }
                else
                {
                    return BadRequest(WebAPIErrorMsg.Failure(ex.Message));
                }
            }

        }
        [HttpPost, Route("[action]")]
        public async Task<IActionResult> Audit([FromForm] string ARTICLEID)
        {
            if (!Permission.check(HttpContext, "OPERATE:WZSH"))
            {
                return NotFound();
            }
            CMS_ARTICLE cms_article = await _context.CMS_ARTICLE.SingleOrDefaultAsync(m => m.ARTICLEID == ARTICLEID);
            cms_article.ISPUB = !cms_article.ISPUB;
            try
            {
                await _context.SaveChangesAsync();
                Log.Write(this.GetType(), "CMS_ARTICLE", "审核文章" + ARTICLEID + "到状态" + cms_article.ISPUB);
                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CMS_ARTICLEExists(cms_article.ARTICLEID))
                {
                    return Ok(new { msg = "数据不存在或已删除" });
                }
                else
                {
                    return Ok(new { msg = ex.Message });
                }
            }
        }
        // DELETE: api/article/ARTICLEID
        [HttpPost, Route("delete")]
        public async Task<IActionResult> Delete([FromForm] string ARTICLEID)
        {
            if (!Permission.check(HttpContext, "OPERATE:WZSC"))
            {
                return Forbid();
            }
            CMS_ARTICLE cms_article = await _context.CMS_ARTICLE.SingleOrDefaultAsync(m => m.ARTICLEID == ARTICLEID);
            if (StateHelper.getCodeByName("删除模式", "直接删除").Equals("1"))
            {
                _context.CMS_ARTICLE.Remove(cms_article);
            }
            else
            {
                cms_article.IS_DELETE = true;
            }
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CMS_ARTICLEExists(cms_article.ARTICLEID))
                {
                    return Ok(new { msg = "数据不存在或已删除" });
                }
                else
                {
                    return Ok(new { msg = ex.Message });
                }
            }
        }


        // 批量删除指定@primaryKeyName的数据
        [HttpPost, Route("bulkdelete")]
        public async Task<IActionResult> bulkDelete([FromForm] string ARTICLEIDs)
        {
            if (!Permission.check(HttpContext, "OPERATE:WZSC"))
            {
                return NotFound();
            }
            string[] ARTICLEID_Array = ARTICLEIDs.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var list = _context.CMS_ARTICLE.Where(w => ARTICLEID_Array.Contains(w.ARTICLEID)).ToList();
            foreach (var item in list)
            {
                if (StateHelper.getCodeByName("删除模式", "直接删除").Equals("1"))
                {
                    _context.CMS_ARTICLE.Remove(item);
                }
                else
                {
                    item.IS_DELETE = true;
                }
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

        private bool CMS_ARTICLEExists(string ARTICLEID)
        {
            return _context.CMS_ARTICLE.Any(e => e.ARTICLEID == ARTICLEID);
        }
    }
}
