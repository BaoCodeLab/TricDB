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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Antlr4.StringTemplate;
using System.Text.RegularExpressions;
using Hangfire;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/reminder")]
    public class API_PF_REMINDER : Controller
    {
        private readonly drugdbContext _context;

        public API_PF_REMINDER(drugdbContext context)
        {
            _context = context;
        }

        // GET: api/API_PF_REMINDER
        /// <summary>
        /// 获取PF_REMINDER数据列表
        ///
        ////</summary>
        /// <returns>api/API_PF_REMINDER视图模型</returns>
        [HttpGet]
        public ResultList<VM_PF_REMINDER>
            Get([FromQuery]int page = 1, int limit = 10, string searchfield = "", string searchword = "", string field = "CREATE_DATE", string order = "DESC")
        {
            searchfield = string.IsNullOrEmpty(searchfield) ? "GID" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = _context.PF_REMINDER
            .Where((searchfield + ".Contains(@0) and is_delete == false"), searchword)
            .OrderBy(field + " " + order)//按条件排序
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
            return new ResultList<VM_PF_REMINDER>
            {
                TotalCount = _context.PF_REMINDER.Where(searchfield + ".Contains(@0) and is_delete == false", searchword).Count(),
                Results = Mapper.Map<List<PF_REMINDER>, List<VM_PF_REMINDER>>(queryResult)
            };
        }

        // GET: api/API_PF_REMINDER/5
        /// <summary>
        /// 获取PF_REMINDER数据详情
        ///</summary>
        /// <returns>api/API_PF_REMINDER视图模型</returns>
        [HttpGet("{GID}")]
        public async Task<IActionResult> Get([FromRoute] string GID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PF_REMINDER queryResult = await _context.PF_REMINDER.SingleOrDefaultAsync(m => m.GID == GID);

            if (queryResult == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PF_REMINDER, VM_PF_REMINDER>(queryResult));
        }

        // PUT: api/API_PF_REMINDER/5
        /// <summary>
        /// 更新单条PF_REMINDER数据
        ///</summary>
        /// <returns>执行结果反馈</returns>
        [HttpPut("{GID}")]
        public async Task<IActionResult> Update([FromRoute] string GID, [FromForm] VM_PF_REMINDER postData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (GID != postData.GID)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            try
            {
                PF_REMINDER entity = Mapper.Map<VM_PF_REMINDER, PF_REMINDER>(postData);
                _context.Update(entity);
                await _context.SaveChangesAsync<VM_PF_REMINDER>();
                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PF_REMINDERExists(postData.GID))
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

        // POST: api/API_PF_REMINDER
        /// <summary>
        /// 新增单条PF_REMINDER数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PF_REMINDER postData)
        {
            ModelState.Remove("CREATE_DATE");
            ModelState.Remove("MODIFY_DATE");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            postData.GID = Guid.NewGuid().ToString().ToLower();
            postData.CREATE_DATE = DateTime.Now;
            postData.MODIFY_DATE = DateTime.Now;
            postData.OPERATOR = Permission.getCurrentUser();
            postData.IS_DELETE = false;
            _context.PF_REMINDER.Add(postData);
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException)
            {
                if (PF_REMINDERExists(postData.GID))//此处请按照业务要求，将字段更改为需要判断非重复的字段，如用户名、客户编号等
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

        }

        // DELETE: api/API_PF_REMINDER/5
        /// <summary>
        /// 删除单条PF_REMINDER数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpDelete("{GID?}")]
        public async Task<IActionResult> Delete([FromForm] string GID)
        {
            //更新删除标记模式
            PF_REMINDER PF_REMINDER = await _context.PF_REMINDER.SingleOrDefaultAsync(m => m.GID == GID);
            PF_REMINDER.IS_DELETE = true;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PF_REMINDERExists(PF_REMINDER.GID))
                {
                    return Ok(new { msg = "数据不存在或已删除" });
                }
                else
                {
                    return Ok(new { msg = ex.Message });
                }
            }
        }


        /// <summary>
        /// 批量删除指定GID的数据
        /// </summary>
        /// <param name="gid">待删除数据的gid集合，以分号隔开</param>
        /// <returns></returns>
        [HttpDelete, Route("bulkdelete")]
        public async Task<IActionResult> bulkDelete([FromForm] string gids)
        {
            string[] GIDs = gids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string queryIn = "(";
            foreach (var GID in GIDs)
            {
                queryIn += "'" + GID + "'";
            }
            queryIn += ")";
            int x = await _context.Database.ExecuteSqlCommandAsync("Update PF_REMINDER set IS_DELETE=1,MODIFY_DATE=getdate() where GID in " + queryIn.Replace("''", "','"));
            if (x > -1)
            {
                return Ok(new { success = "true", msg = "成功删除" + x + "条数据" });
            }
            else
            {
                return Ok(new { success = "true", msg = "未删除数据" });

            }
        }

        /// <summary>
        /// 执行提醒任务
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("checktasks")]
        public IActionResult checkRemindTasks()
        {
            var taskList = _context.PF_REMINDER.Select(s => s).ToList();
            foreach (var task in taskList)
            {
                if (task.IS_DELETE == false && task.SDATE < DateTime.Now && task.EDATE > DateTime.Now)
                {
                    string content = "";
                    if (task.API.Length > 0)
                    {
                        RecurringJob.AddOrUpdate(task.GID, () => doRemindTasks(task), task.RULE, TimeZoneInfo.Local);

                    }
                    else
                    {
                        content = task.CONTENT;
                    }
                }
                else
                {
                    //移除失效的任务
                    RecurringJob.RemoveIfExists(task.GID);
                }

            }
            return Ok();
        }

        /// <summary>
        /// 定时提醒任务的执行
        /// </summary>
        /// <param name="task"></param>
        public void doRemindTasks(PF_REMINDER task)
        {
            string content = "";

            if (task.API.Length > 0)
            {
                string result = HttpClientHelper.GetResponse(task.API);
                string template = task.CONTENT;
                JObject obj = (JObject)JsonConvert.DeserializeObject(result);
                JObject token = (JObject)obj.SelectToken(string.IsNullOrEmpty(task.PARAM1) ? "" : task.PARAM1);
                //使用StringTemplate4渲染模板
                //参考：https://github.com/antlr/antlrcs/blob/master/Antlr4.Test.StringTemplate/TestCoreBasics.cs
                Template tmpl = new Template(template);
                string pattern = @"(?<=<)[^<]*(?=>)";
                Regex regex = new Regex(pattern);
                foreach (Match match in regex.Matches(template))
                {
                    string key = match.Value;
                    string value = token.Property(key).Value.ToString();
                    tmpl.Add(key, value);
                }
                content = tmpl.Render();
            }
            else

            {
                content = task.CONTENT;
            }

            Push.ExecutePush(task.TITLE, content, StringToList(task.USERS), StringToList(task.ROLES), "");
        }
        /// <summary>
        /// 将 编码1（名称1）;编码2（名称2）; 形式的字符串转换为仅包含编码的List<string></string>
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        private List<String> StringToList(string input)
        {
            List<String> list = new List<string>();
            string[] arr = input.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length == 0)
            {
                if (input.Length > 0)
                {
                    list.Add(input.Split(new char[] { '(' }, StringSplitOptions.RemoveEmptyEntries)[0]);
                }
            }
            else
            {
                foreach (string user in arr)
                {
                    string[] arr2 = user.Split(new char[] { '(' }, StringSplitOptions.RemoveEmptyEntries);
                    list.Add(arr2[0]);
                }
            }
            return list;
        }
        private bool PF_REMINDERExists(string GID)
        {
            return _context.PF_REMINDER.Any(e => e.GID == GID);
        }
    }
}
