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
    [Route("api/state")]
    public class API_PF_STATE : Controller
    {
        private readonly drugdbContext _context;

        public API_PF_STATE(drugdbContext context)
        {
            _context = context;
        }

        // GET: api/API_PF_STATE
        /// <summary>
        /// 获取PF_STATE数据列表
        ///
        ///</summary>
        /// <returns>api/API_PF_STATE视图模型</returns>
        [HttpGet]
        public IActionResult Get([FromQuery]int page = 1, int limit = 10, string searchfield = "NAME", string searchword = "", string field = "CREATE_DATE", string order = "DESC",string type="")
        {
            if (!Permission.check(HttpContext, "MENU:PF_STATE"))
            {
                return Forbid();
            }
            searchfield = string.IsNullOrEmpty(searchfield) ? "NAME" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = _context.PF_STATE
            .Where((searchfield + ".Contains(@0) and TYPE.Contains(@1) and is_delete == false"), searchword,type)
            .OrderBy(field + " " + order)//按条件排序
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
            return Ok( new ResultList<VM_PF_STATE>
            {
                TotalCount = _context.PF_STATE.Where(searchfield + ".Contains(@0) and TYPE.Contains(@1) and is_delete == false", searchword, type).Count(),
                Results = Mapper.Map<List<PF_STATE>, List<VM_PF_STATE>>(queryResult)
            });
        }

        // GET: api/API_PF_STATE/5
        /// <summary>
        /// 获取PF_STATE数据详情
        ///</summary>
        /// <returns>api/API_PF_STATE视图模型</returns>
        [HttpGet("{GID}")]
        public async Task<IActionResult> Get([FromRoute] string GID)
        {
            if (!Permission.check(HttpContext, "MENU:PF_STATE"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PF_STATE queryResult = await _context.PF_STATE.SingleOrDefaultAsync(m => m.GID == GID);

            if (queryResult == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PF_STATE, VM_PF_STATE>(queryResult));
        }

        // PUT: api/API_PF_STATE/5
        /// <summary>
        /// 更新单条PF_STATE数据
        ///</summary>
        /// <returns>执行结果反馈</returns>
        [HttpPost("update/{GID}")]
        public async Task<IActionResult> Update([FromRoute] string GID, [FromForm] VM_PF_STATE postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:DMBJ"))
            {
                return Forbid();
            }
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
                var entity = _context.PF_STATE.Where(m => m.GID == GID).ToList().First();
                entity.CODE = postData.CODE;
                entity.NAME = postData.NAME;
                entity.ORDERS = postData.ORDERS;
                entity.MODIFY_DATE = DateTime.Now;
                await _context.SaveChangesAsync();
                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PF_STATEExists(postData.GID))
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

        // POST: api/API_PF_STATE
        /// <summary>
        /// 新增单条PF_STATE数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpPost,Route("create")]
        public async Task<IActionResult> Create([FromForm] PF_STATE postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:DMXZ"))
            {
                return Forbid();
            }
            ModelState.Remove("CREATE_DATE");
            ModelState.Remove("MODIFY_DATE");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var queryresult = _context.PF_STATE.Where(x => x.NAME == postData.NAME && x.IS_DELETE==false && x.TYPE==postData.TYPE);
            if (queryresult.Count() > 0)
            {
                return Ok(new {stateCode="9002",msg="状态代码已存在" });
            }          
            postData.GID = Guid.NewGuid().ToString().ToLower();
            postData.CODE = postData.CODE;
            postData.NAME = postData.NAME;
            postData.ORDERS = postData.ORDERS;
            postData.CREATE_DATE = DateTime.Now;
            postData.MODIFY_DATE = DateTime.Now;
            postData.OPERATOR = Permission.getCurrentUser();
            postData.IS_DELETE = false;
            _context.PF_STATE.Add(postData);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new {stateCode="9001" });
            }
            catch (DbUpdateException)
            {
                if (PF_STATEExists(postData.GID))//此处请按照业务要求，将字段更改为需要判断非重复的字段，如用户名、客户编号等
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

        }

        // DELETE: api/API_PF_STATE/5
        /// <summary>
        /// 删除单条PF_STATE数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpDelete("{GID?}"),Route("delete")]
        public async Task<IActionResult> Delete([FromForm] string GID)
        {
            if (!Permission.check(HttpContext, "OPERATE:DMSC"))
            {
                return Forbid();
            }
            //更新删除标记模式
            PF_STATE pf_state = await _context.PF_STATE.SingleOrDefaultAsync(m => m.GID == GID);
            pf_state.IS_DELETE = true;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PF_STATEExists(pf_state.GID))
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
            if (!Permission.check(HttpContext, "OPERATE:DMSC"))
            {
                return Forbid();
            }
            string[] GIDs = gids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var list = _context.PF_STATE.Where(w => GIDs.Contains(w.GID)).ToList();
            foreach (var item in list)
            {
                item.IS_DELETE = true;
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

        private bool PF_STATEExists(string GID)
        {
            return _context.PF_STATE.Any(e => e.GID == GID);
        }
    }
}
