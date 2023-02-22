using AutoMapper;
using Main.platform;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/form_permission")]
    public class API_FD_FORM_PERMISSION : Controller
    {
        private readonly drugdbContext _context;

        public API_FD_FORM_PERMISSION(drugdbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询全部数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("all")]
        public List<VM_FD_FORM_PERMISSION> GetAll()
        {
            var mData = (from d in _context.FD_FORM_PERMISSION where d.IS_DELETE == false select d).ToList();
            var vmData = Mapper.Map<List<FD_FORM_PERMISSION>, List<VM_FD_FORM_PERMISSION>>(mData);
            return vmData;
        }
        // GET: api/API_FD_FORM_PERMISSION
        /// <summary>
        /// 获取FD_FORM_PERMISSION数据列表
        ///
        ////</summary>
        /// <returns>api/API_FD_FORM_PERMISSION视图模型</returns>
        [HttpGet]
        public IActionResult
            Get([FromQuery]int page = 1, int limit = 10, string searchfield = "", string searchword = "", string field = "CREATE_DATE", string order = "DESC")
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:FORM_PERMISSION"))
            {
                return Forbid();
            }
            else
            {
                searchfield = string.IsNullOrEmpty(searchfield) ? "GID" : searchfield;
                searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
                var queryResult = _context.FD_FORM_PERMISSION
                .Where((searchfield + ".Contains(@0) and is_delete == false"), searchword)
                .OrderBy(field + " " + order)//按条件排序
                .Skip((page - 1) * limit) //跳过前x项
                .Take(limit)//从当前位置开始取前x项
                .ToList();//将结果转为List类型
                return Ok(new ResultList<VM_FD_FORM_PERMISSION>
                {
                    TotalCount = _context.FD_FORM_PERMISSION.Where(searchfield + ".Contains(@0) and is_delete == false", searchword).Count(),
                    Results = Mapper.Map<List<FD_FORM_PERMISSION>, List<VM_FD_FORM_PERMISSION>>(queryResult)
                });
            }
        }

        /// <summary>
        /// 读取表单模板列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchfield"></param>
        /// <param name="searchword"></param>
        /// <param name="field"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet, Route("formlist")]
        public IActionResult Getformlist([FromQuery]int page = 1, int limit = 10, string searchfield = "", string searchword = "", string field = "CREATE_DATE", string order = "DESC")
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:FORM_PERMISSION"))
            {
                return Forbid();
            }
            else
            {
                searchfield = string.IsNullOrEmpty(searchfield) ? "id" : searchfield;
                searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
                var queryResult = _context.FD_FORM
                .Where((searchfield + ".Contains(@0) and is_delete == false"), searchword)
                .OrderBy(field + " " + order)//按条件排序
                .Skip((page - 1) * limit) //跳过前x项
                .Take(limit)//从当前位置开始取前x项
                .ToList();//将结果转为List类型
                return Ok(new ResultList<VM_FD_FORM>
                {
                    TotalCount = _context.FD_FORM.Where(searchfield + ".Contains(@0) and is_delete == false", searchword).Count(),
                    Results = Mapper.Map<List<FD_FORM>, List<VM_FD_FORM>>(queryResult)
                });
            }
        }
        /// <summary>
        /// 读取当前系统角色列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchfield"></param>
        /// <param name="searchword"></param>
        /// <param name="field"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet, Route("rolelist")]
        public IActionResult Getrolelist([FromQuery]int page = 1, int limit = 10, string searchfield = "", string searchword = "", string field = "CREATE_DATE", string order = "DESC")
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:FORM_PERMISSION"))
            {
                return Forbid();
            }
            else
            {
                searchfield = string.IsNullOrEmpty(searchfield) ? "GID" : searchfield;
                searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
                var queryResult = _context.PF_ROLE
                .Where((searchfield + ".Contains(@0) and is_delete == false"), searchword)
                .OrderBy(field + " " + order)//按条件排序
                .Skip((page - 1) * limit) //跳过前x项
                .Take(limit)//从当前位置开始取前x项
                .ToList();//将结果转为List类型
                return Ok(new ResultList<VM_FD_FORM>
                {
                    TotalCount = _context.PF_ROLE.Where(searchfield + ".Contains(@0) and is_delete == false", searchword).Count(),
                    Results = Mapper.Map<List<PF_ROLE>, List<VM_PF_ROLE>>(queryResult)
                });
            }
        }
        /// <summary>
        /// 组织机构树
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="parentId"></param>
        /// <param name="context"></param>
        /// <param name="isLeaf"></param>
        /// <param name="level"></param>
        /// <param name="spread"></param>
        /// <param name="formkey">表单编号</param>
        /// <param name="role_code">角色编号</param>
        /// <returns></returns>
        [HttpPost, Route("orglist"), Authorize]
        public IActionResult Getorglist([FromForm]string nodeId, string parentId, string context, bool isLeaf, int level, string spread, string formkey, string role_code)
        {

            //1、执行查询
            List<PF_ORG> queryResult = _context.PF_ORG
            .Where(w => w.IS_DELETE == false)
            .OrderBy("ORDER asc")//按条件排序
            .ToList();//将结果转为List类型

            List<VM_PF_ORG_Tree> results = new List<VM_PF_ORG_Tree>();
            VM_PF_ORG_Tree ROOT = new VM_PF_ORG_Tree();
            results.Add(ROOT);
            ROOT.GID = "";
            ROOT.TITLE = "机构树";
            ROOT.PATH = "ROOT";
            ROOT.spread = true;
            ROOT.checkArr = new checkArr("0", "0");
            ROOT.isLast = false;
            List<VM_PF_ORG_Tree> trees = new List<VM_PF_ORG_Tree>();
            ROOT.children = trees;
            //2、查询已配置数据，用于默认选中check
            var defData = _context.FD_FORM_PERMISSION.Where(w => w.IS_DELETE == false && w.ROLE_CODE == role_code && w.FORM_KEY == formkey).FirstOrDefault();
            List<string> orgs = new List<string>();
            if (defData != null)
            {
                orgs.AddRange(defData.ORGPATH.Split(',').ToList());
            }
            //3、转换为层级结构
            //查询所有父类菜单
            foreach (PF_ORG query in queryResult)
            {
                if (query.SUPER == "")
                {
                    VM_PF_ORG_Tree PARENT = new VM_PF_ORG_Tree
                    {
                        GID = query.GID,
                        TITLE = query.TITLE,
                        DEPTH = query.DEPTH,
                        PATH = query.PATH,
                        checkArr = orgs.Contains(query.PATH) ? new checkArr("0", "1") : new checkArr("0", "0"),
                        SUPER = query.SUPER,
                        isLast = false
                    };
                    trees.Add(PARENT);
                }
            }

            //嵌套查询子菜单
            foreach (VM_PF_ORG_Tree parent in trees)
            {
                parent.children = childrenSpread(queryResult, parent, orgs);

            }
            return Ok(new
            {
                status = new { code = 200, message = "操作成功" },
                data = results
            });
        }

        // GET: api/API_FD_FORM_PERMISSION/5
        /// <summary>
        /// 获取FD_FORM_PERMISSION数据详情
        ///</summary>
        /// <returns>api/API_FD_FORM_PERMISSION视图模型</returns>
        [HttpGet("{GID}")]
        public async Task<IActionResult> Get([FromRoute] string GID)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:FORM_PERMISSION"))
            {
                return Forbid();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                FD_FORM_PERMISSION queryResult = await _context.FD_FORM_PERMISSION.SingleOrDefaultAsync(m => m.GID == GID);

                if (queryResult == null)
                {
                    return NotFound();
                }

                return Ok(Mapper.Map<FD_FORM_PERMISSION, VM_FD_FORM_PERMISSION>(queryResult));
            }
        }

        // PUT: api/API_FD_FORM_PERMISSION/5
        /// <summary>
        /// 更新单条FD_FORM_PERMISSION数据
        ///</summary>
        /// <returns>执行结果反馈</returns>
        [HttpPut("{GID}")]
        public async Task<IActionResult> Update([FromRoute] string GID, [FromForm] VM_FD_FORM_PERMISSION postData)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:FORM_PERMISSION"))
            {
                return Forbid();
            }
            else
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
                    FD_FORM_PERMISSION entity = Mapper.Map<VM_FD_FORM_PERMISSION, FD_FORM_PERMISSION>(postData);
                    _context.Update(entity);
                    await _context.SaveChangesAsync<VM_FD_FORM_PERMISSION>();
                    return Ok(new { success = "true" });
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!FD_FORM_PERMISSIONExists(postData.GID))
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
        }

        // POST: api/API_FD_FORM_PERMISSION
        /// <summary>
        /// 新增单条FD_FORM_PERMISSION数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] FD_FORM_PERMISSION postData)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:FORM_PERMISSION"))
            {
                return Forbid();
            }
            else
            {
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
                _context.FD_FORM_PERMISSION.Add(postData);
                try
                {
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateException)
                {
                    if (FD_FORM_PERMISSIONExists(postData.GID))//此处请按照业务要求，将字段更改为需要判断非重复的字段，如用户名、客户编号等
                    {
                        return new StatusCodeResult(StatusCodes.Status409Conflict);
                    }
                    else
                    {
                        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                    }
                }
            }

        }

        // DELETE: api/API_FD_FORM_PERMISSION/5
        /// <summary>
        /// 删除单条FD_FORM_PERMISSION数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpDelete("{GID?}")]
        public async Task<IActionResult> Delete([FromForm] string GID)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:FORM_PERMISSION"))
            {
                return Forbid();
            }
            else
            {
                //更新删除标记模式
                FD_FORM_PERMISSION fd_form_permission = await _context.FD_FORM_PERMISSION.SingleOrDefaultAsync(m => m.GID == GID);
                fd_form_permission.IS_DELETE = true;
                try
                {
                    await _context.SaveChangesAsync();
                    return Ok(new { success = "true" });
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!FD_FORM_PERMISSIONExists(fd_form_permission.GID))
                    {
                        return Ok(new { msg = "数据不存在或已删除" });
                    }
                    else
                    {
                        return Ok(new { msg = ex.Message });
                    }
                }
            }
        }


        /// <summary>
        /// 批量删除指定GID的数据
        /// </summary>
        /// <param name="gids">待删除数据的gid集合，以分号隔开</param>
        /// <returns></returns>
        [HttpDelete, Route("bulkdelete")]
        public async Task<IActionResult> bulkDelete([FromForm] string gids)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:FORM_PERMISSION"))
            {
                return Forbid();
            }
            else
            {
                string[] GIDs = gids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                string queryIn = "(";
                foreach (var GID in GIDs)
                {
                    queryIn += "'" + GID + "'";
                }
                queryIn += ")";
                int x = await _context.Database.ExecuteSqlCommandAsync("Update FD_FORM_PERMISSION set IS_DELETE=1,MODIFY_DATE=getdate() where GID in " + queryIn.Replace("''", "','"));
                if (x > -1)
                {
                    return Ok(new { success = "true", msg = "成功删除" + x + "条数据" });
                }
                else
                {
                    return Ok(new { success = "true", msg = "未删除数据" });

                }
            }
        }

        /// <summary>
        /// 查询子树
        /// </summary>
        /// <param name="queryResult"></param>
        /// <param name="parent"></param>
        /// <param name="checkedORG">默认选中的组织</param>
        /// <returns></returns>
        private List<VM_PF_ORG_Tree> childrenSpread(List<PF_ORG> queryResult, VM_PF_ORG_Tree parent, List<string> checkedORG)
        {
            List<VM_PF_ORG_Tree> trees = new List<VM_PF_ORG_Tree>();
            parent.children = trees;
            parent.spread = true;
            foreach (PF_ORG query in queryResult)
            {
                if (query.SUPER.Equals(parent.GID))
                {
                    bool ORGchecked = checkedORG.Contains(query.PATH);
                    VM_PF_ORG_Tree PARENT = new VM_PF_ORG_Tree
                    {
                        GID = query.GID,
                        TITLE = query.TITLE,
                        SUPER = query.SUPER,
                        checkArr = ORGchecked ? new checkArr("0", "1") : new checkArr("0", "0"),
                        DEPTH = query.DEPTH,
                        PATH = query.PATH
                    };
                    PARENT.children = childrenSpread(queryResult, PARENT, checkedORG);
                    trees.Add(PARENT);
                }
                if (trees.Count == 0)
                {
                    parent.isLast = true;
                }
                else
                {
                    parent.isLast = false;
                }
            }
            return trees;
        }
        private bool FD_FORM_PERMISSIONExists(string GID)
        {
            return _context.FD_FORM_PERMISSION.Any(e => e.GID == GID);
        }
    }
}
