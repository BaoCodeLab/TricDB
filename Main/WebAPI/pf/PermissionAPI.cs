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
            .OrderBy(field + " " + order)//按条件排序
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
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



        //更新和新建
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
                        Message = "名称不许为空！"
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
                            Message = "类型不许为空！"
                        });
                    }
                    //新建
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
                            Message = "已存在此权限，请重新创建！"
                        });
                    }
                    //if (MENU != null) {
                    //    //寻找MENU
                    //    PF_MENU PF_MENU = _context.PF_MENU.SingleOrDefault("is_delete == false and GID ==@0",MENU);
                    //    if (PF_MENU != null) {
                    //        PF_MENU.PERMISSION_CODE = postData.CODE;
                    //        PF_MENU.MODIFY_DATE = DateTime.Now;
                    //        _context.Update( PF_MENU);
                    //    }
                    //}
                    _context.Add(postData);
                    Log.Write(GetType(), "创建", "PF_PERMISSION", postData.CODE + "创建");

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
                            Message = "不存在此权限！"

                        });
                    }

                    //  permission.CODE = postData.CODE + ":" + pinyinHelper.IndexCode(postData.NAME).ToUpper();
                    //    permission.CODE = postData.CODE;
                    permission.NAME = postData.NAME;
                    permission.MODIFY_DATE = DateTime.Now;
                    permission.OPERATOR = Permission.getCurrentUser();
                    //更新
                    _context.Update(permission);
                    await _context.SaveChangesAsync();
                    Log.Write(GetType(), "更新", "PF_PERMISSION", permission.CODE + "更新");
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
                    Message = "权限不存在！"

                });
            }
            pf.IS_DELETE = true;
            pf.MODIFY_DATE = DateTime.Now;
            _context.Update(pf);
            _context.SaveChanges();
            Log.Write(GetType(), "删除", "PF_PERMISSION", pf.CODE + "删除");
            return Ok(new ResultList<Object>
            {
                StateCode = 4001,
                Message = "成功删除！",
            });
        }
    }
}


