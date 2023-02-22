using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using TDSCoreLib;
using Main.platform;
using Model.Model;
using Main.PF.ViewModels;
using Main.ViewModels.PF;

namespace Main.WebAPI.PF
{
    class roleper
    {
        string per { get; set; }
        string create_date { get; set; }

    }
    [Produces("application/json")]
    [Route("api/pf/role")]
    public class RoleAPI : Controller
    {
        private readonly drugdbContext _context;

        public RoleAPI(drugdbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Get([FromQuery]int page = 1, int limit = 5, string searchfield = "CODE", string searchword = "", string field = "CODE", string order = "ASC")
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:JSGL"))
            {
                return Forbid();
            }
            searchfield = string.IsNullOrEmpty(searchfield) ? "CODE" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = _context.PF_ROLE
            .Where(searchfield + ".Contains(@0) and is_delete == false", searchword)
            .OrderBy(field + " " + order)//按条件排序
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
            return Ok(new ResultList<RoleViewModel>
            {
                TotalCount = _context.PF_ROLE.Where(searchfield + ".Contains(@0) and is_delete == false", searchword).Count(),
                Results = Mapper.Map<List<PF_ROLE>, List<RoleViewModel>>(queryResult)
            });
        }

        [HttpGet, Route("all")]
        public ResultList<RoleViewModel> GetAll()
        {

            var queryResult = _context.PF_ROLE
            .Where("is_delete == false")
            .ToList();//将结果转为List类型
            return new ResultList<RoleViewModel>
            {
                StateCode = 4001,
                TotalCount = queryResult.Count(),
                Results = Mapper.Map<List<PF_ROLE>, List<RoleViewModel>>(queryResult)
            };
        }

        [HttpGet, Route("getrolepm")]
        public IActionResult GetROLEPM(string GID, int page = 1, int limit = 5)
        {
            if (string.IsNullOrEmpty(GID))
            {
                return Ok(new ResultList<object>
                {
                    TotalCount = 0,
                    Results = null
                });
            }
            var result = from rp in _context.PF_ROLE_PERMISSION
                         join per in _context.PF_PERMISSION
                         on rp.PER_GID
                         equals per.GID
                         where rp.ROLE_GID == GID
                         orderby rp.CREATE_DATE descending
                         select new
                         {
                             per = per.NAME,
                             create_date=rp.CREATE_DATE,
                             percode=per.CODE
                         };

            return Ok(new ResultList<Object> {
                TotalCount = result.Count(),
                Results=result.Skip((page-1)*limit).Take(limit).ToList()
            });
 
        }


        [HttpGet, Route("getroleuser")]
        public IActionResult GetROLEUSER(string GID, int page = 1, int limit = 10)
        {
            if (string.IsNullOrEmpty(GID))
            {
                return Ok(new ResultList<object>
                {
                    TotalCount = 0,
                    Results = null
                });
            }
            var result = from rp in _context.PF_USER_ROLE
                         join per in _context.PF_USER
                         on rp.USER_GID
                         equals per.GID
                         where rp.ROLE_GID == GID
                         join profile in _context.PF_PROFILE
                         on per.USERNAME
                         equals profile.CODE
                         orderby rp.CREATE_DATE descending
                         select new
                         {
                             per.USERNAME,
                             profile.NAME,
                             create_date = rp.CREATE_DATE
                         };

            return Ok(new ResultList<Object>
            {
                TotalCount = result.Count(),
                Results = result.Skip((page - 1) * limit).Take(limit).ToList()
            });

        }

        [HttpGet, Route("id")]
        public IActionResult GetByGid(string GID)
        {
            if (!Permission.check(HttpContext, "JSGL:DETAIL"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PF_ROLE pf = _context.PF_ROLE.SingleOrDefault("GID ==@0 and is_delete == false", GID);

            if (pf == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PF_ROLE, RoleViewModel>(pf));
        }



        //更新和新建
        [HttpPost("form")]
        public IActionResult form([FromForm] PF_ROLE postData)
        {

            try
            {
                if (postData.CODE == null || postData.CODE.Trim().Equals(""))
                {
                    return Ok(new ResultList<Object>
                    {
                        StateCode = 4004,
                        Message = "CODE数据不许为空！"
                    });
                }
                if (postData.GID == null || postData.GID.Trim().Equals(""))
                {
                    if (!Permission.check(HttpContext, "JSGL:ADD"))
                    {
                        return Forbid();
                    }
                    //新建
                    PF_ROLE role = _context.PF_ROLE.SingleOrDefault("code ==@0 and is_delete = false", postData.CODE);
                    if (role != null)
                    {
                        return Ok(new ResultList<Object>
                        {
                            StateCode = 4003,
                            Message = "已存在此角色，请重新创建！"
                        });
                    }
                    postData.OPERATOR = Permission.getCurrentUser();
                    _context.Add(postData);
                    _context.SaveChanges();
                    Log.Write(GetType(), "创建", "PF_ROLE", postData.CODE + "创建");
                }
                else
                {
                    if (!Permission.check(HttpContext, "JSGL:EDIT"))
                    {
                        return Forbid();
                    }
                    PF_ROLE role = _context.PF_ROLE.SingleOrDefault("GID ==@0 and is_delete = false", postData.GID);
                    if (role == null)
                    {
                        return Ok(new ResultList<Object>
                        {
                            StateCode = 4002,
                            Message = "不存在此角色！"

                        });
                    }
                    role.CODE = postData.CODE;
                    role.NAME = postData.NAME;
                    role.MODIFY_DATE = DateTime.Now;
                    role.OPERATOR = Permission.getCurrentUser();
                    //更新
                    _context.Update(role);
                    _context.SaveChanges();
                    Log.Write(GetType(), "更新", "PF_ROLE", postData.CODE + "更新");
                }
                return Ok(new ResultList<Object>
                {
                    StateCode = 4001,
                    Message = "提交成功！"

                });
            }
            catch (Exception e)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 0000,
                    Message = e.ToString()

                });
            }


        }


        //删除账户
        [HttpDelete]
        public IActionResult delete(string GID)
        {
            if (!Permission.check(HttpContext, "JSGL:DEL"))
            {
                return Forbid();
            }
            PF_ROLE pf = _context.PF_ROLE
                .SingleOrDefault("GID==@0 and is_delete == false", GID);
            if (pf == null)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "角色不存在！"

                });
            }
            pf.IS_DELETE = true;
            pf.MODIFY_DATE = DateTime.Now;
            _context.Update(pf);
            _context.SaveChanges();
            Log.Write(GetType(), "删除", "PF_ROLE", pf.CODE + "删除");
            return Ok(new ResultList<Object>
            {
                StateCode = 4001,
                Message = "成功删除！",
            });
        }


        [HttpGet, Route("checklist")]
        public IActionResult getRoleCheckList([FromQuery]string username, string codestr, int page = 1, int limit = 5, string searchfield = "CODE", string searchword = "", string field = "CODE", string order = "ASC")
        {
            if (!Permission.check(HttpContext, "YHZH:JSFP")) return Forbid();
            if (username == null || username == "") return NotFound();

            searchfield = string.IsNullOrEmpty(searchfield) ? "CODE" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;

            //获取用户
            PF_USER pf_user = _context.PF_USER.SingleOrDefault(m => m.USERNAME == username && !m.IS_DELETE);
            if (pf_user == null) return Ok(new { StateCode = 4002, Message = "用户不存在！" });

            //获取系统所有权限
            var PF_ROLE = _context.PF_ROLE.Where(w => w.IS_DELETE == false);

            //获取用户所有权限
            var user_roles = (from s in _context.PF_ROLE
                             join c in _context.PF_USER_ROLE on s.GID equals c.ROLE_GID
                             join d in _context.PF_USER on c.USER_GID equals d.GID
                             where s.IS_DELETE == false
                             && c.IS_DELETE == false
                             && d.IS_DELETE == false
                             && d.USERNAME == username
                             select new
                             {
                                 code = s.CODE,
                                 name = s.NAME,
                             }).ToList();

            if (codestr != "" && codestr != null) PF_ROLE = PF_ROLE.Where(w => w.CODE.Contains(codestr));

            //在当前权限列表中，填用户是否选择状态

            var result = PF_ROLE.Where((searchfield + ".Contains(@0)"), searchword)
                .GroupJoin(user_roles, t => t.CODE, s => s.code, (t, grp) => new { t, grp })
                .SelectMany(temp => temp.grp.DefaultIfEmpty()
                , (temp, g) => new RoleDistribute
                {
                    CODE = temp.t.CODE,
                    NAME = temp.t.NAME,
                    CHECK = (g == null) ? false : true
                });

            return Ok(new ResultList<RoleDistribute>
            {
                TotalCount = result.Count(),
                Results = result.OrderBy(field + " " + order).Skip((page - 1) * limit).Take(limit).ToList()
            });
        }

        [HttpGet, Route("distributeu")]
        public IActionResult getRoleDistribute(string username)
        {
            if (!Permission.check(HttpContext, "YHZH:JSFP"))
            {
                return Forbid();
            }
            //获取用户
            PF_USER pf_user = _context.PF_USER.SingleOrDefault(m => m.USERNAME == username && !m.IS_DELETE);
            if (pf_user == null)
            {
                return Ok(new ResultList<RoleDistribute>
                {
                    StateCode = 4002,
                    Message = "用户不存在！",
                });
            }
            //获取所有权限
            var PF_ROLE = _context.PF_ROLE.Where(m => !m.IS_DELETE);
            //获取用户权限
            var queryResult = from s in _context.PF_ROLE
                              join c in _context.PF_USER_ROLE on s.GID equals c.ROLE_GID
                              join d in _context.PF_USER on c.USER_GID equals d.GID
                              where s.IS_DELETE == false
                              && c.IS_DELETE == false
                              && d.IS_DELETE == false
                              && d.USERNAME == username
                              select new
                              {
                                  code = s.CODE,
                                  name = s.NAME,
                              };

            //赋值
            List<String> userRole = new List<string>();
            foreach (var role in queryResult)
            {
                userRole.Add(role.code);
            }
            List<RoleDistribute> role_distribute = new List<RoleDistribute>();
            bool check = false;
            foreach (var role in PF_ROLE)
            {
                check = false;
                //找寻是否属于用户
                if (userRole.Contains(role.CODE))
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
                StateCode = 4001,
                Message = "获取成功",
                Results = role_distribute
            });



        }



        [HttpGet, Route("distributep")]
        public IActionResult getRoleDistributep([FromQuery]string CODE, int page = 1, int limit = 5, string searchfield = "CODE", string searchword = "", string field = "CODE", string order = "ASC")
        {
            //if (!Permission.check(HttpContext, "QXGL:JSFP"))
            //{
            //    return Forbid();
            //}
            ////获取用户
            //PF_PERMISSION pf_permission = _context.PF_PERMISSION.SingleOrDefault(m => m.CODE == CODE && !m.IS_DELETE);
            //if (pf_permission == null)
            //{
            //    return Ok(new ResultList<RoleDistributeP>
            //    {
            //        StateCode = 4002,
            //        Message = "角色不存在！",
            //    });
            //}
            ////获取所有权限
            //var PF_ROLE = _context.PF_ROLE.Where(m => !m.IS_DELETE);
            ////获取用户权限
            //var queryResult = from s in _context.PF_ROLE
            //                  join c in _context.PF_ROLE_PERMISSION on s.GID equals c.ROLE_GID
            //                  join d in _context.PF_PERMISSION on c.PER_GID equals d.GID
            //                  where s.IS_DELETE == false
            //                  && c.IS_DELETE == false
            //                  && d.IS_DELETE == false
            //                  && d.CODE == CODE
            //                  select new
            //                  {
            //                      code = s.CODE,
            //                      name = s.NAME,
            //                  };

            ////赋值
            //List<String> permissionRole = new List<string>();
            //foreach (var role in queryResult)
            //{
            //    permissionRole.Add(role.code);
            //}
            //List<RoleDistributeP> role_distributep = new List<RoleDistributeP>();
            //bool check = false;
            //foreach (var role in PF_ROLE)
            //{
            //    check = false;
            //    //找寻是否属于用户
            //    if (permissionRole.Contains(role.CODE))
            //    {
            //        check = true;
            //    }

            //    role_distributep.Add(new RoleDistributeP
            //    {
            //        CODE = role.CODE,
            //        NAME = role.NAME,
            //        CHECK = check

            //    });
            //}

            //return Ok(new ResultList<RoleDistributeP>
            //{
            //    StateCode = 4001,
            //    Message = "获取成功",
            //    Results = role_distributep
            //});

            if (!Permission.check(HttpContext, "QXGL:JSFP")) return Forbid();
            if (CODE == null || CODE == "") return NotFound();

            searchfield = string.IsNullOrEmpty(searchfield) ? "CODE" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;

            //获取用户
            PF_PERMISSION pf_permission = _context.PF_PERMISSION.SingleOrDefault(m => m.CODE == CODE && !m.IS_DELETE);
            if (pf_permission == null)
            {
                return Ok(new ResultList<RoleDistributeP>
                {
                    StateCode = 4002,
                    Message = "角色不存在！",
                });
            }

            //获取所有角色
            var PF_ROLE = _context.PF_ROLE.Where(m => !m.IS_DELETE);

            var permission_roles = (from s in _context.PF_ROLE
                                   join c in _context.PF_ROLE_PERMISSION on s.GID equals c.ROLE_GID
                                   join d in _context.PF_PERMISSION on c.PER_GID equals d.GID
                                   where s.IS_DELETE == false
                                   && c.IS_DELETE == false
                                   && d.IS_DELETE == false
                                   && d.CODE == CODE
                                   select new
                                   {
                                       code = s.CODE,
                                       name = s.NAME,
                                   }).ToList();

            //在当前权限列表中，填用户是否选择状态

            var result = PF_ROLE.Where((searchfield + ".Contains(@0)"), searchword)
                .GroupJoin(permission_roles, t => t.CODE, s => s.code, (t, grp) => new { t, grp })
                .SelectMany(temp => temp.grp.DefaultIfEmpty()
                , (temp, g) => new RoleDistributeP
                {
                    CODE = temp.t.CODE,
                    NAME = temp.t.NAME,
                    CHECK = (g == null) ? false : true
                });

            return Ok(new ResultList<RoleDistributeP>
            {
                TotalCount = result.Count(),
                Results = result.OrderBy(field + " " + order).Skip((page - 1) * limit).Take(limit).ToList()
            });

        }


        //修改用户权限
        [HttpPost, Route("distribute")]
        public IActionResult updateRoleForUser(string username, string code, bool check)
        {
            if (!Permission.check(HttpContext, "YHZH:JSFP"))
            {
                return Forbid();
            }
            try
            {
                //获取用户
                PF_USER pf_user = _context.PF_USER.SingleOrDefault(m => m.USERNAME == username && !m.IS_DELETE);
                if (pf_user == null)
                {
                    return Ok(new ResultList<Object>
                    {
                        StateCode = 4002,
                        Message = "用户不存在！",
                    });
                }
                //获取角色
                PF_ROLE pf_role = _context.PF_ROLE.SingleOrDefault(m => m.CODE == code && !m.IS_DELETE);
                if (pf_role == null)
                {
                    return Ok(new ResultList<Object>
                    {
                        StateCode = 4003,
                        Message = "角色不存在！",
                    });
                }
                //查询用户角色管理关系
                bool isCreate = false;
                PF_USER_ROLE pf_user_role = _context.PF_USER_ROLE.SingleOrDefault(m => m.USER_GID == pf_user.GID && m.ROLE_GID == pf_role.GID && !m.IS_DELETE);
                if (pf_user_role == null)
                {
                    pf_user_role = new PF_USER_ROLE
                    {
                        USER_GID = pf_user.GID,
                        ROLE_GID = pf_role.GID
                    };
                    isCreate = true;
                }
                pf_user_role.IS_DELETE = !check;
                pf_user_role.MODIFY_DATE = DateTime.Now;
                pf_user_role.OPERATOR = Permission.getCurrentUser();
                //修改权限
                if (isCreate)
                {
                    _context.Add(pf_user_role);
                }
                else
                {
                    _context.Update(pf_user_role);
                }
                _context.SaveChanges();



                return Ok(new ResultList<Object>
                {
                    StateCode = 4001,
                    Message = "修改成功"
                });
            }
            catch (Exception e)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 0000,
                    Message = e.ToString()
                });
            }
        }



        //修改用户权限
        [HttpPost, Route("UPDATEdistributep")]
        public IActionResult updateRoleForPermission(string code_p, string code_r, bool check)
        {
            if (!Permission.check(HttpContext, "QXGL:JSFP"))
            {
                return Forbid();
            }
            try
            {
                //获取用户
                PF_PERMISSION pf_permission = _context.PF_PERMISSION.SingleOrDefault(m => m.CODE == code_p && !m.IS_DELETE);
                if (pf_permission == null)
                {
                    return Ok(new ResultList<Object>
                    {
                        StateCode = 4002,
                        Message = "权限不存在！",
                    });
                }
                //获取角色
                PF_ROLE pf_role = _context.PF_ROLE.SingleOrDefault(m => m.CODE == code_r && !m.IS_DELETE);
                if (pf_role == null)
                {
                    return Ok(new ResultList<Object>
                    {
                        StateCode = 4003,
                        Message = "角色不存在！",
                    });
                }
                //查询用户角色管理关系
                bool isCreate = false;
                PF_ROLE_PERMISSION pf_role_permission = _context.PF_ROLE_PERMISSION.SingleOrDefault(m => m.PER_GID == pf_permission.GID && m.ROLE_GID == pf_role.GID && !m.IS_DELETE);
                if (pf_role_permission == null)
                {
                    pf_role_permission = new PF_ROLE_PERMISSION
                    {
                        PER_GID = pf_permission.GID,
                        ROLE_GID = pf_role.GID
                    };
                    isCreate = true;
                }
                pf_role_permission.IS_DELETE = !check;
                pf_role_permission.MODIFY_DATE = DateTime.Now;
                //修改权限
                if (isCreate)
                {
                    _context.Add(pf_role_permission);
                }
                else
                {
                    _context.Update(pf_role_permission);
                }
                _context.SaveChanges();



                return Ok(new ResultList<Object>
                {
                    StateCode = 4001,
                    Message = "修改成功"
                });
            }
            catch (Exception e)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 0000,
                    Message = e.ToString()
                });
            }
        }


    }
}


