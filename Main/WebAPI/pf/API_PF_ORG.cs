using AutoMapper;
using Main.platform;
using Main.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/pf/org")]
    public class API_PF_ORG : Controller
    {
        private readonly drugdbContext _context;

        public API_PF_ORG(drugdbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Get(int page = 1, int limit = 5, string searchfield = "SUPER", string searchword = "", string field = "ORDER", string order = "ASC")
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:ZZGL"))
            {
                return Forbid();
            }
            //1、设定检索默认值
            searchfield = string.IsNullOrEmpty(searchfield) ? "SUPER" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            //2、执行查询
            var queryResult = _context.PF_ORG
            .Where(searchfield + "==@0 and is_delete == false", searchword)
            .OrderBy(field + " " + order)//按条件排序
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
            //3、返回结果
            return Ok(new ResultList<VM_PF_ORG>
            {
                TotalCount = _context.PF_ORG.Where(searchfield + "==@0 and is_delete == false", searchword).Count(),
                Results = Mapper.Map<List<PF_ORG>, List<VM_PF_ORG>>(queryResult)
            });
        }

        [HttpGet, Route("gid")]
        public async Task<IActionResult> GetByID(string GID)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:ZZGL"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PF_ORG PF_ORG = await _context.PF_ORG.SingleOrDefaultAsync(m => m.GID == GID && m.IS_DELETE == false);

            if (PF_ORG == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PF_ORG, VM_PF_ORG>(PF_ORG));
        }

        //关联权限
        [HttpGet, Route("relate")]
        public async Task<IActionResult> RelatePer(string CODE, string MENU)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:ZZGL"))
            {
                return Forbid();
            }

            PF_ORG PF_ORG = await _context.PF_ORG.SingleOrDefaultAsync(m => m.GID == MENU && m.IS_DELETE == false);

            if (PF_ORG == null)
            {
                return NotFound();
            }

            if (CODE != null)
            {
                PF_ORG.MODIFY_DATE = DateTime.Now;
            }

            _context.Update(PF_ORG);
            await _context.SaveChangesAsync();
            return Ok(Mapper.Map<PF_ORG, VM_PF_ORG>(PF_ORG));
        }

        //关联权限
        [HttpDelete, Route("relate")]
        public async Task<IActionResult> RelatePer(string GID)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:ZZGL"))
            {
                return Forbid();
            }
            PF_ORG PF_ORG = await _context.PF_ORG.SingleOrDefaultAsync(m => m.GID == GID && m.IS_DELETE == false);

            if (PF_ORG == null)
            {
                return NotFound();
            }

            PF_ORG.MODIFY_DATE = DateTime.Now;

            _context.Update(PF_ORG);
            await _context.SaveChangesAsync();
            return Ok(Mapper.Map<PF_ORG, VM_PF_ORG>(PF_ORG));
        }


        [HttpGet("tree")]
        public IActionResult GetDirs()
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:ZZGL"))
            {
                return Forbid();
            }
            //1、执行查询
            List<PF_ORG> queryResult = _context.PF_ORG
            .Where(w => w.IS_DELETE == false)
            .OrderBy("ORDER ASC")//按条件排序
            .ToList();//将结果转为List类型

            List<VM_PF_ORG> results = new List<VM_PF_ORG>();
            List<VM_PF_ORG> trees = new List<VM_PF_ORG>();

            //2、转换为层级结构
            //查询所有父类菜单
            foreach (PF_ORG query in queryResult)
            {
                if (query.SUPER == "")
                {
                    VM_PF_ORG PARENT = new VM_PF_ORG
                    {
                        id = query.GID,
                        title = query.TITLE,
                        GID = query.GID,
                        TITLE = query.TITLE,
                        CODE = query.CODE,
                        DEPTH = query.DEPTH,
                        PATH = query.PATH,
                        TYPE = query.TYPE,
                        children = trees,
                        SPREAD = true
                    };
                    results.Add(PARENT);
                }
            }

            //嵌套查询子菜单
            foreach (VM_PF_ORG parent in results)
            {
                parent.children = childrenSpread(queryResult, parent);
              

            }

            //3、返回结果
            return Ok(new ResultList<VM_PF_ORG>
            {
                Results = results
            });
        }

        //查询子树
        public List<VM_PF_ORG> childrenSpread(List<PF_ORG> queryResult, VM_PF_ORG parent)
        {
            List<VM_PF_ORG> trees = new List<VM_PF_ORG>();
            parent.children = trees;
            foreach (PF_ORG query in queryResult)
            {
                if (query.SUPER.Equals(parent.GID))
                {
                    VM_PF_ORG PARENT = new VM_PF_ORG
                    {
                        id = query.GID,
                        title = query.TITLE,
                        SPREAD=true,
                        GID = query.GID,
                        TITLE = query.TITLE,
                        CODE = query.CODE,
                        DEPTH = query.DEPTH,
                        PATH = query.PATH,
                        TYPE = query.TYPE
                    };
                    PARENT.children = childrenSpread(queryResult, PARENT);
                    trees.Add(PARENT);

                }
            }
            return trees;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PF_ORG PF_ORG)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:ZZGL"))
            {
                return Forbid();
            }
            //设置排序
            //获取当前排序最大值
            if (PF_ORG.SUPER == null)
            {
                PF_ORG.SUPER = "";
            }

            double? max = 0;
            try
            {
                max = _context.PF_ORG.Where("is_delete == false  and super ==@0", PF_ORG.SUPER)
                .Select(e => e.ORDER).Max();
            }
            catch { }
            if (max == null)
            {
                max = 1;
            }
            string newGID = Guid.NewGuid().ToString();
            PF_ORG.GID = newGID;
            PF_ORG.ORDER = max.Value + 1;
            PF_ORG.OPERATOR = Permission.getCurrentUser();
            PF_ORG.CREATE_DATE = DateTime.Now;
            PF_ORG.MODIFY_DATE = DateTime.Now;
            PF_ORG.BZ1 = "启用";
            PF_ORG.BZ2 = "";
            PF_ORG.BZ3 = "";
            PF_ORG.DEPTH = PF_ORG.DEPTH + 1;
            if (PF_ORG.SUPER == string.Empty)
            {

                PF_ORG.PATH = PF_ORG.CODE;
            }
            else
            {
                PF_ORG.PATH = PF_ORG.PATH + "." + PF_ORG.CODE;

            }
            _context.Add(PF_ORG);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                msg = "创建成功"
            });
        }

        // POST: api/yh/5
        [HttpPost("update")]
        public async Task<IActionResult> Update(string GID, [FromForm] VM_PF_ORG PF_ORG)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:ZZGL"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PF_ORG.GID = GID;
            PF_ORG.MODIFY_DATE = DateTime.Now;
            var oldpath = PF_ORG.PATH;
            var newpath = string.Empty;
            if (string.IsNullOrEmpty(PF_ORG.SUPER))
            {
                PF_ORG.SUPER = "";
            }
            else
            {
                var pf = this._context.PF_ORG.Find(PF_ORG.SUPER);
                if(pf!=null)
                    newpath = pf.PATH + "." + PF_ORG.CODE;
            }
            PF_ORG entity = Mapper.Map<VM_PF_ORG, PF_ORG>(PF_ORG);
            _context.Update(entity);
            await _context.SaveChangesAsync<VM_PF_ORG>();
            this._context.Database.ExecuteSqlCommand("update pf_org set path=replace(path,@oldpath,@newpath) where path like @oldpathnotexact",new MySqlParameter[] { new MySqlParameter("oldpath", oldpath),new MySqlParameter("newpath",newpath),new MySqlParameter("oldpathnotexact",oldpath+"%") });
            return Ok(new { msg = "更新成功" });
        }


        [HttpDelete("deletes")]
        public async Task<IActionResult> Deletes(string[] data)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:ZZGL"))
            {
                return Forbid();
            }
            if (data == null || data.Length == 0)
            {
                return Ok(new { code = "4001", msg = "没有有效数据" });
            }

            //获取当前用户的所有消息
            foreach (string gid in data)
            {
                PF_ORG PF_ORG = _context.PF_ORG.SingleOrDefault("is_delete == false and gid ==@0", gid);
                if (PF_ORG != null)
                {
                    PF_ORG.IS_DELETE = true;
                    PF_ORG.MODIFY_DATE = DateTime.Now;
                    _context.Update(PF_ORG);
                }
            }
            await _context.SaveChangesAsync();

            return Ok(new { msg = "成功" });
        }

        [HttpPost("up")]
        public async Task<IActionResult> Up(string GID)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:ZZGL"))
            {
                return Forbid();
            }
            PF_ORG PF_ORG = _context.PF_ORG.SingleOrDefault("is_delete == false and gid ==@0", GID);
            if (PF_ORG == null)
            {
                return NotFound();
            }
            //搜索上两级
            List<PF_ORG> PF_ORGs = _context.PF_ORG
                .Where("is_delete == false  and super ==@1", GID, PF_ORG.SUPER)
                .OrderBy("ORDER ASC")
                .ToList();

            PF_ORG first_one = null;
            PF_ORG first_two = null;

            foreach (PF_ORG menu in PF_ORGs)
            {
                first_one = first_two;
                first_two = menu;
                if (menu.GID.Equals(GID))
                {
                    break;
                }
            }

            if (first_one == null)
            {
                return Ok(new { msg = "已置顶" });
            }
            double temp = 0;


            temp = PF_ORG.ORDER;
            PF_ORG.ORDER = first_one.ORDER;
            first_one.ORDER = temp;

            if (PF_ORG.ORDER == first_one.ORDER)
            {
                int i = 1;
                foreach (PF_ORG menu in PF_ORGs)
                {
                    if (menu.GID.Equals(GID))
                    {
                        menu.ORDER = i - 1;
                    }
                    else if (menu.GID.Equals(first_one.GID))
                    {
                        menu.ORDER = i + 1;
                    }
                    else
                    {
                        menu.ORDER = i;
                    }
                    i++;
                    _context.Update(PF_ORG);
                }
            }
            else
            {
                _context.Update(PF_ORG);
                _context.Update(first_one);
            }

            await _context.SaveChangesAsync();

            return Ok(new { msg = "成功" });


        }


        [HttpPost("down")]
        public async Task<IActionResult> Down(string GID)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:ZZGL"))
            {
                return Forbid();
            }
            PF_ORG PF_ORG = _context.PF_ORG.SingleOrDefault("is_delete == false and gid ==@0", GID);
            if (PF_ORG == null)
            {
                return NotFound();
            }
            //搜索上两级
            List<PF_ORG> PF_ORGs = _context.PF_ORG
                .Where("is_delete == false  and super ==@1", GID, PF_ORG.SUPER)
                .OrderBy("ORDER asc")
                .ToList();

            PF_ORG last_one = null;

            for (int i = 0; i < PF_ORGs.Count(); i++)
            {

                if (PF_ORGs[i].GID.Equals(GID))
                {
                    if ((i + 1) < PF_ORGs.Count())
                    {
                        last_one = PF_ORGs[i + 1];
                    }
                    break;
                }
            }

            if (last_one == null)
            {
                return Ok(new { msg = "已置底" });
            }
            double temp = 0;

            temp = PF_ORG.ORDER;
            PF_ORG.ORDER = last_one.ORDER;
            last_one.ORDER = temp;

            if (PF_ORG.ORDER == last_one.ORDER)
            {
                int i = 1;
                foreach (PF_ORG menu in PF_ORGs)
                {
                    if (menu.GID.Equals(GID))
                    {
                        menu.ORDER = i + 1;
                    }
                    else if (menu.GID.Equals(last_one.GID))
                    {
                        menu.ORDER = i - 1;
                    }
                    else
                    {
                        menu.ORDER = i;
                    }
                    i++;
                    _context.Update(PF_ORG);
                }
            }
            else
            {
                _context.Update(PF_ORG);
                _context.Update(last_one);
            }
            await _context.SaveChangesAsync();

            return Ok(new { msg = "成功" });


        }
    }


}