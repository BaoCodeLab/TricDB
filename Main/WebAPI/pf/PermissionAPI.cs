using AutoMapper;
using Main.PF.ViewModels;
using Main.platform;
using Microsoft.AspNetCore.Mvc;
using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.WebAPI.PF
{
    class KHXX
    {
        public KHXX() { }
        public string KHBM { get; set; }
        public string KHMC { get; set; }
    }

    [Produces("application/json")]
    [Route("api/pf/permission")]
    [ApiController]
    public class PermissionAPI : Controller
    {
        private readonly drugdbContext _context;

        public PermissionAPI(drugdbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Get([FromQuery]int page = 1, int limit = 5, string searchfield = "CODE", string searchword = "", string field = "CODE", string order = "ASC")
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:QXGL"))
            {
                return Forbid();
            }
            searchfield = string.IsNullOrEmpty(searchfield) ? "CODE" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = _context.PF_PERMISSION
            .Where(searchfield + ".Contains(@0) and is_delete == false", searchword)
            .OrderBy(field + " " + order)//����������
            .Skip((page - 1) * limit) //����ǰx��
            .Take(limit)//�ӵ�ǰλ�ÿ�ʼȡǰx��
            .ToList();//�����תΪList����
            return Ok(new ResultList<RoleViewModel>
            {
                TotalCount = _context.PF_PERMISSION.Where(searchfield + ".Contains(@0) and is_delete == false", searchword).Count(),
                Results = Mapper.Map<List<PF_PERMISSION>, List<RoleViewModel>>(queryResult)
            });
        }


        [HttpGet, Route("id")]
        public IActionResult GetByGid(string GID)
        {
            if (!Permission.check(HttpContext, "QXGL:DETAIL"))
            {
                return Forbid();
            }

            PF_PERMISSION pf = _context.PF_PERMISSION.SingleOrDefault("GID ==@0 and is_delete == false", GID);

            if (pf == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PF_PERMISSION, RoleViewModel>(pf));
        }



        //���º��½�
        [HttpPost("form")]
        public async Task<IActionResult> form(string MENU, [FromForm] PF_PERMISSION postData)
        {
            try
            {
                if (postData.NAME == null || postData.NAME.Trim().Equals(""))
                {
                    return Ok(new ResultList<Object>
                    {
                        StateCode = 4004,
                        Message = "���Ʋ���Ϊ�գ�"
                    });
                }
                if (postData.GID == null || postData.GID.Trim().Equals(""))
                {
                    if (!Permission.check(HttpContext, "QXGL:ADD"))
                    {
                        return Forbid();
                    }
                    if (string.IsNullOrEmpty(postData.CODE))
                    {
                        return Ok(new ResultList<Object>
                        {
                            StateCode = 4004,
                            Message = "���Ͳ���Ϊ�գ�"
                        });
                    }
                    //�½�
                    postData.CODE = postData.CODE + ":" + pinyinHelper.IndexCode(postData.NAME).ToUpper();
                    postData.GID = Guid.NewGuid().ToString().ToLower();
                    postData.OPERATOR = Permission.getCurrentUser();
                    postData.MODIFY_DATE = DateTime.Now;
                    postData.CREATE_DATE = DateTime.Now;
                    PF_PERMISSION permission = _context.PF_PERMISSION.SingleOrDefault("code ==@0 and is_delete = false", postData.CODE);
                    if (permission != null)
                    {
                        return Ok(new ResultList<Object>
                        {
                            StateCode = 4003,
                            Message = "�Ѵ��ڴ�Ȩ�ޣ������´�����"
                        });
                    }
                    //if (MENU != null) {
                    //    //Ѱ��MENU
                    //    PF_MENU PF_MENU = _context.PF_MENU.SingleOrDefault("is_delete == false and GID ==@0",MENU);
                    //    if (PF_MENU != null) {
                    //        PF_MENU.PERMISSION_CODE = postData.CODE;
                    //        PF_MENU.MODIFY_DATE = DateTime.Now;
                    //        _context.Update( PF_MENU);
                    //    }
                    //}
                    _context.Add(postData);
                    Log.Write(GetType(), "����", "PF_PERMISSION", postData.CODE + "����");

                    await _context.SaveChangesAsync();
                }
                else
                {
                    if (!Permission.check(HttpContext, "QXGL:EDIT"))
                    {
                        return Forbid();
                    }
                    PF_PERMISSION permission = _context.PF_PERMISSION.SingleOrDefault("GID ==@0 and is_delete = false", postData.GID);
                    if (permission == null)
                    {
                        return Ok(new ResultList<Object>
                        {
                            StateCode = 4002,
                            Message = "�����ڴ�Ȩ�ޣ�"

                        });
                    }

                    //  permission.CODE = postData.CODE + ":" + pinyinHelper.IndexCode(postData.NAME).ToUpper();
                    //    permission.CODE = postData.CODE;
                    permission.NAME = postData.NAME;
                    permission.MODIFY_DATE = DateTime.Now;
                    permission.OPERATOR = Permission.getCurrentUser();
                    //����
                    _context.Update(permission);
                    await _context.SaveChangesAsync();
                    Log.Write(GetType(), "����", "PF_PERMISSION", permission.CODE + "����");
                }
                return Ok(new ResultList<Object>
                {
                    StateCode = 4001,
                    Message = "�ύ�ɹ���"

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
        //ɾ���˻�
        [HttpDelete]
        public IActionResult delete([FromForm]string GID)
        {
            if (!Permission.check(HttpContext, "QXGL:DEL"))
            {
                return Forbid();
            }
            PF_PERMISSION pf = _context.PF_PERMISSION
                .SingleOrDefault("GID==@0 and is_delete == false", GID);
            if (pf == null)
            {
                return Ok(new ResultList<Object>
                {
                    StateCode = 4002,
                    Message = "Ȩ�޲����ڣ�"

                });
            }
            pf.IS_DELETE = true;
            pf.MODIFY_DATE = DateTime.Now;
            _context.Update(pf);
            _context.SaveChanges();
            Log.Write(GetType(), "ɾ��", "PF_PERMISSION", pf.CODE + "ɾ��");
            return Ok(new ResultList<Object>
            {
                StateCode = 4001,
                Message = "�ɹ�ɾ����",
            });
        }
    }
}


