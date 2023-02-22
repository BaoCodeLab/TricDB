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
            //1���趨����Ĭ��ֵ
            searchfield = string.IsNullOrEmpty(searchfield) ? "TITLE" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            //2��ִ�в�ѯ
            var queryResult = _context.PF_MSG
            .Where((searchfield + ".Contains(@0) and is_delete == false"), searchword) //like����
            .OrderBy(field + " " + order)//���������� desc asc
            .Skip((page - 1) * limit) //����ǰx��
            .Take(limit)//�ӵ�ǰλ�ÿ�ʼȡǰx��
            .ToList();//�����תΪList����
            //3�����ؽ��
            return new ResultList<PF_MSG>
            {
                TotalCount = _context.PF_MSG.Where(searchfield + ".Contains(@0) and is_delete == false", searchword).Count(),
                Results = queryResult
            };
        }

        //������Ϣ
        [HttpPost("create")]
        public IActionResult Create(String title, String content, string[] role, string user)
        {
            List<string> users_ = new List<string>();
            if (user != null)
            {
                string[] users = user.Split(';');
                //ȥ��users��������name��
                foreach (string user_ in users)
                {
                    string user_first = user_.Split('(')[0];
                    users_.Add(user_first);
                }
            }
            Push.ExecutePush(title, content, users_, role.ToList(), null);
            return Ok(new
            {
                msg = "���ͳɹ�"
            });
        }

        public async Task<IActionResult> Deletes(string[] data)
        {
            if (data == null || data.Length == 0)
            {
                return Ok(new { code = "4001", msg = "û����Ч����" });
            }
            //��ȡ��ǰ�û�
            string username = Permission.getCurrentUser();


            //��ȡ��ǰ�û���������Ϣ
            foreach (string gid in data)
            {
               _context.Database.ExecuteSqlCommand("update PF_MSG set is_delete = 1 where gid = '"+gid+"'");
               _context.Database.ExecuteSqlCommand("update PF_MSG_STATE set is_delete = 1 where MSG_GID = '" + gid+"'");
            }
            await _context.SaveChangesAsync();

            return Ok(new { code = "4001", msg = "�ɹ�" });
        }






    }


}