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

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/user_org")]
    public class API_PF_USER_ORG : Controller
    {
        private readonly drugdbContext _context;

        public API_PF_USER_ORG(drugdbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 获取PF_USER_ORG数据列表
        /// </summary>
        /// <param name="ORG_GID"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchfield"></param>
        /// <param name="searchword"></param>
        /// <param name="field"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet]
        public ResultList<VM_PF_USER_ORG> Get([FromQuery]string ORG_GID, int page = 1, int limit = 10, string searchfield = "USER_NAME", string searchword = "", string field = "CREATE_DATE", string order = "DESC")
        {

            searchfield = string.IsNullOrEmpty(searchfield) ? "USER_NAME" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = _context.PF_USER_ORG
            .Where(("ORG_GID.Contains(@0) and is_delete == false"), ORG_GID)
            .Join(_context.PF_PROFILE, T1 => T1.USER_NAME, T2 => T2.DLZH, (T1, T2) => new { T1.GID, T1.USER_GID, T1.USER_NAME, T1.ORG_NAME, USER_REALNAME = T2.NAME, CREATE_DATE = T1.CREATE_DATE.ToString("yyyy-MM-dd"), T1.OPERATOR })
            .OrderBy(field + " " + order)
            .Where(searchfield + ".Contains(@0)", searchword);
            return new ResultList<VM_PF_USER_ORG>
            {
                TotalCount = queryResult.Count(),
                Results = queryResult.Skip((page - 1) * limit).Take(limit).ToList()
            };

        }

        // GET: api/API_PF_USER_ORG/5
        /// <summary>
        /// 获取PF_USER_ORG数据详情
        ///</summary>
        /// <returns>api/API_PF_USER_ORG视图模型</returns>
        [HttpGet("{GID}")]
        public async Task<IActionResult> Get([FromRoute] string GID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PF_USER_ORG queryResult = await _context.PF_USER_ORG.SingleOrDefaultAsync(m => m.GID == GID);

            if (queryResult == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PF_USER_ORG, VM_PF_USER_ORG>(queryResult));
        }

        // PUT: api/API_PF_USER_ORG/5
        /// <summary>
        /// 更新单条PF_USER_ORG数据
        ///</summary>
        /// <returns>执行结果反馈</returns>
        [HttpPut("{GID}")]
        public async Task<IActionResult> Update([FromRoute] string GID, [FromForm] VM_PF_USER_ORG postData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (GID != postData.GID)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            try
            {
                PF_USER_ORG entity = Mapper.Map<VM_PF_USER_ORG, PF_USER_ORG>(postData);
                _context.Update(entity);
                await _context.SaveChangesAsync<VM_PF_USER_ORG>();
                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PF_USER_ORGExists(postData.GID))
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

        // POST: api/API_PF_USER_ORG
        /// <summary>
        /// 新增单条PF_USER_ORG数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PF_USER_ORG postData)
        {
            ModelState.Remove("CREATE_DATE");
            ModelState.Remove("MODIFY_DATE");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string userRealName = "未命名";
            int hasDocument = _context.PF_PROFILE.Where(w => w.CODE == postData.USER_NAME).Count();
            if (hasDocument > 0)
            {
                userRealName = _context.PF_PROFILE.Where(w => w.CODE == postData.USER_NAME).First().NAME;
            }
            postData.BZ1 = userRealName;
            postData.BZ2 = "";
            postData.GID = Guid.NewGuid().ToString().ToLower();
            postData.CREATE_DATE = DateTime.Now;
            postData.MODIFY_DATE = DateTime.Now;
            postData.OPERATOR = Permission.getCurrentUser();
            postData.IS_DELETE = false;
            _context.PF_USER_ORG.Add(postData);
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException)
            {
                if (PF_USER_ORGExists(postData.GID))//此处请按照业务要求，将字段更改为需要判断非重复的字段，如用户名、客户编号等
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }

        }

        // DELETE: api/API_PF_USER_ORG/5
        /// <summary>
        /// 删除单条PF_USER_ORG数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpDelete("{GID?}")]
        public async Task<IActionResult> Delete([FromForm] string GID)
        {
            //更新删除标记模式
            PF_USER_ORG pf_user_org = await _context.PF_USER_ORG.SingleOrDefaultAsync(m => m.GID == GID);
            pf_user_org.IS_DELETE = true;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PF_USER_ORGExists(pf_user_org.GID))
                {
                    return Ok(new { msg = "数据不存在或已删除" });
                }
                else
                {
                    return Ok(new { msg = ex.Message });
                }
            }
        }


        /// <summary>
        /// 批量删除指定GID的数据
        /// </summary>
        /// <param name="gid">待删除数据的gid集合，以分号隔开</param>
        /// <returns></returns>
        [HttpDelete, Route("bulkdelete")]
        public async Task<IActionResult> bulkDelete([FromForm] string gids)
        {
            string[] GIDs = gids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string queryIn = "(";
            foreach (var GID in GIDs)
            {
                queryIn += "'" + GID + "'";
            }
            queryIn += ")";
            int x = await _context.Database.ExecuteSqlCommandAsync("Update PF_USER_ORG set IS_DELETE=1,MODIFY_DATE=getdate() where GID in " + queryIn.Replace("''", "','"));
            if (x > -1)
            {
                return Ok(new { success = "true", msg = "成功删除" + x + "条数据" });
            }
            else
            {
                return Ok(new { success = "true", msg = "未删除数据" });

            }
        }

        private bool PF_USER_ORGExists(string GID)
        {
            return _context.PF_USER_ORG.Any(e => e.GID == GID);
        }
    }
}
