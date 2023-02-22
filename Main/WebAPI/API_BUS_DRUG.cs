using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Model;
using Main.ViewModels;
using AutoMapper;
using TDSCoreLib;
using Main.platform;
using Main.Extensions;
using Main.Utils;
using System.Data;
using System.Data.OleDb;
using System.IO;
using ExcelDataReader;
using System.Web;
using MySql.Data.MySqlClient;
using Senparc.Weixin;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/drug")]
    public class API_BUS_DRUG : Controller
    {
        private readonly drugdbContext _context;

        public API_BUS_DRUG(drugdbContext context)
        {
            _context = context;
        }

        // GET: api/API_BUS_DRUG
        /// <summary>
        /// 获取BUS_DRUG数据列表
        ///</summary>
        /// <returns>api/API_BUS_DRUG视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult Get([FromQuery]int page = 1, int limit = 10, string searchfield = "DRUG_NAME", string searchword = "", string field = "DRUG_NAME", string order = "ASC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchfield = string.IsNullOrEmpty(searchfield) ? "DRUG_NAME" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = _context.BUS_DRUG.Where(m => m.VERSION.Equals(culture))
           .Where(searchfield + ".Contains(@0) and IS_DELETE==false", searchword)
           .OrderBy(field + " " + order)//按条件排序
           .Skip((page - 1) * limit) //跳过前x项
           .Take(limit)//从当前位置开始取前x项
           .ToList();//将结果转为List类型
            return Ok(new ResultList<VM_BUS_DRUG>
            {
                TotalCount = _context.BUS_DRUG.Where(searchfield + ".Contains(@0) and IS_DELETE==false", searchword).Count(),
                Results = Mapper.Map<List<BUS_DRUG>, List<VM_BUS_DRUG>>(queryResult)
            });
        }
        /// <summary>
        /// 获取BUS_DRUG数据列表
        ///</summary>
        /// <returns>api/API_BUS_DRUG视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult SearchAll([FromQuery]int page = 1, int limit = 10, string searchword = "", string field = "DRUG_NAME", string order = "ASC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = this._context.BUS_DRUG.Where(m => m.VERSION.Equals(culture) && m.IS_PUB && !m.IS_DELETE)
           .Where(m => m.BRAND_NAME.Contains(searchword) || m.DRUGCODE.Equals(searchword) || m.DRUG_TYPE.Equals(searchword) || m.DRUG_NAME.Contains(searchword)).AsQueryable();
            var ret = queryResult.OrderBy(field + " " + order)//按条件排序
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
            return Ok(new ResultList<BUS_DRUG>
            {
                TotalCount = queryResult.Count(),
                Results = ret
            });
        }
        // GET: api/API_BUS_DRUG
        /// <summary>
        /// 获取BUS_DRUG数据列表
        ///</summary>
        /// <returns>api/API_BUS_DRUG视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult Search([FromQuery]int page = 1, int limit = 10, string searchword = "", string field = "DRUG_NAME", string order = "ASC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword.ToLower();
            string query = "select * from bus_all";
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>(query).Where(m => m.VERSION.Equals(culture)).Where(m => m.DATA_TYPE != "germline")
           .Where(m => (m.BRAND_NAME != null && m.BRAND_NAME.ToLower().Contains(searchword)) || (m.DRUGCODE != null && m.DRUGCODE.ToLower().Contains(searchword)) || (m.DRUG_TYPE != null && m.DRUG_TYPE.ToLower().Contains(searchword)) || (m.DRUG_NAME != null && m.DRUG_NAME.ToLower().Contains(searchword)) || (m.DRUG_TARGET != null && m.DRUG_TARGET.ToLower().Contains(searchword)) || (m.EVIDENCE_LEVEL != null && m.EVIDENCE_LEVEL.ToLower().Contains(searchword))).AsQueryable();
            if (!User.Identity.IsAuthenticated)
            {
                queryResult = queryResult.Take(10);
            }
            var ret = queryResult
            .OrderBy(field + " " + order)//按条件排序
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
            return Ok(new ResultList<VM_BUS_ALL>
            {
                TotalCount = queryResult.Count(),
                Results = ret
            });
        }

        [HttpGet, Route("[action]")]
        public IActionResult DrugSearch([FromQuery] int page = 1, int limit = 10, string searchword = "", string field = "DRUG_NAME", string order = "ASC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword.ToLower();
            string query = "select * from bus_all";
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>(query).Where(m => m.VERSION.Equals(culture))
                .Where(m => (m.BRAND_NAME != null && m.BRAND_NAME.ToLower().Contains(searchword)) ||
                            (m.DRUGCODE != null && m.DRUGCODE.ToLower().Contains(searchword)) ||
                            (m.DRUG_TYPE != null && m.DRUG_TYPE.ToLower().Contains(searchword)) ||
                            (m.DRUG_NAME != null && m.DRUG_NAME.ToLower().Contains(searchword)) ||
                            (m.DRUG_TARGET != null && m.DRUG_TARGET.ToLower().Contains(searchword)) ||
                            (m.EVIDENCE_LEVEL != null && m.EVIDENCE_LEVEL.ToLower().Contains(searchword)))
                .AsQueryable().Select(m => new
                {
                    drugid = m.DRUGID,
                    drug_name = m.DRUG_NAME,
                    brand = m.BRAND_NAME,
                    drug_type = m.DRUG_TYPE,
                    drug_target = m.DRUG_TARGET,
                    company = m.COMPANY,
                    mechanism = m.MECHANISM_OF_ACTION
                });
            if (!User.Identity.IsAuthenticated)
            {
                queryResult = queryResult.Take(10);
            }
            var ret = queryResult
                .Take(limit)//从当前位置开始取前x项
                .ToList();//将结果转为List类型
            return Ok(new ResultList<VM_BUS_ALL>
            {
                TotalCount = queryResult.Count(),
                Results = ret
            });
        }


        /// <summary>
        /// 获取BUS_DRUG数据列表
        ///</summary>
        /// <returns>api/API_BUS_DRUG视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult ListAll([FromQuery]int page = 1, int limit = 10, string DRUGID = "", string field = "DISEASECODE", string order = "DESC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select * from bus_all where DRUGID=@DRUGID", new MySqlParameter[] { new MySqlParameter("DRUGID", DRUGID) }).Where(m => m.VERSION.Equals(culture)).AsQueryable();
            var ret = queryResult.OrderBy(field + " " + order)//按条件排序
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
            return Ok(new ResultList<BUS_DISEASE>
            {
                TotalCount = queryResult.Count(),
                Results = ret
            });
        }
        [HttpGet, Route("Detail")]
        public async Task<IActionResult> Detail([FromQuery] string DRUGID)
        {
            BUS_DRUG queryResult = await _context.BUS_DRUG.SingleOrDefaultAsync(m => m.DRUGID == DRUGID);

            if (queryResult == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<BUS_DRUG, VM_BUS_DRUG>(queryResult));
        }


        /// <summary>
        /// 获取本地结构文件位置
        /// </summary>
        /// <param name="str_id"></param>
        /// <returns></returns>
        [HttpGet, Route("[action]")]
        public IActionResult GetStructure([FromQuery] string str_id)
        {
            string xdlj = "pdbfile/" + str_id + ".pdb";
            string jdlj = WebPath.FILE_ABSOLUTE;//绝对路径
            var stream = System.IO.File.OpenRead(jdlj + xdlj);
            if (stream == null)
            {
                return NotFound(new { msg = "文件不存在" });
            }
            return File(stream, "application/octet-stream");
        }



        // PUT: api/API_BUS_DRUG/5
        /// <summary>
        /// 更新单条BUS_DRUG数据
        ///</summary>
        /// <returns>执行结果反馈</returns>
        [HttpPost, Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(VM_BUS_DRUG postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:YWBJ"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                BUS_DRUG entity = Mapper.Map<VM_BUS_DRUG, BUS_DRUG>(postData);
                entity.MODIFY_DATE = DateTime.Now;
                entity.DRUG_NAME = postData.DRUG_NAME ?? "";
                entity.BRAND_NAME = postData.BRAND_NAME ?? "";
                entity.COMPANY = postData.COMPANY ?? "";
                entity.COMPANY_ALIAS = postData.COMPANY_ALIAS ?? "";
                entity.DRUG_TARGET = postData.DRUG_TARGET ?? "";
                entity.DRUG_TYPE = postData.DRUG_TYPE ?? "";
                entity.MECHANISM_OF_ACTION = postData.MECHANISM_OF_ACTION ?? "";
                entity.STRUCTURE = postData.STRUCTURE ?? "";
                entity.VERSION = postData.VERSION;
                entity.IS_PUB = postData.IS_PUB;
                entity.OPERATOR = Permission.getCurrentUser();
                _context.BUS_DRUG.Update(entity);
                await _context.SaveChangesAsync<VM_BUS_DRUG>();
                Log.Write(GetType(), "update", "BUS_DRUG", "将药物编号为" + postData.DRUGCODE + "的数据进行更新，操作者为" + Permission.getCurrentUser());

                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!BUS_DRUGExists(postData.DRUGID))
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

        // POST: api/API_BUS_DRUG
        /// <summary>
        /// 新增单条BUS_DRUG数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BUS_DRUG postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:YWXZ"))
            {
                return Forbid();
            }
            ModelState.Remove("CREATE_DATE");
            ModelState.Remove("MODIFY_DATE");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //postData.DID = Guid.NewGuid().ToString().ToLower();
            postData.CREATE_DATE = DateTime.Now;
            postData.MODIFY_DATE = DateTime.Now;
            postData.DRUG_NAME = postData.DRUG_NAME ?? "";
            postData.BRAND_NAME = postData.BRAND_NAME ?? "";
            postData.COMPANY = postData.COMPANY ?? "";
            postData.DRUG_TARGET = postData.DRUG_TARGET ?? "";
            postData.DRUG_TYPE = postData.DRUG_TYPE ?? "";
            postData.MECHANISM_OF_ACTION = postData.MECHANISM_OF_ACTION ?? "";
            postData.STRUCTURE = postData.STRUCTURE ?? "";
            postData.VERSION = postData.VERSION;
            postData.OPERATOR = Permission.getCurrentUser();
            postData.IS_PUB = true;
            postData.IS_DELETE = false;
            _context.BUS_DRUG.Add(postData);
            try
            {
                await _context.SaveChangesAsync();
                Log.Write(GetType(), "create", "BUS_DRUG", "创建药物编号为" + postData.DRUGCODE + "，操作者为" + Permission.getCurrentUser());
                return Ok();
            }
            catch (DbUpdateException)
            {
                if (BUS_DRUGExists(postData.DRUGID))//此处请按照业务要求，将字段更改为需要判断非重复的字段，如用户名、客户编号等
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

        }

        // DELETE: api/API_BUS_DRUG/5
        /// <summary>
        /// 删除单条BUS_DRUG数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpDelete("{DRUGID?}")]
        public async Task<IActionResult> Delete([FromForm] string DRUGID)
        {
            if (!Permission.check(HttpContext, "OPERATE:YWSC"))
            {
                return Forbid();
            }
            BUS_DRUG bus_drug = await _context.BUS_DRUG.SingleOrDefaultAsync(m => m.DRUGID == DRUGID);
            if (bus_drug == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            if (StateHelper.getCodeByName("删除模式", "直接删除").Equals("1"))
            {
                _context.BUS_DRUG.Remove(bus_drug);
            }
            else
            {
                bus_drug.IS_DELETE = true;
            }
            Log.Write(_context, GetType(), "delete", "BUS_DRUG", "将药物编号" + bus_drug.DRUGCODE + "的数据的删除，操作者为" + Permission.getCurrentUser());
            await _context.SaveChangesAsync();

            return Ok(new { success = "true" });
        }

        private bool BUS_DRUGExists(string DRUGID)
        {
            return _context.BUS_DRUG.Any(e => e.DRUGID == DRUGID);
        }
        /// <summary>
        /// 查询当前平台用户总数
        /// </summary>
        /// <returns>查询结果</returns>
        [HttpGet, Route("[action]")]
        public object DrugDB()
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            int drugsum = this._context.BUS_DRUG.Where(n => n.IS_DELETE.Equals(false) && n.IS_PUB.Equals(true) && n.VERSION.Equals(culture)).Count();
            int disease = this._context.BUS_DISEASE.Where(n => n.IS_DELETE.Equals(false) && n.IS_PUB.Equals(true) && n.VERSION.Equals(culture)).Count();
            int gene = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select distinct target from bus_target").Count();
            int target = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select distinct target,alteration from bus_target").Count();
            int total = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select VERSION from bus_all").Where(m => m .VERSION.Equals(culture)).Count();
            var Main = new
            {
                Total = total,
                DrugSum = drugsum,
                DiseaseSum = disease,
                GeneSum = gene,
                TargetSum = target
            };
            return Main;
        }

        /// <summary>
        /// 当月签到结果统计
        /// </summary>
        /// <returns>查询结果</returns>
        [HttpGet, Route("[action]")]
        public object dyqd()
        {
            var series = new List<object>();
            var datas = new
            {
                id = "label",
                type = "scatter",
                coordinateSystem = "calendar",
                symbolSize = 1,
            };
            series.Add(datas);

            var Main = new
            {
                calender = new
                {
                    left = " center",
                    top = "middle",
                    cellSize = "[70, 70]",
                    range = DateTime.Now.ToString("yyyy-MM"),
                    //    cellsize = "auto",
                    orient = "vertical",
                    splitLine = new { show = true },
                    dayLabel = new { firstDay = 1, nameMap = "cn" },
                    monthLabel = new { nameMap = "cn" }

                },
                visualMap = new
                {
                    show = false,
                    min = 0,
                    max = 300,
                    calculable = true,
                    seriesIndex = "[2]",
                    orient = "horizontal",
                    left = " center",
                    bottom = 20,
                    inRange = new
                    {
                        color = "['#e0ffff', '#006edd']",
                        opacity = 0.3
                    },
                    controller = new
                    {
                        inRange = new
                        {
                            opacity = 0.5
                        }
                    }
                },

                series
            };

            return Main;
        }
        /// <summary>
        /// 取最近五条新闻
        /// </summary>
        /// <param name="field"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        /// 
        [HttpGet, Route("[action]")]
        public object Drug_sta( )
        {
            var d_all = MySQLDB.GetSimpleTFromQuery<VM_BUS_DRUG>("select distinct drug_name,company_alias,drug_type from bus_drug where is_pub=TRUE");
            var d_company = from n in d_all group n by n.COMPANY_ALIAS into g orderby g.Count() descending select new { value = g.Count(), name = g.Key };
            var d_type = from n in d_all group n by n.DRUG_TYPE into g orderby g.Count() descending select new { value = g.Count(), name = g.Key };
            var data = new
            {
                company = d_company.ToArray().Take(10),
                type = d_type.ToArray()
            };
            return data;

        }


        [HttpGet, Route("[action]")]
        public IActionResult GetLatest(string field = "PUBLISH_TIME", string order = "DESC", int page = 1, int limit = 10)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:YLWZ"))
            {
                return Forbid();
            }
            var queryResult = _context.PF_NEW
           .Where("IS_DELETE==false and STATE==@0", "1")
           .OrderBy(field + " " + order)//按条件排序
           .Take(5)//从当前位置开始取前x项
           .ToList();//将结果转为List类型
            return Ok(new ResultList<VM_PF_NEW>
            {
                Results = Mapper.Map<List<PF_NEW>, List<VM_PF_NEW>>(queryResult)
            });
        }
        [HttpGet, Route("[action]")]
        public IActionResult GetReference(string drug_name, string target, string alternation, string talias, string disease, string dalias)
        {
            try
            {
                var base_url = "https://eutils.ncbi.nlm.nih.gov/entrez/eutils/";
                var api_key = "59cb69596ed88a80ec8189e2809251996e08";
                List<string> list = new List<string>();


                var drugterm = "(";
                if (string.IsNullOrEmpty(drug_name))
                {
                    drugterm = "";
                }
                else
                {
                    drug_name = HttpUtility.UrlEncode(drug_name).ToString().Replace("plus", " + ").Replace(";", ",").Replace("+++", "+");
                    var drug_names = drug_name.Split(',');
                    if (drug_name.Contains('+'))
                    {
                        drug_names = drug_name.Split('+');
                    }
                    list.Clear();
                    foreach (var r in drug_names)
                    {
                        list.Add("(" + r + "[Title/Abstract])");
                    }
                    drugterm += string.Join(" OR ", list.ToArray());
                    drugterm += ")";
                }


                var targetterm = "(";
                if (string.IsNullOrEmpty(target))
                {
                    targetterm = "";
                }
                else
                {
                    target = target.Replace("plus", " + ").Replace(";", ",");
                    if (talias != null && talias != "")
                    {
                        target = $"{target},{talias.Replace("plus", " + ").Replace(";", ",")}";
                    }
                    var targets = target.Split(',');
                    
                    
                    list.Clear();
                    foreach (var r in targets)
                    {
                        list.Add("(" + r + "[Title/Abstract])");
                    }
                    targetterm += string.Join(" OR ", list.ToArray());
                    targetterm += ")";
                    
                }


                var alternationterm = "(";
                if (string.IsNullOrEmpty(alternation))
                {
                    alternationterm = "";
                }
                else
                {
                    alternation = alternation.Replace("plus", " + ").Replace(";", ",").Replace("p.", "");
                    var alternations = alternation.Split(',');
                    
                    list.Clear();
                    foreach (var r in alternations)
                    {
                        list.Add("(" + r + "[Title/Abstract])");
                    }
                    alternationterm += string.Join(" OR ", list.ToArray());
                    alternationterm += ")";
                }



                var diseaseterm = "(";
                if (string.IsNullOrEmpty(disease))
                {
                    diseaseterm = "";
                }
                else
                {
                    disease = disease.Replace("plus", " + ").Replace(";", ",");
                    if (dalias != null && dalias != "")
                    {
                        disease = $"{disease},{dalias.Replace("plus", " + ").Replace(";", ",").Replace(" | ", ",")}";
                    }
                    var diseases = disease.Split(',');
                    list.Clear();
                    foreach (var r in diseases)
                    {
                        list.Add("(" + r + "[Title/Abstract])");
                    }
                    diseaseterm += string.Join(" OR ", list.ToArray());
                    diseaseterm += ")";

                }


                var term = HttpUtility.UrlEncode(drugterm + " And " + targetterm + " And " + alternationterm + " And " + diseaseterm);
                Log.Write(this.GetType(), "AA", term);
                BaseNcbi bn = new BaseNcbi
                {
                    api_key = api_key,
                    base_url = base_url,
                    term = term
                };
                var res = bn.getRefArticleInfo(10);
                if (res.Count > 1)
                {
                    return Ok(new { code = 0, count = res.Count, data = res});
                }
                term = HttpUtility.UrlEncode(drugterm + " And " + targetterm + " And " + diseaseterm);
                bn.term = term;
                res = bn.getRefArticleInfo(10);
                if (res.Count < 1)
                {
                    term = HttpUtility.UrlEncode(drugterm + " And " + diseaseterm);
                    bn.term = term;
                    res = bn.getRefArticleInfo(10);
                }
                return Ok(new { code = 0, count = res.Count, data = res});
            }
            catch (Exception ex)
            {
                Log.Write(this.GetType(), "异常", "", ex.ToString());
                return Ok(new { code = 1, msg = "读取数据异常" });
            }
        }
    }
}
