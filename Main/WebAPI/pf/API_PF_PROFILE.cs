using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model.Model;
using Main.ViewModels;
using TDSCoreLib;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Main.platform;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.WebAPI
{
    /// <summary>
    /// 用户档案
    /// </summary>
    [Produces("application/json")]
    [Route("api/profile")]
    public class API_PF_PROFILE : Controller

    {
        private readonly drugdbContext _context;
        public API_PF_PROFILE(drugdbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Get([FromQuery]int page = 1, int limit = 5, string searchfield = "CODE", string searchword = "", string field = "CREATE_DATE", string order = "DESC")
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:XSRYDAGL"))
            {
                return Forbid();
            }
            searchfield = string.IsNullOrEmpty(searchfield) ? "CODE" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;

            var queryResult = _context.PF_PROFILE
            .Where(searchfield + ".Contains(@0) and IS_DELETE == false ", searchword)
            .OrderBy(field + " " + order)//按条件排序
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
            return Ok(new ResultList<VM_PF_PROFILE>
            {
                TotalCount = _context.PF_PROFILE.Where(searchfield + ".Contains(@0) and IS_DELETE == false ", searchword).Count(),
                Results = Mapper.Map<List<PF_PROFILE>, List<VM_PF_PROFILE>>(queryResult)
            });
        }

        // GET: api/yh/5
        [HttpGet("{gid}")]
        public async Task<IActionResult> Get([FromRoute] string gid)
        {
            if (!Permission.check(HttpContext, "XSRYDAGL:DETAIL"))
            {
                return Forbid();
            }

            PF_PROFILE profile = await _context.PF_PROFILE.SingleOrDefaultAsync(m => m.GID == gid);

            if (profile == null)
            {
                profile = new PF_PROFILE();
                profile.GID = gid;
            }

            return Ok(Mapper.Map<PF_PROFILE, VM_PF_PROFILE>(profile));
        }

        // POST: api/yh/5
        [HttpPost, Route("update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] VM_PF_PROFILE postData)
        {
            if (!Permission.check(HttpContext, "XSRYDAGL:EDIT"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                PF_PROFILE entity = Mapper.Map<VM_PF_PROFILE, PF_PROFILE>(postData);
                entity.AGE = (byte)(DateTime.Now.Year - entity.SR.Year);
                entity.MODIFY_DATE = DateTime.Now;
                if (string.IsNullOrEmpty(entity.GRAH)) entity.GRAH = string.Empty;
                if (string.IsNullOrEmpty(entity.TXDZ)) entity.TXDZ = string.Empty;
                if (string.IsNullOrEmpty(entity.TXDZ)) entity.TXDZ = string.Empty;
                if (string.IsNullOrEmpty(entity.PHONE)) entity.PHONE = string.Empty;
                _context.Update(entity);
                await _context.SaveChangesAsync<VM_PF_PROFILE>();
                Log.Write(GetType(), "update", "PF_PROFILE", "将人员档案gid为" + postData.GID + "的数据进行更新，操作者为" + Permission.getCurrentUser());

                return Ok(new { StatusCode = 2001 });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PF_PROFILEExists(postData.GID))
                {
                    return Ok(new { msg = "数据不存在或已删除" });
                }
                else
                {
                    return Ok(new { msg = ex.Message });
                }
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Update_ISDelete(string gid)
        {
            if (!Permission.check(HttpContext, "XSRYDAGL:DEL"))
            {
                return Forbid();
            }
            PF_PROFILE profile = await _context.PF_PROFILE.SingleOrDefaultAsync(m => m.GID == gid);
            profile.MODIFY_DATE = DateTime.Now;
            profile.IS_DELETE = true;

            try
            {
                await _context.SaveChangesAsync();
                Log.Write(GetType(), "delete", "PF_PROFILE", "将销售人员档案gid为" + gid + "的数据的删除状态置为true，操作者为" + Permission.getCurrentUser());

                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PF_PROFILEExists(profile.GID))
                {
                    return Ok(new { msg = "数据不存在或已删除" });
                }
                else
                {
                    return Ok(new { msg = ex.Message });
                }
            }
        }

        [HttpPost, Route("create")]
        public async Task<IActionResult> Create([FromForm] PF_PROFILE profile)
        {
            if (!Permission.check(HttpContext, "XSRYDAGL:ADD"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var exist = _context.PF_PROFILE.Where(m => m.CODE == profile.CODE && m.IS_DELETE == false);
                profile.AGE = (byte)(DateTime.Now.Year - profile.SR.Year);
                profile.OPERATOR = Permission.getCurrentUser();
                profile.CREATE_DATE = DateTime.Now;
                profile.MODIFY_DATE = DateTime.Now;
                profile.BZ = "";
                profile.DLZH = profile.CODE;
                if (exist.Count() == 0)
                {
                    profile.BZ = "";
                    PF_USER pf_user = _context.PF_USER.SingleOrDefault("USERNAME==@0 and is_delete == false ", profile.DLZH);
                    if (pf_user != null)
                    {
                        pf_user.RYBM = profile.CODE;
                        _context.Update(pf_user);
                        Log.Write(GetType(), "update", "PF_USER", "将用户表GID为" + pf_user.GID + "的数据进行更新，操作者为" + Permission.getCurrentUser());
                    }
                    _context.PF_PROFILE.Add(profile);
                    await _context.SaveChangesAsync();
                    Log.Write(GetType(), "create", "PF_PROFILE", "创建gid为" + profile.GID + "的销售人员档案，操作者为" + Permission.getCurrentUser());
                    return Ok(new { StatusCode = 2001 });
                }
                else
                {
                    return Ok(new { msg = "已存在相同联系人" });
                }

            }
            catch (DbUpdateException)
            {
                if (PF_PROFILEExists(profile.GID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }
        }


        private bool PF_PROFILEExists(string gid)
        {
            return _context.PF_PROFILE.Any(e => e.GID == gid);
        }


    }
}
