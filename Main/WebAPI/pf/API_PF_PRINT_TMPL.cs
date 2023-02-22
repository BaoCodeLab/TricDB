using AutoMapper;
using Main.platform;
using Main.ViewModels;
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
    [Route("api/pf/print_tmpl")]
    [ApiController]
    public class API_PF_PRINT_TMPL : Controller
    {
        private readonly drugdbContext _context;

        public API_PF_PRINT_TMPL(drugdbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Get(int page = 1, int limit = 5, string searchfield = "SUPER", string searchword = "", string field = "ORDER", string order = "ASC")
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:MBGL"))
            {
                return Forbid();
            }
            //1���趨����Ĭ��ֵ
            searchfield = string.IsNullOrEmpty(searchfield) ? "SUPER" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            //2��ִ�в�ѯ
            var queryResult = _context.PF_PRINT_TMPL
            .Where(searchfield + "==@0 and is_delete == false", searchword)
            .OrderBy(field + " " + order)//����������
            .Skip((page - 1) * limit) //����ǰx��
            .Take(limit)//�ӵ�ǰλ�ÿ�ʼȡǰx��
            .ToList();//�����תΪList����
            //3�����ؽ��
            return Ok(new ResultList<VM_PF_PRINT_TMPL>
            {
                TotalCount = _context.PF_PRINT_TMPL.Where(searchfield + "==@0 and is_delete == false", searchword).Count(),
                Results = Mapper.Map<List<PF_PRINT_TMPL>, List<VM_PF_PRINT_TMPL>>(queryResult)
            });
        }

        [HttpGet, Route("gid")]
        public async Task<IActionResult> GetByID(string GID)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:MBGL"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PF_PRINT_TMPL PF_PRINT_TMPL = await _context.PF_PRINT_TMPL.SingleOrDefaultAsync(m => m.GID == GID && m.IS_DELETE == false);

            if (PF_PRINT_TMPL == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PF_PRINT_TMPL, VM_PF_PRINT_TMPL>(PF_PRINT_TMPL));
        }

        //����Ȩ��
        [HttpGet, Route("relate")]
        public async Task<IActionResult> RelatePer(string CODE, string MENU)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:MBGL"))
            {
                return Forbid();
            }

            PF_PRINT_TMPL PF_PRINT_TMPL = await _context.PF_PRINT_TMPL.SingleOrDefaultAsync(m => m.GID == MENU && m.IS_DELETE == false);

            if (PF_PRINT_TMPL == null)
            {
                return NotFound();
            }

            if (CODE != null)
            {
                PF_PRINT_TMPL.MODIFY_DATE = DateTime.Now;
            }

            _context.Update(PF_PRINT_TMPL);
            await _context.SaveChangesAsync();
            return Ok(Mapper.Map<PF_PRINT_TMPL, VM_PF_PRINT_TMPL>(PF_PRINT_TMPL));
        }


        /// <summary>
        /// ��ȡ��
        /// </summary>
        /// <param name="CODE">���ڵ����</param>
        /// <returns></returns>
        [HttpGet("tree")]
        public IActionResult GetDirs(string SUPER = "root")
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:MBGL"))
            {
                return Forbid();
            }
            //1��ִ�в�ѯ
            List<PF_PRINT_TMPL> queryResult = _context.PF_PRINT_TMPL
            .Where(w => w.IS_DELETE == false)
            .OrderBy("ORDER asc")//����������
            .ToList();//�����תΪList����
            List<VM_PF_PRINT_TMPL> results = new List<VM_PF_PRINT_TMPL>();
            List<VM_PF_PRINT_TMPL> trees = new List<VM_PF_PRINT_TMPL>();

            //2��ת��Ϊ�㼶�ṹ
            //��ѯ���и���˵�
            foreach (PF_PRINT_TMPL query in queryResult)
            {
                if (query.SUPER == "root")
                {
                    VM_PF_PRINT_TMPL PARENT = new VM_PF_PRINT_TMPL
                    {
                        id = query.GID,
                        title = query.TITLE,
                        GID = query.GID,
                        TITLE = query.TITLE,
                        CODE = query.CODE,
                        DEPTH = query.DEPTH,
                        children = trees,
                        spread = true
                    };
                    results.Add(PARENT);
                }
            }

            //Ƕ�ײ�ѯ�Ӳ˵�
            foreach (VM_PF_PRINT_TMPL parent in results)
            {
                parent.children = childrenSpread(queryResult, parent);

            }

            //3�����ؽ��
            return Ok(new ResultList<VM_PF_PRINT_TMPL>
            {
                Results = results
            });
        }

        //��ѯ����
        public List<VM_PF_PRINT_TMPL> childrenSpread(List<PF_PRINT_TMPL> queryResult, VM_PF_PRINT_TMPL parent)
        {
            List<VM_PF_PRINT_TMPL> trees = new List<VM_PF_PRINT_TMPL>();
            parent.children = trees;
            foreach (PF_PRINT_TMPL query in queryResult)
            {
                if (query.SUPER.Equals(parent.GID))
                {
                    VM_PF_PRINT_TMPL PARENT = new VM_PF_PRINT_TMPL
                    {
                        GID = query.GID,
                        id = query.GID,
                        TITLE = query.TITLE,
                        title = query.TITLE,
                        CODE = query.CODE,
                        DEPTH = query.DEPTH
                    };
                    PARENT.children = childrenSpread(queryResult, PARENT);
                    trees.Add(PARENT);

                }
            }
            return trees;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PF_PRINT_TMPL PF_PRINT_TMPL)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:MBGL"))
            {
                return Forbid();
            }

            int hasSameCode = _context.PF_PRINT_TMPL.Where("is_delete == false  and CODE ==@0", PF_PRINT_TMPL.CODE).Count();
            if (hasSameCode > 0)
            {
                return Ok(new
                {
                    msg = "ģ������Ѵ��ڣ����޸ĺ󱣴�"
                });
            }
            else
            {
                //��������
                //��ȡ��ǰ�������ֵ
                if (PF_PRINT_TMPL.SUPER == null)
                {
                    PF_PRINT_TMPL.SUPER = "";
                }
                int hasSiblings = _context.PF_PRINT_TMPL.Where("is_delete == false  and super ==@0", PF_PRINT_TMPL.SUPER).Count();

                double? max = 0;
                try
                {
                    max = _context.PF_PRINT_TMPL.Where("is_delete == false  and super ==@0", PF_PRINT_TMPL.SUPER)
                    .Select(e => e.ORDER).Max();
                }
                catch { }
                PF_PRINT_TMPL.ORDER = max.Value + 1;
                PF_PRINT_TMPL.OPERATOR = Permission.getCurrentUser();
                PF_PRINT_TMPL.CREATE_DATE = DateTime.Now;
                PF_PRINT_TMPL.MODIFY_DATE = DateTime.Now;
                PF_PRINT_TMPL.BZ1 = "����";
                PF_PRINT_TMPL.BZ2 = "";
                PF_PRINT_TMPL.DEPTH = PF_PRINT_TMPL.DEPTH + 1;
                _context.Add(PF_PRINT_TMPL);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    msg = "�����ɹ�"
                });
            }
        }

        // POST: api/yh/5
        [HttpPost("update")]
        public async Task<IActionResult> Update(string GID, [FromForm] VM_PF_PRINT_TMPL PF_PRINT_TMPL)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:MBGL"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PF_PRINT_TMPL.GID = GID;
            PF_PRINT_TMPL.MODIFY_DATE = DateTime.Now;

            if (string.IsNullOrEmpty(PF_PRINT_TMPL.SUPER))
            {
                PF_PRINT_TMPL.SUPER = "";
            }
            PF_PRINT_TMPL entity = Mapper.Map<VM_PF_PRINT_TMPL, PF_PRINT_TMPL>(PF_PRINT_TMPL);
            _context.Update(entity);
            await _context.SaveChangesAsync<VM_PF_PRINT_TMPL>();
            return Ok(new { msg = "���³ɹ�" });


        }


        [HttpDelete("deletes")]
        public async Task<IActionResult> Deletes(string[] data)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:MBGL"))
            {
                return Forbid();
            }
            if (data == null || data.Length == 0)
            {
                return Ok(new { code = "4001", msg = "û����Ч����" });
            }

            //��ȡ��ǰ�û���������Ϣ
            foreach (string gid in data)
            {
                PF_PRINT_TMPL PF_PRINT_TMPL = _context.PF_PRINT_TMPL.SingleOrDefault("is_delete == false and gid ==@0", gid);
                if (PF_PRINT_TMPL != null)
                {
                    PF_PRINT_TMPL.IS_DELETE = true;
                    PF_PRINT_TMPL.MODIFY_DATE = DateTime.Now;
                    _context.Update(PF_PRINT_TMPL);
                }
            }
            await _context.SaveChangesAsync();

            return Ok(new { msg = "�ɹ�" });
        }

        [HttpPost("up")]
        public async Task<IActionResult> Up(string GID)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:MBGL"))
            {
                return Forbid();
            }
            PF_PRINT_TMPL PF_PRINT_TMPL = _context.PF_PRINT_TMPL.SingleOrDefault("is_delete == false and gid ==@0", GID);
            if (PF_PRINT_TMPL == null)
            {
                return NotFound();
            }
            //����������
            List<PF_PRINT_TMPL> PF_PRINT_TMPLs = _context.PF_PRINT_TMPL
                .Where("is_delete == false  and super ==@1", GID, PF_PRINT_TMPL.SUPER)
                .OrderBy("ORDER ASC")
                .ToList();

            PF_PRINT_TMPL first_one = null;
            PF_PRINT_TMPL first_two = null;

            foreach (PF_PRINT_TMPL menu in PF_PRINT_TMPLs)
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
                return Ok(new { msg = "���ö�" });
            }
            double temp = 0;


            temp = PF_PRINT_TMPL.ORDER;
            PF_PRINT_TMPL.ORDER = first_one.ORDER;
            first_one.ORDER = temp;

            if (PF_PRINT_TMPL.ORDER == first_one.ORDER)
            {
                int i = 1;
                foreach (PF_PRINT_TMPL menu in PF_PRINT_TMPLs)
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
                    _context.Update(PF_PRINT_TMPL);
                }
            }
            else
            {
                _context.Update(PF_PRINT_TMPL);
                _context.Update(first_one);
            }

            await _context.SaveChangesAsync();

            return Ok(new { msg = "�ɹ�" });


        }


        [HttpPost("down")]
        public async Task<IActionResult> Down(string GID)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:MBGL"))
            {
                return Forbid();
            }
            PF_PRINT_TMPL PF_PRINT_TMPL = _context.PF_PRINT_TMPL.SingleOrDefault("is_delete == false and gid ==@0", GID);
            if (PF_PRINT_TMPL == null)
            {
                return NotFound();
            }
            //����������
            List<PF_PRINT_TMPL> PF_PRINT_TMPLs = _context.PF_PRINT_TMPL
                .Where("is_delete == false  and super ==@1", GID, PF_PRINT_TMPL.SUPER)
                .OrderBy("ORDER ASC")
                .ToList();

            PF_PRINT_TMPL last_one = null;

            for (int i = 0; i < PF_PRINT_TMPLs.Count(); i++)
            {

                if (PF_PRINT_TMPLs[i].GID.Equals(GID))
                {
                    if ((i + 1) < PF_PRINT_TMPLs.Count())
                    {
                        last_one = PF_PRINT_TMPLs[i + 1];
                    }
                    break;
                }
            }

            if (last_one == null)
            {
                return Ok(new { msg = "���õ�" });
            }
            double temp = 0;

            temp = PF_PRINT_TMPL.ORDER;
            PF_PRINT_TMPL.ORDER = last_one.ORDER;
            last_one.ORDER = temp;

            if (PF_PRINT_TMPL.ORDER == last_one.ORDER)
            {
                int i = 1;
                foreach (PF_PRINT_TMPL menu in PF_PRINT_TMPLs)
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
                    _context.Update(PF_PRINT_TMPL);
                }
            }
            else
            {
                _context.Update(PF_PRINT_TMPL);
                _context.Update(last_one);
            }
            await _context.SaveChangesAsync();

            return Ok(new { msg = "�ɹ�" });


        }
    }


}