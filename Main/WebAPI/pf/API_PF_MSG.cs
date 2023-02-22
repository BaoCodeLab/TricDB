using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Model;
using System.Linq.Dynamic.Core;
using TDSCoreLib;
using Main.platform;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/pf/msg")]
    public class API_PF_MSG : Controller
    {
        private readonly drugdbContext _context;

        public API_PF_MSG(drugdbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ResultList<PF_MSG> Get([FromQuery]int page = 1, int limit = 5, string searchfield = "", string searchword = "", string field = "CREATE_DATE", string order = "DESC")
        {
            //1、设定检索默认值
            searchfield = string.IsNullOrEmpty(searchfield) ? "TITLE" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            //2、执行查询
            var queryResult = _context.PF_MSG
            .Where((searchfield + ".Contains(@0) and is_delete == false"), searchword) //like查找
            .OrderBy(field + " " + order)//按条件排序 desc asc
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
            //3、返回结果
            return new ResultList<PF_MSG>
            {
                TotalCount = _context.PF_MSG.Where(searchfield + ".Contains(@0) and is_delete == false", searchword).Count(),
                Results = queryResult
            };
        }

        //创建消息
        [HttpPost("create")]
        public IActionResult Create(String title, String content, string[] role, string user)
        {
            List<string> users_ = new List<string>();
            if (user != null)
            {
                string[] users = user.Split(';');
                //去掉users的姓名（name）
                foreach (string user_ in users)
                {
                    string user_first = user_.Split('(')[0];
                    users_.Add(user_first);
                }
            }
            Push.ExecutePush(title, content, users_, role.ToList(), null);
            return Ok(new
            {
                msg = "发送成功"
            });
        }

        public async Task<IActionResult> Deletes(string[] data)
        {
            if (data == null || data.Length == 0)
            {
                return Ok(new { code = "4001", msg = "没有有效数据" });
            }
            //获取当前用户
            string username = Permission.getCurrentUser();


            //获取当前用户的所有消息
            foreach (string gid in data)
            {
               _context.Database.ExecuteSqlCommand("update PF_MSG set is_delete = 1 where gid = '"+gid+"'");
               _context.Database.ExecuteSqlCommand("update PF_MSG_STATE set is_delete = 1 where MSG_GID = '" + gid+"'");
            }
            await _context.SaveChangesAsync();

            return Ok(new { code = "4001", msg = "成功" });
        }






    }


}