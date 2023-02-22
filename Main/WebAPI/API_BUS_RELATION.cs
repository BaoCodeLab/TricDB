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
using JiebaNet.Segmenter;
using JiebaNet.Segmenter.Common;
using JiebaNet.Analyser;
using System.Net;
using System.Text;
using AngleSharp;
using AngleSharp.Html.Parser;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/relation")]
    public class API_BUS_RELATION : Controller
    {
        private readonly drugdbContext _context;

        public API_BUS_RELATION(drugdbContext context)
        {
            _context = context;
        }

        // GET: api/API_BUS_RELATION
        /// <summary>
        /// 获取BUS_RELATION数据列表
        ///</summary>
        /// <returns>api/API_BUS_RELATION视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult Get([FromQuery]int page = 1, int limit = 10, string searchfield = "TARGET", string searchword = "", string field = "TARGET", string order = "DESC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchfield = string.IsNullOrEmpty(searchfield) ? "TARGET" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_BUS_RELATION>("select a.*,d.target,d.alteration,b.drug_name,c.disease from bus_relation a ,bus_drug b,bus_disease c,bus_target d where a.targetid=d.targetid and a.drugid=b.drugid and a.diseaseid=c.diseaseid").AsQueryable().Where(m => m.IS_DELETE != true && m.IS_PUB)
           .Where(searchfield + ".Contains(@0)", searchword);
            var ret = queryResult
           .OrderBy(field + " " + order)//按条件排序
           .Skip((page - 1) * limit) //跳过前x项
           .Take(limit)//从当前位置开始取前x项
           .ToList();//将结果转为List类型
            return Ok(new ResultList<VM_BUS_RELATION>
            {
                TotalCount = queryResult.Count(),
                Results = ret
            });
        }

        [HttpGet, Route("[action]")]
        public object WordCloud()
        {
            var data1 = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("SELECT distinct indications FROM bus_all");
            var data2 = MySQLDB.GetSimpleTFromQuery<VM_BUS_DRUG>("SELECT distinct mechanism_of_action FROM bus_drug WHERE is_pub=TRUE");
            string all = "";
            foreach(var n in data1)
            {
                all = all + n.INDICATIONS;
            }
            foreach (var n in data2)
            {
                all = all + n.MECHANISM_OF_ACTION;
            }

            var tfidf = new TfidfExtractor();
            var result = tfidf.ExtractTagsWithWeight(all,100);
            List<object> text = new List<object>();
            foreach (var tag in result)
            {
                var word = new { name = tag.Word, weight = tag.Weight };
                text.Add(word);
            }

            return text;

        }



        [HttpGet, Route("[action]")]
        public IActionResult Search([FromQuery] int page = 1, int limit = 10, string searchword = "", string field = "DRUG_NAME", string order = "ASC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword.ToLower();
            string query = "select * from bus_all";
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>(query).Where(m => m.VERSION.Equals(culture)).Where(m => m.DATA_TYPE != "germline")
           .Where(m => (m.BRAND_NAME != null && m.BRAND_NAME.ToLower().Contains(searchword)) || (m.DRUGCODE != null && m.DRUGCODE.ToLower().Contains(searchword)) || (m.DRUG_TYPE != null && m.DRUG_TYPE.ToLower().Contains(searchword)) || (m.DRUG_NAME != null && m.DRUG_NAME.ToLower().Contains(searchword)) || (m.DRUG_TARGET != null && m.DRUG_TARGET.ToLower().Contains(searchword)) || (m.EVIDENCE_LEVEL != null && m.EVIDENCE_LEVEL.ToLower().Contains(searchword)) || (m.TARGET != null && m.TARGET.ToLower().Contains(searchword)) || (m.ALTERATION != null && m.ALTERATION.ToLower().Contains(searchword)) || (m.GENE_ALIAS != null && m.GENE_ALIAS.ToLower().Contains(searchword)) || (m.ENTREZ_GENEID != null && m.ENTREZ_GENEID.ToLower().Contains(searchword)) || (m.ENSEMBL_ID != null && m.ENSEMBL_ID.ToLower().Contains(searchword)) || (m.CLINICAL_SIGNIFICANCE != null && m.CLINICAL_SIGNIFICANCE.ToLower().Contains(searchword)) || (m.COSMIC_EVIDENCE != null && m.COSMIC_EVIDENCE.ToLower().Contains(searchword)) || (m.HGNC_ID != null && m.HGNC_ID.ToLower().Contains(searchword)) || (m.SPECIFICITY != null && m.SPECIFICITY.ToLower().Contains(searchword)) || (m.DISEASE != null && m.DISEASE.ToLower().Contains(searchword)) || (m.DISEASECODE != null && m.DISEASECODE.ToLower().Contains(searchword)) || (m.DISEASE_ALIAS != null && m.DISEASE_ALIAS.ToLower().Contains(searchword)) || (m.CLASSIFICATION != null && m.CLASSIFICATION.ToLower().Contains(searchword))).Where(t => t.TARGET != " ").AsQueryable();
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
        public object data_statistics()
        {
            var drug_num = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select distinct drugid from bus_all").Count();
            var gene_num = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select distinct target from bus_all").Count();
            var target_num = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select distinct targetid from bus_all").Count();
            var disease_num = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select distinct diseaseid from bus_all").Count();
            var variant_num = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select distinct variant_classification from bus_all").Count();
            var association = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select distinct relationid from bus_all").Count();

            string com = "";
            var clinialtrial = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select clinical_trial from bus_all");
            foreach (var ct in clinialtrial)
            {
                com = com + ", " + ct.CLINICAL_TRIAL;
            }

            string[] all_ct = com.Split(", ");
            var clinical_num = all_ct.Length;

            var data = new
            {
                drug_num = drug_num,
                gene_num = gene_num,
                target_num = target_num,
                disease_num = disease_num,
                variant_num = variant_num,
                clinical_num = clinical_num,
                association = association
            };
            return data;

        }


        [HttpGet, Route("Detail")]
        public async Task<IActionResult> Detail([FromQuery] string TARGETID)
        {
            BUS_RELATION queryResult = await _context.BUS_RELATION.SingleOrDefaultAsync(m => m.TARGETID == TARGETID);

            if (queryResult == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<BUS_RELATION, VM_BUS_RELATION>(queryResult));
        }

        [HttpGet, Route("[action]")]
        public IActionResult GetFdaTable()
        {
           var url =
                "https://www.fda.gov/drugs/resources-information-approved-drugs/oncology-cancer-hematologic-malignancies-approval-notifications#subscribe";

  
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //设置请求方法
                request.Method = "GET";
                //设置请求头
                request.UserAgent = "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:94.0) Gecko/20100101 Firefox/94.0";
                //获取resonse
                WebResponse response = request.GetResponse();
                //利用Stream流读取返回数据
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                //获得最终数据，一般是json
                string html = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(html))
                {
                    var parser = new HtmlParser();
                    var document = parser.ParseDocument(html);
                    var table = document.QuerySelector("tbody");
                    var tr_list = table.QuerySelectorAll("tr").Take(10);

                    List<Object> fdaData = new List<Object>();
                    foreach (var tr in tr_list)
                    {
                        var title = tr.QuerySelector("a").TextContent.Trim();
                        var href = tr.QuerySelector("a").GetAttribute("href");
                        var description = tr.QuerySelectorAll("td")[1].TextContent.Trim();
                        var pubDate = tr.QuerySelectorAll("td")[2].TextContent.Trim();
                        var article = new
                        {
                            art_title = title,
                            art_href = "https://www.fda.gov" + href,
                            art_description = description,
                            art_pubdate = pubDate
                        };
                        fdaData.Add(article);
                    }
                    return Ok(new { code = 0, data = fdaData });

                }
                return Ok(new { code = 0, data = " " });



        }


        // POST: api/API_BUS_RELATION
        /// <summary>
        /// 新增单条BUS_RELATION数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] VM_BUS_RELATION postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:GXXZ"))
            {
                return Forbid();
            }
            ModelState.Remove("CREATE_DATE");
            ModelState.Remove("MODIFY_DATE");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var drugids = postData.DRUGID.Split(",");
            var diseases = postData.DISEASEID.Split(",");
            int t = 0;
            for (int i = 0; i < drugids.Length; i++)
            {
                for (int j = 0; j < diseases.Length; j++)
                {
                    try
                    {
                        var row = new BUS_RELATION();
                        row.CREATE_DATE = DateTime.Now;
                        row.DISEASEID = diseases[j];
                        row.DRUGID = drugids[i];
                        row.IS_DELETE = false;
                        row.IS_PUB = true;
                        row.MODIFY_DATE = DateTime.Now;
                        row.OPERATOR = User.Identity.Name;
                        row.RELATIONID = Guid.NewGuid().ToString();
                        row.TARGETID = postData.TARGETID;
                        row.MUTATION_RATE = postData.MUTATION_RATE ?? "0";
                        row.CHINESE_10000_RATE = postData.CHINESE_10000_RATE ?? "0";
                        row.GERMLINE_10389_RATE = postData.GERMLINE_10389_RATE ?? "0";
                        row.ALTERATION_RATE = postData.MUTATION_RATE ?? "0";
                        row.SPECIFICITY = postData.SPECIFICITY ?? "";
                        row.EVIDENCE_LEVEL = postData.EVIDENCE_LEVEL ?? "";
                        row.CLINICAL_TRIAL = postData.CLINICAL_TRIAL ?? "";
                        row.APPROVED = postData.APPROVED ?? "";
                        row.APPROVAL_TIME = postData.APPROVAL_TIME ?? "";
                        row.INDICATIONS = postData.INDICATIONS ?? "";
                        row.DOSAGE = postData.DOSAGE ?? "";
                        row.FUNCTION_AND_CLINICAL_IMPLICATIONS = postData.FUNCTION_AND_CLINICAL_IMPLICATIONS ?? "";
                        row.THERAPY_INTERPRETATION = postData.THERAPY_INTERPRETATION ?? "";
                        row.REFERENCE_LINK = postData.REFERENCE_LINK ?? "";
                        _context.BUS_RELATION.Add(row);
                        Log.Write(GetType(), "create", "BUS_RELATION", "创建靶点、疾病、药物关联关系完成，编号为" + row.RELATIONID + "，操作者为" + Permission.getCurrentUser());
                        await _context.SaveChangesAsync();
                        t++;
                    }
                    catch (Exception ex)
                    {
                        Log.Write(GetType(), "exception", "BUS_RELATION", ex.ToString());
                    }
                }
            }
            return Ok(new { success = "true", msg = "共新增" + t.ToString() + "行数据" });
        }

        // DELETE: api/API_BUS_RELATION/5
        /// <summary>
        /// 删除单条BUS_RELATION数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpDelete("{RELATIONID?}")]
        public async Task<IActionResult> Delete([FromForm] string RELATIONID)
        {
            if (!Permission.check(HttpContext, "OPERATE:GXSC"))
            {
                return Forbid();
            }
            BUS_RELATION bus_relation = await _context.BUS_RELATION.SingleOrDefaultAsync(m => m.RELATIONID == RELATIONID);
            if (bus_relation == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            if (StateHelper.getCodeByName("删除模式", "直接删除").Equals("1"))
            {
                _context.BUS_RELATION.Remove(bus_relation);
            }
            else
            {
                bus_relation.IS_DELETE = true;
            }
            Log.Write(_context, GetType(), "delete", "BUS_RELATION", "将靶点编号" + bus_relation.RELATIONID + "的数据的删除，操作者为" + Permission.getCurrentUser());
            await _context.SaveChangesAsync();

            return Ok(new { success = "true" });
        }
        // PUT: api/API_BUS_RELATION/5
        /// <summary>
        /// 更新单条BUS_RELATION数据
        ///</summary>
        /// <returns>执行结果反馈</returns>
        [HttpPost, Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(VM_BUS_RELATION postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:GXBJ"))
            {
                return Forbid();
            }
            try
            {
                BUS_RELATION entity = this._context.BUS_RELATION.Find(postData.RELATIONID);
                entity.MODIFY_DATE = DateTime.Now;
                entity.MUTATION_RATE = postData.MUTATION_RATE ?? "";
                entity.CHINESE_10000_RATE = postData.CHINESE_10000_RATE ?? "";
                entity.GERMLINE_10389_RATE = postData.GERMLINE_10389_RATE ?? "";
                entity.ALTERATION_RATE = postData.ALTERATION_RATE ?? "";
                entity.EVIDENCE_LEVEL = postData.EVIDENCE_LEVEL ?? "";
                entity.CLINICAL_TRIAL = postData.CLINICAL_TRIAL ?? "";
                entity.APPROVED = postData.APPROVED ?? "";
                entity.APPROVAL_TIME = postData.APPROVAL_TIME ?? "";
                entity.INDICATIONS = postData.INDICATIONS ?? "";
                entity.DOSAGE = postData.DOSAGE ?? "";
                entity.SPECIFICITY = postData.SPECIFICITY ?? "";
                entity.FUNCTION_AND_CLINICAL_IMPLICATIONS = postData.FUNCTION_AND_CLINICAL_IMPLICATIONS ?? "";
                entity.THERAPY_INTERPRETATION = postData.THERAPY_INTERPRETATION ?? "";
                entity.REFERENCE_LINK = postData.REFERENCE_LINK ?? "";
                entity.IS_PUB = postData.IS_PUB;
                entity.OPERATOR = Permission.getCurrentUser();
                await this._context.SaveChangesAsync();
                Log.Write(GetType(), "update", "BUS_RELATION", "关联关系编号为" + postData.RELATIONID + "的数据进行更新，操作者为" + Permission.getCurrentUser());

                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!BUS_RELATIONExists(postData.RELATIONID))
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

        private bool BUS_RELATIONExists(string RELATIONID)
        {
            return _context.BUS_RELATION.Any(e => e.RELATIONID == RELATIONID);
        }
    }
}
