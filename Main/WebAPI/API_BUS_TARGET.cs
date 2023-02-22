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
using System.Text.RegularExpressions;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/target")]
    public class API_BUS_TARGET : Controller
    {
        private readonly drugdbContext _context;

        public API_BUS_TARGET(drugdbContext context)
        {
            _context = context; 
        }

        private static string AA_position(string protein_pos)
        {
            string real_pos = "";
            if (protein_pos != null && protein_pos.Length != 0)
            {
                if (protein_pos.IndexOf("?") >= 0)
                {
                    real_pos = Regex.Match(protein_pos, @"\d+").ToString();
                }
                else
                {
                    if (protein_pos.IndexOf("-") >= 0)
                    {
                        string[] pos_array = protein_pos.Split("/");
                        string[] pos_array_one = pos_array[0].Split("-");
                        if (int.TryParse(pos_array_one[0], out int i) && int.TryParse(pos_array_one[1], out int j))
                        {
                            real_pos = ((i + j) / 2).ToString();
                        }
                    }
                    else
                    {
                        real_pos = Regex.Match(protein_pos, @"\d+").ToString();
                    }
                }
            }
            else
            {
                real_pos = protein_pos;
            }
            return real_pos;
        }

        [HttpGet, Route("[action]")]
        public IActionResult GetMutationText([FromQuery]string target)
        {
            var db = MySQLDB.GetSimpleTFromQuery<VM_BUS_TARGET_MUTATION>(
                "SELECT TARGET,VARIANT_CLASSIFICATION,ALTERATION,PROTEIN_POSITION FROM bus_target b, bus_target_sample_relation a, bus_clinical_sample c where b.target=@Target and a.targetid=b.targetid and a.sample_id=c.sample_id",
                new MySqlParameter[] { new MySqlParameter("TARGET", target) });

            var q = from n in db
                    where n.TARGET.ToLower().Equals(target.ToLower())
                    select new
                    {
                        Target = n.TARGET,
                        Variant_Classification = n.VARIANT_CLASSIFICATION,
                        Protein_Change = n.ALTERATION,
                        AA_Position = AA_position(n.PROTEIN_POSITION)
                    };

            return Ok(new ResultList<VM_BUS_TARGET_MUTATION>
            {
                TotalCount = q.Count(),
                Results = q
            });
        }

        [HttpGet, Route("[action]")]
        public IActionResult GetGermlineText([FromQuery] string target)
        {
            var db = MySQLDB.GetSimpleTFromQuery<VM_FREQ_FROM_DATASET>(
                "SELECT TARGET,VARIANT_CLASSIFICATION,ALTERATION,PROTEIN_POSITION,ALTERATION_NUM from frequency_from_dataset WHERE study='Germline_10389' AND is_pub=TRUE AND target=@Target",
                new MySqlParameter[] { new MySqlParameter("TARGET", target) });

            var q = from n in db
                    where n.TARGET.ToLower().Equals(target.ToLower())
                    select new
                    {
                        Target = n.TARGET,
                        Variant_Classification = n.VARIANT_CLASSIFICATION,
                        Protein_Change = n.ALTERATION,
                        AA_Position = AA_position(n.PROTEIN_POSITION),
                        AlterNum = n.ALTERATION_NUM
                    };
            List<object> x = new List<object>();
            foreach(var i in q)
            {
                for(var j = 0; j < i.AlterNum; j++)
                {
                    x.Add(i);
                }
            }

            return Ok(new ResultList<VM_FREQ_FROM_DATASET>
            {
                TotalCount = x.Count(),
                Results = x
            });
        }


        [HttpGet, Route("[action]")]
        public IActionResult GetKeggPathway([FromQuery] string entrezid)
        {
           
                KEGG KG = new KEGG();
                var test = KG.get_hsa(entrezid);

                return Ok(new { code = 0,data = test });

        }


        [HttpGet, Route("[action]")]
        public object Target_sta()
        {
            var d_var_cla = MySQLDB.GetSimpleTFromQuery<VM_BUS_TARGET_MUTATION>("SELECT COUNT(*) as Count,variant_classification FROM bus_target GROUP BY variant_classification ORDER BY Count DESC");
            var d_target = MySQLDB.GetSimpleTFromQuery<VM_BUS_TARGET_MUTATION>("SELECT COUNT(*) as Count,target FROM bus_target GROUP BY target ORDER BY Count DESC");
            var d_fusion = MySQLDB.GetSimpleTFromQuery<VM_BUS_TARGET>("SELECT DISTINCT fusion FROM bus_fusion");
            var target_num = from n in d_target select new { value = n.Count, name = n.TARGET };
            var var_cla_num = from n in d_var_cla select new { value = n.Count, name = n.VARIANT_CLASSIFICATION };

            var data = new
            {   
                target = target_num.ToArray().Take(10),
                var_cla = var_cla_num.ToArray().Take(5),
                fusion = new { value = d_fusion.Count(), name = "Fusions" }
            };
            return data;

        }

        [HttpGet, Route("[action]")]
        public IActionResult AlterationShow([FromQuery] string target)
        {
            var d = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select DISTINCT ALTERATION,TARGETID,COSMIC,CLINVAR,DBSNP from bus_all where Target=@target AND target != '' AND alteration != ''", new MySqlParameter[] { new MySqlParameter("Target", target) });
            var all_data = from n in d
                           orderby n.ALTERATION_RATE descending
                           select new
                           {
                               id = n.TARGETID,
                               key = n.ALTERATION,
                               cosmic = n.COSMIC,
                               clinvar = n.CLINVAR,
                               dbsnp = n.DBSNP

                           };
            var data = all_data.ToArray();
            return Ok(new ResultList<Array> { Results = data });
        }

        [HttpGet, Route("[action]")]
        public IActionResult PopulationShow([FromQuery] string target)
        {
            var d = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("SELECT t.targetid,target,alteration,cosmic,clinvar,dbsnp,COUNT(*) as num  FROM bus_target_sample_relation s,(SELECT DISTINCT targetid,target,alteration,cosmic,clinvar,dbsnp FROM bus_target WHERE target=@target) t WHERE s.targetid=t.targetid AND t.target != '' AND t.alteration != '' GROUP BY targetid ORDER BY num DESC LIMIT 100;", new MySqlParameter[] { new MySqlParameter("Target", target) });
            var all_data = from n in d
                           select new
                           {
                               id = n.TARGETID,
                               key = n.ALTERATION,
                               cosmic = n.COSMIC,
                               clinvar = n.CLINVAR,
                               dbsnp = n.DBSNP

                           };
            var data = all_data.ToArray();
            return Ok(new ResultList<Array> { Results = data });
        }

        [HttpGet, Route("[action]")]
        public IActionResult GermlineShow([FromQuery] string target)
        {
            var d = MySQLDB.GetSimpleTFromQuery<VM_FREQ_ALL>("SELECT distinct target,alteration,cosmic,clinvar,dbsnp FROM freq_all WHERE study='Germline_10389'AND target=@target AND alteration!='' AND is_pub=TRUE", new MySqlParameter[] { new MySqlParameter("Target", target) });
            var all_data = from n in d
                           select new
                           {
                               key = n.ALTERATION,
                               cosmic = n.COSMIC,
                               clinvar = n.CLINVAR,
                               dbsnp = n.DBSNP

                           };
            var data = all_data.ToArray();
            return Ok(new ResultList<Array> { Results = data });
        }


        // GET: api/API_BUS_TARGET
        /// <summary>
        /// 获取BUS_TARGET数据列表
        ///</summary>
        /// <returns>api/API_BUS_TARGET视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult Get([FromQuery]int page = 1, int limit = 10, string searchfield = "TARGET", string searchword = "", string field = "TARGETCODE", string order = "DESC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchfield = string.IsNullOrEmpty(searchfield) ? "TARGET" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            var queryResult = _context.BUS_TARGET.Where(m => m.VERSION.Equals(culture))
           .Where(searchfield + ".Contains(@0) and IS_DELETE==false", searchword);
            var ret = queryResult
           .OrderBy(field + " " + order)//按条件排序
           .Skip((page - 1) * limit) //跳过前x项
           .Take(limit)//从当前位置开始取前x项
           .ToList();//将结果转为List类型
            return Ok(new ResultList<VM_BUS_TARGET>
            {
                TotalCount = queryResult.Count(),
                Results = Mapper.Map<List<BUS_TARGET>, List<VM_BUS_TARGET>>(ret)
            });
        }
        /// <summary>
        /// 获取BUS_DISEASE数据列表
        ///</summary>
        /// <returns>api/API_BUS_DISEASE视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult ListAll([FromQuery]int page = 1, int limit = 10, string searchword = "", string field = "DISEASECODE", string order = "DESC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select * from bus_all where Target=@Target", new MySqlParameter[] { new MySqlParameter("Target", searchword) }).Where(m => m.VERSION.Equals(culture)).AsQueryable();
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
        // GET: api/API_BUS_TARGET
        /// <summary>
        /// 获取BUS_TARGET数据列表
        ///</summary>
        /// <returns>api/API_BUS_DRUG视图模型</returns>
        [HttpGet, Route("[action]")]
        public IActionResult Search([FromQuery]int page = 1, int limit = 10, string searchword = "", string field = "TARGET", string order = "ASC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword.ToLower();
            string query = "select * from bus_all";
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>(query).Where(m => m.VERSION.Equals(culture)).Where(m => m.DATA_TYPE != "germline").AsQueryable()
           .Where(m => (m.TARGET != null && m.TARGET.ToLower().Contains(searchword)) || (m.ALTERATION != null && m.ALTERATION.ToLower().Contains(searchword)) || (m.GENE_ALIAS != null && m.GENE_ALIAS.ToLower().Contains(searchword)) || (m.ENTREZ_GENEID != null && m.ENTREZ_GENEID.ToLower().Contains(searchword)) || (m.ENSEMBL_ID != null && m.ENSEMBL_ID.ToLower().Contains(searchword)) || (m.HGNC_ID != null && m.HGNC_ID.ToLower().Contains(searchword)) || (m.SPECIFICITY != null && m.SPECIFICITY.ToLower().Contains(searchword)) || (m.EVIDENCE_LEVEL != null && m.EVIDENCE_LEVEL.ToLower().Contains(searchword))).Where(t => t.TARGET != " ");
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
        public IActionResult TargetSearch([FromQuery] int page = 1, int limit = 10, string searchword = "", string field = "TARGET", string order = "ASC")
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword.ToLower();
            string query = "select * from bus_all";
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>(query).Where(m => m.VERSION.Equals(culture))
                .AsQueryable()
                .Where(m => (m.TARGET != null && m.TARGET.ToLower().Contains(searchword)) ||
                            (m.ALTERATION != null && m.ALTERATION.ToLower().Contains(searchword)) ||
                            (m.GENE_ALIAS != null && m.GENE_ALIAS.ToLower().Contains(searchword)) ||
                            (m.ENTREZ_GENEID != null && m.ENTREZ_GENEID.ToLower().Contains(searchword)) ||
                            (m.ENSEMBL_ID != null && m.ENSEMBL_ID.ToLower().Contains(searchword)) ||
                            (m.HGNC_ID != null && m.HGNC_ID.ToLower().Contains(searchword)) ||
                            (m.SPECIFICITY != null && m.SPECIFICITY.ToLower().Contains(searchword)) ||
                            (m.EVIDENCE_LEVEL != null && m.EVIDENCE_LEVEL.ToLower().Contains(searchword)))
                .Where(t => t.TARGET != " ").Select(m => new
                {
                    targetid = m.TARGETID,
                    gene = m.TARGET,
                    alteration = m.ALTERATION,
                    resistance = m.RESISTANCE,
                    clinical_significance = m.CLINICAL_SIGNIFICANCE,
                    VIC_evidence = m.VIC_EVIDENCE,
                    cosmic = m.COSMIC,
                    clinvar = m.CLINVAR,
                    dbsnp = m.DBSNP,
                    variant_classification = m.VARIANT_CLASSIFICATION,
                    refseq_transcript = m.REFSEQ_TRANSCRIPT

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
        public async Task<IActionResult> Detail([FromQuery] string TARGETID)
        {
            BUS_TARGET queryResult = await _context.BUS_TARGET.SingleOrDefaultAsync(m => m.TARGETID == TARGETID);

            if (queryResult == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<BUS_TARGET, VM_BUS_TARGET>(queryResult));
        }

        // PUT: api/API_BUS_TARGET/5
        /// <summary>
        /// 更新单条BUS_TARGET数据
        ///</summary>
        /// <returns>执行结果反馈</returns>
        [HttpPost, Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(VM_BUS_TARGET postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:BDBJ"))
            {
                return Forbid();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                BUS_TARGET entity = Mapper.Map<VM_BUS_TARGET, BUS_TARGET>(postData);
                entity.RESISTANCE = postData.RESISTANCE ?? "";
                entity.CLINICAL_SIGNIFICANCE = postData.CLINICAL_SIGNIFICANCE ?? "";
                entity.COSMIC = postData.COSMIC ?? "";
                entity.CLINVAR = postData.CLINVAR ?? "";
                entity.DBSNP = postData.DBSNP ?? "";
                entity.OMIMID = postData.OMIMID ?? "";
                entity.PATHWAY_LINKS_KEGG = postData.PATHWAY_LINKS_KEGG ?? "";
                entity.VIC_EVIDENCE = postData.VIC_EVIDENCE ?? "";
                entity.VERSION = postData.VERSION;
                entity.IS_PUB = postData.IS_PUB;
                entity.MODIFY_DATE = DateTime.Now;
                entity.OPERATOR = Permission.getCurrentUser();
                _context.BUS_TARGET.Update(entity);
                await _context.SaveChangesAsync<VM_BUS_TARGET>();
                Log.Write(GetType(), "update", "BUS_TARGET", "将靶点编号为" + postData.TARGETCODE + "的数据进行更新，操作者为" + Permission.getCurrentUser());

                return Ok(new { success = "true" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!BUS_TARGETExists(postData.TARGETID))
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

        // POST: api/API_BUS_TARGET
        /// <summary>
        /// 新增单条BUS_TARGET数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BUS_TARGET postData)
        {
            if (!Permission.check(HttpContext, "OPERATE:BDXZ"))
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
            postData.TARGET = postData.TARGET ?? "";
            postData.ALTERATION = postData.ALTERATION ?? "";
            postData.ENTREZ_GENEID = postData.ENTREZ_GENEID ?? "";
            postData.ENSEMBL_ID = postData.ENSEMBL_ID ?? "";
            postData.HGNC_ID = postData.HGNC_ID ?? "";
            postData.GENE_ALIAS = postData.GENE_ALIAS ?? "";
            postData.REFSEQ_TRANSCRIPT = postData.REFSEQ_TRANSCRIPT ?? "";
            postData.CHROMOSOME = postData.CHROMOSOME ?? "";
            postData.POSITION = postData.POSITION ?? "";
            postData.STRAND = postData.STRAND ?? "";
            postData.VARIANT_CLASSIFICATION = postData.VARIANT_CLASSIFICATION ?? "";
            postData.VARIANT_TYPE = postData.VARIANT_TYPE ?? "";
            postData.PROTEIN_POSITION = postData.PROTEIN_POSITION ?? "";
            postData.SWISSPORT = postData.SWISSPORT ?? "";
            postData.PFAM = postData.PFAM ?? "";
            postData.PATHWAY_FIGURE = postData.PATHWAY_FIGURE ?? "";
            postData.RESISTANCE = postData.RESISTANCE ?? "";
            postData.CLINICAL_SIGNIFICANCE = postData.CLINICAL_SIGNIFICANCE ?? "";
            postData.VIC_EVIDENCE = postData.VIC_EVIDENCE ?? "";
            postData.COSMIC = postData.COSMIC ?? "";
            postData.CLINVAR = postData.CLINVAR ?? "";
            postData.DBSNP = postData.DBSNP ?? "";
            postData.OMIMID = postData.OMIMID ?? "";
            postData.PATHWAY_LINKS_KEGG = postData.PATHWAY_LINKS_KEGG ?? "";
            postData.VERSION = postData.VERSION;
            postData.IS_PUB = postData.IS_PUB;
            postData.CREATE_DATE = DateTime.Now;
            postData.MODIFY_DATE = DateTime.Now;
            postData.OPERATOR = Permission.getCurrentUser();
            postData.IS_DELETE = false;
            postData.PATHWAY_FIGURE = postData.PATHWAY_FIGURE ?? string.Empty;
            _context.BUS_TARGET.Add(postData);
            try
            {
                await _context.SaveChangesAsync();
                Log.Write(GetType(), "create", "BUS_TARGET", "创建靶点编号为" + postData.TARGETCODE + "，操作者为" + Permission.getCurrentUser());
                return Ok();
            }
            catch (DbUpdateException)
            {
                if (BUS_TARGETExists(postData.TARGETID))//此处请按照业务要求，将字段更改为需要判断非重复的字段，如用户名、客户编号等
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

        }

        // DELETE: api/API_BUS_TARGET/5
        /// <summary>
        /// 删除单条BUS_TARGET数据
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpDelete("{TARGETID?}")]
        public async Task<IActionResult> Delete([FromForm] string TARGETID)
        {
            if (!Permission.check(HttpContext, "OPERATE:BDSC"))
            {
                return Forbid();
            }
            BUS_TARGET bus_target = await _context.BUS_TARGET.SingleOrDefaultAsync(m => m.TARGETID == TARGETID);
            if (bus_target == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            if (StateHelper.getCodeByName("删除模式", "直接删除").Equals("1"))
            {
                _context.BUS_TARGET.Remove(bus_target);
            }
            else
            {
                bus_target.IS_DELETE = true;
            }
            Log.Write(_context, GetType(), "delete", "BUS_TARGET", "将靶点编号" + bus_target.TARGETCODE + "的数据的删除，操作者为" + Permission.getCurrentUser());
            await _context.SaveChangesAsync();

            return Ok(new { success = "true" });
        }

        private bool BUS_TARGETExists(string TARGETID)
        {
            return _context.BUS_TARGET.Any(e => e.TARGETID == TARGETID);
        }
    }
}
