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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Text.RegularExpressions;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/disease")]
    public class API_BUS_DISEASE : Controller
    {
        private readonly drugdbContext _context;

        public API_BUS_DISEASE(drugdbContext context)
        {
            _context = context;
        }

        // GET: api/API_BUS_DISEASE
        /// <summary>
        /// 获取BUS_DISEASE数据列表
        ///</summary>
        /// <returns>api/API_BUS_DISEASE视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult Get([FromQuery]int page = 1, int limit = 10, string searchfield = "DISEASE", string searchword = "", string field = "DISEASECODE", string order = "DESC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchfield = string.IsNullOrEmpty(searchfield) ? "DISEASE" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = _context.BUS_DISEASE.Where(m => m.VERSION.Equals(culture))
           .Where(searchfield + ".Contains(@0) and IS_DELETE==false", searchword);
            var ret = queryResult
           .OrderBy(field + " " + order)//按条件排序
           .Skip((page - 1) * limit) //跳过前x项
           .Take(limit)//从当前位置开始取前x项
           .ToList();//将结果转为List类型
            return Ok(new ResultList<VM_BUS_DISEASE>
            {
                TotalCount = queryResult.Count(),
                Results = Mapper.Map<List<BUS_DISEASE>, List<VM_BUS_DISEASE>>(ret)
            });
        }
        /// <summary>
        /// 获取BUS_DISEASE数据列表
        ///</summary>
        /// <returns>api/API_BUS_DISEASE视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult ListAll([FromQuery]int page = 1, int limit = 10, string DISEASEID = "", string field = "DISEASECODE", string order = "DESC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select * from bus_all where DISEASEID=@DISEASEID", new MySqlParameter[] { new MySqlParameter("DISEASEID", DISEASEID) }).Where(m => m.VERSION.Equals(culture)).AsQueryable();
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
        /// <summary>
        /// 获取BUS_DISEASE数据列表
        ///</summary>
        /// <returns>api/API_BUS_DISEASE视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult SearchAll([FromQuery]int page = 1, int limit = 10, string searchword = "", string field = "DISEASECODE", string order = "DESC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = this._context.BUS_DISEASE.Where(m => m.VERSION.Equals(culture) && m.IS_PUB && !m.IS_DELETE)
           .Where(m => m.DISEASE.Contains(searchword) || m.DISEASECODE.Equals(searchword) || m.DISEASE_ALIAS.Contains(searchword)).AsQueryable();
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
        /// <summary>
        /// 获取BUS_DISEASE数据列表
        ///</summary>
        /// <returns>api/API_BUS_DISEASE视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult Search([FromQuery]int page = 1, int limit = 10, string searchword = "", string field = "DISEASE", string order = "ASC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword.ToLower();
            string query = "select * from bus_all";
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>(query).Where(m => m.VERSION.Equals(culture)).Where(m => m.DATA_TYPE != "germline")
           .Where(m => (m.DISEASE != null && m.DISEASE.ToLower().Contains(searchword)) || (m.DISEASECODE != null && m.DISEASECODE.ToLower().Contains(searchword)) || (m.DISEASE_ALIAS != null && m.DISEASE_ALIAS.ToLower().Contains(searchword))).AsQueryable();
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

        [HttpGet,Route("[action]")]
        public IActionResult GetDiseaseName()
        {
            var DiseaseName = _context.BUS_DISEASE.OrderBy(n => n.DISEASE).Select(m => m.DISEASE);
            return Ok(DiseaseName);
        }

        [HttpGet, Route("[action]")]
        public IActionResult DiseaseSearch([FromQuery] int page = 1, int limit = 10, string searchword = "", string field = "DISEASE", string order = "ASC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword.ToLower();
            string query = "select * from bus_all";
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>(query).Where(m => m.VERSION.Equals(culture))
                .Where(m => (m.DISEASE != null && m.DISEASE.ToLower().Contains(searchword)) ||
                            (m.DISEASECODE != null && m.DISEASECODE.ToLower().Contains(searchword)) ||
                            (m.DISEASE_ALIAS != null && m.DISEASE_ALIAS.ToLower().Contains(searchword)))
                .AsQueryable().Select(m => new
                {
                    diseaseid = m.DISEASEID,
                    disese_alias = m.DISEASE_ALIAS,
                    classification = m.CLASSIFICATION,
                    NCI_code = m.NCI_CODE,
                    ONCOTREE_code = m.ONCOTREE_CODE,
                    NCI_Disease_Definition = m.NCI_DISEASE_DEFINITION
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


        [HttpGet, Route("Detail")]
        public async Task<IActionResult> Detail([FromQuery] string DISEASEID)
        {
            BUS_DISEASE queryResult = await _context.BUS_DISEASE.SingleOrDefaultAsync(m => m.DISEASEID == DISEASEID);

            if (queryResult == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<BUS_DISEASE, VM_BUS_DISEASE>(queryResult));
        }

        // PUT: api/API_BUS_DISEASE/5
        /// <summary>
        /// 更新单条BUS_DISEASE数据
        ///</summary>
        /// <returns>执行结果反馈</returns>
        [HttpPost, Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(VM_BUS_DISEASE postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:JBBJ"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                BUS_DISEASE entity = Mapper.Map<VM_BUS_DISEASE, BUS_DISEASE>(postData);
                entity.MODIFY_DATE = DateTime.Now;
                entity.DISEASE_PATHWAY = entity.DISEASE_PATHWAY ?? string.Empty;
                entity.PATHWAY_REFER = postData.PATHWAY_REFER ?? "";
                entity.DISEASE = postData.DISEASE ?? "";
                entity.DISEASE_ALIAS = postData.DISEASE_ALIAS ?? "";
                entity.CLASSIFICATION = postData.CLASSIFICATION ?? "";
                entity.NCI_CODE = postData.NCI_CODE ?? "";
                entity.ONCOTREE_CODE = postData.ONCOTREE_CODE ?? "";
                entity.DISEASE_PATH = postData.DISEASE_PATH ?? "";
                entity.NCI_DISEASE_DEFINITION = postData.NCI_DISEASE_DEFINITION ?? "";
                entity.NCCN_LINK = postData.NCCN_LINK ?? "";
                entity.IS_PUB = postData.IS_PUB;
                entity.VERSION = postData.VERSION;
                entity.OPERATOR = Permission.getCurrentUser();
                _context.BUS_DISEASE.Update(entity);
                await _context.SaveChangesAsync<VM_BUS_DISEASE>();
                Log.Write(GetType(), "update", "BUS_DISEASE", "将疾病编号为" + postData.DISEASECODE + "的数据进行更新，操作者为" + Permission.getCurrentUser());

                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!BUS_DISEASEExists(postData.DISEASEID))
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

        // POST: api/API_BUS_DISEASE
        /// <summary>
        /// 新增单条BUS_DISEASE数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BUS_DISEASE postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:JBXZ"))
            {
                return Forbid();
            }
            ModelState.Remove("CREATE_DATE");
            ModelState.Remove("MODIFY_DATE");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            postData.CREATE_DATE = DateTime.Now;
            postData.MODIFY_DATE = DateTime.Now;
            postData.DISEASE_PATHWAY = postData.DISEASE_PATHWAY ?? string.Empty;
            postData.PATHWAY_REFER = postData.PATHWAY_REFER ?? "";
            postData.DISEASE = postData.DISEASE ?? "";
            postData.DISEASE_ALIAS = postData.DISEASE_ALIAS ?? "";
            postData.CLASSIFICATION = postData.CLASSIFICATION ?? "";
            postData.NCI_CODE = postData.NCI_CODE ?? "";
            postData.ONCOTREE_CODE = postData.ONCOTREE_CODE ?? "";
            postData.DISEASE_PATH = postData.DISEASE_PATH ?? "";
            postData.NCI_DISEASE_DEFINITION = postData.NCI_DISEASE_DEFINITION ?? "";
            postData.NCCN_LINK = postData.NCCN_LINK ?? "";
            postData.IS_PUB = postData.IS_PUB;
            postData.VERSION = postData.VERSION;
            postData.OPERATOR = Permission.getCurrentUser();
            postData.IS_DELETE = false;
            _context.BUS_DISEASE.Add(postData);
            try
            {
                await _context.SaveChangesAsync();
                Log.Write(GetType(), "create", "BUS_DISEASE", "创建疾病编号为" + postData.DISEASECODE + "，操作者为" + Permission.getCurrentUser());
                return Ok();
            }
            catch (DbUpdateException)
            {
                if (BUS_DISEASEExists(postData.DISEASEID))//此处请按照业务要求，将字段更改为需要判断非重复的字段，如用户名、客户编号等
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

        }

        // DELETE: api/API_BUS_DISEASE/5
        /// <summary>
        /// 删除单条BUS_DISEASE数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpDelete("{DISEASEID?}")]
        public async Task<IActionResult> Delete([FromForm] string DISEASEID)
        {
            if (!Permission.check(HttpContext, "OPERATE:JBSC"))
            {
                return Forbid();
            }
            BUS_DISEASE bus_disease = await _context.BUS_DISEASE.SingleOrDefaultAsync(m => m.DISEASEID == DISEASEID);
            if (bus_disease == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            if (StateHelper.getCodeByName("删除模式", "直接删除").Equals("1"))
            {
                _context.BUS_DISEASE.Remove(bus_disease);
            }
            else
            {
                bus_disease.IS_DELETE = true;
            }
            Log.Write(_context, GetType(), "delete", "BUS_DISEASE", "将疾病编号" + bus_disease.DISEASECODE + "的数据的删除，操作者为" + Permission.getCurrentUser());
            await _context.SaveChangesAsync();

            return Ok(new { success = "true" });
        }
        /// <summary>
        /// 当月签到结果统计
        /// </summary>
        /// <returns>查询结果</returns>
        /// 
        [HttpGet, Route("[action]")]
        public object Disease_sta()
        {
            var d_all = MySQLDB.GetSimpleTFromQuery<VM_BUS_DISEASE>("select disease,classification from bus_disease");
            var d_class = from n in d_all
                          group n by new { n.CLASSIFICATION } into g
                          orderby g.Count() descending
                          select new { name = g.Key, children = g.Select(m => m.DISEASE) };

            List<object> y = new List<object>();

            foreach (var n in d_class.Take(30))
            {
                List<object> x = new List<object>();
                foreach (var m in n.children)
                {
                    var s = new { name = m };
                    x.Add(s);
                };
                var da = new
                {
                    name = n.name.CLASSIFICATION,
                    children = x
                };
                y.Add(da);

            }
            var data = new
            {
                name = "Classificaton",
                children = y
            };

            return data;
        }

        [HttpGet, Route("[action]")]
        public IActionResult childTable([FromQuery] string Disease, string Target, string Study, int page = 1, int limit = 10, string field = "ALTERATION", string order = "ASC")
        {
            var result = _context.FREQ_FROM_DATASET.Where(m => m.DISEASE.Equals(Disease) && m.TARGET.Equals(Target) && m.STUDY.Equals(Study));
            var ret = result
            .OrderBy(field + " " + order)//按条件排序
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
            return Ok(new ResultList<FREQ_FROM_DATASET>
            {
                TotalCount = result.Count(),
                Results = ret
            });
        }

        [HttpGet, Route("[action]")]
        public IActionResult FreqAll(string study = "Germline_10389", int page = 1, int limit = 10, string searchword = "", string field = "DISEASE", string order = "ASC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword.ToLower();
            string query = "select * from freq_relat_all";
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_FREQ_ALL>(query).Where(m => (m.DISEASE != null && m.DISEASE.ToLower().Contains(searchword)) || (m.CLASSIFICATION != null && m.CLASSIFICATION.ToLower().Contains(searchword)) || (m.TARGET != null && m.TARGET.ToLower().Contains(searchword)) || (m.ALTERATION != null && m.ALTERATION.ToLower().Contains(searchword)) || (m.DRUG_NAME != null && m.DRUG_NAME.ToLower().Contains(searchword)) || (m.CLINVAR_SIGNIFICANCE != null && m.CLINVAR_SIGNIFICANCE.ToLower().Contains(searchword)) || (m.EVIDENCE_LEVEL != null && m.EVIDENCE_LEVEL.ToLower().Contains(searchword))).AsQueryable();
            if (!User.Identity.IsAuthenticated)
            {
                queryResult = queryResult.Take(10);
            }

            var ret = queryResult
            .OrderBy(field + " " + order)//按条件排序
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型

            return Ok(new ResultList<FREQ_FROM_DATASET>
            {
                TotalCount = queryResult.Count(),
                Results = ret
            });
        }

        // 疾病详情页面第一个bar图数据
        [HttpGet, Route("[action]")]
        public IActionResult MutationShow([FromQuery] string Disease)
        {
            //var d = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select DISTINCT ALTERATION,MUTATION_RATE,TARGET,TARGETID from bus_all where DiseaseId=@DiseaseId", new MySqlParameter[] { new MySqlParameter("DiseaseId", diseaseid) });
            var sample_num = _context.BUS_CLINICAL_SAMPLE.Where(m => m.CANCER_TYPE_DETAILED.Equals(Disease)).Distinct().Count();
            var d = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>($"SELECT entrez_geneid,target,alteration,cosmic,dbsnp,clinvar,count(DISTINCT sample_id)/{sample_num}*100 AS Mutation_rate FROM bus_target as a,(SELECT a.targetid,a.sample_id FROM  bus_target_sample_relation a,bus_clinical_sample c WHERE c.cancer_type_detailed=@Disease and a.sample_id=c.sample_id) as d WHERE d.targetid=a.targetid AND target != '' AND alteration != '' GROUP BY d.targetid ORDER BY Mutation_rate DESC LIMIT 20", new MySqlParameter[] { new MySqlParameter("Disease", Disease) });
            var fusion = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>($"SELECT DISTINCT a.target,a.fusion AS alteration,COUNT(DISTINCT b.sample_id)/{sample_num}*100 AS Mutation_rate FROM bus_fusion a,bus_clinical_sample b WHERE b.cancer_type_detailed=@Disease AND b.sample_id=a.sample_id GROUP BY target,fusion ORDER BY Mutation_rate DESC LIMIT 20", new MySqlParameter[] { new MySqlParameter("Disease", Disease) });
            // var cna = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>($"SELECT DISTINCT a.target,a.cna_type AS alteration,COUNT(DISTINCT b.sample_id)/{sample_num}*100 AS Mutation_rate FROM bus_cna a,bus_clinical_sample b WHERE b.cancer_type_detailed=@Disease AND b.sample_id=a.sample_id GROUP BY target,cna_type ORDER BY Mutation_rate DESC LIMIT 20", new MySqlParameter[] { new MySqlParameter("Disease", Disease) });
            if (sample_num < 1)
            {
                sample_num = _context.BUS_CLINICAL_SAMPLE.Where(m => m.CANCER_TYPE.Equals(Disease)).Count();
                d = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>($"SELECT entrez_geneid,target,alteration,cosmic,dbsnp,clinvar,count(DISTINCT sample_id)/{sample_num}*100 AS Mutation_rate FROM bus_target as a,(SELECT a.targetid,a.sample_id FROM  bus_target_sample_relation a,bus_clinical_sample c WHERE c.cancer_type=@Disease and a.sample_id=c.sample_id) as d WHERE d.targetid=a.targetid AND target != '' AND alteration != '' GROUP BY d.targetid ORDER BY Mutation_rate DESC LIMIT 20", new MySqlParameter[] { new MySqlParameter("Disease", Disease) });
                fusion = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>($"SELECT DISTINCT a.target,a.fusion AS alteration,COUNT(DISTINCT b.sample_id)/{sample_num}*100 AS Mutation_rate FROM bus_fusion a,bus_clinical_sample b WHERE b.cancer_type=@Disease AND b.sample_id=a.sample_id GROUP BY target,fusion ORDER BY Mutation_rate DESC LIMIT 20", new MySqlParameter[] { new MySqlParameter("Disease", Disease) });
                // cna = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>($"SELECT DISTINCT a.target,a.cna_type AS alteration,COUNT(DISTINCT b.sample_id)/{sample_num}*100 AS Mutation_rate FROM bus_cna a,bus_clinical_sample b WHERE b.cancer_type=@Disease AND b.sample_id=a.sample_id GROUP BY target,cna_type ORDER BY Mutation_rate DESC LIMIT 20", new MySqlParameter[] { new MySqlParameter("Disease", Disease) });
            }
            var all_alteration = d.Concat(fusion);
            

            var all_data = from n in all_alteration
                           where !string.IsNullOrEmpty(n.ALTERATION) select new
                           {
                               key = n.ALTERATION,
                               target = n.TARGET,
                               cosmic = n.COSMIC,
                               clinvar = n.CLINVAR,
                               dbsnp = n.DBSNP,
                               value = Math.Round(double.Parse(n.MUTATION_RATE), 3)
                           };
            var data = all_data.AsQueryable().OrderByDescending(x => x.value).ThenByDescending(x => x.key).Take(10).ToArray();
            return Ok(new ResultList<Array> { Results = data });
        }

        // 疾病详情页面第二、三个bar图数据
        [HttpGet, Route("[action]")]
        public IActionResult OtherMutationShow([FromQuery] string Disease, string Study)
        {
            var d = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>($"SELECT DISTINCT f.freq_id,f.targetid,f.target,f.alteration,f.frequency*100 AS mutation_rate,t.cosmic,t.dbsnp,t.clinvar FROM frequency_from_dataset f LEFT JOIN bus_target t ON f.targetid=t.targetid WHERE f.disease=@Disease AND f.study=@Study AND f.target != '' AND f.alteration != '' ORDER BY mutation_rate DESC LIMIT 20", new MySqlParameter[] { new MySqlParameter("Disease", Disease), new MySqlParameter("Study", Study) });
            if (d.Count < 1)
            {
                return Ok(new ResultList<Array> { Results = "" });
            }

            var all_data = from n in d
                           where !string.IsNullOrEmpty(n.ALTERATION)
                           select new
                           {
                               key = n.ALTERATION,
                               target = n.TARGET,
                               cosmic = n.COSMIC,
                               clinvar = n.CLINVAR,
                               dbsnp = n.DBSNP,
                               value = Math.Round(double.Parse(n.MUTATION_RATE), 3) + "%"
                           };
            var data = all_data.Take(10).ToArray();
            return Ok(new ResultList<Array> { Results = data });
        }


        // cytoscape js 插件绘图
        [HttpGet, Route("[action]")]
        public IActionResult CyScape([FromQuery] string disease, string diseasecode)
        {
            var style = new string[] { "coexp", "coloc", "path", "pi", "predict", "spd" };
            Random rd = new Random();
            var db_each_gene = MySQLDB.GetSimpleTFromQuery<VM_BUS_CyScape>(
                "SELECT TARGET,d.TARGETID,ENTREZ_GENEID,SAMPLE_ID,count(DISTINCT sample_id) as GENECOUNT FROM bus_target as a,(SELECT DISTINCT a.targetid,a.sample_id,c.patient_id ,c.cancer_type_detailed FROM  bus_target_sample_relation a,bus_clinical_sample c WHERE c.cancer_type_detailed = @disease and a.sample_id=c.sample_id) as d WHERE d.targetid=a.targetid GROUP BY target ORDER BY GENECOUNT DESC",
                new MySqlParameter[] { new MySqlParameter("DISEASE", disease) });

            if (db_each_gene.Count() < 1)
            {
                db_each_gene = MySQLDB.GetSimpleTFromQuery<VM_BUS_CyScape>(
                "SELECT TARGET,d.TARGETID,ENTREZ_GENEID,SAMPLE_ID,count(DISTINCT sample_id) as GENECOUNT FROM bus_target as a,(SELECT DISTINCT a.targetid,a.sample_id,c.patient_id ,c.cancer_type FROM  bus_target_sample_relation a,bus_clinical_sample c WHERE c.cancer_type=@disease and a.sample_id=c.sample_id) as d WHERE d.targetid=a.targetid GROUP BY target ORDER BY GENECOUNT DESC",
                new MySqlParameter[] { new MySqlParameter("DISEASE", disease) });
            };

            var db_total = MySQLDB.GetSimpleTFromQuery<VM_BUS_CLINICAL_SAMPLE>(
                "SELECT * FROM bus_clinical_sample as d WHERE d.cancer_type_detailed=@disease",
                new MySqlParameter[] { new MySqlParameter("DISEASE", disease) });

            if (db_total.Count() < 1)
            {
                db_total = MySQLDB.GetSimpleTFromQuery<VM_BUS_CLINICAL_SAMPLE>(
                "SELECT * FROM bus_clinical_sample as d WHERE d.cancer_type=@disease",
                new MySqlParameter[] { new MySqlParameter("DISEASE", disease) });
            }

            var db_drug = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>(
                "SELECT DISTINCT relationid,drug_name,target,drugcode,drugid,disease FROM bus_all WHERE disease=@disease AND target!='' GROUP BY drugcode",
                new MySqlParameter[] { new MySqlParameter("DISEASE", disease) });
            
            var db_target = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>(
                "SELECT DISTINCT targetid,target,targetcode,entrez_geneid,disease FROM bus_all WHERE disease=@disease AND target!=''",
                new MySqlParameter[] { new MySqlParameter("DISEASE", disease) });

            var db_drug_target = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>(
                "SELECT DISTINCT target,entrez_geneid,drug_name,drugcode,drugid,disease FROM bus_all WHERE disease=@disease AND target!=''",
                new MySqlParameter[] { new MySqlParameter("DISEASE", disease) });

            //有药物的基因(去重)
            var gene_nd_g = from n in db_target
                            group n by new { n.TARGET, n.ENTREZ_GENEID, n.TARGETID } into g
                            where g.Key.TARGET != " "
                            select new
                            {
                                data = new
                                {
                                    id = g.Key.ENTREZ_GENEID,
                                    //idInt = int.Parse(n.ENTREZ_GENEID),
                                    name = g.Key.TARGET,
                                    score = (float)db_each_gene.Where(m => m.TARGET.ToString().Equals(g.Key.TARGET.ToString())).Select(m => m.GENECOUNT).FirstOrDefault() / db_total.Count(),
                                    importance = 2,
                                    onlyID = g.Key.TARGETID,
                                    disease = false,
                                    gene = true,
                                    drug = false
                                },
                                @group = "nodes",
                                selected = false
                            };


            //基因节点
            var gene_nd = from n in db_each_gene
                          where !Regex.IsMatch(JsonConvert.SerializeObject(gene_nd_g), string.Format(@"\b{0}\b", n.TARGET.ToString())) && n.TARGET.ToString() != " "
                          select new
                          {
                              data = new
                              {
                                  id = n.ENTREZ_GENEID,
                                  //idInt = int.Parse(n.ENTREZ_GENEID),
                                  name = n.TARGET,
                                  score = (float)n.GENECOUNT / db_total.Count(),
                                  importance = 2,
                                  disease = false,
                                  gene = true,
                                  drug = false

                              },
                              @group = "nodes",
                              selected = false
                          };

            //疾病节点
            var disease_nd = new
            {
                data = new
                {
                    id = (string)diseasecode,
                    //idInt = int.Parse(n.ENTREZ_GENEID),
                    name = disease,
                    score = 1,
                    importance = 1,
                    disease = true,
                    gene = false,
                    drug = false
                },
                group = "nodes",
                selected = false
            };
            //药物节点
            var drug_nd = from n in db_drug
                          where n.TARGET != " "
                          select new
                          {
                              data = new
                              {
                                  id = n.DRUGCODE,
                                  //idInt = int.Parse(n.ENTREZ_GENEID),
                                  name = n.DRUG_NAME,
                                  score = (float)db_each_gene.Where(m => m.TARGET.ToString().Equals(n.TARGET.ToString())).Select(m => m.GENECOUNT).FirstOrDefault() / db_total.Count(),
                                  importance = 3,
                                  onlyID = n.DRUGID,
                                  relationID = n.RELATIONID,
                                  disease = false,
                                  gene = false,
                                  drug = true
                              },
                              @group = "nodes",
                              selected = false
                          };



            //疾病-基因边
            var disease_gene = from n in db_each_gene
                               where !Regex.IsMatch(JsonConvert.SerializeObject(gene_nd_g), string.Format(@"\b{0}\b", n.TARGET.ToString())) && n.TARGET.ToString() != " "
                               select new
                               {
                                   data = new
                                   {
                                       source = diseasecode,
                                       target = n.ENTREZ_GENEID,
                                       weight = (float)n.GENECOUNT / db_total.Count(),
                                       @group = "user",
                                       dis_gene = true,
                                       gene_drug = false
                                   },
                                   @group = "edges"
                               };

            //有药物的基因(去重)—疾病-基因边
            var disease_gene_g = from n in db_target
                                 group n by new { n.TARGET, n.ENTREZ_GENEID } into g
                                 where g.Key.TARGET != " "
                                 select new
                                 {
                                     data = new
                                     {
                                         source = diseasecode,
                                         target = g.Key.ENTREZ_GENEID,
                                         weight = 0.1,
                                         @group = "user",
                                         dis_gene = true,
                                         gene_drug = false
                                     },
                                     @group = "edges"
                                 };


            //基因-药物边
            var gene_drug = from n in db_drug_target
                            where n.DISEASE.ToLower().Equals(disease.ToLower()) && n.TARGET.ToString() != " "
                            select new
                            {
                                data = new
                                {
                                    source = n.ENTREZ_GENEID,
                                    target = n.DRUGCODE,
                                    weight = (float)0.08,
                                    //group = ?  组别，用来设置边的样式
                                    @group = "coloc",
                                    dis_gene = false,
                                    gene_drug = true
                                },
                                @group = "edges"
                            };

            //
            List<Object> x = new List<Object>();
            List<Array> alldata = new List<Array> { gene_nd.Take(15).ToArray(), gene_nd_g.ToArray(), drug_nd.ToArray(), disease_gene.Take(15).ToArray(), disease_gene_g.ToArray(), gene_drug.ToArray() };
            foreach (var i in alldata)
            {
                foreach (var j in i)
                {
                    x.Add(j);
                }
            }
            x.Add(disease_nd);

            return Ok(new ResultList<VM_BUS_CyScape> { Results = x });
        }

        // 疾病表格页面突变数据
        [HttpGet, Route("action")]
        public object DisMutTab([FromQuery] string Relationid, string Study)
        {
            var relation_data = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("SELECT disease,target,alteration FROM bus_all WHERE relationid=@Relationid", new MySqlParameter[] { new MySqlParameter("Relationid", Relationid) }).FirstOrDefault();
            var freqs_data = _context.FREQ_FROM_DATASET.Where(m => m.DISEASE.Equals(relation_data.DISEASE)&&m.TARGET.Equals(relation_data.TARGET)&&m.ALTERATION.Equals(relation_data.ALTERATION)&&m.STUDY.Equals(Study));
            var freq = freqs_data.Take(10).ToArray();
            return freq;

        }

        //获取与该疾病相关的所有突变相关信息
        [HttpGet, Route("[action]")]
        public IActionResult DisMutGet([FromQuery] string diseaseid, int limit=10)
        {
            var disease = MySQLDB.GetSimpleTFromQuery<VM_BUS_DISEASE>("SELECT * FROM bus_disease WHERE diseaseid=@diseaseid", new MySqlParameter[] { new MySqlParameter("diseaseid", diseaseid) }).Select(m => m.DISEASE).FirstOrDefault();
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("SELECT DISTINCT target,alteration,clinical_significance,VIC_evidence,cosmic,dbsnp,clinvar,refseq_transcript FROM bus_target as d,(SELECT DISTINCT b.sample_id,b.targetid FROM bus_clinical_sample a,bus_target_sample_relation b WHERE a.cancer_type_detailed=@disease AND b.sample_id=a.sample_id) as c WHERE d.targetid=c.targetid", new MySqlParameter[] { new MySqlParameter("disease", disease) }).AsQueryable();
            if (!User.Identity.IsAuthenticated)
            {
                queryResult = queryResult.Take(10);
            }
            var ret = queryResult.Select(m =>new
                {
                    target = m.TARGET,
                    alteration = m.ALTERATION,
                    clinical_significance = m.CLINICAL_SIGNIFICANCE,
                    VIC_evidence = m.VIC_EVIDENCE,
                    clinvar = m.CLINVAR,
                    dbsnp = m.DBSNP,
                    cosmic = m.COSMIC,
                    refseq_transcript = m.REFSEQ_TRANSCRIPT
            })
                .Take(limit)
                .ToList();//将结果转为List类型
            return Ok(new ResultList<VM_BUS_ALL>
            {
                TotalCount = queryResult.Count(),
                Results = ret
            });

        }

        // 绘制疾病详情页面的第一个bar图（GENIE数据）
        [HttpGet, Route("[action]")]
        public object DiseaseMutation([FromQuery]string Disease)
        {
            var sample_num = _context.BUS_CLINICAL_SAMPLE.Where(m => m.CANCER_TYPE_DETAILED.Equals(Disease)).Distinct().Count();
            var d = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>($"SELECT entrez_geneid,target,alteration,cosmic,dbsnp,clinvar,count(DISTINCT sample_id)/{sample_num}*100 AS Mutation_rate FROM bus_target as a,(SELECT a.targetid,a.sample_id FROM  bus_target_sample_relation a,bus_clinical_sample c WHERE c.cancer_type_detailed=@Disease and a.sample_id=c.sample_id) as d WHERE d.targetid=a.targetid AND target != '' AND alteration != '' GROUP BY d.targetid ORDER BY Mutation_rate DESC LIMIT 20", new MySqlParameter[] { new MySqlParameter("Disease", Disease) });
            var fusion = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>($"SELECT DISTINCT a.target,a.fusion AS alteration,COUNT(DISTINCT b.sample_id)/{sample_num}*100 AS Mutation_rate FROM bus_fusion a,bus_clinical_sample b WHERE b.cancer_type_detailed=@Disease AND b.sample_id=a.sample_id GROUP BY target,fusion ORDER BY Mutation_rate DESC LIMIT 20", new MySqlParameter[] { new MySqlParameter("Disease", Disease) });
            // var cna = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>($"SELECT DISTINCT a.target,a.cna_type AS alteration,COUNT(DISTINCT b.sample_id)/{sample_num}*100 AS Mutation_rate FROM bus_cna a,bus_clinical_sample b WHERE b.cancer_type_detailed=@Disease AND b.sample_id=a.sample_id GROUP BY target,cna_type ORDER BY Mutation_rate DESC LIMIT 20", new MySqlParameter[] { new MySqlParameter("Disease", Disease) });
            if (sample_num < 1)
            {
                sample_num = _context.BUS_CLINICAL_SAMPLE.Where(m => m.CANCER_TYPE.Equals(Disease)).Count();
                d = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>($"SELECT entrez_geneid,target,alteration,cosmic,dbsnp,clinvar,count(DISTINCT sample_id)/{sample_num}*100 AS Mutation_rate FROM bus_target as a,(SELECT a.targetid,a.sample_id FROM  bus_target_sample_relation a,bus_clinical_sample c WHERE c.cancer_type=@Disease and a.sample_id=c.sample_id) as d WHERE d.targetid=a.targetid AND target != '' AND alteration != '' GROUP BY d.targetid ORDER BY Mutation_rate DESC LIMIT 20", new MySqlParameter[] { new MySqlParameter("Disease", Disease) });
                if (d.Count < 1)
                {
                    return "";
                }
                fusion = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>($"SELECT DISTINCT a.target,a.fusion AS alteration,COUNT(DISTINCT b.sample_id)/{sample_num}*100 AS Mutation_rate FROM bus_fusion a,bus_clinical_sample b WHERE b.cancer_type=@Disease AND b.sample_id=a.sample_id GROUP BY target,fusion ORDER BY Mutation_rate DESC LIMIT 20", new MySqlParameter[] { new MySqlParameter("Disease", Disease) });
                // cna = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>($"SELECT DISTINCT a.target,a.cna_type AS alteration,COUNT(DISTINCT b.sample_id)/{sample_num}*100 AS Mutation_rate FROM bus_cna a,bus_clinical_sample b WHERE b.cancer_type=@Disease AND b.sample_id=a.sample_id GROUP BY target,cna_type ORDER BY Mutation_rate DESC LIMIT 20", new MySqlParameter[] { new MySqlParameter("Disease", Disease) });
            }
            var all_alteration = d.Concat(fusion);
            var all_data = from n in all_alteration
                           where !string.IsNullOrEmpty(n.ALTERATION)
                           select new
                           {
                               key = n.ALTERATION,
                               target = n.TARGET,
                               cosmic = n.COSMIC,
                               clinvar = n.CLINVAR,
                               dbsnp = n.DBSNP,
                               value = Math.Round(double.Parse(n.MUTATION_RATE), 3)
                           };

            var data = all_data.AsQueryable().OrderByDescending(x => x.value).ThenByDescending(x => x.key).Take(10).ToArray();

            //var d = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select DISTINCT ALTERATION,MUTATION_RATE,TARGET from bus_all where DiseaseId=@DiseaseId", new MySqlParameter[] { new MySqlParameter("DiseaseId", DiseaseId) });
            var series = new List<object>();
            var colorALL = new string[] { "#EFE42A", "#64BD3D", "#EE9201", "#29AAE3", "#B74AE5", "#483D8B", "#0AAF9F", "#E89589", "#16A085", "#99ccff" };
            Random rd = new Random();
            var serie = new
            {
                name = "Carrier Rate (GENIE)",
                type = "bar", //柱状
                data = from n in data where !string.IsNullOrEmpty(n.key) select n.value,
                itemStyle = new
                {
                    normal = new
                    { //柱子颜色
                      //Random rd = new Random();
                      //return "#" + Convert.ToString(rd.Next() * (256 * 256 * 256 - 1),16);


                        color = colorALL.GetValue(rd.Next(0, 9))
                    }
                }
            };
            series.Add(serie);

            var Main = new
            {
                tooltip = new { },
                animation = false,
                legend = new
                {
                    data = new string[] { "Carrier Rate (GENIE)" },
                    textStyle = new { fontSize = 18,fontWeight = "bold"}
                },
                xAxis = new
                {
                    axisLabel = new
                    {
                        interval = 0,
                        rotate = 15
                    },
                    splitLine = new
                    {
                        show = false
                    },
                    splitArea = new
                    {
                        show = true
                    },
                    data = from n in data where !string.IsNullOrEmpty(n.key) select n.target + "/" + n.key
                },
                yAxis = new
                {
                    name = "Rate/%",
                    type = "value",
                    nameLocation = "end",
                    nameTextStyle = new
                    {
                        color = "#222",
                        fontStyle = "normal",
                        fontSize = 18
                    },
                    splitLine = new
                    {
                        show = false
                    },
                    splitArea = new
                    {
                        show = false
                    }
                },
                series
            };
            return Main;
        }

        // 绘制其他数据集的bar图（10000chines somatic和10389 germline）
        [HttpGet, Route("[action]")]
        public object Dataset_mutation([FromQuery] string Disease, string Study)
        {
            var d = MySQLDB.GetSimpleTFromQuery<FREQ_FROM_DATASET>("SELECT target,alteration,frequency*100 AS Frequency FROM frequency_from_dataset WHERE disease=@Disease and study=@Study AND target != '' AND alteration != '' ORDER BY Frequency DESC LIMIT 20", new MySqlParameter[] { new MySqlParameter("Disease", Disease), new MySqlParameter("Study", Study) });
            if (d.Count < 1)
            {
                return "";
            }

            else
            {

                var title = "Germline Carrier Rate";
                if (Study == "Somatic_10000Chinese")
                {
                    title = "Carrier Rate (Chinese)";
                }

                var series = new List<object>();
                var colorALL = new string[] { "#EFE42A", "#64BD3D", "#EE9201", "#29AAE3", "#B74AE5", "#483D8B", "#0AAF9F", "#E89589", "#16A085", "#99ccff" };
                Random rd = new Random();
                var serie = new
                {
                    name = title,

                    type = "bar", //柱状
                    data = (from n in d where !string.IsNullOrEmpty(n.ALTERATION) && !string.IsNullOrEmpty(n.TARGET) select n.FREQUENCY).Take(10).ToArray(),
                    itemStyle = new
                    {
                        normal = new
                        { //柱子颜色
                          //Random rd = new Random();
                          //return "#" + Convert.ToString(rd.Next() * (256 * 256 * 256 - 1),16);


                            color = colorALL.GetValue(rd.Next(0, 9))
                        }
                    }
                };
                series.Add(serie);

                var Main = new
                {
                    tooltip = new { },
                    animation = false,
                    legend = new
                    {
                        data = new string[] { title },
                        textStyle = new { fontSize = 18, fontWeight = "bold" }
                    },
                    xAxis = new
                    {
                        axisLabel = new
                        {
                            interval = 0,
                            rotate = 15
                        },
                        splitLine = new
                        {
                            show = false
                        },
                        splitArea = new
                        {
                            show = true
                        },
                        data = (from n in d where !string.IsNullOrEmpty(n.ALTERATION) && !string.IsNullOrEmpty(n.TARGET) select n.TARGET + "/" + n.ALTERATION).Take(10).ToArray()
                    },
                    yAxis = new
                    {
                        name = "Rate/%",
                        type = "value",
                        nameLocation = "end",
                        nameTextStyle = new
                        {
                            color = "#222",
                            fontStyle = "normal",
                            fontSize = 18
                        },
                        splitLine = new
                        {
                            show = false
                        },
                        splitArea = new
                        {
                            show = false
                        }
                    },
                    series
                };

                return Main;
            }
        }



        private bool BUS_DISEASEExists(string DISEASEID)
        {
            return _context.BUS_DISEASE.Any(e => e.DISEASEID == DISEASEID);
        }
    }
}
