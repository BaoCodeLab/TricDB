using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Model;
using Main.ViewModels;
using TDSCoreLib;
using System.Linq.Dynamic.Core;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Main.platform;
using Microsoft.AspNetCore.Authorization;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/cms/channel")]
    public class API_CMS_CHANNEL : Controller
    {
        private readonly drugdbContext _context;

        public API_CMS_CHANNEL(drugdbContext context)
        {
            _context = context;
        }
        [HttpGet, Route("all")]
        public List<VM_CMS_CHANNEL> GetAll()
        {
            var mData = (from d in _context.CMS_CHANNEL where d.IS_DELETE == false select d).ToList();
            var vmData = Mapper.Map<List<CMS_CHANNEL>, List<VM_CMS_CHANNEL>>(mData);
            return vmData;
        }
        // GET: api/channel
        [HttpGet]
        public ResultList<VM_CMS_CHANNEL> Get([FromQuery]int page = 1, int limit = 10, string searchfield = "", string searchword = "", string field = "CHANNELXH", string order = "ASC")
        {
            searchfield = string.IsNullOrEmpty(searchfield) ? "PARENTCHANNELID" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "root" : searchword;
            string searchtype = ".Contains(@0)";
            if (typeof(CMS_CHANNEL).GetProperty(searchfield).PropertyType != typeof(System.String))
            {
                searchtype = "=@0";
            }
            var query = _context.CMS_CHANNEL.Where((searchfield + searchtype + " and is_delete == false"), searchword);
            var result = query.OrderBy(field + " " + order).Skip((page - 1) * limit).Take(limit).ToList();
            return new ResultList<VM_CMS_CHANNEL>
            {
                TotalCount = query.Count(),
                Results = Mapper.Map<List<CMS_CHANNEL> , List<VM_CMS_CHANNEL>>(result)
            };
        }

        // GET: api/channel/CHANNELID
        [HttpGet("{CHANNELID}")]
        public async Task<IActionResult> Get([FromRoute] string CHANNELID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CMS_CHANNEL queryResult = await _context.CMS_CHANNEL.SingleOrDefaultAsync(m => m.CHANNELID == CHANNELID);

            if (queryResult == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<CMS_CHANNEL, VM_CMS_CHANNEL>(queryResult));
        }

        // PUT: api/channel/CHANNELID
        [HttpPut("{CHANNELID}")]
        public async Task<IActionResult> Update([FromRoute] string CHANNELID, [FromForm] VM_CMS_CHANNEL postData)
        {
            if (CHANNELID != postData.CHANNELID)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            try
            {
                CMS_CHANNEL entity = Mapper.Map<VM_CMS_CHANNEL, CMS_CHANNEL>(postData);
                if (entity.CHANNELREDIRECT == null) entity.CHANNELREDIRECT = string.Empty;
                if (entity.BZ == null) entity.BZ = string.Empty;
                if (entity.CHANNELNAME == null) entity.CHANNELNAME = string.Empty;
                if (entity.PRECHANNELNAME == null) entity.PRECHANNELNAME = string.Empty;
                entity.OPERATOR = Permission.getCurrentUser();
                entity.MODIFY_DATE = DateTime.Now;
                _context.Update(entity);
                await _context.SaveChangesAsync<VM_CMS_CHANNEL>();
                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CMS_CHANNELExists(postData.CHANNELID))
                {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
                else
                {
                    return Ok(new { success = "false", msg = ex.Message });
                    //日志记录
                }
            }
        }

        // POST: api/channel
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] VM_CMS_CHANNEL postData)
        {
            var data = Mapper.Map<VM_CMS_CHANNEL, CMS_CHANNEL>(postData);
            if (data.CHANNELREDIRECT == null) data.CHANNELREDIRECT = string.Empty;
            if (data.BZ == null) data.BZ = string.Empty;
            if (data.CHANNELNAME == null) data.CHANNELNAME = string.Empty;
            if (data.PRECHANNELNAME == null) data.PRECHANNELNAME = string.Empty;

            _context.CMS_CHANNEL.Add(data);
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException ex)
            {
                if (CMS_CHANNELExists(postData.CHANNELID))
                {
                    return BadRequest(WebAPIErrorMsg.Failure("已存在此编号数据，请检查"));
                }
                else
                {
                    return BadRequest(WebAPIErrorMsg.Failure(ex.Message));
                }
            }

        }

        // DELETE: api/channel/CHANNELID
        [HttpDelete("{CHANNELID?}")]
        public async Task<IActionResult> Delete([FromForm] string CHANNELID)
        {
            CMS_CHANNEL cms_channel = await _context.CMS_CHANNEL.SingleOrDefaultAsync(m => m.CHANNELID == CHANNELID);
            cms_channel.IS_DELETE = true;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CMS_CHANNELExists(cms_channel.CHANNELID))
                {
                    return Ok(new { msg = "数据不存在或已删除" });
                }
                else
                {
                    return Ok(new { msg = ex.Message });
                }
            }
        }


        // 批量删除指定@primaryKeyName的数据
        [HttpDelete, Route("bulkdelete")]
        public async Task<IActionResult> bulkDelete([FromForm] string data)
        {
            string[] CHANNELID_Array = data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var list = _context.CMS_CHANNEL.Where(w => CHANNELID_Array.Contains(w.CHANNELID)).ToList();
            foreach (var item in list)
            {
                item.IS_DELETE = true;
                item.MODIFY_DATE = DateTime.Now;
                item.OPERATOR = Permission.getCurrentUser();
            }
            int x = await _context.SaveChangesAsync();
            if (x > -1)
            {
                return Ok(new { success = "true", msg = "成功删除" + x + "条数据" });
            }
            else
            {
                return Ok(new { success = "true", msg = "删除失败" });

            }
        }
        [HttpGet("tree")]
        public IActionResult GetDirs()
        {
            if (!Permission.check(HttpContext, "OPERATE:DQLMML"))
            {
                return Forbid();
            }
            try
            {
                //1、执行查询
                List<CMS_CHANNEL> queryResult = _context.CMS_CHANNEL.Where("is_delete = false")
                .OrderBy("CHANNELXH asc")//按条件排序
                .ToList();//将结果转为List类型
                List<VM_CHANNEL_DIR> results = new List<VM_CHANNEL_DIR>();
                VM_CHANNEL_DIR ROOT = new VM_CHANNEL_DIR();
                ROOT.id = "root";
                ROOT.parentid = "root";
                ROOT.title = "根目录";
                ROOT.name = "根目录";
                ROOT.spread = true;
                ROOT.href = string.Empty;
                ROOT.redirect = string.Empty;
                results.Add(ROOT);
                List<VM_CHANNEL_DIR> trees = new List<VM_CHANNEL_DIR>();
                ROOT.children = trees;
                //2、转换为层级结构
                //查询所有父类菜单
                foreach (CMS_CHANNEL query in queryResult)
                {
                    if (query.PARENTCHANNELID.Equals("root"))
                    {
                        VM_CHANNEL_DIR PARENT = new VM_CHANNEL_DIR
                        {
                            id = query.CHANNELID,
                            title = query.CHANNELNAME,
                            name = query.CHANNELNAME,
                            href = query.CHANNELREDIRECT,
                            parentid = query.PARENTCHANNELID,
                            spread = true
                        };
                        trees.Add(PARENT);
                    }
                }

                //嵌套查询子菜单
                foreach (VM_CHANNEL_DIR parent in trees)
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
        private List<VM_CHANNEL_DIR> childrenSpread(List<CMS_CHANNEL> queryResult, VM_CHANNEL_DIR parent)
        {
            List<VM_CHANNEL_DIR> trees = new List<VM_CHANNEL_DIR>();
            parent.children = trees;
            parent.spread = true;
            var list = from n in queryResult where n.PARENTCHANNELID.Equals(parent.id) select n;
            foreach (CMS_CHANNEL query in list)
            {
                VM_CHANNEL_DIR PARENT = new VM_CHANNEL_DIR
                {
                    id = query.CHANNELID,
                    title = query.CHANNELNAME,
                    name = query.CHANNELNAME,
                    href = query.CHANNELREDIRECT,
                    redirect=query.CHANNELREDIRECT,
                    parentid=query.PARENTCHANNELID
                };
                PARENT.children = childrenSpread(queryResult, PARENT);
                trees.Add(PARENT);
            }
            return trees;
        }
        [HttpPost("up")]
        public async Task<IActionResult> Up(string CHANNELID)
        {
            if (!Permission.check(HttpContext, "OPERATE:LMBJ"))
            {
                return Forbid();
            }
            CMS_CHANNEL channel = _context.CMS_CHANNEL.SingleOrDefault("is_delete == false and CHANNELID ==@0", CHANNELID);
            if (channel == null)
            {
                return NotFound();
            }
            //搜索上两级
            List<CMS_CHANNEL> channels = _context.CMS_CHANNEL
                .Where("is_delete == false  and PARENTCHANNELID==@0 and CHANNELXH<@1",channel.PARENTCHANNELID,channel.CHANNELXH)
                .OrderBy("CHANNELXH DESC")
                .ToList();
            if (channels.Count == 0)
            {
                return Ok(new { msg = "已置顶" });
            }
            else
            {
                int current = channel.CHANNELXH;
                int pre = channels[0].CHANNELXH;
                channel.CHANNELXH = pre;
                channels[0].CHANNELXH = current; 
                await _context.SaveChangesAsync();
            }
            return Ok(new { msg = "成功" });
        }


        [HttpPost("down")]
        public async Task<IActionResult> Down(string CHANNELID)
        {
            if (!Permission.check(HttpContext, "OPERATE:LMBJ"))
            {
                return Forbid();
            }
            CMS_CHANNEL channel = _context.CMS_CHANNEL.SingleOrDefault("is_delete == false and CHANNELID ==@0", CHANNELID);
            if (channel == null)
            {
                return NotFound();
            }
            //搜索上两级
            List<CMS_CHANNEL> channels = _context.CMS_CHANNEL
                .Where("is_delete == false  and PARENTCHANNELID==@0 and CHANNELXH>@1", channel.PARENTCHANNELID, channel.CHANNELXH)
                .OrderBy("CHANNELXH ASC")
                .ToList();
            if (channels.Count == 0)
            {
                return Ok(new { msg = "已置底" });
            }
            else
            {
                int current = channel.CHANNELXH;
                int next = channels[0].CHANNELXH;
                channel.CHANNELXH = next;
                channels[0].CHANNELXH = current;
                await _context.SaveChangesAsync();
            }
            return Ok(new { msg = "成功" });
        }
        private bool CMS_CHANNELExists(string CHANNELID)
        {
            return _context.CMS_CHANNEL.Any(e => e.CHANNELID == CHANNELID);
        }
    }
}
