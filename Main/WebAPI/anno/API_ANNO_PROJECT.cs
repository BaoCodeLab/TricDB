using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ICSharpCode.SharpZipLib.Zip;
using Main.Extensions;
using Main.platform;
using Main.Utils;
using Main.ViewModels;
using Main.ViewModels.Annotation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Model.Model;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.IO;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using TDSCoreLib;
using System.Web;
using MySql.Data.MySqlClient;
using System.Resources;
using System.Collections;
using System.Resources.NetStandard;
using Newtonsoft.Json.Linq;
using Json.Net;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net;
using System.Collections.Concurrent;
using System.Threading;
using MiniSoftware;

namespace Main.WebAPI.anno
{
    [Produces("application/json")]
    [Route("api/anno/project")]
    public class API_ANNO_PROJECT : Controller
    {
        private readonly drugdbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private IMemoryCache _memoryCache;
        private IHttpClientFactory _clientFactory;

        public API_ANNO_PROJECT(drugdbContext context, IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment, IHttpClientFactory clientFactory)
        {
            _context = context;
            _memoryCache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
            _clientFactory = clientFactory;
        }


        string user_name = Permission.getCurrentUser();
        //private static readonly HttpClient hc = new HttpClient();
        
     


        // 获取当前用户已建立的project
        [HttpGet,Route("GetProject")]
        public IActionResult GetProject([FromQuery]string user_id, int page = 1, int limit = 10)
        {
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_ANNO_PROJECT>("select * from anno_project where user_id=@user_id", new MySqlParameter[] { new MySqlParameter("user_id", user_id) }).AsQueryable();
            var ret = queryResult.OrderBy("ORDER DESC")
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();
            return Ok(new ResultList<VM_ANNO_PROJECT> {
                TotalCount = ret.Count(),
                Results = ret
            });
        }

        // 获取指定project所包含的sample、patient信息
        [HttpGet, Route("GetSample")]
        public IActionResult GetSample([FromQuery] string user_id)
        {
            var sampleResult = MySQLDB.GetSimpleTFromQuery<VM_ANNO_ALL>("SELECT s.*,p.patient_name,p.patient_diag,p.patient_stage,f.FILENAME,f.FILEURI,f.LX FROM anno_sample s left join anno_patient p on s.patient_id=p.patient_id left join pf_file f on s.anno_sampleid=f.WGID where user_id=@user_id", new MySqlParameter[] { new MySqlParameter("user_id", user_id) }).AsQueryable();
            var sampleAndFile = from n in sampleResult
                                group n by n.ANNO_SAMPLEID into g
                                select new
                                {
                                    ANNO_SAMPLEID = g.Select(m => m.ANNO_SAMPLEID).FirstOrDefault(),
                                    PATIENT_NAME = g.Select(m => m.PATIENT_NAME).FirstOrDefault(),
                                    PATIENT_DIAG = g.Select(m => m.PATIENT_DIAG).FirstOrDefault(),
                                    PATIENT_STAGE = g.Select(m => m.PATIENT_STAGE).FirstOrDefault(),
                                    SAMPLE_NAME = g.Select(m => m.SAMPLE_NAME).FirstOrDefault(),
                                    SAMPLE_SOURCE = g.Select(m => m.SAMPLE_SOURCE).FirstOrDefault(),
                                    SAMPLE_TYPE = g.Select(m => m.SAMPLE_TYPE).FirstOrDefault(),
                                    SAMPLE_POSI = g.Select(m => m.SAMPLE_POSI).FirstOrDefault(),
                                    SAMPLE_METHOD = g.Select(m => m.SAMPLE_METHOD).FirstOrDefault(),
                                    FILES = g.Select(m => m.LX)
                                };

            var ret = sampleAndFile.ToList();
            return Ok(new ResultList<VM_ANNO_ALL>
            {
                TotalCount = ret.Count(),
                Results = ret
            });

        }

        // 获取指定project所包含的sample、patient信息
        [HttpGet, Route("ViewSample")]
        public IActionResult ViewSample([FromQuery] string user_id, string project_id)
        {
            var sampleResult = MySQLDB.GetSimpleTFromQuery<VM_ANNO_ALL>("SELECT p.project_id,p.project_name,p.project_descrip,s.*,f.* FROM anno_prosamp ps LEFT JOIN anno_project p ON ps.project_id=p.project_id LEFT JOIN anno_sample s ON ps.anno_sampleid=s.anno_sampleid LEFT JOIN pf_file f ON ps.anno_sampleid=f.WGID WHERE ps.project_id=@project_id AND s.user_id=@user_id", new MySqlParameter[] { new MySqlParameter("user_id", user_id), new MySqlParameter("project_id", project_id) }).AsQueryable();
            var sampleAndFile = from n in sampleResult
                                group n by n.ANNO_SAMPLEID into g
                                select new
                                {
                                    ANNO_SAMPLEID = g.Select(m => m.ANNO_SAMPLEID).FirstOrDefault(),
                                    PATIENT_NAME = g.Select(m => m.PATIENT_NAME).FirstOrDefault(),
                                    PATIENT_DIAG = g.Select(m => m.PATIENT_DIAG).FirstOrDefault(),
                                    PATIENT_STAGE = g.Select(m => m.PATIENT_STAGE).FirstOrDefault(),
                                    SAMPLE_NAME = g.Select(m => m.SAMPLE_NAME).FirstOrDefault(),
                                    SAMPLE_SOURCE = g.Select(m => m.SAMPLE_SOURCE).FirstOrDefault(),
                                    SAMPLE_TYPE = g.Select(m => m.SAMPLE_TYPE).FirstOrDefault(),
                                    SAMPLE_POSI = g.Select(m => m.SAMPLE_POSI).FirstOrDefault(),
                                    SAMPLE_METHOD = g.Select(m => m.SAMPLE_METHOD).FirstOrDefault(),
                                    FILES = g.Select(m => m.LX)
                                };

            var ret = sampleAndFile.ToList();
            return Ok(new ResultList<VM_ANNO_ALL>
            {
                TotalCount = ret.Count(),
                Results = ret
            });

        }





        // 添加project，并为project添加sample
        [HttpPost, Route("ProSample")]
        public IActionResult ProSample([FromForm] VM_ANNO_PROJECT postData,string[] ANNO_SAMPLEID)
        {
            VM_ANNO_PROJECT projeData = new VM_ANNO_PROJECT();
            int max_order = _context.ANNO_PROJECT.Select(m => m.ORDER).Max();
            // 添加project基本属性
            projeData.PROJECT_NAME = postData.PROJECT_NAME;
            projeData.PROJECT_DESCRIP = postData.PROJECT_DESCRIP;
            projeData.PROJECT_ID = postData.PROJECT_ID;
            projeData.ORDER = max_order + 1;
            projeData.USER_ID = postData.USER_ID;
            projeData.CREATE_DATE = DateTime.Now;
            projeData.MODIFY_DATE = DateTime.Now;
            projeData.IS_PUB = true;
            projeData.IS_DELETE = false;
            projeData.OPERATOR = user_name;
            projeData.VERSION = "en-US";
            ANNO_PROJECT entity = Mapper.Map<VM_ANNO_PROJECT, ANNO_PROJECT>(projeData);
            _context.Add(entity);
            _context.SaveChanges<VM_ANNO_PROJECT>();
            Log.Write(this.GetType(), "project", "Update Project:" + projeData.PROJECT_ID + ", Operator is" + user_name + ", Add base information");

            // 添加project和sample关联表的关联关系
            foreach (var id in ANNO_SAMPLEID)
            {
                VM_ANNO_PROSAMP relaData = new VM_ANNO_PROSAMP();
                relaData.PROSAMP_ID = Guid.NewGuid().ToString();
                relaData.PROJECT_ID = postData.PROJECT_ID;
                relaData.ANNO_SAMPLEID = id;
                relaData.CREATE_DATE = DateTime.Now;
                relaData.MODIFY_DATE = DateTime.Now;
                relaData.IS_PUB = true;
                relaData.IS_DELETE = false;
                relaData.OPERATOR = user_name;
                relaData.VERSION = "en-US";
                ANNO_PROSAMP e = Mapper.Map<VM_ANNO_PROSAMP, ANNO_PROSAMP>(relaData);
                _context.ANNO_PROSAMP.Add(e);
                _context.SaveChanges<VM_ANNO_PROSAMP>();
                Log.Write(this.GetType(), "prosample", "New Project:" + projeData.PROJECT_ID + ", Operator is" + user_name);
            }

            
            return Ok(new { result = true, msg = "The project information has been saved, please check in the project information page." });


        }





        // 验证project是否存在
        private bool ANNO_PROJECTExists(string project_id)
        {
            return _context.ANNO_PROJECT.Any(e => e.PROJECT_ID == project_id);
        }



        /// <summary>
        /// 更新选中的PROJECT数据
        ///</summary>
        /// <returns>执行结果反馈</returns>
        [HttpPost, Route("Update")]
        public async Task<IActionResult> Update([FromForm] VM_ANNO_PROJECT postData, string[] ANNO_SAMPLEID)
        {

            try
            {
                // 更新project表关于name和description数据
                postData.PROJECT_ID = postData.PROJECT_ID;
                ANNO_PROJECT projeData = Mapper.Map<VM_ANNO_PROJECT, ANNO_PROJECT>(postData);
                // projeData.ORDER = projeData.ORDER;
                projeData.USER_ID = postData.USER_ID;
                projeData.IS_PUB = true;
                projeData.IS_DELETE = false;
                projeData.PROJECT_NAME = postData.PROJECT_NAME;
                projeData.PROJECT_DESCRIP = postData.PROJECT_DESCRIP;
                projeData.MODIFY_DATE = DateTime.Now;
                projeData.OPERATOR = user_name;
                
                _context.ANNO_PROJECT.Update(projeData);
                await _context.SaveChangesAsync<VM_ANNO_PROJECT>();
                // 写入日志
                Log.Write(this.GetType(), "project", "Update Project:" + projeData.PROJECT_ID + ", Operator is" + user_name + ", Edit base information");

                // 更新project和sample关联表的关联数据
                // project与sample是n:n的关系
                var old_prosample = _context.ANNO_PROSAMP.Where(n => n.PROJECT_ID == postData.PROJECT_ID);
                var old_sampleid = old_prosample.Select(m => m.ANNO_SAMPLEID).ToList();
                VM_ANNO_PROSAMP e = new VM_ANNO_PROSAMP();
                

                foreach (var id in old_sampleid) // 与之前project包含的sample比较，获取需要删除的sample
                {
                    if (ANNO_SAMPLEID.Contains(id))
                    {
                        continue;
                    }
                    else
                    {

                        //updateData.PROSAMP_ID = old_prosample.Where(m => m.PROJECT_ID == projeData.PROJECT_ID && m.ANNO_SAMPLEID == id).Select(n => n.PROSAMP_ID).FirstOrDefault().ToString();
                        e.PROSAMP_ID = old_prosample.Where(m => m.PROJECT_ID == projeData.PROJECT_ID && m.ANNO_SAMPLEID == id).Select(n => n.PROSAMP_ID).FirstOrDefault().ToString();
                        ANNO_PROSAMP updateData = Mapper.Map<VM_ANNO_PROSAMP, ANNO_PROSAMP>(e);
                        updateData.ANNO_SAMPLEID = id;
                        updateData.PROJECT_ID = projeData.PROJECT_ID;
                        updateData.IS_PUB = false;
                        updateData.IS_DELETE = true;
                        updateData.MODIFY_DATE = DateTime.Now;
                        updateData.OPERATOR = user_name;
                        updateData.VERSION = "en-US";
                        _context.ANNO_PROSAMP.Update(updateData);
                        await _context.SaveChangesAsync<VM_ANNO_PROSAMP>();
                        Log.Write(this.GetType(), "prosample", "Update prosample:" + projeData.PROJECT_ID + ", Operator is" + user_name + ", Remove Sample " + id);
                    }
                }

                foreach (var newid in ANNO_SAMPLEID)
                    {   // 与之前project包含的sample比较，获取需要添加的sample，并添加sample与projct的关联
                        // 若之前sample已经被选中但又被删除，则把该sample的is_delete改为false
                        if (old_sampleid.Contains(newid))
                        {
                            if(!string.IsNullOrEmpty(old_prosample.Select(m => m.ANNO_SAMPLEID == newid && m.IS_DELETE == true).ToString()))
                            {
                                //updateData.PROSAMP_ID = old_prosample.Where(m => m.PROJECT_ID == projeData.PROJECT_ID && m.ANNO_SAMPLEID == newid).Select(n => n.PROSAMP_ID).FirstOrDefault().ToString();
                                e.PROSAMP_ID = old_prosample.Where(m => m.PROJECT_ID == projeData.PROJECT_ID && m.ANNO_SAMPLEID == newid).Select(n => n.PROSAMP_ID).FirstOrDefault().ToString();
                                ANNO_PROSAMP changeData = Mapper.Map<VM_ANNO_PROSAMP, ANNO_PROSAMP>(e);
                                changeData.PROJECT_ID = postData.PROJECT_ID;
                                changeData.ANNO_SAMPLEID = newid;
                                changeData.CREATE_DATE = DateTime.Now;
                                changeData.MODIFY_DATE = DateTime.Now;
                                changeData.IS_PUB = true;
                                changeData.IS_DELETE = false;
                                changeData.OPERATOR = user_name;
                                changeData.VERSION = "en-US";
                                _context.ANNO_PROSAMP.Update(changeData);
                                await _context.SaveChangesAsync<VM_ANNO_PROSAMP>();
                                Log.Write(this.GetType(), "prosample", "Update prosample:" + projeData.PROJECT_ID + ", Operator is" + user_name + ", Update Sample Pub");

                            }

                        }
                        else
                        {
                            ANNO_PROSAMP newData = Mapper.Map<VM_ANNO_PROSAMP, ANNO_PROSAMP>(e);
                            newData.PROSAMP_ID = Guid.NewGuid().ToString();
                            newData.PROJECT_ID = postData.PROJECT_ID;
                            newData.ANNO_SAMPLEID = newid;
                            newData.CREATE_DATE = DateTime.Now;
                            newData.MODIFY_DATE = DateTime.Now;
                            newData.IS_PUB = true;
                            newData.IS_DELETE = false;
                            newData.OPERATOR = user_name;
                            newData.VERSION = "en-US";
                            _context.ANNO_PROSAMP.Add(newData);
                            await _context.SaveChangesAsync<VM_ANNO_PROSAMP>();
                            Log.Write(this.GetType(), "prosample", "Update prosample:" + projeData.PROJECT_ID + ", Operator is" + user_name + ", Add Sample " + newid);
                        }
                }

                
                return Ok(new { result = true, msg = "The project information has been saved, please check in the project information page." });

            }

            catch (DbUpdateConcurrencyException ex)
            {
                if (!ANNO_PROJECTExists(postData.PROJECT_ID))
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

        // 获取当前project已选择的project
        [HttpGet, Route("GetCheckSample")]
        public IActionResult GetCheckSample([FromQuery] string project_id)
        {
            var queryResult = MySQLDB.GetSimpleTFromQuery<VM_ANNO_PROSAMP>("select * from anno_prosamp where is_delete=false and project_id=@project_id", new MySqlParameter[] { new MySqlParameter("project_id", project_id) }).AsQueryable();
            var sampleID = from n in queryResult select n.ANNO_SAMPLEID;
            var ret = sampleID.ToArray();
            return Ok(new ResultList<VM_ANNO_PROSAMP>
            {
                TotalCount = ret.Count(),
                Results = ret
            });
        }


        // DELETE: api/API_ANNO_PROJECT/5
        /// <summary>
        /// 删除所选中的Project
        ///</summary>
        /// <returns>执行结果</returns>
        [HttpDelete("{PROJECT_ID?}")]
        public async Task<IActionResult> Delete([FromForm] string PROJECT_ID)
        {
            if (!Permission.check(HttpContext, "OPERATE:YWSC"))
            {
                return Forbid();
            }
            // 获取要删除的Project对应数据对象
            ANNO_PROJECT project = await _context.ANNO_PROJECT.SingleOrDefaultAsync(m => m.PROJECT_ID == PROJECT_ID);

            if (project == null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            // 选择假性删除
            project.IS_PUB = false;
            project.IS_DELETE = true;
            Log.Write(_context, GetType(), "delete", "ANNO_PROJECT", "Delete Project,ID is" + project.PROJECT_ID + ". Operator is" + Permission.getCurrentUser());
            await _context.SaveChangesAsync();


            var prosamp_data = _context.ANNO_PROSAMP.Where(m => m.PROJECT_ID == PROJECT_ID).Select(n => n.PROSAMP_ID).ToList();
            foreach (var prosampID in prosamp_data)
            {
                ANNO_PROSAMP prosamp = await _context.ANNO_PROSAMP.SingleOrDefaultAsync(m => m.PROSAMP_ID == prosampID);
                prosamp.IS_PUB = false;
                prosamp.IS_DELETE = true;
                await _context.SaveChangesAsync();

            }

            return Ok(new { success = "true" });
        }



        [HttpPost("Order")]
        public IActionResult Order(string PROJECT_ID, string type)
        {
            if (!Permission.check(HttpContext, "MENU:ITEM:CDGL"))
            {
                return Forbid();
            }
            // 获取当前选定行的数据
            ANNO_PROJECT PROJECT = _context.ANNO_PROJECT.SingleOrDefault("is_delete == false and project_id ==@0", PROJECT_ID);
            if (PROJECT == null)
            {
                return NotFound();
            }

            if (type == "up") {
                //判断order是否最大，获取上一级的order，与之对调
                int max_order = _context.ANNO_PROJECT.Select(m => m.ORDER).Max();
                if (PROJECT.ORDER == max_order)
                {
                    return Ok(new { msg = "Success" });
                }
                else
                {
                    var upOrder = PROJECT.ORDER + 1;
                    ANNO_PROJECT upPROJECT = _context.ANNO_PROJECT.SingleOrDefault("is_delete == false and order ==@0", upOrder);
                    upPROJECT.ORDER = PROJECT.ORDER;
                    PROJECT.ORDER = upOrder;
                }
            }

            if (type == "down")
            {
                //判断order是否最小，获取下一级的order，与之对调
                int min_order = _context.ANNO_PROJECT.Select(m => m.ORDER).Min();
                if (PROJECT.ORDER == min_order)
                {
                    return Ok(new { msg = "Success" });
                }
                else
                {
                    var downOrder = PROJECT.ORDER - 1;
                    ANNO_PROJECT downPROJECT = _context.ANNO_PROJECT.SingleOrDefault("is_delete == false and order ==@0", downOrder);
                    downPROJECT.ORDER = PROJECT.ORDER;
                    PROJECT.ORDER = downOrder;
                }
            }

             _context.SaveChanges();

            return Ok(new { msg = "Success" });

        }



        // 开始project的注释分析
        [HttpGet, Route("Start")]
        public IActionResult Start(string PROJECT_ID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                throw new Exception("Please Login");
            }

            // 获取该project下所有的sample
            var sampleResult = MySQLDB.GetSimpleTFromQuery<VM_ANNO_ALL>("SELECT rel.*,f.* FROM (SELECT p.project_id,p.project_name,p.project_descrip,s.*,t.patient_name,t.patient_gender,t.patient_age,t.patient_stage,t.patient_diag,t.prior_treat_hist,t.ethnicity,t.family_history FROM anno_prosamp ps LEFT JOIN anno_project p ON ps.project_id=p.project_id LEFT JOIN anno_sample s ON ps.anno_sampleid=s.anno_sampleid LEFT JOIN anno_patient t ON s.patient_id=t.patient_id WHERE ps.IS_DELETE=FALSE AND ps.project_id=@project_id) as rel, pf_file as f WHERE rel.anno_sampleid=f.WGID", new MySqlParameter[] { new MySqlParameter("project_id", PROJECT_ID) }).AsQueryable();

            // 分样本进行分别注释
            var sams = from n in sampleResult
                       group n by n.ANNO_SAMPLEID into g
                       select g;

            List<SampleAnnotate> allSampleInProject = new List<SampleAnnotate>();
            List<Dictionary<string, dynamic>> allDocData = new List<Dictionary<string, dynamic>>();

            // 创建用户文件夹
            var savePath = $"{user_name}/";
            var userPath = WebPath.FILE_ABSOLUTE + WebPath.FILE_REALTE + savePath;

            if (!Directory.Exists(userPath))//判断文件夹是否存在 
            {
                Directory.CreateDirectory(userPath);//不存在则创建文件夹 
            }

            

            // Pass the handler to httpclient(from you are calling api)
            var hc = _clientFactory.CreateClient("HttpClientWithSSLUntrusted");

            foreach (var eachGroup in sams)
            {
                // 以下处理的是project I下的[Sample J]的所有文件
                // [Sample J]

                var SAMPLE_ID = eachGroup.Key;

                // 该sample的patient基本信息
                var PATIENT_BASIC = from m in eachGroup
                                    select new VM_ANNO_PATIENT
                                    {
                                        PATIENT_ID = m.PATIENT_ID,
                                        PATIENT_NAME = m.PATIENT_NAME,
                                        PATIENT_GENDER = m.PATIENT_GENDER,
                                        PATIENT_AGE = m.PATIENT_AGE,
                                        PATIENT_DIAG = m.PATIENT_DIAG,
                                        PATIENT_STAGE = m.PATIENT_STAGE,
                                        PRIOR_TREAT_HIST = m.PRIOR_TREAT_HIST,
                                        ETHNICITY = m.ETHNICITY,
                                        FAMILY_HISTORY = m.FAMILY_HISTORY,
                                        OPERATOR = m.OPERATOR,
                                        IS_DELETE = m.IS_DELETE,
                                        IS_PUB = m.IS_PUB
                                    };

                // 该sample的patient基本信息
                var SAMPLE_BASIC = from n in eachGroup
                                   select new VM_ANNO_SAMPLE
                                   {
                                       ANNO_SAMPLEID = SAMPLE_ID,
                                       PROJECT_ID = n.PROJECT_ID,
                                       USER_ID = n.USER_ID,
                                       SAMPLE_NAME = n.SAMPLE_NAME,
                                       SAMPLE_SOURCE = n.SAMPLE_SOURCE,
                                       SAMPLE_TYPE = n.SAMPLE_TYPE,
                                       SAMPLE_POSI = n.SAMPLE_POSI,
                                       SAMPLE_METHOD = n.SAMPLE_METHOD,
                                       SAMPLE_DATE = n.SAMPLE_DATE,
                                       ACCESSION_DATE = n.ACCESSION_DATE,
                                       SAMPLE_DIAG = n.SAMPLE_DIAG,
                                       MUTATION_TYPE = n.MUTATION_TYPE,
                                       SEQUENCE_TYPE = n.SEQUENCE_TYPE,
                                       MSI = n.MSI,
                                       CAPTURE_SIZE = n.CAPTURE_SIZE,
                                       OPERATOR = n.OPERATOR,
                                       IS_DELETE = n.IS_DELETE,
                                       IS_PUB = n.IS_PUB

                                   };

                string MMRGene1 = ",MLH1,MSH2,MSH6,PMS2,MLH3,EPCAM,POLE,POLD1,";
                string MMRGene2 = ",MDM2,MDM4,B2M,";
                string MMRGene3 = ",CCND1,FGF3,FGF4,FGF19,";
                string MMRGene = MMRGene1 + MMRGene2 + MMRGene3;

                string HRRGene = ",BRCA1,BRCA2,PALB2,RAD51,HDAC2,POLB,CHEK2,FANCA,ATM,NBN,MLH3,MSH2,MRE11A,";

                List<ExtractVariants.needAnnotateData> MMR = new List<ExtractVariants.needAnnotateData>();
                List<ExtractVariants.needAnnotateData> HRR = new List<ExtractVariants.needAnnotateData>();
                List<ExtractVariants.needAnnotateData> allVariants = new List<ExtractVariants.needAnnotateData>();
                Dictionary<string, dynamic> DocDataDict = new Dictionary<string, dynamic>();


                Dictionary<string, dynamic> MMR_HRR_Dict = new Dictionary<string, dynamic>();
                Dictionary<string, dynamic> Summary_Dict = new Dictionary<string, dynamic>();
                Dictionary<string, dynamic> Picture_Dict = new Dictionary<string, dynamic>();

                SampleAnnotate sampleAnnotate = new SampleAnnotate();

                sampleAnnotate.SAMPLE_BASIC = SAMPLE_BASIC.FirstOrDefault();
                sampleAnnotate.PATIENT_BASIC = PATIENT_BASIC.FirstOrDefault();

                sampleAnnotate.SAMPLE_BASIC.MSI_Type = MSI_Status(sampleAnnotate.SAMPLE_BASIC.MSI);


                DocDataDict.Add("DISEASECODE", "000000");
                if (!string.IsNullOrEmpty(sampleAnnotate.SAMPLE_BASIC.SAMPLE_DIAG))
                {
                    var QueryDisease = _context.BUS_DISEASE.Where(m => m.DISEASE == sampleAnnotate.SAMPLE_BASIC.SAMPLE_DIAG).FirstOrDefault();
                    if (QueryDisease != null)
                    {
                        var DISEASECODE = QueryDisease.DISEASECODE.ToString();
                        DocDataDict["DISEASECODE"] = DISEASECODE;
                    }
                    

                }

                

                Summary_Dict.Add("MSI", "");
                Summary_Dict.Add("SNV_Count", 0);
                Summary_Dict.Add("CNV_Count", 0);
                Summary_Dict.Add("SV_Count", 0);
                Summary_Dict.Add("MMR_Count", 0);
                Summary_Dict.Add("HRR_Count", 0);
                Summary_Dict.Add("TMB", 0);
                Summary_Dict.Add("MS", "");
                Summary_Dict["MSI"] = sampleAnnotate.SAMPLE_BASIC.MSI_Type;

                Picture_Dict.Add("SBS96", "/api/pf/file/file/decc21d3-e64e-4531-9d70-1d55ea6cebcd");
                Picture_Dict.Add("DBS87", "/api/pf/file/file/decc21d3-e64e-4531-9d70-1d55ea6cebcd");
                Picture_Dict.Add("ID83", "/api/pf/file/file/decc21d3-e64e-4531-9d70-1d55ea6cebcd");
                Picture_Dict.Add("TMB_png", "/api/pf/file/file/decc21d3-e64e-4531-9d70-1d55ea6cebcd");

                Picture_Dict.Add("CytoNet", "/api/pf/file/file/decc21d3-e64e-4531-9d70-1d55ea6cebcd");
                Picture_Dict.Add("bar1", "/api/pf/file/file/decc21d3-e64e-4531-9d70-1d55ea6cebcd");
                Picture_Dict.Add("bar2", "/api/pf/file/file/decc21d3-e64e-4531-9d70-1d55ea6cebcd");
                Picture_Dict.Add("bar3", "/api/pf/file/file/decc21d3-e64e-4531-9d70-1d55ea6cebcd");


                List<ExtractVariants.matchCombine> ImTherapy = new List<ExtractVariants.matchCombine>();
                List<ExtractVariants.matchCombine> TarTherapy = new List<ExtractVariants.matchCombine>();
                List<ExtractVariants.matchCombine> ImmuneAnno = new List<ExtractVariants.matchCombine>();
                var ImmuneDrugs = _context.BUS_DRUG.Where(m => m.DRUG_TYPE.Contains("Immunotherapy")).Select(n => n.DRUG_NAME).ToString();
                string snvFileID = "";
                foreach (var eachRec in eachGroup)
                {
                    // 以下处理的是Sample J下的 File M

                    PF_FILE PF_FILE = _context.PF_FILE.SingleOrDefault(m => m.GID == eachRec.GID && m.IS_DELETE == false);
                    if (PF_FILE == null)
                    {
                        return NotFound(new { msg = "File Not Found" });
                    }

                    // 判断文件是否存在
                    string xdlj = PF_FILE.FILEURI;
                    string jdlj = WebPath.FILE_ABSOLUTE;//绝对路径
                    string filePath = jdlj + xdlj;
                    var stream = System.IO.File.OpenRead(filePath);
                    if (stream == null)
                    {
                        return NotFound(new { msg = "File Not Found" });
                    }
                    stream.Close();


                    var tmbFilePath = userPath + "maf/" + eachRec.FILENAME + "_" + eachRec.GID + "_" + "tmb.csv";
                    var ex = new ExtractVariants();
                    Summary_Dict["TMB"] = "";


                    // 设置：一个sample最多只能上传1个SNV,1个CNV,1个SV


                    if (eachRec.LX == "SNV")
                    {
                        snvFileID = eachRec.GID;
                        List<ExtractVariants.matchCombine> snvAnno = snvAnnoation(hc,eachRec.GID, SAMPLE_ID, eachRec.FILENAME, filePath, userPath, eachRec.TYPE, eachRec.LX, allVariants, sampleAnnotate.SAMPLE_BASIC.CAPTURE_SIZE);
                        if (!string.IsNullOrEmpty(sampleAnnotate.SAMPLE_BASIC.SAMPLE_DIAG) && sampleAnnotate.SAMPLE_BASIC.SAMPLE_DIAG != " ")
                        {
                            snvAnno = snvAnno.Where(m => m.Disease == sampleAnnotate.SAMPLE_BASIC.SAMPLE_DIAG).ToList();
                        }
                        
                        sampleAnnotate.snvAnno = snvAnno;
                        Summary_Dict["SNV_Count"] = snvAnno.Count;
                        if (snvAnno.Count >= 1)
                        {
                            ImmuneAnno.AddRange(snvAnno);
                        }

                        DocDataDict.Add("snvAnno",snvAnno);

                        var tmbDT = ex.ReadToTable(tmbFilePath, ',');
                        Summary_Dict["TMB"] = tmbDT.Rows[0][1];


                        string mafPath = userPath + "maf";
                        DirectoryInfo folder = new DirectoryInfo(mafPath);
                        foreach (FileInfo file in folder.GetFiles())
                        {
                            var MS_name = eachRec.FILENAME + "_" + eachRec.GID + "_";
                            if (file.FullName.Contains(MS_name + "SBS96.png"))
                            {
                                Summary_Dict["MS"] = Summary_Dict["MS"] + "SBS" + "/";
                                Picture_Dict["SBS96"] = "/api/anno/project/file/" + snvFileID + "/SBS96.png";
                                continue;
                            }
                            if (file.FullName.Contains(MS_name + "SBS96_catalogue.png"))
                            {
                                Summary_Dict["MS"] = Summary_Dict["MS"] + "SBS" + "/";
                                Picture_Dict["SBS96"] = "/api/anno/project/file/" + snvFileID + "/SBS96_catalogue.png";
                            }

                            if (file.FullName.Contains(MS_name + "DBS87"))
                            {
                                Summary_Dict["MS"] = Summary_Dict["MS"] + "DBS" + "/";
                                Picture_Dict["DBS87"] = "/api/anno/project/file/" + snvFileID + "/DBS87.png";
                            }
                            if (file.FullName.Contains(MS_name + "DBS87_catalogue.png"))
                            {
                                Summary_Dict["MS"] = Summary_Dict["MS"] + "DBS" + "/";
                                Picture_Dict["DBS87"] = "/api/anno/project/file/" + snvFileID + "/DBS87_catalogue.png";
                            }

                            if (file.FullName.Contains(MS_name + "ID83"))
                            {
                                Summary_Dict["MS"] = Summary_Dict["MS"] + "ID" + "/";
                                Picture_Dict["ID83"] = "/api/anno/project/file/" + snvFileID + "/ID83.png";
                            }
                            if (file.FullName.Contains(MS_name + "ID83_catalogue.png"))
                            {
                                Summary_Dict["MS"] = Summary_Dict["MS"] + "DBS" + "/";
                                Picture_Dict["ID83"] = "/api/anno/project/file/" + snvFileID + "/ID83_catalogue.png";
                            }
                            if (file.FullName.Contains(MS_name + "tmb.png"))
                            {
                                Picture_Dict["TMB_png"] = "/api/anno/project/file/" + snvFileID + "/tmb.png";
                            }
                        }
                    }

                    else if (eachRec.LX == "CNV")
                    {
                        List<ExtractVariants.matchCombine> cnvAnno = cnvAnnoation(hc, eachRec.GID, SAMPLE_ID, eachRec.FILENAME, filePath, userPath, eachRec.TYPE, eachRec.LX, allVariants);

                        if (!string.IsNullOrEmpty(sampleAnnotate.SAMPLE_BASIC.SAMPLE_DIAG) && sampleAnnotate.SAMPLE_BASIC.SAMPLE_DIAG != " ")
                        {
                            cnvAnno = cnvAnno.Where(m => m.Disease == sampleAnnotate.SAMPLE_BASIC.SAMPLE_DIAG).ToList();
                        }

                        sampleAnnotate.cnvAnno = cnvAnno;
                        Summary_Dict["CNV_Count"] = cnvAnno.Count;
                        if (cnvAnno.Count >= 1)
                        {
                            ImmuneAnno.AddRange(cnvAnno);
                        }
                        DocDataDict.Add("cnvAnno", cnvAnno);
                    }

                    else if (eachRec.LX == "SV")
                    {
                        List<ExtractVariants.matchCombine> svAnno = svAnnoation(hc, eachRec.GID, SAMPLE_ID, eachRec.FILENAME, filePath, userPath, eachRec.TYPE, eachRec.LX, allVariants);

                        if (!string.IsNullOrEmpty(sampleAnnotate.SAMPLE_BASIC.SAMPLE_DIAG) && sampleAnnotate.SAMPLE_BASIC.SAMPLE_DIAG != " ")
                        {
                            svAnno = svAnno.Where(m => m.Disease == sampleAnnotate.SAMPLE_BASIC.SAMPLE_DIAG).ToList();
                        }

                        sampleAnnotate.svAnno = svAnno;
                        Summary_Dict["SV_Count"] = svAnno.Count;
                        if (svAnno.Count >= 1)
                        {
                            ImmuneAnno.AddRange(svAnno);
                        }
                        DocDataDict.Add("svAnno", svAnno);
                    }

                    else
                    {
                        return Ok(new { StateCode = 5002, Message = "File has no type specified！" });
                    }

                }

                if (allVariants.Count > 0)
                {
                    foreach (var variant in allVariants.DistinctBy(m => new { m.geneA, m.geneB, m.variant }))
                    {
                        if (!string.IsNullOrEmpty(variant.geneA.ToString()) && MMRGene.Contains(variant.geneA.ToString()))
                        {
                            MMR.Add(variant);
                            MMR_HRR_Dict.Add(variant.geneA,"Yes");
                        }

                        if (!string.IsNullOrEmpty(variant.geneB.ToString()) && MMRGene.Contains(variant.geneB.ToString()))
                        {
                            MMR.Add(variant);
                            MMR_HRR_Dict.Add(variant.geneB, "Yes");
                        }

                        if (!string.IsNullOrEmpty(variant.geneA.ToString()) && HRRGene.Contains(variant.geneA.ToString()))
                        {
                            HRR.Add(variant);
                            MMR_HRR_Dict.Add(variant.geneA, "Yes");
                        }

                        if (!string.IsNullOrEmpty(variant.geneB.ToString()) && HRRGene.Contains(variant.geneB.ToString()))
                        {
                            HRR.Add(variant);
                            MMR_HRR_Dict.Add(variant.geneB, "Yes");
                        }

                    }
                }

                var MMR_HRR = MMRGene + HRRGene;
                foreach (var mmr in MMR_HRR.Split(','))
                {
                    if (MMR_HRR_Dict.Keys.Contains(mmr))
                    {
                        continue;
                    }else
                    {
                        MMR_HRR_Dict.Add(mmr,"No");
                    }
                }
                sampleAnnotate.MMR = MMR.DistinctBy(m => new { m.geneA, m.geneB }).ToList();
                sampleAnnotate.HRR = HRR.DistinctBy(m => new { m.geneA, m.geneB }).ToList();
                Summary_Dict["MMR_Count"] = sampleAnnotate.MMR.Count;
                Summary_Dict["HRR_Count"] = sampleAnnotate.HRR.Count;

                

                

                if (!string.IsNullOrEmpty(sampleAnnotate.SAMPLE_BASIC.MSI))
                {
                    List<ExtractVariants.matchCombine> msiAnno = msiAnnoation(sampleAnnotate.SAMPLE_BASIC.MSI);
                    sampleAnnotate.msiAnno = msiAnno;
                }



                foreach (var record in ImmuneAnno)
                {
                    if (record.Drug.Contains(ImmuneDrugs))
                    {
                        ImTherapy.Add(record);
                    }
                    else
                    {
                        TarTherapy.Add(record);
                    }
                }

                List<ExtractVariants.matchCombine> ImmuneAnnotation = ImmuneTherapy(MMR_HRR_Dict, Summary_Dict["TMB"], sampleAnnotate.SAMPLE_BASIC.MSI_Type, sampleAnnotate.SAMPLE_BASIC.SAMPLE_DIAG);

                sampleAnnotate.ImTherapy = ImmuneAnnotation;
                sampleAnnotate.TarTherapy = TarTherapy;
                sampleAnnotate.MMR_HRR_Dict = MMR_HRR_Dict;
                sampleAnnotate.Summary_Dict = Summary_Dict;
                sampleAnnotate.Picture_Dict = Picture_Dict;
                var SampleBasic = ObjectToMap(sampleAnnotate.SAMPLE_BASIC);
                var PatientBasic = ObjectToMap(sampleAnnotate.PATIENT_BASIC);
                AppendElement(SampleBasic, DocDataDict);
                AppendElement(PatientBasic, DocDataDict);
                AppendElement(sampleAnnotate.Summary_Dict, DocDataDict);
                AppendElement(sampleAnnotate.Picture_Dict, DocDataDict);
                AppendElement(sampleAnnotate.MMR_HRR_Dict, DocDataDict);
                DocDataDict.Add("TarTherapy", TarTherapy);
                DocDataDict.Add("ImTherapy", ImmuneAnnotation);




                allDocData.Add(DocDataDict);
                allSampleInProject.Add(sampleAnnotate);

                
                //GenerateReport(sampleAnnotate);



            }

            return Ok(allDocData);
        }







        public List<ExtractVariants.matchCombine> snvAnnoation(HttpClient hc, string FILE_ID,string SAMPLE_ID, string FILENAME, string FILEPATH, string USERPATH, string TYPE, string LX, List<ExtractVariants.needAnnotateData> allVariants,double CaptureSize)
        {

            if (LX != "SNV" || TYPE !=".vcf")
            {
                throw new Exception("File Type errors");
            }


            // 执行Annovar注释脚本
            var scriptPath = WebPath.FILE_ABSOLUTE + WebPath.SHELL_PATH; //脚本保存路径
            var shellPath = scriptPath + "shell.sh"; // Shell脚本保存路径
            var annovarSavePath = USERPATH + "annovar/";//  Annovar注释生成的文件保存的文件夹路径:/file/{user}/annovar/:
            var mafSavePath = USERPATH + "maf/";

            if (!Directory.Exists(annovarSavePath))//判断文件夹是否存在 
            {
                Directory.CreateDirectory(annovarSavePath);//不存在则创建文件夹 
            }

            if (!Directory.Exists(mafSavePath))//判断文件夹是否存在 
            {
                Directory.CreateDirectory(mafSavePath);//不存在则创建文件夹 
            }


            var annovarOutputName = FILENAME + "_" + FILE_ID; // Annovar注释生成的文件名为pf_file的file id

            // 从注释后文件提取突变信息
            ExtractVariants extractSNV = new ExtractVariants();

            // 若该File未被注释，则调用annovar注释，否则直接从user下的annovar注释文件夹取文件
            var fileStatus = _context.PF_FILE.Where(m => m.GID == FILE_ID).Select(n => n.BZ).FirstOrDefault().ToString();
            if (fileStatus.Contains("false"))
            {
                extractSNV.Execute(shellPath, FILEPATH, annovarSavePath, mafSavePath, annovarOutputName, scriptPath, CaptureSize); // 运行shell脚本执行annovar注释，/file/{user}/annovar/{fileName}
                PF_FILE pf_file = _context.PF_FILE.SingleOrDefault(m => m.GID == FILE_ID);
                pf_file.BZ = "true";
                Log.Write(_context, GetType(), "Annovar", "PF_FILE", "ANNOVAR FILE ID is" + pf_file.GID + ". Operator is" + Permission.getCurrentUser());
                _context.SaveChanges();
            }

            var snvData = extractSNV.GetSNV(annovarSavePath + annovarOutputName + ".hg19_multianno.txt", annovarSavePath + annovarOutputName + ".csv"); // 从注释文件提取SNV，返回 List<SNVInfo>
            var oncokbRefSeq = extractSNV.ONCOKBGene(hc).Result; // 对于每个project，只获取一次oncokb的geneList

            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            // 将提取的突变逐行返回给各数据库一一注释

            // 新建一个队列，储存输入数据，保证线程安全
            ConcurrentQueue<ExtractVariants.SNVInfo> quene = new ConcurrentQueue<ExtractVariants.SNVInfo>(snvData);
            // 储存线程
            Task<List<GetAnnotatorResponse.TaskCollect>>[] tasks = new Task<List<GetAnnotatorResponse.TaskCollect>>[10];
            // 对每一个线程开启一个任务
            for (int i = 0; i < 10; i++)
            {
                tasks[i] = Task.Factory.StartNew<List<GetAnnotatorResponse.TaskCollect>>(() =>
                {
                    List<GetAnnotatorResponse.TaskCollect> rList = new List<GetAnnotatorResponse.TaskCollect>();

                    while (quene.TryDequeue(out ExtractVariants.SNVInfo t))
                    {
                        ///operation
                        // 每一个SNV在各数据库注释的内容
                        snvAnnotator SNV_ANNO = new snvAnnotator();
                        GetAnnotatorResponse.TaskCollect SNVTaskCollect = new GetAnnotatorResponse.TaskCollect();
                        var TricDB_Matches = SNV_ANNO.TricDB_Match(t);

                        var civicResponse = SNV_ANNO.civicResponse(hc, t);

                        string RefSeq = "";
                        if (oncokbRefSeq.Keys.Contains(t.gene))
                        {
                            RefSeq = oncokbRefSeq[t.gene];
                        }

                        var oncokbResponse = SNV_ANNO.oncokbResponse(hc, t, RefSeq,allVariants);

                        SNVTaskCollect.TricDB_Matches = TricDB_Matches;
                        SNVTaskCollect.civicResponse = civicResponse;
                        SNVTaskCollect.oncokbResponse = oncokbResponse;
                        rList.Add(SNVTaskCollect);

                    }
                    return rList;
                });
            }

            Task.WaitAll(tasks); // 等待任务全部完成后，提取结果


            List<ExtractVariants.matchCombine> allSNV = new List<ExtractVariants.matchCombine>();
            for (int i = 0; i < 10; i++)
            {

                foreach (var snvTask in tasks[i].Result)
                {

                    var TricDB_Matches = snvTask.TricDB_Matches.Where(m => m.Evidence_Level == "1" || m.Evidence_Level == "2").DistinctBy(d => new { d.Drug, d.Gene, d.Annoation_Alteration, d.Disease, d.Resource, d.Observe_RefSeq }).ToList(); ;
                    GetAnnotatorResponse cnvanno = new GetAnnotatorResponse();
                    var CIVIC_Matches = cnvanno.CIVIC_Match(snvTask.civicResponse).Where(m => m.Evidence_Level == "A" || m.Evidence_Level == "B").DistinctBy(d => new { d.Drug, d.Gene, d.Annoation_Alteration, d.Disease, d.Resource, d.Observe_RefSeq }).ToList(); ;
                    var OncoKB_Matches = cnvanno.ONCOKB_Match(snvTask.oncokbResponse).Where(m => m.treatmentLevel == "LEVEL_1" || m.treatmentLevel == "LEVEL_2" || m.treatmentLevel == "LEVEL_R1" || m.treatmentLevel == "LEVEL_R2").DistinctBy(d => new { d.Drug, d.Gene, d.Annoation_Alteration, d.Disease, d.Resource, d.Observe_RefSeq }).ToList(); ;
                    List<ExtractVariants.matchCombine> CombineAnno = new List<ExtractVariants.matchCombine>();
                    CombineAnno = extractSNV.CombineAnno(TricDB_Matches, CIVIC_Matches, OncoKB_Matches).DistinctBy(d => new { d.Drug, d.Gene, d.Annoation_Alteration, d.Disease, d.Resource, d.Observe_RefSeq }).ToList();
                    foreach (var snvRecord in CombineAnno)
                    {
                        allSNV.Add(snvRecord);
                    }

                }

            }

            return allSNV;

        }


        public List<ExtractVariants.matchCombine> cnvAnnoation(HttpClient hc, string FILE_ID, string SAMPLE_ID, string FILENAME, string FILEPATH, string USERPATH, string TYPE, string LX, List<ExtractVariants.needAnnotateData> allVariants)
        {


            if (LX != "CNV")
            {
                throw new Exception("File Type errors");
            }

            ExtractVariants extractCNV = new ExtractVariants();
            var cnvData = extractCNV.GetCNV(FILEPATH);
            var oncokbRefSeq = extractCNV.ONCOKBGene(hc).Result; // 对于每个project，只获取一次oncokb的geneList
            

            ServicePointManager.DefaultConnectionLimit = int.MaxValue;

            // 将提取的突变逐行返回给各数据库一一注释

            // 新建一个队列，储存输入数据，保证线程安全
            ConcurrentQueue<ExtractVariants.CNVInfo> quene = new ConcurrentQueue<ExtractVariants.CNVInfo>(cnvData);
            // 储存线程
            Task<List<GetAnnotatorResponse.TaskCollect>>[] tasks = new Task<List<GetAnnotatorResponse.TaskCollect>>[10];
            // 对每一个线程开启一个任务
            for (int i = 0; i < 10; i++)
            {
                tasks[i] = Task.Factory.StartNew<List<GetAnnotatorResponse.TaskCollect>>(() =>
                {
                    List<GetAnnotatorResponse.TaskCollect> rList = new List<GetAnnotatorResponse.TaskCollect>();
                    
                    while (quene.TryDequeue(out ExtractVariants.CNVInfo t))
                    {
                        ///operation
                        // 每一个CNV在各数据库注释的内容
                        cnvAnnotator CNV_ANNO = new cnvAnnotator();
                        GetAnnotatorResponse.TaskCollect CNVTaskCollect = new GetAnnotatorResponse.TaskCollect();
                        var TricDB_Matches = CNV_ANNO.TricDB_Match(t);

                        var civicResponse = CNV_ANNO.civicResponse(hc, t);

                        string RefSeq = "";
                        if (oncokbRefSeq.Keys.Contains(t.GeneSymbol))
                        {
                            RefSeq = oncokbRefSeq[t.GeneSymbol];
                        }

                        var oncokbResponse = CNV_ANNO.oncokbResponse(hc, t, RefSeq,allVariants);

                        CNVTaskCollect.TricDB_Matches = TricDB_Matches;
                        CNVTaskCollect.civicResponse = civicResponse;
                        CNVTaskCollect.oncokbResponse = oncokbResponse;
                        rList.Add(CNVTaskCollect);
                        
                    }
                    return rList;
                });
            }

            Task.WaitAll(tasks); // 等待任务全部完成后，提取结果


            
            List<ExtractVariants.matchCombine> allCNV = new List<ExtractVariants.matchCombine>();
            for (int i = 0; i < 10; i ++)
            {

                foreach (var cnvTask in tasks[i].Result)
                {

                    var TricDB_Matches = cnvTask.TricDB_Matches;
                    GetAnnotatorResponse cnvanno = new GetAnnotatorResponse();
                    var CIVIC_Matches = cnvanno.CIVIC_Match(cnvTask.civicResponse);
                    var OncoKB_Matches = cnvanno.ONCOKB_Match(cnvTask.oncokbResponse);
                    List<ExtractVariants.matchCombine> CombineAnno = new List<ExtractVariants.matchCombine>();
                    CombineAnno = extractCNV.CombineAnno(TricDB_Matches, CIVIC_Matches, OncoKB_Matches).DistinctBy(d => new { d.Drug, d.Gene, d.Annoation_Alteration, d.Disease, d.Resource, d.Observe_RefSeq }).ToList();
                    foreach (var cnvRecord in CombineAnno)
                    {
                        allCNV.Add(cnvRecord);
                    }

                }

            }

            return allCNV;
        }



        public List<ExtractVariants.matchCombine> svAnnoation(HttpClient hc,string FILE_ID, string SAMPLE_ID, string FILENAME, string FILEPATH, string USERPATH, string TYPE, string LX, List<ExtractVariants.needAnnotateData> allVariants)
        {
            if (LX != "SV")
            {
                throw new Exception("File Type errors");
            }

            ExtractVariants extractSV = new ExtractVariants();
            var svData = extractSV.GetSV(FILEPATH);
            var oncokbRefSeq = extractSV.ONCOKBGene(hc).Result; // 对于每个project，只获取一次oncokb的geneList


            ServicePointManager.DefaultConnectionLimit = int.MaxValue;

            // 将提取的突变逐行返回给各数据库一一注释

            // 新建一个队列，储存输入数据，保证线程安全
            ConcurrentQueue<ExtractVariants.SVInfo> quene = new ConcurrentQueue<ExtractVariants.SVInfo>(svData);
            // 储存线程
            Task<List<GetAnnotatorResponse.TaskCollect>>[] tasks = new Task<List<GetAnnotatorResponse.TaskCollect>>[10];
            // 对每一个线程开启一个任务
            for (int i = 0; i < 10; i++)
            {
                tasks[i] = Task.Factory.StartNew<List<GetAnnotatorResponse.TaskCollect>>(() =>
                {
                    List<GetAnnotatorResponse.TaskCollect> rList = new List<GetAnnotatorResponse.TaskCollect>();

                    while (quene.TryDequeue(out ExtractVariants.SVInfo t))
                    {
                        ///operation
                        // 每一个CNV在各数据库注释的内容
                        svAnnotator SV_ANNO = new svAnnotator();
                        GetAnnotatorResponse.TaskCollect TaskCollect = new GetAnnotatorResponse.TaskCollect();
                        var TricDB_Matches = SV_ANNO.TricDB_Match(t);

                        var civicResponse = SV_ANNO.civicResponse(hc, t);

                        string RefSeq = "";
                        if (oncokbRefSeq.Keys.Contains(t.GeneA))
                        {
                            RefSeq = oncokbRefSeq[t.GeneA];
                        }

                        var oncokbResponse = SV_ANNO.oncokbResponse(hc, t, RefSeq, allVariants);

                        TaskCollect.TricDB_Matches = TricDB_Matches;
                        TaskCollect.civicResponse = civicResponse;
                        TaskCollect.oncokbResponse = oncokbResponse;
                        rList.Add(TaskCollect);

                    }
                    return rList;
                });
            }

            Task.WaitAll(tasks); // 等待任务全部完成后，提取结果

            List<ExtractVariants.matchCombine> allSV = new List<ExtractVariants.matchCombine>();
            for (int i = 0; i < 10; i++)
            {

                foreach (var svTask in tasks[i].Result)
                {

                    var TricDB_Matches = svTask.TricDB_Matches;
                    GetAnnotatorResponse svanno = new GetAnnotatorResponse();

                    var CIVIC_Matches = svanno.CIVIC_Match(svTask.civicResponse);
                    var OncoKB_Matches = svanno.ONCOKB_Match(svTask.oncokbResponse);
                    List<ExtractVariants.matchCombine> CombineAnno = new List<ExtractVariants.matchCombine>();
                    CombineAnno = extractSV.CombineAnno(TricDB_Matches, CIVIC_Matches, OncoKB_Matches).DistinctBy(d => new { d.Drug, d.Gene, d.Annoation_Alteration, d.Disease, d.Resource, d.Observe_RefSeq }).ToList();
                    foreach (var cnvRecord in CombineAnno)
                    {
                        allSV.Add(cnvRecord);
                    }

                }

            }


            return allSV;

        }

        [HttpGet,Route("msiAnnoation")]
        public List<ExtractVariants.matchCombine> msiAnnoation(string msiResult)
        {

            // Pass the handler to httpclient(from you are calling api)
            var hc = _clientFactory.CreateClient("HttpClientWithSSLUntrusted");
            ExtractVariants extractSV = new ExtractVariants();
            msiAnnotator msiAnno = new msiAnnotator();
            var TricDB_Matches = msiAnno.TricDB_Match(msiResult);
            List<ExtractVariants.civicMatch> CIVIC_Matches = new List<ExtractVariants.civicMatch>();
            var oncokbResponse = msiAnno.oncokbResponse(hc,msiResult);
            GetAnnotatorResponse msianno = new GetAnnotatorResponse();
            var OncoKB_Matches = msianno.ONCOKB_Match(oncokbResponse);
            List<ExtractVariants.matchCombine> CombineAnno = new List<ExtractVariants.matchCombine>();
            CombineAnno = extractSV.CombineAnno(TricDB_Matches, CIVIC_Matches, OncoKB_Matches).DistinctBy(d => new { d.Drug, d.Gene, d.Annoation_Alteration, d.Disease, d.Resource, d.Observe_RefSeq }).ToList();
            return CombineAnno;
        }


        public List<ExtractVariants.matchCombine> ImmuneTherapy(Dictionary<string, dynamic> MMR_HRR_Dict,string TMB,string MSI,string disease)
        {
            
            ExtractVariants extractTric = new ExtractVariants();
            
            List<ExtractVariants.TricdbMatch> MatchData = new List<ExtractVariants.TricdbMatch>();
            List<ExtractVariants.civicMatch> CIVIC_Matches = new List<ExtractVariants.civicMatch>();
            List<ExtractVariants.oncokbMatch> OncoKB_Matches = new List<ExtractVariants.oncokbMatch>();

            List<ExtractVariants.matchCombine> allTric = new List<ExtractVariants.matchCombine>();
            List<ExtractVariants.matchCombine> CombineAnno = new List<ExtractVariants.matchCombine>();

            var MMR_SEN = "MLH1,MSH2,MSH6,PMS2,MLH3,EPCAM,POLE,POLD1";
            var MMR = " ";
            var MSI_Status = " ";
            foreach (var key in MMR_HRR_Dict.Keys)
            {
                if (MMR_SEN.Contains(key) && MMR_HRR_Dict[key] == "Yes")
                {
                    MMR = "Yes";
                    break;
                }
            }


            if (string.IsNullOrEmpty(TMB))
            {
                TMB = "No";
            }
            else
            {
                if (float.Parse(TMB) > 10)
                {
                    TMB = "Yes";
                }
            }


            if (MSI.Contains("MSI"))
            {
                MSI_Status = "Yes";
            }



            if (MMR == "Yes" || TMB == "Yes" || MSI_Status == "Yes")
            {
                var wildcard = "CD274,PDCD1,CTLA4,MSI/MMR";
                var bus_all = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select * from bus_all where disease=@disease", new MySqlParameter[] { new MySqlParameter("disease", disease) }).AsQueryable();
                var matches = from n in bus_all
                              where wildcard.Contains(n.TARGET)
                              select new ExtractVariants.TricdbMatch
                              {
                                  Guid = n.RELATIONID,
                                  Gene = n.TARGET,
                                  Observe_Alteration = " ",
                                  Observe_RefSeq = " ",
                                  Observe_Exon = " ",
                                  Observe_CdsChange = " ",
                                  Annoation_Alteration = n.ALTERATION.Replace("p.", ""),
                                  Annoation_RefSeq = n.REFSEQ_TRANSCRIPT,
                                  //Transcript = n.REFSEQ_TRANSCRIPT,
                                  Variant_Type = n.VARIANT_CLASSIFICATION,
                                  Disease = n.DISEASE,
                                  Disease_Ncit = n.NCI_CODE,
                                  Disease_Type = n.DRUG_NAME,
                                  Drug = n.DRUG_NAME,
                                  Resource = "DB Matches",
                                  Evidence_Level = n.EVIDENCE_LEVEL,
                                  Clinical_Significance = n.CLINICAL_SIGNIFICANCE,
                                  Reference = n.EVIDENCE_LEVEL == "1" ? "FDA Label" : "NCCN Guideline",
                                  Clinicals = n.CLINICAL_TRIAL,
                                  Indication = n.INDICATIONS,
                                  Dosage = n.DOSAGE,
                                  Gene_Function = n.FUNCTION_AND_CLINICAL_IMPLICATIONS,
                                  Therapy = n.THERAPY_INTERPRETATION,
                                  Mutation_Rate = n.MUTATION_RATE,
                                  Mechanism = n.MECHANISM_OF_ACTION
                              };

                foreach (var tric in matches)
                {
                    MatchData.Add(tric);
                }


            }


            CombineAnno = extractTric.CombineAnno(MatchData, CIVIC_Matches, OncoKB_Matches).DistinctBy(d => new { d.Drug, d.Gene, d.Annoation_Alteration, d.Disease, d.Resource, d.Observe_RefSeq }).ToList();
            foreach (var tricRecord in CombineAnno)
            {
                allTric.Add(tricRecord);
            }
            return allTric.Take(4).ToList();
        }


        public string MSI_Status(string MSI)
        {
            var msiList = MSI.Split(':');
            if (msiList.Length <= 1)
            {
                return "No";
            }

            string msiStatus = " ";
            float score;

            if (msiList[0] == "MSIsensor")
            {
                score = float.Parse(msiList[1]);
                if (score > 3.5)
                {
                    msiStatus = "MSI-H";
                }
                else { msiStatus = "MSS"; }
            }

            if (msiList[0] == "mSINGS")
            {
                score = float.Parse(msiList[1]);
                if (score > 0.2)
                {
                    msiStatus = "MSI-H";
                }
                else { msiStatus = "MSS"; }
            }

            if (msiList[0] == "MANTIS")
            {
                score = float.Parse(msiList[1]);
                if (score > 0.4)
                {
                    msiStatus = "MSI-H";
                }
                else { msiStatus = "MSS"; }
            }

            if (msiList[0] == "Direct")
            {
                msiStatus = msiList[1];
            }

            if (msiStatus == "MSS")
            {
                return "No";
            }

            return msiStatus;
        }



        [HttpGet,Route("GetFile")]
        public object GetFile()
        {
            string current_path = System.IO.Directory.GetCurrentDirectory();
            var strFilePath = current_path + "/Views/ANNO_PROJECT/report2.docx";
            System.Text.Encoding encoding = System.Text.Encoding.Default;
            FileStream stream = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);
            return stream;
        }


        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="GID"></param>
        /// <returns></returns>
        [HttpGet("file/{GID?}/{imageNAME?}")]
        public IActionResult Download([FromRoute]string GID,string imageNAME)
        {
            PF_FILE PF_FILE = _context.PF_FILE.SingleOrDefault(m => m.GID == GID && m.IS_DELETE == false);
            if (PF_FILE == null)
            {
                return NotFound(new { msg = "SNV File Not Found" });
            }
            var USERNAME = Permission.getCurrentUser();
            var annovarOutputName = PF_FILE.FILENAME + "_" + PF_FILE.GID;

            string jdlj = WebPath.FILE_ABSOLUTE;//绝对路径
            var stream = System.IO.File.OpenRead(jdlj + "/files/" + USERNAME + "/maf/" + annovarOutputName + "_" + imageNAME);
            if (stream == null)
            {
                return NotFound(new { msg = "Picture File Not Found" });
            }
            return File(stream, Helper.GetContentType(imageNAME.Split('.')[1]), imageNAME);
        }

        public static void AppendElement(Dictionary<string,dynamic> d1, Dictionary<string, dynamic> d2)
        {
            foreach (var i in d1)
            {
                d2[i.Key.ToString()] = i.Value;
            }
        }



        /// <summary>
        /// 对象转换为字典
        /// </summary>
        /// <param name="obj">待转化的对象</param>
        /// <param name="isIgnoreNull">是否忽略NULL 这里我不需要转化NULL的值，正常使用可以不穿参数 默认全转换</param>
        /// <returns></returns>
        public static Dictionary<string, object> ObjectToMap(object obj, bool isIgnoreNull = false)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();

            Type t = obj.GetType(); // 获取对象对应的类， 对应的类型

            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance); // 获取当前type公共属性

            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetGetMethod();

                if (m != null && m.IsPublic)
                {
                    // 进行判NULL处理 
                    if (m.Invoke(obj, new object[] { }) != null || !isIgnoreNull)
                    {
                        map.Add(p.Name, m.Invoke(obj, new object[] { })); // 向字典添加元素
                    }
                }
            }
            return map;
        }

        [HttpGet,Route("GenerateReport")]
        public IActionResult GenerateReport(SampleAnnotate sampleData)
        {
            string current_path = System.IO.Directory.GetCurrentDirectory();
            var templatePath = current_path + "/Views/ANNO_PROJECT/reportTemplate.docx";
            var path = current_path + "/Views/ANNO_PROJECT/demo.docx";
            var value = ObjectToMap(sampleData);
            MemoryStream memoryStream = new MemoryStream();
            MiniWord.SaveAsByTemplate(path, templatePath, value["PATIENT_BASIC"]);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return Ok(value.ToString());
        }

        public System.Threading.Tasks.Task ExecuteResultAsync(Microsoft.AspNetCore.Mvc.ActionContext context)
        {
            throw new NotImplementedException();
        }



        public class SampleAnnotate
        {
            public VM_ANNO_SAMPLE SAMPLE_BASIC { get; set; }
            public VM_ANNO_PATIENT PATIENT_BASIC { get; set; }
            public Dictionary<string, dynamic> Summary_Dict { get; set; }
            public Dictionary<string, dynamic> Picture_Dict { get; set; }
            public Dictionary<string,dynamic> MMR_HRR_Dict { get; set; }
            public List<ExtractVariants.needAnnotateData> MMR { get; set; }
            public List<ExtractVariants.needAnnotateData> HRR { get; set; }
            public List<ExtractVariants.matchCombine> ImTherapy { get; set; }
            public List<ExtractVariants.matchCombine> TarTherapy { get; set; }
            public List<ExtractVariants.matchCombine> snvAnno { get; set; }
            public List<ExtractVariants.matchCombine> cnvAnno { get; set; }
            public List<ExtractVariants.matchCombine> svAnno { get; set; }
            public List<ExtractVariants.matchCombine> msiAnno { get; set; }
        }






    }

}
