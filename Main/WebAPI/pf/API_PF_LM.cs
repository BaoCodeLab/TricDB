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
    [Route("api/lm")]
    public class API_PF_LM : Controller
    {
        private readonly drugdbContext _context;

        public API_PF_LM(drugdbContext context)
        {
            _context = context;
        }

        // GET: api/API_PF_LM
        /// <summary>
        /// 获取PF_LM数据列表
        ///</summary>
        /// <returns>api/API_PF_LM视图模型</returns>
        [HttpGet]
        public IActionResult Get([FromQuery]int page = 1, int limit = 10, string searchfield = "NAME", string searchword = "", string field = "CREATE_DATE", string order = "DESC")
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:LMSZ"))
            {
                return Forbid();
            }
            searchfield = string.IsNullOrEmpty(searchfield) ? "NAME" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = _context.PF_LM
           .Where(searchfield + ".Contains(@0) and IS_DELETE==false and GID!=@1", searchword, "000000000")
           .OrderBy(field + " " + order)//按条件排序
           .Skip((page - 1) * limit) //跳过前x项
           .Take(limit)//从当前位置开始取前x项
           .ToList();//将结果转为List类型
            return Ok(new ResultList<VM_PF_LM>
            {
                TotalCount = _context.PF_LM.Where(searchfield + ".Contains(@0) and IS_DELETE==false and GID!=@1 ", searchword, "000000000").Count(),
                Results = Mapper.Map<List<PF_LM>, List<VM_PF_LM>>(queryResult)
            });
        }

        // GET: api/API_PF_LM/5
        /// <summary>
        /// 获取PF_LM数据详情
        ///</summary>
        /// <returns>api/API_PF_LM视图模型</returns>
        [HttpGet("{GID}")]
        public async Task<IActionResult> Get([FromRoute] string GID)
        {
            if (!Permission.check(HttpContext, "LMSZ:DETAIL"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PF_LM queryResult = await _context.PF_LM.SingleOrDefaultAsync(m => m.GID == GID);

            if (queryResult == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PF_LM, VM_PF_LM>(queryResult));
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllLM()
        {
            if (!Permission.check(HttpContext, "MENU:DIR:WZGL"))
            {
                return Forbid();
            }
            var queryResult = _context.PF_LM
           .Where("is_delete == false")
           .ToList();//将结果转为List类型
            return Ok(new ResultList<VM_PF_LM>
            {
                StateCode = 5001,
                TotalCount = queryResult.Count(),
                Results = Mapper.Map<List<PF_LM>, List<VM_PF_LM>>(queryResult)
            });
        }

        // PUT: api/API_PF_LM/5
        /// <summary>
        /// 更新单条PF_LM数据
        ///</summary>
        /// <returns>执行结果反馈</returns>
        [HttpPost("{GID}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] string GID, VM_PF_LM postData)
        {
            if (!Permission.check(HttpContext, "LMSZ:EDIT"))
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
                PF_LM entity = Mapper.Map<VM_PF_LM, PF_LM>(postData);
                entity.MODIFY_DATE = DateTime.Now;
                entity.OPERATOR = Permission.getCurrentUser();
                _context.Update(entity);
                await _context.SaveChangesAsync<VM_PF_LM>();
                Log.Write(GetType(), "update", "PF_LM", "将栏目gid为" + postData.GID + "的数据进行更新，操作者为" + Permission.getCurrentUser());

                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PF_LMExists(postData.GID))
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

        // POST: api/API_PF_LM
        /// <summary>
        /// 新增单条PF_LM数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PF_LM postData)
        {
            if (!Permission.check(HttpContext, "LMSZ:ADD"))
            {
                return Forbid();
            }
            ModelState.Remove("CREATE_DATE");
            ModelState.Remove("MODIFY_DATE");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            postData.GID = Guid.NewGuid().ToString().ToLower();
            postData.CREATE_DATE = DateTime.Now;
            postData.MODIFY_DATE = DateTime.Now;
            postData.OPERATOR = Permission.getCurrentUser();
            postData.IS_DELETE = false;
            _context.PF_LM.Add(postData);
            try
            {
                await _context.SaveChangesAsync();
                Log.Write(GetType(), "create", "PF_LM", "创建gid为" + postData.GID + "的栏目信息，操作者为" + Permission.getCurrentUser());
                return Ok();
            }
            catch (DbUpdateException)
            {
                if (PF_LMExists(postData.GID))//此处请按照业务要求，将字段更改为需要判断非重复的字段，如用户名、客户编号等
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

        }

        // DELETE: api/API_PF_LM/5
        /// <summary>
        /// 删除单条PF_LM数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpDelete("{GID?}")]
        public async Task<IActionResult> Delete([FromForm] string GID)
        {
            if (!Permission.check(HttpContext, "LMSZ:DEL"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            PF_LM pF_LM = await _context.PF_LM.SingleOrDefaultAsync(m => m.GID == GID);
            pF_LM.IS_DELETE = true;
            Log.Write(_context,GetType(), "delete", "PF_LM", "将栏目gid为" + GID + "的数据的删除状态置为true，操作者为" + Permission.getCurrentUser());

            if (pF_LM == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            var queryResult = _context.PF_NEW.Where(m => m.LM_GID ==GID);
            foreach(var item in queryResult)
            {
                item.LM_GID = "000000000";
                Log.Write(_context,GetType(), "update", "PF_NEW", "将文章lm_gid为" + GID + "的数据的栏目gid置为000000000，操作者为" + Permission.getCurrentUser());

            }
            await _context.SaveChangesAsync();
           
            return Ok(new { success = "true" });
        }

        private bool PF_LMExists(string GID)
        {
            return _context.PF_LM.Any(e => e.GID == GID);
        }
    }
}
