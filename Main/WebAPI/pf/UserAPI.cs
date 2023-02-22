using AutoMapper;
using Main.PF.ViewModels;
using Main.platform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Model;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.WebAPI.PF
{
    [Produces("application/json")]
    [Route("api/pf/user")]
    public class UserAPI : Controller
    {
        private readonly drugdbContext _context;

        public UserAPI(drugdbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]int page = 1, int limit = 5, string searchfield = "USERNAME", string searchword = "", string field = "USERNAME", string order = "ASC")
        {

            if (!Permission.check(HttpContext, "MENU:ITEM:YHZH"))
            {
                return Forbid();
            }
            searchfield = string.IsNullOrEmpty(searchfield) ? "USERNAME" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;

            var queryResult = from s in _context.PF_USER
                              join c in _context.PF_PROFILE on s.USERNAME equals c.DLZH into grp
                              from g in grp.DefaultIfEmpty()
                              where s.IS_DELETE == false
                              select new
                              {
                                  s.GID,
                                  s.RYBM,
                                  s.OPERATOR,
                                  s.USERNAME,
                                  NAME = g == null ? "" : g.NAME,
                                  s.CREATE_DATE,
                                  s.MODIFY_DATE,
                                  s.XMBM,
                                  s.SJHM,
                                  s.YHZT
                              };

            return Ok(new ResultList<UserViewModel>
            {
                TotalCount = queryResult.Where(searchfield + ".Contains(@0)", searchword).Count(),
                Results = queryResult.Where(searchfield + ".Contains(@0)", searchword).OrderBy(field + " " + order)//按条件排序

            .Skip((page - 1) * limit) //跳过前x项 
            .Take(limit)//从当前位置开始取前x项
            .ToList()//将结果转为List类型
            });
        }
        [HttpGet("{GID}")]
        public async Task<IActionResult> Get([FromRoute] string GID)
        {
            if (!Permission.check(HttpContext, "YHZH:DETAIL"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PF_USER queryResult = await _context.PF_USER.SingleOrDefaultAsync(m => m.GID == GID);

            if (queryResult == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PF_USER, UserViewModel>(queryResult));
        }

        [HttpGet, Route("id")]
        public IActionResult GetByGid(string GID)
        {
            if (!Permission.check(HttpContext, "YHZH:DETAIL"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PF_USER pf_user = _context.PF_USER.SingleOrDefault("GID ==@0 and is_delete == false", GID);

            if (pf_user == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PF_USER, UserViewModel>(pf_user));
        }

        [HttpGet("current")]
        public ResultList<User> GetCurrentUser()
        {
            return new ResultList<User>
            {
                StateCode = 4001,
                Message = "获取成功",
                Results = Permission.getCurrentUserObj()
            };
        }
        //修改密码
        [HttpPost("psw")]
        public IActionResult ModifyPsw(string GID, String PASSWORD)
        {
            if (!Permission.check(HttpContext, "YHZH:XGMM"))
            {
                return Forbid();
            }
            if (PASSWORD.Length < 6)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "密码长度不够！"

                });
            }
            PF_USER pf_user = _context.PF_USER.SingleOrDefault(m => m.GID == GID && !m.IS_DELETE);
            if (pf_user == null)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "账号不存在或已删除！"

                });
            }
            pf_user.OPERATOR = Permission.getCurrentUser();
            pf_user.MODIFY_DATE = DateTime.Now;
            pf_user.PASSWORD = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(PASSWORD))).ToLower().Replace("-", ""); ;
            _context.Update(pf_user);
            _context.SaveChanges();
            Log.Write(GetType(), "更新", "PF_USER", pf_user.USERNAME + "管理员修改密码");
            return Ok(new ResultList<Object>
            {
                StateCode = 4001,
                Message = "密码修改成功！",
            });
        }

        //修改密码
        [HttpPost("mypsw")]
        public IActionResult ModifyMyPsw(string OLDPASSWORD, String PASSWORD)
        {
            if (PASSWORD.Length < 6)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "密码长度不够！"

                });
            }
            //获取当前用户
            string username = Permission.getCurrentUser();
            var roles = Permission.getCurrentUserRoles();
            OLDPASSWORD = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(OLDPASSWORD))).ToLower().Replace("-", ""); ;
            //判断旧密码
            PF_USER pf_user = _context.PF_USER.SingleOrDefault(m => m.USERNAME == username && !m.IS_DELETE && m.PASSWORD == OLDPASSWORD);
            if (pf_user == null)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "旧密码输入错误，无法修改密码！"

                });
            }
            pf_user.MODIFY_DATE = DateTime.Now;
            var code = 4001;
            pf_user.PASSWORD = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(PASSWORD))).ToLower().Replace("-", "");
            if (pf_user.YHZT.Equals("初始化") && (roles.Contains("宣传部门") || roles.Contains("文旅厅") || roles.Contains("admin")))
            {
                code = 4003;
                pf_user.YHZT = "绑定手机";
            }
            _context.Update(pf_user);
            _context.SaveChanges();
            Log.Write(GetType(), "更新", "PF_USER", pf_user.USERNAME + "用户修改密码");
            return Ok(new ResultList<Object>
            {
                StateCode = code,
                Message = "密码修改成功，请牢记新设置的密码！",
            });
        }
        //修改手机号码
        [HttpPost("ModifySJHM")]
        public IActionResult ModifySJHM(string SJHM, string Code, string Password)
        {
            if (string.IsNullOrEmpty(SJHM))
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "手机号码不能为空！"
                });
            }
            if (string.IsNullOrEmpty(Code))
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "手机验证码不能为空！"
                });
            }
            if (string.IsNullOrEmpty(Password))
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "密码不能为空！"
                });
            }
            //获取当前用户
            string username = Permission.getCurrentUser();
            var roles = Permission.getCurrentUserRoles();
            Password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Password))).ToLower().Replace("-", ""); ;
            //判断旧密码
            PF_USER pf_user = _context.PF_USER.SingleOrDefault(m => m.USERNAME == username && !m.IS_DELETE && m.PASSWORD == Password);
            if (pf_user == null)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "密码输入错误，无法修改手机号！"
                });
            }
            var pf_smscode = _context.PF_SMSCODE.Where(m => !m.IS_DELETE && m.CODE.Equals(Code) && m.MOBILE.Equals(SJHM) && m.CREATE_DATE.AddMinutes(10) > DateTime.Now);
            if (pf_smscode.Count() == 0)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "验证短信验证码失败，请再次请求发送！"
                });
            }
            pf_user.MODIFY_DATE = DateTime.Now;
            pf_user.SJHM = SJHM;
            this._context.Database.ExecuteSqlCommand("update bus_company set LXRSJHM=@0 where OPERATOR=@1", SJHM, username);
            this._context.Database.ExecuteSqlCommand("update bus_company_history set LXRSJHM=@0 where OPERATOR=@1", SJHM, username);
            var code = 4001;
            if (pf_user.YHZT.Equals("绑定手机") && (roles.Contains("宣传部门") || roles.Contains("文旅厅") || roles.Contains("admin")))
            {
                code = 4003;
                pf_user.YHZT = "正常";
            }
            _context.Update(pf_user);
            _context.SaveChanges();
            Log.Write(GetType(), "更新", "PF_USER", pf_user.USERNAME + "用户修改手机号");
            return Ok(new ResultList<Object>
            {
                StateCode = code,
                Message = "手机号码修改完成！",
            });
        }
        //修改账户
        [HttpPost("editaccount")]
        public IActionResult EditAccount(string gid, string username, string sjhm, string xmbm)
        {
            if (!Permission.check(HttpContext, "YHZH:EDIT"))
            {
                return Forbid();
            }
            int has = _context.PF_USER.Where("GID<>@0 and USERNAME==@1 and is_delete == false", gid, username).Count();
            if (has > 0)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4003,
                    Message = "账号已存在！"
                });
            }

            PF_USER pf_user = this._context.PF_USER.Find(gid);
            if (pf_user == null)
            {
                return Ok(new ResultList<Object> { StateCode = 4003, Message = "用户检索失败!" });
            }
            pf_user.OPERATOR = Permission.getCurrentUser();
            pf_user.USERNAME = username;
            pf_user.RYBM = username;
            pf_user.SJHM = sjhm;
            pf_user.XMBM = xmbm;
            //pf_user.YHZT = "正常";
            _context.Update(pf_user);
            _context.SaveChanges();
            Log.Write(GetType(), "更新", "PF_USER", pf_user.USERNAME + "用户更新");
            return Ok(new ResultList<Object>
            {
                StateCode = 4001,
                Message = "账户提交成功！",
            });
        }
        [HttpPost("create")]
        public IActionResult CreateAccount(string username, string password, string sjhm, string xmbm)
        {
            if (!Permission.check(HttpContext, "YHZH:ADD"))
            {
                return Forbid();
            }
            if (password.Length < 6)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "密码长度不够！"

                });
            }
            int has = _context.PF_USER.Where("USERNAME==@0 and is_delete == false", username).Count();
            if (has > 0)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4003,
                    Message = "账号已存在！"

                });
            }
            has = _context.PF_USER.Where("SJHM==@0 and is_delete ==false", sjhm).Count();
            if (has > 0)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4003,
                    Message = "手机号已被注册！"

                });
            }
            PF_USER pf_user = new PF_USER
            {
                OPERATOR = Permission.getCurrentUser(),
                USERNAME = username,
                RYBM = username,
                PASSWORD = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password))).ToLower().Replace("-", ""),
                SJHM = sjhm,
                XMBM = xmbm,
                YHZT = "初始化"
            };
            _context.Add(pf_user);
            _context.SaveChanges();
            Log.Write(GetType(), "创建", "PF_USER", pf_user.USERNAME + "用户创建");
            return Ok(new ResultList<Object>
            {
                StateCode = 4001,
                Message = "账户提交成功！",
            });
        }
        //删除账户
        [HttpDelete]
        public IActionResult delete(string GID)
        {
            if (!Permission.check(HttpContext, "YHZH:DEL"))
            {
                return Forbid();
            }
            PF_USER pf_user = _context.PF_USER
                .SingleOrDefault("GID==@0 and is_delete == false", GID);
            if (pf_user == null)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "账号不存在！"

                });
            }
            pf_user.IS_DELETE = true;
            pf_user.MODIFY_DATE = DateTime.Now;
            _context.Update(pf_user);
            _context.SaveChanges();
            Log.Write(GetType(), "删除", "PF_USER", pf_user.USERNAME + "删除用户");
            return Ok(new ResultList<Object>
            {
                StateCode = 4001,
                Message = "成功删除！",
            });
        }

        //删除账户
        [HttpPost]
        public IActionResult Pass(string GID)
        {
            if (!Permission.check(HttpContext, "OPERATE:YHZC:SH"))
            {
                return Forbid();
            }
            PF_USER pf_user = _context.PF_USER.SingleOrDefault("GID==@0 and is_delete == false", GID);
            if (pf_user == null)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "账号不存在！"

                });
            }
            pf_user.YHZT = pf_user.YHZT.Equals("register") || pf_user.YHZT.Equals("not_allow") ? "allow" : "not_allow";
            pf_user.MODIFY_DATE = DateTime.Now;
            _context.Update(pf_user);
            _context.SaveChanges();
            Log.Write(GetType(), "更新", "PF_USER", pf_user.USERNAME + "审批用户");
            return Ok(new ResultList<Object>
            {
                StateCode = 4001,
                Message = "成功审批！",
            });
        }

        [HttpDelete("da")]
        public IActionResult deleteDA(string USERNAME)
        {
            if (!Permission.check(HttpContext, "YHZH:DAGL"))
            {
                return Forbid();
            }
            PF_USER pf_user = _context.PF_USER
                .SingleOrDefault("USERNAME==@0 and is_delete == false", USERNAME);
            if (pf_user == null)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "账号不存在！"

                });
            }
            PF_PROFILE PF_PROFILE = _context.PF_PROFILE.SingleOrDefault("IS_DELETE == false and CODE ==@0", pf_user.RYBM);
            if (PF_PROFILE != null)
            {
                PF_PROFILE.DLZH = "";
                PF_PROFILE.MODIFY_DATE = DateTime.Now;
                _context.Update(PF_PROFILE);
            }
            pf_user.RYBM = "";
            //查询用户档案
            pf_user.MODIFY_DATE = DateTime.Now;
            _context.Update(pf_user);
            _context.SaveChanges();
            return Ok(new ResultList<Object>
            {
                StateCode = 4001,
                Message = "成功注销！",
            });
        }

        //获取用户销售人员档案
        [HttpGet("da")]
        public IActionResult UserDa(string username)
        {
            if (!string.IsNullOrEmpty(username) && !Permission.check(HttpContext, "YHZH:DAGL"))
            {
                return Forbid();
            }
            if (string.IsNullOrEmpty(username))
            {
                username = Permission.getCurrentUser();
            }
            //获取用户
            PF_USER PF_USER = _context.PF_USER.SingleOrDefault("is_delete == false and USERNAME ==@0", username);
            if (PF_USER == null || PF_USER.RYBM.Equals(""))
            {
                return Ok(new ResultList<PF_PROFILE>
                {
                    StateCode = 4002,
                    Message = "当前无此用户档案，请是否考虑新建或关联？",
                });
            }
            //获取档案
            PF_PROFILE PF_PROFILE = _context.PF_PROFILE.SingleOrDefault("is_delete == false and CODE == @0", PF_USER.RYBM);
            if (PF_PROFILE == null)
            {
                return Ok(new ResultList<PF_PROFILE>
                {
                    StateCode = 4002,
                    Message = "当前无此用户档案，请是否考虑新建或关联？",
                });
            }

            //返回档案信息
            return Ok(new ResultList<PF_PROFILE>
            {
                StateCode = 4001,
                Message = "获取成功",
                Results = PF_PROFILE
            });
        }

        //销售人员档案关联
        [HttpGet("dagl")]
        public IActionResult UserDaGL(string USERNAME, string CODE)
        {
            if (!Permission.check(HttpContext, "YHZH:DAGL"))
            {
                return Forbid();
            }
            //获取用户
            PF_USER PF_USER = _context.PF_USER.SingleOrDefault("is_delete == false and USERNAME ==@0", USERNAME);
            if (PF_USER == null)
            {
                return Ok(new ResultList<PF_PROFILE>
                {
                    StateCode = 4002,
                    Message = "当前无此用户！",
                });
            }
            if (!PF_USER.RYBM.Equals(""))
            {
                return Ok(new ResultList<PF_PROFILE>
                {
                    StateCode = 4003,
                    Message = "当前用户已绑定档案！",
                });
            }
            //获取档案
            PF_PROFILE PF_PROFILE = _context.PF_PROFILE.SingleOrDefault("is_delete == false and CODE == @0", CODE);
            if (PF_PROFILE == null)
            {
                return Ok(new ResultList<PF_PROFILE>
                {
                    StateCode = 4003,
                    Message = "当前无此档案",
                });
            }
            if (!PF_PROFILE.DLZH.Equals(""))
            {
                return Ok(new ResultList<PF_PROFILE>
                {
                    StateCode = 4004,
                    Message = "当前档案已被绑定！",
                });
            }
            PF_USER.RYBM = CODE;
            PF_PROFILE.DLZH = USERNAME;
            _context.SaveChanges();

            //返回档案信息
            return Ok(new ResultList<PF_PROFILE>
            {
                StateCode = 4001,
                Message = "关联成功",
                Results = PF_PROFILE
            });
        }
        private bool TBExists(string id)
        {
            return _context.PF_USER.Any(e => e.GID == id && !e.IS_DELETE);
        }
    }
}


