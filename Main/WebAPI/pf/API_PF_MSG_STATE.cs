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
using System.Data.SqlClient;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/pf/msg/state")]
    public class API_PF_MSG_STATE : Controller
    {
        private readonly drugdbContext _context;

        public API_PF_MSG_STATE(drugdbContext context)
        {
            _context = context;
        }



        [HttpGet]
        public ResultList<PF_MSG_STATE> Get([FromQuery]int page = 1, int limit = 5, string field = "CREATE_DATE", string order = "DESC")
        {

            //��ȡ��ǰ�û�
            string username = Permission.getCurrentUser();

            //��ȡ��ǰ�û���������Ϣ
            List<PF_MSG_STATE> PF_MSG_STATE = _context.PF_MSG_STATE.Where("USERNAME == @0 and is_delete == false", username)
               .OrderBy("IS_READ DESC")
               .OrderBy(field + " " + order)
               .Skip((page - 1) * limit) //����ǰx��
               .Take(limit)//�ӵ�ǰλ�ÿ�ʼȡǰx��
               .ToList();


            return new ResultList<PF_MSG_STATE>
            {
                TotalCount = _context.PF_MSG_STATE.Where("USERNAME == @0 and is_delete == false", username).Count(),
                Results = PF_MSG_STATE
            };
        }

        [HttpGet("details")]
        public ResultList<PF_MSG_STATE> GetByMsg([FromQuery]int page = 1, int limit = 5, string field = "CREATE_DATE", string order = "DESC",string msg = "")
        {


            //��ȡ��ǰ�û���������Ϣ
            var queryResult = from s in _context.PF_MSG_STATE
                              join c in _context.PF_PROFILE on s.USERNAME equals c.DLZH
                              where s.IS_DELETE == false
                              && c.IS_DELETE == false
                              && s.MSG_GID == msg
                              select new
                              {
                                  s.USERNAME,
                                  c.NAME,
                                  s.IS_READ

                              };

            return new ResultList<PF_MSG_STATE>
            {
                TotalCount = queryResult.Count(),
                Results = queryResult.Skip((page - 1) * limit).Take(limit)
            };
        }



        //��ȡδ������
        [HttpGet("notread")]
        public ResultList<PF_MSG_STATE> GetNotReadCount([FromQuery]int page = 1, int limit = 5, string field = "CREATE_DATE", string order = "DESC")
        {

            //��ȡ��ǰ�û�
            string username = Permission.getCurrentUser();

            return new ResultList<PF_MSG_STATE>
            {
                TotalCount = _context.PF_MSG_STATE.Where("USERNAME == @0 and is_delete == false and is_read == false", username).Count(),
            };
        }


        [HttpGet("notify")]
        public async Task<IActionResult> Notify()
        {

            //��ȡ��ǰ�û�
            string username = Permission.getCurrentUser();

            //��ȡ��ǰ�û���������Ϣ
            List<PF_MSG_STATE> PF_MSG_STATE = _context.PF_MSG_STATE.Where("USERNAME == @0 and is_delete == false and is_notify == false", username)
                .OrderBy("CREATE_DATE ASC").ToList();

            foreach (PF_MSG_STATE p in PF_MSG_STATE)
            {
                p.IS_NOTIFY = true;
                p.MODIFY_DATE = DateTime.Now;
                _context.Update(PF_MSG_STATE);
            }
            await _context.SaveChangesAsync();

            return Ok(new
            {
                code = 4001,
                msg = "��ȡ�ɹ�",
                count = PF_MSG_STATE.Count(),
                data = PF_MSG_STATE
            });
        }

        [HttpGet("notreadall")]
        public async Task<IActionResult> NotRead()
        {

            //��ȡ��ǰ�û�
            string username = Permission.getCurrentUser();

            //��ȡ��ǰ�û���������Ϣ
            List<PF_MSG_STATE> PF_MSG_STATE = _context.PF_MSG_STATE.Where("USERNAME == @0 and is_delete == false and is_read == false", username)
                .OrderBy("CREATE_DATE ASC").ToList();

            foreach (PF_MSG_STATE p in PF_MSG_STATE)
            {
                p.IS_NOTIFY = true;
                p.MODIFY_DATE = DateTime.Now;
                _context.Update(PF_MSG_STATE);
            }
            await _context.SaveChangesAsync();

            return Ok(new
            {
                code = 4001,
                msg = "��ȡ�ɹ�",
                count = PF_MSG_STATE.Count(),
                data = PF_MSG_STATE
            });
        }

        [HttpGet("read")]
        public async Task<IActionResult> Read(string[] data)
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
                PF_MSG_STATE PF_MSG_STATE = _context.PF_MSG_STATE.SingleOrDefault("is_delete == false and gid ==@0", gid);
                if (PF_MSG_STATE != null)
                {
                    PF_MSG_STATE.IS_READ = true;
                    PF_MSG_STATE.MODIFY_DATE = DateTime.Now;
                    _context.Update(PF_MSG_STATE);
                }
            }
            await _context.SaveChangesAsync();

            return Ok(new { code = "4001", msg = "�ɹ�" });
        }

        //ȫ���Ѷ�
        [HttpGet("allread")]
        public async Task<IActionResult> AllRead()
        {

            //��ȡ��ǰ�û�
            string username = Permission.getCurrentUser();

            //��ȡ��ǰ�û���������Ϣ
            _context.Database.ExecuteSqlCommand("update PF_MSG_STATE set is_read = 1 where USERNAME =@USERNAME", new[] { new SqlParameter("USERNAME", username) });
            await _context.SaveChangesAsync();

            return Ok(new { code = "4001", msg = "�ɹ�" });
        }

        [HttpDelete("deletes")]
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
                PF_MSG_STATE PF_MSG_STATE = _context.PF_MSG_STATE.SingleOrDefault("is_delete == false and gid ==@0", gid);
                if (PF_MSG_STATE != null)
                {
                    PF_MSG_STATE.IS_DELETE = true;
                    PF_MSG_STATE.MODIFY_DATE = DateTime.Now;
                    _context.Update(PF_MSG_STATE);
                }
            }
            await _context.SaveChangesAsync();

            return Ok(new { code = "4001", msg = "�ɹ�" });
        }

        [HttpDelete("deleteall")]
        public async Task<IActionResult> Deleteall()
        {
          
            //��ȡ��ǰ�û�
            string username = Permission.getCurrentUser();
             _context.Database.ExecuteSqlCommand("Update PF_MSG_STATE SET IS_DELETE=1 WHERE USERNAME=@USERNAME", new[] { new SqlParameter("USERNAME", username) });

            await _context.SaveChangesAsync();

            return Ok(new { code = "4001", msg = "�ɹ�ɾ��" });
        }


    }


}