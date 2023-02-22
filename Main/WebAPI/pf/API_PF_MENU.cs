using AutoMapper;
using Main.platform;
using Main.ViewModels;
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
    [Route("api/pf/menu")]
    public class API_PF_MENU : Controller
    {
        private readonly drugdbContext _context;

        public API_PF_MENU(drugdbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Get(int page = 1, int limit = 5, string searchfield = "SUPER", string searchword = "", string field = "ORDERD", string order = "ASC")
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:CDGL"))
            {
                return Forbid();
            }
            //1、设定检索默认值
            searchfield = string.IsNullOrEmpty(searchfield) ? "SUPER" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            //2、执行查询
            var queryResult = _context.PF_MENU
            .Where(searchfield + "==@0 and is_delete == false", searchword)
            .OrderBy(field + " " + order)//按条件排序
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
            //3、返回结果
            return Ok(new ResultList<VM_PF_MENU>
            {
                TotalCount = _context.PF_MENU.Where(searchfield + "==@0 and is_delete == false", searchword).Count(),
                Results = Mapper.Map<List<PF_MENU>, List<VM_PF_MENU>>(queryResult)
            });
        }

        [HttpGet, Route("gid")]
        public async Task<IActionResult> GetByID(string GID)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:CDGL"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PF_MENU PF_MENU = await _context.PF_MENU.SingleOrDefaultAsync(m => m.GID == GID && m.IS_DELETE == false);

            if (PF_MENU == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PF_MENU, VM_PF_MENU>(PF_MENU));
        }

        //关联权限
        [HttpGet, Route("relate")]
        public async Task<IActionResult> RelatePer(string CODE, string MENU)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:CDGL"))
            {
                return Forbid();
            }

            PF_MENU PF_MENU = await _context.PF_MENU.SingleOrDefaultAsync(m => m.GID == MENU && m.IS_DELETE == false);

            if (PF_MENU == null)
            {
                return NotFound();
            }

            if (CODE != null)
            {
                PF_MENU.PERMISSION_CODE = CODE;
                PF_MENU.MODIFY_DATE = DateTime.Now;
            }

            _context.Update(PF_MENU);
            await _context.SaveChangesAsync();
            return Ok(Mapper.Map<PF_MENU, VM_PF_MENU>(PF_MENU));
        }

        //关联权限
        [HttpDelete, Route("relate")]
        public async Task<IActionResult> RelatePer(string GID)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:CDGL"))
            {
                return Forbid();
            }
            PF_MENU PF_MENU = await _context.PF_MENU.SingleOrDefaultAsync(m => m.GID == GID && m.IS_DELETE == false);

            if (PF_MENU == null)
            {
                return NotFound();
            }

            PF_MENU.PERMISSION_CODE = "";
            PF_MENU.MODIFY_DATE = DateTime.Now;

            _context.Update(PF_MENU);
            await _context.SaveChangesAsync();
            return Ok(Mapper.Map<PF_MENU, VM_PF_MENU>(PF_MENU));
        }

        //获取当前用户的菜单
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            try
            {
                //获取当前用户
                List<string> permissions = Permission.getCurrentUserPermissions();
                var claims = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "CHECKMM");
                if (claims == null || claims.Value == "false")
                {
                    return Ok(new { data = "" });

                }
                //1、执行查询(带权限)
                List<PF_MENU> queryResult = _context.PF_MENU
                .Where("is_delete = false")
                .Where(e => permissions.Contains(e.PERMISSION_CODE))
                .OrderBy("ORDERD asc")//按条件排序
                .ToList();//将结果转为List类型

                List<VM_PF_MENU_DIR> trees = new List<VM_PF_MENU_DIR>();

                //2、转换为层级结构
                //查询所有父类菜单
                foreach (PF_MENU query in queryResult)
                {
                    if (query.SUPER == "" && query.TYPE == "1")
                    {
                        VM_PF_MENU_DIR PARENT = new VM_PF_MENU_DIR
                        {
                            GID = query.GID,
                            title = query.TITLE,
                            name = query.TITLE,
                            id = query.GID,
                            href = query.URL,
                            spread = true,
                            icon = query.ICON,
                            type = query.TYPE
                        };
                        trees.Add(PARENT);
                    }
                }

                //嵌套查询子菜单
                foreach (VM_PF_MENU_DIR parent in trees)
                {
                    parent.children = childrenSpread(queryResult, parent);

                }

                //3、返回结果
                return Ok(new ResultList<VM_PF_MENU>
                {
                    Results = trees
                });
            }
            catch (Exception e)
            {
                return Ok(new ResultList<VM_PF_MENU>
                {
                    Results = e.ToString()
                });
            }
        }

        /// <summary>
        /// 获取指定Type包含的Method
        /// </summary>
        /// <returns></returns>
        [HttpGet("getmethods")]
        public IActionResult GetMethods([FromQuery]string type)
        {
            Type t = Type.GetType(type);
            var methods = t.GetMethods().Where(w => w.Module.Name == "main.dll").Select(s => s.Name).ToList();
            return Ok(methods);
        }

        [HttpGet("tree")]
        public IActionResult GetDirs()
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:CDGL"))
            {
                return Forbid();
            }
            try
            {
                //1、执行查询
                List<PF_MENU> queryResult = _context.PF_MENU
                .Where(("TYPE == \"1\" and  is_delete = false"))
                .OrderBy("ORDERD asc")//按条件排序
                .ToList();//将结果转为List类型

                List<VM_PF_MENU_DIR> results = new List<VM_PF_MENU_DIR>();
                VM_PF_MENU_DIR ROOT = new VM_PF_MENU_DIR();
                results.Add(ROOT);
                ROOT.GID = "";
                ROOT.title = "根目录";
                ROOT.spread = true;
                List<VM_PF_MENU_DIR> trees = new List<VM_PF_MENU_DIR>();
                ROOT.children = trees;

                //2、转换为层级结构
                //查询所有父类菜单
                foreach (PF_MENU query in queryResult)
                {
                    if (query.SUPER == "" && query.TYPE == "1")
                    {
                        VM_PF_MENU_DIR PARENT = new VM_PF_MENU_DIR
                        {
                            id = query.GID,
                            GID = query.GID,
                            title = query.TITLE,
                            name = query.TITLE,
                            href = query.URL,
                            spread = true
                        };
                        trees.Add(PARENT);
                    }
                }

                //嵌套查询子菜单
                foreach (VM_PF_MENU_DIR parent in trees)
                {
                    parent.children = childrenSpread(queryResult, parent);

                }

                //3、返回结果
                return Ok(new ResultList<VM_PF_MENU>
                {
                    Results = results
                });
            }
            catch (Exception e)
            {
                return Ok(new ResultList<VM_PF_MENU>
                {
                    Results = e.ToString()
                });
            }
        }

        //查询子树
        public List<VM_PF_MENU_DIR> childrenSpread(List<PF_MENU> queryResult, VM_PF_MENU_DIR parent)
        {
            List<VM_PF_MENU_DIR> trees = new List<VM_PF_MENU_DIR>();
            parent.children = trees;
            foreach (PF_MENU query in queryResult)
            {
                if (query.SUPER.Equals(parent.GID))
                {
                    VM_PF_MENU_DIR PARENT = new VM_PF_MENU_DIR
                    {
                        GID = query.GID,
                        id = query.GID,
                        title = query.TITLE,
                        name = query.TITLE,
                        type = query.TYPE
                    };
                    if (query.TYPE == "1")//目录
                    {
                        PARENT.spread = true;
                        PARENT.children = childrenSpread(queryResult, PARENT);
                    }
                    else
                    {
                        PARENT.href = query.URL;
                    }
                    trees.Add(PARENT);

                }
            }
            return trees;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PF_MENU PF_MENU)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:CDGL"))
            {
                return Forbid();
            }
            //设置排序
            //获取当前排序最大值
            if (PF_MENU.SUPER == null)
            {
                PF_MENU.SUPER = "";
            }
            double? max = 0;
            try
            {
                max = _context.PF_MENU.Where(m => m.IS_DELETE == false && m.SUPER == "")
                .Select(e => e.ORDERD).Max();
            }
            catch { }
            PF_MENU.SUPER = PF_MENU.SUPER;
            PF_MENU.ORDERD = max.Value + 1;
            PF_MENU.OPERATOR = Permission.getCurrentUser();
            _context.Add(PF_MENU);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                msg = "创建成功"
            });
        }

        // POST: api/yh/5
        [HttpPost("update")]
        public async Task<IActionResult> Update(string GID, [FromForm] VM_PF_MENU PF_MENU)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:CDGL"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PF_MENU.GID = GID;
            if (PF_MENU.TYPE == "1")
            {
                PF_MENU.URL = "";
            }
            if (string.IsNullOrEmpty(PF_MENU.SUPER))
            {
                PF_MENU.SUPER = "";
            }
            PF_MENU entity = Mapper.Map<VM_PF_MENU, PF_MENU>(PF_MENU);
            _context.Update(entity);
            await _context.SaveChangesAsync<VM_PF_MENU>();
            return Ok(new { msg = "更新成功" });


        }


        [HttpDelete("deletes")]
        public async Task<IActionResult> Deletes(string[] data)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:CDGL"))
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
                PF_MENU PF_MENU = _context.PF_MENU.SingleOrDefault("is_delete == false and gid ==@0", gid);
                if (PF_MENU != null)
                {
                    PF_MENU.IS_DELETE = true;
                    PF_MENU.MODIFY_DATE = DateTime.Now;
                    _context.Update(PF_MENU);
                }
            }
            await _context.SaveChangesAsync();

            return Ok(new { msg = "成功" });
        }

        [HttpPost("up")]
        public async Task<IActionResult> Up(string GID)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:CDGL"))
            {
                return Forbid();
            }
            PF_MENU PF_MENU = _context.PF_MENU.SingleOrDefault("is_delete == false and gid ==@0", GID);
            if (PF_MENU == null)
            {
                return NotFound();
            }
            //搜索上两级
            List<PF_MENU> PF_MENUs = _context.PF_MENU
                .Where("is_delete == false  and super ==@1", GID, PF_MENU.SUPER)
                .OrderBy("ORDERD ASC")
                .ToList();

            PF_MENU first_one = null;
            PF_MENU first_two = null;

            foreach (PF_MENU menu in PF_MENUs)
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


            temp = PF_MENU.ORDERD;
            PF_MENU.ORDERD = first_one.ORDERD;
            first_one.ORDERD = temp;

            if (PF_MENU.ORDERD == first_one.ORDERD)
            {
                int i = 1;
                foreach (PF_MENU menu in PF_MENUs)
                {
                    if (menu.GID.Equals(GID))
                    {
                        menu.ORDERD = i - 1;
                    }
                    else if (menu.GID.Equals(first_one.GID))
                    {
                        menu.ORDERD = i + 1;
                    }
                    else
                    {
                        menu.ORDERD = i;
                    }
                    i++;
                    _context.Update(PF_MENU);
                }
            }
            else
            {
                _context.Update(PF_MENU);
                _context.Update(first_one);
            }

            await _context.SaveChangesAsync();

            return Ok(new { msg = "成功" });


        }


        [HttpPost("down")]
        public async Task<IActionResult> Down(string GID)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:CDGL"))
            {
                return Forbid();
            }
            PF_MENU PF_MENU = _context.PF_MENU.SingleOrDefault("is_delete == false and gid ==@0", GID);
            if (PF_MENU == null)
            {
                return NotFound();
            }
            //搜索上两级
            List<PF_MENU> PF_MENUs = _context.PF_MENU
                .Where("is_delete == false  and super ==@1", GID, PF_MENU.SUPER)
                .OrderBy("ORDERD ASC")
                .ToList();

            PF_MENU last_one = null;

            for (int i = 0; i < PF_MENUs.Count(); i++)
            {

                if (PF_MENUs[i].GID.Equals(GID))
                {
                    if ((i + 1) < PF_MENUs.Count())
                    {
                        last_one = PF_MENUs[i + 1];
                    }
                    break;
                }
            }

            if (last_one == null)
            {
                return Ok(new { msg = "已置底" });
            }
            double temp = 0;

            temp = PF_MENU.ORDERD;
            PF_MENU.ORDERD = last_one.ORDERD;
            last_one.ORDERD = temp;

            if (PF_MENU.ORDERD == last_one.ORDERD)
            {
                int i = 1;
                foreach (PF_MENU menu in PF_MENUs)
                {
                    if (menu.GID.Equals(GID))
                    {
                        menu.ORDERD = i + 1;
                    }
                    else if (menu.GID.Equals(last_one.GID))
                    {
                        menu.ORDERD = i - 1;
                    }
                    else
                    {
                        menu.ORDERD = i;
                    }
                    i++;
                    _context.Update(PF_MENU);
                }
            }
            else
            {
                _context.Update(PF_MENU);
                _context.Update(last_one);
            }
            await _context.SaveChangesAsync();

            return Ok(new { msg = "成功" });


        }




    }


}