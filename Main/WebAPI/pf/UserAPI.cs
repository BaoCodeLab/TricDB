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
                Results = queryResult.Where(searchfield + ".Contains(@0)", searchword).OrderBy(field + " " + order)//����������

            .Skip((page - 1) * limit) //����ǰx�� 
            .Take(limit)//�ӵ�ǰλ�ÿ�ʼȡǰx��
            .ToList()//�����תΪList����
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
                Message = "��ȡ�ɹ�",
                Results = Permission.getCurrentUserObj()
            };
        }
        //�޸�����
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
                    Message = "���볤�Ȳ�����"

                });
            }
            PF_USER pf_user = _context.PF_USER.SingleOrDefault(m => m.GID == GID && !m.IS_DELETE);
            if (pf_user == null)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "�˺Ų����ڻ���ɾ����"

                });
            }
            pf_user.OPERATOR = Permission.getCurrentUser();
            pf_user.MODIFY_DATE = DateTime.Now;
            pf_user.PASSWORD = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(PASSWORD))).ToLower().Replace("-", ""); ;
            _context.Update(pf_user);
            _context.SaveChanges();
            Log.Write(GetType(), "����", "PF_USER", pf_user.USERNAME + "����Ա�޸�����");
            return Ok(new ResultList<Object>
            {
                StateCode = 4001,
                Message = "�����޸ĳɹ���",
            });
        }

        //�޸�����
        [HttpPost("mypsw")]
        public IActionResult ModifyMyPsw(string OLDPASSWORD, String PASSWORD)
        {
            if (PASSWORD.Length < 6)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "���볤�Ȳ�����"

                });
            }
            //��ȡ��ǰ�û�
            string username = Permission.getCurrentUser();
            var roles = Permission.getCurrentUserRoles();
            OLDPASSWORD = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(OLDPASSWORD))).ToLower().Replace("-", ""); ;
            //�жϾ�����
            PF_USER pf_user = _context.PF_USER.SingleOrDefault(m => m.USERNAME == username && !m.IS_DELETE && m.PASSWORD == OLDPASSWORD);
            if (pf_user == null)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "��������������޷��޸����룡"

                });
            }
            pf_user.MODIFY_DATE = DateTime.Now;
            var code = 4001;
            pf_user.PASSWORD = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(PASSWORD))).ToLower().Replace("-", "");
            if (pf_user.YHZT.Equals("��ʼ��") && (roles.Contains("��������") || roles.Contains("������") || roles.Contains("admin")))
            {
                code = 4003;
                pf_user.YHZT = "���ֻ�";
            }
            _context.Update(pf_user);
            _context.SaveChanges();
            Log.Write(GetType(), "����", "PF_USER", pf_user.USERNAME + "�û��޸�����");
            return Ok(new ResultList<Object>
            {
                StateCode = code,
                Message = "�����޸ĳɹ������μ������õ����룡",
            });
        }
        //�޸��ֻ�����
        [HttpPost("ModifySJHM")]
        public IActionResult ModifySJHM(string SJHM, string Code, string Password)
        {
            if (string.IsNullOrEmpty(SJHM))
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "�ֻ����벻��Ϊ�գ�"
                });
            }
            if (string.IsNullOrEmpty(Code))
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "�ֻ���֤�벻��Ϊ�գ�"
                });
            }
            if (string.IsNullOrEmpty(Password))
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "���벻��Ϊ�գ�"
                });
            }
            //��ȡ��ǰ�û�
            string username = Permission.getCurrentUser();
            var roles = Permission.getCurrentUserRoles();
            Password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Password))).ToLower().Replace("-", ""); ;
            //�жϾ�����
            PF_USER pf_user = _context.PF_USER.SingleOrDefault(m => m.USERNAME == username && !m.IS_DELETE && m.PASSWORD == Password);
            if (pf_user == null)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "������������޷��޸��ֻ��ţ�"
                });
            }
            var pf_smscode = _context.PF_SMSCODE.Where(m => !m.IS_DELETE && m.CODE.Equals(Code) && m.MOBILE.Equals(SJHM) && m.CREATE_DATE.AddMinutes(10) > DateTime.Now);
            if (pf_smscode.Count() == 0)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "��֤������֤��ʧ�ܣ����ٴ������ͣ�"
                });
            }
            pf_user.MODIFY_DATE = DateTime.Now;
            pf_user.SJHM = SJHM;
            this._context.Database.ExecuteSqlCommand("update bus_company set LXRSJHM=@0 where OPERATOR=@1", SJHM, username);
            this._context.Database.ExecuteSqlCommand("update bus_company_history set LXRSJHM=@0 where OPERATOR=@1", SJHM, username);
            var code = 4001;
            if (pf_user.YHZT.Equals("���ֻ�") && (roles.Contains("��������") || roles.Contains("������") || roles.Contains("admin")))
            {
                code = 4003;
                pf_user.YHZT = "����";
            }
            _context.Update(pf_user);
            _context.SaveChanges();
            Log.Write(GetType(), "����", "PF_USER", pf_user.USERNAME + "�û��޸��ֻ���");
            return Ok(new ResultList<Object>
            {
                StateCode = code,
                Message = "�ֻ������޸���ɣ�",
            });
        }
        //�޸��˻�
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
                    Message = "�˺��Ѵ��ڣ�"
                });
            }

            PF_USER pf_user = this._context.PF_USER.Find(gid);
            if (pf_user == null)
            {
                return Ok(new ResultList<Object> { StateCode = 4003, Message = "�û�����ʧ��!" });
            }
            pf_user.OPERATOR = Permission.getCurrentUser();
            pf_user.USERNAME = username;
            pf_user.RYBM = username;
            pf_user.SJHM = sjhm;
            pf_user.XMBM = xmbm;
            //pf_user.YHZT = "����";
            _context.Update(pf_user);
            _context.SaveChanges();
            Log.Write(GetType(), "����", "PF_USER", pf_user.USERNAME + "�û�����");
            return Ok(new ResultList<Object>
            {
                StateCode = 4001,
                Message = "�˻��ύ�ɹ���",
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
                    Message = "���볤�Ȳ�����"

                });
            }
            int has = _context.PF_USER.Where("USERNAME==@0 and is_delete == false", username).Count();
            if (has > 0)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4003,
                    Message = "�˺��Ѵ��ڣ�"

                });
            }
            has = _context.PF_USER.Where("SJHM==@0 and is_delete ==false", sjhm).Count();
            if (has > 0)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4003,
                    Message = "�ֻ����ѱ�ע�ᣡ"

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
                YHZT = "��ʼ��"
            };
            _context.Add(pf_user);
            _context.SaveChanges();
            Log.Write(GetType(), "����", "PF_USER", pf_user.USERNAME + "�û�����");
            return Ok(new ResultList<Object>
            {
                StateCode = 4001,
                Message = "�˻��ύ�ɹ���",
            });
        }
        //ɾ���˻�
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
                    Message = "�˺Ų����ڣ�"

                });
            }
            pf_user.IS_DELETE = true;
            pf_user.MODIFY_DATE = DateTime.Now;
            _context.Update(pf_user);
            _context.SaveChanges();
            Log.Write(GetType(), "ɾ��", "PF_USER", pf_user.USERNAME + "ɾ���û�");
            return Ok(new ResultList<Object>
            {
                StateCode = 4001,
                Message = "�ɹ�ɾ����",
            });
        }

        //ɾ���˻�
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
                    Message = "�˺Ų����ڣ�"

                });
            }
            pf_user.YHZT = pf_user.YHZT.Equals("register") || pf_user.YHZT.Equals("not_allow") ? "allow" : "not_allow";
            pf_user.MODIFY_DATE = DateTime.Now;
            _context.Update(pf_user);
            _context.SaveChanges();
            Log.Write(GetType(), "����", "PF_USER", pf_user.USERNAME + "�����û�");
            return Ok(new ResultList<Object>
            {
                StateCode = 4001,
                Message = "�ɹ�������",
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
                    Message = "�˺Ų����ڣ�"

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
            //��ѯ�û�����
            pf_user.MODIFY_DATE = DateTime.Now;
            _context.Update(pf_user);
            _context.SaveChanges();
            return Ok(new ResultList<Object>
            {
                StateCode = 4001,
                Message = "�ɹ�ע����",
            });
        }

        //��ȡ�û�������Ա����
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
            //��ȡ�û�
            PF_USER PF_USER = _context.PF_USER.SingleOrDefault("is_delete == false and USERNAME ==@0", username);
            if (PF_USER == null || PF_USER.RYBM.Equals(""))
            {
                return Ok(new ResultList<PF_PROFILE>
                {
                    StateCode = 4002,
                    Message = "��ǰ�޴��û����������Ƿ����½��������",
                });
            }
            //��ȡ����
            PF_PROFILE PF_PROFILE = _context.PF_PROFILE.SingleOrDefault("is_delete == false and CODE == @0", PF_USER.RYBM);
            if (PF_PROFILE == null)
            {
                return Ok(new ResultList<PF_PROFILE>
                {
                    StateCode = 4002,
                    Message = "��ǰ�޴��û����������Ƿ����½��������",
                });
            }

            //���ص�����Ϣ
            return Ok(new ResultList<PF_PROFILE>
            {
                StateCode = 4001,
                Message = "��ȡ�ɹ�",
                Results = PF_PROFILE
            });
        }

        //������Ա��������
        [HttpGet("dagl")]
        public IActionResult UserDaGL(string USERNAME, string CODE)
        {
            if (!Permission.check(HttpContext, "YHZH:DAGL"))
            {
                return Forbid();
            }
            //��ȡ�û�
            PF_USER PF_USER = _context.PF_USER.SingleOrDefault("is_delete == false and USERNAME ==@0", USERNAME);
            if (PF_USER == null)
            {
                return Ok(new ResultList<PF_PROFILE>
                {
                    StateCode = 4002,
                    Message = "��ǰ�޴��û���",
                });
            }
            if (!PF_USER.RYBM.Equals(""))
            {
                return Ok(new ResultList<PF_PROFILE>
                {
                    StateCode = 4003,
                    Message = "��ǰ�û��Ѱ󶨵�����",
                });
            }
            //��ȡ����
            PF_PROFILE PF_PROFILE = _context.PF_PROFILE.SingleOrDefault("is_delete == false and CODE == @0", CODE);
            if (PF_PROFILE == null)
            {
                return Ok(new ResultList<PF_PROFILE>
                {
                    StateCode = 4003,
                    Message = "��ǰ�޴˵���",
                });
            }
            if (!PF_PROFILE.DLZH.Equals(""))
            {
                return Ok(new ResultList<PF_PROFILE>
                {
                    StateCode = 4004,
                    Message = "��ǰ�����ѱ��󶨣�",
                });
            }
            PF_USER.RYBM = CODE;
            PF_PROFILE.DLZH = USERNAME;
            _context.SaveChanges();

            //���ص�����Ϣ
            return Ok(new ResultList<PF_PROFILE>
            {
                StateCode = 4001,
                Message = "�����ɹ�",
                Results = PF_PROFILE
            });
        }
        private bool TBExists(string id)
        {
            return _context.PF_USER.Any(e => e.GID == id && !e.IS_DELETE);
        }
    }
}


