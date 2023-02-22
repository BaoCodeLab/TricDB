using Main.Extensions;
using Main.platform;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using TDSCoreLib;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/report")]
    public class API_REPORT : Controller
    {
        private readonly drugdbContext _context;
        private static readonly string host = AppConfigurtaionServices.Configuration["AppSettings:host"];
        public API_REPORT(drugdbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 高级检索（报表）
        /// </summary>
        /// <param name="pFormCollection"></param>
        /// <returns></returns>
        [HttpPost("advsearch")]
        public ResultList<object> AdvSearch(IFormCollection pFormCollection)
        {
            //判断是否为符合要求的请求
            string ORGPATH = "";
            try
            {
                ORGPATH = Permission.getCurrentUserOrg().ORG_PATH;
            }
            catch
            {
                new ResultList<object>
                {
                    TotalCount = 0,
                    Message = "未授权",
                    Results = null,
                    StateCode = 403
                };

            }
            int limit = pFormCollection.Where(w => w.Key == "limit").Select(f => int.Parse(f.Value)).FirstOrDefault();
            int page = pFormCollection.Where(w => w.Key == "page").Select(f => int.Parse(f.Value)).FirstOrDefault();
            string field = pFormCollection.Where(w => w.Key == "field").Select(f => f.Value).FirstOrDefault();
            string order = pFormCollection.Where(w => w.Key == "order").Select(f => f.Value).FirstOrDefault();
            string viewmodel = pFormCollection.Where(w => w.Key == "viewmodel").Select(f => f.Value).FirstOrDefault();
            string model = pFormCollection.Where(w => w.Key == "model").Select(f => f.Value).FirstOrDefault();
            string controller = pFormCollection.Where(w => w.Key == "c").Select(f => f.Value).FirstOrDefault();
            string action = pFormCollection.Where(w => w.Key == "a").Select(f => f.Value).FirstOrDefault();
            string url = pFormCollection.Where(w => w.Key == "url").Select(f => f.Value).FirstOrDefault();
            string type = pFormCollection.Where(w => w.Key == "type").Select(f => f.Value).FirstOrDefault();
            Type ViewModel = Assembly.Load(new AssemblyName("Main")).GetType("Main.ViewModels." + viewmodel);
            if (field == null)
            {
                field = "CREATE_DATE";
            }
            if (order == null)
            {
                order = "DESC";
            }
            string query = string.Empty;

            //对于提供了Model参数的，默认从数据库查询；没有Model参数的，从远程接口查询
            if (!string.IsNullOrEmpty(model))
            {
                PropertyInfo[] pi = ViewModel.GetProperties();
                int PropIndex = 0;
                List<object> values = new List<object>();
                for (int i = 0; i < pi.Length; i++)
                {
                    var p = pi[i];
                    UIHintAttribute UIHint = (UIHintAttribute)p.GetCustomAttribute(typeof(UIHintAttribute));
                    enableSearchAttribute EnableSearch = (enableSearchAttribute)p.GetCustomAttribute(typeof(enableSearchAttribute));
                    string key = p.Name;
                    string value = pFormCollection.Where(w => w.Key == key).Select(s => s.Value).FirstOrDefault();
                    if (!string.IsNullOrEmpty(value) && value != "0" && EnableSearch != null)
                    {
                        if (UIHint != null)
                        {
                            if (UIHint.UIHint.ToLower() == "date")
                            {
                                string[] timeRange = value.Replace(" - ", "|").Split('|');
                                DateTime start = Convert.ToDateTime(timeRange[0]);
                                DateTime end = Convert.ToDateTime(timeRange[1]);
                                query += "Convert.ToDateTime(" + key + ")" + ">@" + PropIndex + " and " + "Convert.ToDateTime(" + key + ")" + "<@" + (PropIndex + 1);
                                values.Add(start);
                                values.Add(end);
                                PropIndex += 2;
                            }
                            else
                            {
                                query += key + ".Contains(@" + PropIndex + ")";
                                values.Add(value);
                                PropIndex++;
                            }
                        }
                        else if (p.PropertyType != typeof(System.String))
                        {

                            query += key + "=@" + PropIndex;
                            values.Add(value);
                            PropIndex++;
                        }
                        else
                        {
                            query += key + ".Contains(@" + PropIndex + ")";
                            values.Add(value);
                            PropIndex++;
                        }
                        query += " and ";
                    }
                }
                query += "IS_DELETE=false";
                Type Model = Assembly.Load(new AssemblyName("Model")).GetType("Model.Model." + model);
                //如果该实体包含数据权限定义，则控制数据权限范围
                if (Model.GetProperty("ORGPATH") != null)
                {
                    query = query + " and ORGPATH.Contains(@" + PropIndex + ")";
                    values.Add(ORGPATH);
                }
                var queryResult = _context.Query(Model).Where(query, values.ToArray());
                if (limit > 0 && page > 0)
                {

                    return new ResultList<dynamic>
                    {
                        TotalCount = queryResult.Count(),
                        Results = queryResult.OrderBy(field + " " + order).Skip((page - 1) * limit).Take(limit).AsEnumerable().ToList()
                    };
                }
                else
                {
                    return new ResultList<dynamic>
                    {
                        TotalCount = queryResult.Count(),
                        Results = queryResult.AsEnumerable().ToList()
                    };

                }
            }
            else if (!string.IsNullOrEmpty(url))
            {
                foreach (var param in pFormCollection)
                {
                    query += param.Key + "=" + param.Value + "&";
                }
                string remote = "";
                try
                {
                    if (type.ToLower() == "get")
                    {
                        remote = HttpClientHelper.GetResponse(url + query);
                    }
                    else
                    {
                        remote = HttpClientHelper.PostResponse(url, query);

                    }
                    var result = JsonConvert.DeserializeObject(url);
                    return new ResultList<object> { Results = result };
                }
                catch (Exception ex)
                {
                    Log.Write(this.GetType(), "高级报表查询失败", "REPORT", ex.ToString());
                    return new ResultList<object>
                    {
                        StateCode = 404,
                        Message = "未找到符合条件的数据或查询结果超出限制"
                    };

                }
            }
            else
            {
                //远程API必须以ResultList对象格式返回
                foreach (var param in pFormCollection)
                {
                    query += param.Key + "=" + param.Value + "&";
                }
                string remoteurl = HttpContext.Request.Scheme + "://" + host + Url.Action(action, controller);

                string remote = HttpClientHelper.PostResponse(remoteurl, query);
                try
                {
                    var result = JsonConvert.DeserializeObject<ResultList<dynamic>>(remote);
                    return result;
                }
                catch (Exception ex)
                {
                    Log.Write(this.GetType(), "远程报表查询失败", "REPORT", ex.ToString());
                    return new ResultList<object>
                    {
                        StateCode = 404,
                        Message = "未找到符合条件的数据或查询结果超出限制"
                    };

                }
            }
        }

        /// <summary>
        /// 表单设计器-表单报表
        /// 分为基本查询条件（时间、创建人等通用信息）以及字段查询条件
        /// </summary>
        /// <param name="pFormCollection"></param>
        /// <returns></returns>
        [HttpPost("formsearch")]
        public ResultList<object> FormSearch(IFormCollection pFormCollection)
        {
            string FORM = pFormCollection.Where(w => w.Key == "FORM").Select(f => f.Value).FirstOrDefault();
            string KEY = pFormCollection.Where(w => w.Key == "KEY").Select(f => f.Value).FirstOrDefault();
            string VALUE = pFormCollection.Where(w => w.Key == "VALUE").Select(f => f.Value).FirstOrDefault();
            string ORGPATH = pFormCollection.Where(w => w.Key == "ORGPATH").Select(f => f.Value).FirstOrDefault();
            string USERID = pFormCollection.Where(w => w.Key == "USERID").Select(f => f.Value).FirstOrDefault();
            string USERNAME = pFormCollection.Where(w => w.Key == "USERNAME").Select(f => f.Value).FirstOrDefault();
            string ROLE = pFormCollection.Where(w => w.Key == "ROLE").Select(f => f.Value).FirstOrDefault();
            string CREATE_DATE = pFormCollection.Where(w => w.Key == "CREATE_DATE").Select(f => f.Value).FirstOrDefault();
            var RYDA = _context.PF_PROFILE.Where(w => w.IS_DELETE == false).Select(s => new { s.CODE, s.NAME }).ToList();
            DateTime start = Convert.ToDateTime("1900-01-01");
            DateTime end = Convert.ToDateTime("2999-01-01");
            List<string> UserIDs = new List<string>();
            List<string> OrgPaths = ORGPATH.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //处理参数
            if (!string.IsNullOrEmpty(CREATE_DATE))
            {
                string[] timeRange = CREATE_DATE.Replace(" - ", "|").Split('|');
                start = Convert.ToDateTime(timeRange[0]);
                end = Convert.ToDateTime(timeRange[1]);
            }
            //权限检查并构建限制语句
            string permissionStr = " and 1>2";
            ArrayList queryParams = new ArrayList() { FORM, start, end };
            var formPermission = _context.FD_FORM_PERMISSION.Where(w => w.IS_DELETE == false && w.FORM_KEY == FORM).ToList();
            var userRoles = Permission.getCurrentUserRoles();
            var permissionRoles = formPermission.Select(s => s.ROLE_CODE).Intersect(userRoles).ToList();
            if (permissionRoles.Count > 0)
            {
                string permissionType = "仅个人";
                List<string> Types = formPermission.Where(w => permissionRoles.Contains(w.ROLE_CODE)).Select(s => s.TYPE).ToList();
                //一个用户可能有多个角色，会对应多个表单权限，以最高权限为准：全部>自定义>下级>仅个人
                List<string> typeQueue = new List<string>() { "全部", "自定义", "下级", "仅个人" };
                foreach (string type in Types)
                {
                    if (typeQueue.FindIndex(s => s == type) < typeQueue.FindIndex(s => s == permissionType))
                    {
                        permissionType = type;
                    }
                }
                switch (permissionType)
                {
                    case "全部":
                        {
                            if (OrgPaths.Count > 0)
                            {
                                permissionStr = " and @" + queryParams.Count + ".Contains(orgPath)";
                                queryParams.Add(OrgPaths);
                            }
                            else
                            {
                                permissionStr = "";
                            }
                        }; break;
                    case "下级":
                        {
                            permissionStr = " and orgPath.Contains(@" + queryParams.Count + ") and @" + (queryParams.Count + 1) + ".Contains(orgPath) or operator.Contains(@" + (queryParams.Count + 2) + ")";
                            queryParams.Add(Permission.getCurrentUserOrg().ORG_PATH);
                            queryParams.Add(OrgPaths);
                            queryParams.Add(Permission.getCurrentUser());//包括自己
                        }; break;
                    case "仅个人":
                        {
                            permissionStr = " and operator.Contains(@" + queryParams.Count + ")";
                            queryParams.Add(Permission.getCurrentUser());
                        }; break;
                    case "自定义":
                        {
                            List<string> allowORGs = new List<string>();
                            foreach (string role in permissionRoles)
                            {
                                List<string> tempORGs = formPermission.Where(w => w.ROLE_CODE == role).Select(s => s.ORGPATH).First().Split(',').ToList();
                                allowORGs.Union(tempORGs).ToList();
                            }
                            permissionStr = " and @" + queryParams.Count + ".Contains(orgPath)";
                            //权限允许的组织，与检索条件的组织取交集
                            var orgs = allowORGs.Intersect(OrgPaths);
                            queryParams.Add(orgs.ToList());
                        }; break;
                }
            }
            //按用户名、姓名、角色检索时
            if (!string.IsNullOrEmpty(ROLE))
            {
                UserIDs.Clear();
                UserIDs = Permission.getUsersByRole(_context, ROLE);
            }
            if (!string.IsNullOrEmpty(USERNAME))
            {
                try
                {
                    UserIDs.Clear();
                    USERID = _context.PF_PROFILE.Where(w => w.NAME == USERNAME).FirstOrDefault().CODE;
                    UserIDs.Add(USERID);
                }
                catch
                {

                }
            }
            if (!string.IsNullOrEmpty(USERID))
            {
                //用户编号查询优先级 > 用户名 > 按角色
                UserIDs.Clear();
                UserIDs.Add(USERID);
            }
            if (UserIDs.Count > 0)
            {
                permissionStr += " and @" + (queryParams.Count) + ".Contains(operator)";
                queryParams.Add(UserIDs);
            }

            //查出符合基本查询条件的表单实例
            var query = _context.FD_FORM_CASE
            .Where("form_key=@0 and is_delete == false and  CREATE_DATE>@1 and CREATE_DATE<@2" + permissionStr, queryParams.ToArray());
            var formCaseIDs = query.Select(s => s.ID).ToList();
            var formDatas = _context.FD_FORM_DATA.Where(w => formCaseIDs.Contains(w.FORM_CASE_ID) && w.IS_DELETE == false);
            if (!string.IsNullOrEmpty(KEY))
            {
                //找出符合字段查询条件的实例
                var fc = formDatas.Where("key=@0 and value" + ".Contains(@1)", KEY, VALUE).Select(s => s.FORM_CASE_ID).ToList();
                //构建LayUI-Table动态表头列数据
                var formFields =
                new List<LayUI_Table>() {
                        new LayUI_Table("false","创建日期","false","CREATE_DATE","","180"),
                        new LayUI_Table("false","所属组织","false","ORGNAME","","150"),
                        new LayUI_Table("false","提交人","false","USERNAME","","120")
                    }.Union
                (_context.FD_FORM_FIELD.Where(w => w.IS_DELETE == false && w.FORM_ID == query.FirstOrDefault().FORM_ID && w.NAME == KEY).Select(s => new LayUI_Table("false","false", s.LABEL, "false", s.NAME, "", "50%", "", "", "",""))
                    .ToList());

                var result = query.Where(w => fc.Contains(w.ID)).Select(s => Helper.MapToObject<object>(

                      formDatas.Where(w => w.FORM_CASE_ID == s.ID && w.KEY == KEY && w.VALUE.Contains(VALUE)).Select(sel => new KeyValuePair<string, object>(KEY, sel.VALUE))
                      .Union(new Dictionary<string, object>() {
                    {"CASEID",s.ID },
                    { "CREATE_DATE",s.CREATE_DATE.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "ORGNAME",s.ORGNAME },
                    { "USERNAME",RYDA.Where(w => w.CODE == s.OPERATOR).Select(sl => sl.NAME).FirstOrDefault() } })
                      .ToDictionary(t => t.Key, t => t.Value)
                  )).ToList();
                return new ResultList<object>
                {
                    TotalCount = query.Count(),
                    Results = result,
                    Append = formFields
                };
            }
            else
            {
                //构建LayUI-Table动态表头列数据
                var formFields =
                new List<LayUI_Table>() {
                        new LayUI_Table("false","创建日期","false","CREATE_DATE","","180"),
                        new LayUI_Table("false","所属组织","false","ORGNAME","","150"),
                        new LayUI_Table("false","提交人","false","USERNAME","","120")
                    }.Union
                (_context.FD_FORM_FIELD.Where(w => w.IS_DELETE == false && w.FORM_ID == query.FirstOrDefault().FORM_ID).Select(s => new LayUI_Table("false", "false",s.LABEL, "false", s.NAME, "", "150", "", "", "",""))
                    .ToList());
                var result = query.Select(s => Helper.MapToObject<object>(
                    formDatas.Where(w => w.FORM_CASE_ID == s.ID).Select(sel => new KeyValuePair<string, object>(sel.KEY, sel.VALUE)).Distinct()
                      .Union(new Dictionary<string, object>() {
                    {"CASEID",s.ID },
                    { "CREATE_DATE",s.CREATE_DATE.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "ORGNAME",s.ORGNAME },
                    { "USERNAME",RYDA.Where(w => w.CODE == s.OPERATOR).Select(sl => sl.NAME).FirstOrDefault() } })
                      .ToDictionary(t => t.Key, t => t.Value)
                  )).ToList();
                return new ResultList<object>
                {
                    TotalCount = query.Count(),
                    Results = result,
                    Append = formFields
                };
            }
        }
    }
}
