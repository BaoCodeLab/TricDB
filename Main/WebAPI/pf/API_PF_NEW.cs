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
using Main.ViewModels.PF;
using Main.platform;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/new")]
    public class API_PF_NEW : Controller
    {
        private readonly drugdbContext _context;

        public API_PF_NEW(drugdbContext context)
        {
            _context = context;
        }

        // GET: api/API_PF_NEW
        /// <summary>  
        /// 获取PF_NEW数据列表
        ///</summary>
        /// <returns>api/API_PF_NEW视图模型</returns>
        [HttpGet]
        public IActionResult Get([FromQuery]int page = 1, int limit = 10, string searchfield = "TITLE", string searchword = "", string field = "PUBLISH_TIME", string order = "DESC")
        {
            if (!Permission.check(HttpContext, "MENU:DIR:WZGL"))
            {
                return Forbid();
            }
            var user = Permission.getCurrentUser();
            searchfield = string.IsNullOrEmpty(searchfield) ? "TITLE" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            if (Permission.getCurrentUserRoles().Contains("admin"))
            {
                user = "";
            }
            var queryResult = _context.PF_NEW
           .Where(searchfield + ".Contains(@0) and IS_DELETE==false and AUTHOR.Contains(@1) and STATE==\"1\" ", searchword,user)
           .OrderBy(field + " " + order);
            return Ok(new ResultList<VM_PF_NEW>
            {
                TotalCount = queryResult.Count(),
                Results = Mapper.Map<List<PF_NEW>, List<VM_PF_NEW>>(queryResult.Skip((page - 1) * limit).Take(limit).ToList())
            });
        }

        [HttpGet, Route("getbylm")]
        public IActionResult GetByLM([FromQuery]string LM_GID, string searchfield = "TITLE", string searchword = "", int page = 1, int limit = 10, string field = "PUBLISH_TIME", string order = "DESC")
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:YLWZ"))
            {
                return Forbid();
            }
            LM_GID = string.IsNullOrEmpty(LM_GID) ? "0" : LM_GID;
            searchfield = string.IsNullOrEmpty(searchfield) ? "TITLE" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = _context.PF_NEW
           .Where(searchfield + ".Contains(@0) and LM_GID==@1 and IS_DELETE==false and STATE==\"1\" ", searchword, LM_GID)
           .OrderBy(field + " " + order)//按条件排序
           .Skip((page - 1) * limit) //跳过前x项s
           .Take(limit)//从当前位置开始取前x项
           .ToList();//将结果转为List类型
            return Ok(new ResultList<VM_PF_NEW>
            {
                TotalCount = _context.PF_NEW.Where(searchfield + ".Contains(@0) and LM_GID==@1 and IS_DELETE==false and STATE==\"1\" ", searchword, LM_GID).Count(),
                Results = Mapper.Map<List<PF_NEW>, List<VM_PF_NEW>>(queryResult)
            });
        }


        [HttpGet, Route("getdraft")]
        public IActionResult GetDraft([FromQuery]int page = 1, int limit = 10, string searchfield = "TITLE", string searchword = "", string field = "PUBLISH_TIME", string order = "DESC")
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:CGX"))
            {
                return Forbid();
            }
            searchfield = string.IsNullOrEmpty(searchfield) ? "TITLE" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = _context.PF_NEW
           .Where(searchfield + ".Contains(@0) and IS_DELETE==false and OPERATOR==@1 and STATE==\"0\" ", searchword, Permission.getCurrentUser())
           .OrderBy(field + " " + order);//按条件排序

            return Ok( new ResultList<VM_PF_NEW>
            {
                TotalCount = queryResult.Count(),
                Results = Mapper.Map<List<PF_NEW>, List<VM_PF_NEW>>(queryResult.Skip((page - 1) * limit).Take(limit).ToList())
            });
        }
        [HttpGet, Route("getfive")]
        public IActionResult GetFive(string searchfield = "LM_GID", string searchword = "", string field = "PUBLISH_TIME", string order = "DESC")
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:YLWZ"))
            {
                return Forbid();
            }
            searchfield = string.IsNullOrEmpty(searchfield) ? "LM_GID" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = _context.PF_NEW
           .Where(searchfield + ".Contains(@0) and IS_DELETE==false and STATE==\"1\" ", searchword)
           .OrderBy(field + " " + order)//按条件排序
           .Take(5)//从当前位置开始取前x项
           .ToList();//将结果转为List类型
            return Ok(new ResultList<VM_PF_NEW>
            {
                Results = Mapper.Map<List<PF_NEW>, List<VM_PF_NEW>>(queryResult)
            });
        }
        // GET: api/API_PF_NEW/5
        /// <summary>
        /// 获取PF_NEW数据详情
        ///</summary>
        /// <returns>api/API_PF_NEW视图模型</returns>
        [HttpGet("{GID}")]
        public async Task<IActionResult> Get([FromRoute] string GID)
        {
            if (!Permission.check(HttpContext, "GLWZ:DETAIL"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PF_NEW queryResult = await _context.PF_NEW.SingleOrDefaultAsync(m => m.GID == GID);

            if (queryResult == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PF_NEW, VM_PF_NEW>(queryResult));
        }

        // PUT: api/API_PF_NEW/5
        /// <summary>
        /// 更新单条PF_NEW数据
        ///</summary>
        /// <returns>执行结果反馈</returns>
        [HttpPost("{GID}"), Route("update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string GID, [FromForm] VM_PF_NEW postData, [FromForm] string[] role, string lm, string content_new)
        {
            if (!Permission.check(HttpContext, "GLWZ:EDIT"))
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
                PF_NEW entity = Mapper.Map<VM_PF_NEW, PF_NEW>(postData);
                entity.LM_GID = lm;
                entity.MODIFY_DATE = DateTime.Now;
                entity.CONTENT = content_new;
                entity.OPERATOR = Permission.getCurrentUser();
                _context.Update(entity);
                await _context.SaveChangesAsync<VM_PF_NEW>();
                Log.Write(_context,GetType(), "update", "PF_NEW", "将文章表GID为" + entity.GID + "的数据进行更新，操作者为" + Permission.getCurrentUser());


                _context.PF_NEW_ROLE.RemoveRange(_context.PF_NEW_ROLE.Where("NEW_GID==@0 ", GID));
                Log.Write(_context,GetType(), "delete", "PF_NEW_ROLE", "将文章角色表中NEW_GID为" + GID + "的数据的删除，操作者为" + Permission.getCurrentUser());

                foreach (var new_role in role)
                {
                    PF_NEW_ROLE pf_new_role = new PF_NEW_ROLE
                    {
                        GID = Guid.NewGuid().ToString().ToLower(),
                        NEW_GID = postData.GID,
                        ROLE_GID = new_role,
                        CREATE_DATE = DateTime.Now,
                        MODIFY_DATE = DateTime.Now,
                        OPERATOR = Permission.getCurrentUser(),
                        IS_DELETE = false
                    };
                    _context.PF_NEW_ROLE.Add(pf_new_role);
                    Log.Write(_context,GetType(), "create", "PF_NEW_ROLE", "创建gid为" + pf_new_role.GID + "的文章角色对照关系，操作者为" + Permission.getCurrentUser());

                }
                await _context.SaveChangesAsync();
                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PF_NEWExists(postData.GID))
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

        // POST: api/API_PF_NEW
        /// <summary>
        /// 新增单条PF_NEW数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpPost]
        public async Task<IActionResult> Create(string gid, PF_NEW postData, string lm, string[] role)
        {
            if (!Permission.check(HttpContext, "GLWZ:ADD"))
            {
                return Forbid();
            }
            ModelState.Remove("CREATE_DATE");
            ModelState.Remove("MODIFY_DATE");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            postData.GID = gid;
            postData.LM_GID = lm;
            postData.AUTHOR = Permission.getCurrentUser();
            postData.VI_TIME = 0;
            postData.CREATE_DATE = DateTime.Now;
            postData.MODIFY_DATE = DateTime.Now;
            postData.OPERATOR = Permission.getCurrentUser();
            postData.IS_DELETE = false;
            _context.PF_NEW.Add(postData);
            Log.Write(_context,GetType(), "create", "PF_NEW", "创建gid为" + postData.GID + "的文章数据，操作者为" + Permission.getCurrentUser());

            foreach (var arole in role)
            {
                PF_NEW_ROLE pf_new_role = new PF_NEW_ROLE
                {
                    GID = Guid.NewGuid().ToString().ToLower(),
                    NEW_GID = gid,
                    ROLE_GID = arole,
                    CREATE_DATE = DateTime.Now,
                    MODIFY_DATE = DateTime.Now,
                    OPERATOR = Permission.getCurrentUser(),
                    IS_DELETE = false
                };
                _context.PF_NEW_ROLE.Add(pf_new_role);
                Log.Write(_context, GetType(), "create", "PF_NEW_ROLE", "创建gid为" + pf_new_role.GID + "的文章角色对照关系，操作者为" + Permission.getCurrentUser());

            }
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { StatusCode = 5001, msg = "添加成功！" });
            }
            catch (DbUpdateException)
            {                      
                   throw;               
            }

        }

        // DELETE: api/API_PF_NEW/5
        /// <summary>
        /// 删除单条PF_NEW数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpPost,Route("delete")]
        public async Task<IActionResult> Delete([FromForm] string gid)
        {
            if (!Permission.check(HttpContext, "GLWZ:DEL"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            PF_NEW pF_NEW = await _context.PF_NEW.SingleOrDefaultAsync(m => m.GID == gid);
            if (pF_NEW == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            pF_NEW.IS_DELETE = true;
            Log.Write(_context,GetType(), "delete", "PF_NEW", "将文章表中gid为" + gid + "的数据的删除状态置为true，操作者为" + Permission.getCurrentUser());

            var queryResult = _context.PF_NEW_ROLE
            .Where("NEW_GID==@0 and IS_DELETE==false", gid);//将结果转为List类型
            foreach (var new_role in queryResult)
            {
                new_role.IS_DELETE = true;
                Log.Write(_context,GetType(), "delete", "PF_NEW_ROLE", "将文章角色表中NEW_GID为" + gid + "的数据的删除状态置为true，操作者为" + Permission.getCurrentUser());

            }

            await _context.SaveChangesAsync();
            return Ok(new { success = "true" });
        }


        [HttpPost("{GID?}"), Route("publish")]
        public async Task<IActionResult> Publish([FromForm] string GID)
        {
            if (!Permission.check(HttpContext, "WZFB:PUBLISH"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            PF_NEW pF_NEW = await _context.PF_NEW.SingleOrDefaultAsync(m => m.GID == GID);
            if (pF_NEW == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            pF_NEW.STATE = "1";
            pF_NEW.PUBLISH_TIME = DateTime.Now;
            pF_NEW.MODIFY_DATE = DateTime.Now;
            Log.Write(GetType(), "update", "PF_NEW", "将文章表GID为" + pF_NEW.GID + "的数据的STATE置为1，操作者为" + Permission.getCurrentUser());


            await _context.SaveChangesAsync();
            return Ok(new { success = "true" });
        }



        [HttpGet, Route("distributerole")]
        public IActionResult getRoleDistribute(string newid)
        {
            if (!Permission.check(HttpContext, "GLWZ:EDIT"))
            {
                return Forbid();
            }

            //获取文章
            PF_NEW pf_new = _context.PF_NEW.SingleOrDefault(m => m.GID == newid && !m.IS_DELETE);
            if (pf_new == null)
            {
                return Ok(new ResultList<RoleDistribute>
                {
                    StateCode = 5002,
                    Message = "文章不存在！",
                });
            }
            //获取所有角色
            var PF_ROLE = _context.PF_ROLE.Where(m => !m.IS_DELETE);
            //获取新闻分配给的角色
            var queryResult = from s in _context.PF_NEW
                              join c in _context.PF_NEW_ROLE on s.GID equals c.NEW_GID
                              join d in _context.PF_ROLE on c.ROLE_GID equals d.CODE
                              where s.IS_DELETE == false
                              && c.IS_DELETE == false
                              && d.IS_DELETE == false
                              && s.GID == newid
                              select new
                              {
                                  code = d.CODE,
                                  name = d.NAME,
                              };

            //赋值
            List<String> newRole = new List<string>();
            foreach (var role in queryResult)
            {
                newRole.Add(role.code);
            }
            List<RoleDistribute> role_distribute = new List<RoleDistribute>();
            bool check = false;
            foreach (var role in PF_ROLE)
            {
                check = false;
                //找寻是否属于新闻
                if (newRole.Contains(role.CODE))
                {
                    check = true;
                }

                role_distribute.Add(new RoleDistribute
                {
                    CODE = role.CODE,
                    NAME = role.NAME,
                    CHECK = check

                });
            }

            return Ok(new ResultList<RoleDistribute>
            {
                StateCode = 5001,
                Message = "获取成功",
                Results = role_distribute
            });

        }

        [HttpGet, Route("getLM")]
        public IActionResult getLM(string newid)
        {
            if (!Permission.check(HttpContext, "GLWZ:EDIT"))
            {
                return Forbid();
            }
            PF_NEW pf_new = _context.PF_NEW.SingleOrDefault(m => m.GID == newid && !m.IS_DELETE);
            if (pf_new == null)
            {
                return Ok(new ResultList<VM_LMDistribute>
                {
                    StateCode = 5002,
                    Message = "文章不存在！",
                });
            }
            var queryResult = from s in _context.PF_NEW
                              join c in _context.PF_LM on s.LM_GID equals c.GID
                              where s.IS_DELETE == false
                              && c.IS_DELETE == false
                              && s.GID == newid
                              select new
                              {
                                  code = c.GID,
                                  name = c.NAME,
                              };
            var pf_lms = _context.PF_LM.Where(m => !m.IS_DELETE);
            string newLM = queryResult.Count() == 0 ? newLM = "000000000" : queryResult.First().code.ToString();



            List<VM_LMDistribute> lm_distribute = new List<VM_LMDistribute>();
            bool check = false;
            foreach (var lm in pf_lms)
            {
                check = false;
                //找寻是否属于新闻
                if (newLM.Equals(lm.GID))
                {
                    check = true;
                }

                lm_distribute.Add(new VM_LMDistribute
                {
                    CODE = lm.GID,
                    NAME = lm.NAME,
                    CHECK = check

                });
            }
            return Ok(new ResultList<VM_LMDistribute>
            {
                StateCode = 5001,
                Message = "获取成功",
                Results = lm_distribute
            });

        }

        [HttpPost, Route("addtime")]
        public async Task<IActionResult> Addtime(string gid)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:YLWZ"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            PF_NEW pF_NEW = await _context.PF_NEW.SingleOrDefaultAsync(m => m.GID == gid);
            if (pF_NEW == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            pF_NEW.VI_TIME = pF_NEW.VI_TIME + 1;
            await _context.SaveChangesAsync();
            return Ok(new { success = "true" });

        }


        private bool PF_NEWExists(string GID)
        {
            return _context.PF_NEW.Any(e => e.GID == GID);
        }
    }
}
