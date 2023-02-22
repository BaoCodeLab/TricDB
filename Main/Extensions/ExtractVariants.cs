using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using CsvHelper;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Json.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Net;

namespace Main.Extensions
{
    public class ExtractVariants
    {

        public DataTable ReadToTable(string FilePath, char sep)
        {

            FileStream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            if (stream == null)
            {
                throw new NullReferenceException();
            }

            DataTable dt = new DataTable();
            System.Text.Encoding encoding = System.Text.Encoding.Default;
            StreamReader sr = new StreamReader(stream, encoding);
            // 每读取一行的记录
            string strLine = "";
            // 每行记录中各字段的内容
            string[] aryLine = null;
            string[] tableHead = null;
            // 标示列数
            int columnCount = 0;
            // 标示读取的是否是第一行
            bool IsFirst = true;

            // 开始逐行读取数据
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(sep);
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    // 创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(sep);
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);

                }

            }

            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }

            sr.Close();
            stream.Close();

            return dt;

        }


        public List<SNVInfo> GetSNV(string ANNOATION_FILE_PATH, string SAVE_CSV_PATH)
        {
            List<SNVInfo> SNVList = new List<SNVInfo>();

            var dt = ReadToTable(ANNOATION_FILE_PATH,'\t');

            foreach (DataRow dr in dt.Rows)
            {
                SNVInfo SNV = new SNVInfo();
                SNV.gene = dr["Gene.refGene"].ToString();
                //SNV.transcript = dt.Rows[num][""].ToString();
                SNV.chromosome = dr["Chr"].ToString();
                SNV.startPosition = dr["Start"].ToString();
                SNV.endPosition = dr["End"].ToString();
                //SNV.proteinChange = dt.Rows[num][""].ToString();
                SNV.refSeq = dr["Ref"].ToString();
                SNV.altSeq = dr["Alt"].ToString();
                SNV.AAChange = dr["AAChange.refGene"].ToString().Split(','); //数组 储存Annovar注释文件的AAChange字段
                SNVList.Add(SNV);
            }

            // 正式环境中以下部分需要去除
            var writer = new StreamWriter(SAVE_CSV_PATH);
            var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);
            csv.WriteRecords(SNVList);
            writer.Close();

            return SNVList;

        }

        public List<CNVInfo> GetCNV(string CNV_PATH)
        {

            var dt = ReadToTable(CNV_PATH,'\t');
            List<CNVInfo> CNVList = new List<CNVInfo>();

            foreach (DataRow row in dt.Rows)
            {
                CNVInfo cnvInfo = new CNVInfo();
                var col = dt.Columns;
                if (col.Contains("Locus ID") && col.Contains("Cytoband"))
                {
                    cnvInfo.GeneSymbol = row["Gene Symbol"].ToString();
                    //cnvInfo.Cytoband = "if" + col.ToString();
                    var value = row[col[3].ToString()].ToString();
                    if (int.TryParse(value, out int i))
                    {
                        cnvInfo.value = i;
                    } else
                    {
                        cnvInfo.value = 0;
                    }


                }
                else
                {
                    if (col.Count == 2)
                    {
                        cnvInfo.GeneSymbol = row["Gene Symbol"].ToString();
                        //cnvInfo.Cytoband = "else1" + col.ToString();
                        var value = row[col[1].ToString()].ToString();
                        if (int.TryParse(value, out int i))
                        {
                            cnvInfo.value = i;
                        }
                        else
                        {
                            cnvInfo.value = 0;
                        }
                    }

                    if (col.Contains("Locus ID") || col.Contains("Cytoband"))
                    {
                        cnvInfo.GeneSymbol = row["Gene Symbol"].ToString();
                        //cnvInfo.Cytoband = "else2" + col[3].ToString() + col[2].ToString() + col[1].ToString() + col[0].ToString();
                        var value = row[col[2].ToString()].ToString();
                        if (int.TryParse(value, out int i))
                        {
                            cnvInfo.value = i;
                        }
                        else
                        {
                            cnvInfo.value = 0;
                        }

                    }
                    
                }

                
                CNVList.Add(cnvInfo);


            }
            // 正式环境中以下部分需要去除
            var writer = new StreamWriter(@"D:\360CloudSync\同步文件夹\Project\DrugDB\Main\bin\Debug\netcoreapp2.1\files\system\annovar\cnv.csv");
            var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);
            csv.WriteRecords(CNVList);
            writer.Close();
            return CNVList;
        }


        public List<SVInfo> GetSV(string SV_PATH)
        { 
            var dt = ReadToTable(SV_PATH,'\t');
            List<SVInfo> SVList = new List<SVInfo>();

            foreach (DataRow row in dt.Rows)
            {
                
                if (dt.Columns.Contains("GeneA") && dt.Columns.Contains("GeneB") && dt.Columns.Contains("SV_Type"))
                {
                    SVInfo svInfo = new SVInfo();
                    svInfo.GeneA = row["GeneA"].ToString();
                    svInfo.GeneB = row["GeneB"].ToString();
                    svInfo.SvType = row["SV_Type"].ToString();
                    SVList.Add(svInfo);
                }else
                {
                    throw new Exception("Please specify the GeneA,GeneB and SV_Type");
                }
  
            }
            // 正式环境中以下部分需要去除
            var writer = new StreamWriter(@"D:\360CloudSync\同步文件夹\Project\DrugDB\Main\bin\Debug\netcoreapp2.1\files\system\annovar\sv.csv");
            var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);
            csv.WriteRecords(SVList);
            writer.Close();
            return SVList;

        }





            /// <summary>
            /// Shell脚本
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            
        public string Execute(string shellPath, string vcfFilePath, string annovarSavePath, string mafSavePath, string annovarOutputName,string scriptPath,double CaptureSize)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash", // 解释器路径
                    Arguments = $"\"{shellPath}\" -i \"{vcfFilePath}\" -o \"{annovarSavePath}\" -n \"{annovarOutputName}\" -s \"{scriptPath}\" -m  \"{mafSavePath}\" -c \"{CaptureSize}\" ", // 脚本文件路径和参数
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            if (process == null)

            {
                return "Can not run shell !";
            }

            else

            {
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit(); //等待进程关闭或退出后执行以下步骤
                process.Dispose();
                return result;
            }

        }


        // 递归解析传入的[CIVIC]多重嵌套json节点
        public List<CIVIC_DATA> CIVICNode(dynamic node, List<CIVIC_DATA> CIVIC_SAVE, ExtractVariants.needAnnotateData needAnnotateData, string Descri = "")
        {
            foreach (var i in node)
            {
                if (i.drugs != null) // 判断是否是第二层node,第二层node有drug
                {
                    CIVIC_DATA civicData = new CIVIC_DATA();
                    if (i.drugs.Count >= 1 && i.status == "ACCEPTED") // 没有drug和未接收的不要
                    {
                        civicData.queryGene = needAnnotateData.geneA + "-" + needAnnotateData.geneB;
                        civicData.queryVariant = needAnnotateData.variant;
                        civicData.queryTranscript = needAnnotateData.transcript;
                        civicData.queryExon = needAnnotateData.exon;
                        civicData.queryCdsChange = needAnnotateData.cds_change;

                        civicData.nodeDescri = Descri;

                        Func<JArray, string> drugName = x => string.Join(" + ", x.ToObject<List<drugs>>().Where(m => !string.IsNullOrEmpty(m.name)).Select(n => n.name));
                        civicData.drug_name = drugName(i.drugs);

                        Func<JArray, string> drugId = x => string.Join(" + ", x.ToObject<List<drugs>>().Where(m => !string.IsNullOrEmpty(m.ncitId)).Select(n => n.ncitId));
                        civicData.drug_ncitid = drugId(i.drugs);

                        civicData.drugInteractionType = i.drugInteractionType == null ? "" : i.drugInteractionType;
                        civicData.disease = i.disease.name == null ? "" : i.disease.name;
                        civicData.diseaseId = i.disease.id;
                        civicData.diseaseUrl = i.disease.diseaseUrl == null ? "" : i.disease.diseaseUrl;
                        civicData.gene = i.variant.gene.name == null ? "" : i.variant.gene.name;
                        civicData.entrezId = i.variant.gene.entrezId == null ? "" : i.variant.gene.entrezId;

                        Func<JArray, string> geneSource = x => string.Join(", ", x.ToObject<List<sources>>().Where(m => !string.IsNullOrEmpty(m.pmcId)).Select(n => n.pmcId));
                        civicData.gene_sources = i.variant.gene.sources == null ? "" : geneSource(i.variant.gene.sources);
                        civicData.variant_description = i.description == null ? "" : i.description;
                        civicData.variant_name = i.variant.name == null ? "" : i.variant.name;
                        civicData.hgvsDescriptions = i.variant.hgvsDescriptions == null ? "" : string.Join(" | ", i.variant.hgvsDescriptions);

                        Func<JArray, string> variantType = x => string.Join(", ", x.ToObject<List<variantTypes>>().Where(m => !string.IsNullOrEmpty(m.name)).Select(n => n.name));
                        civicData.variantTypes = i.variant.variantTypes == null ? "" : variantType(i.variant.variantTypes);
                        civicData.variantLink = i.variant.link == null ? "" : i.variant.link;
                        civicData.variantStart = i.variant.primaryCoordinates == null ? "" : i.variant.primaryCoordinates.start;
                        civicData.variantStop = i.variant.primaryCoordinates == null ? "" : i.variant.primaryCoordinates.stop;
                        civicData.variantChromo = i.variant.primaryCoordinates == null ? "" : i.variant.primaryCoordinates.chromosome;
                        civicData.variantRepTrans = i.variant.primaryCoordinates == null ? "" : i.variant.primaryCoordinates.representativeTranscript;
                        civicData.variantRefBase = i.variant.referenceBases == null ? "" : i.variant.referenceBases;
                        civicData.variantAltBase = i.variant.variantBases == null ? "" : i.variant.variantBases;
                        civicData.SV_Type = needAnnotateData.type;

                        civicData.clinicalSignificance = i.clinicalSignificance == null ? "" : i.clinicalSignificance;
                        civicData.eviItemLevel = i.evidenceLevel == null ? "" : i.evidenceLevel;
                        civicData.eviItemType = i.evidenceType == null ? "" : i.evidenceType;
                        civicData.eviItemRating = i.evidenceRating == null ? "" : i.evidenceRating;
                        civicData.eviItemDirection = i.evidenceDirection == null ? "" : i.evidenceDirection;
                        civicData.eviItemLink = i.link == null ? "" : i.link;
                        civicData.eviItemStatus = i.status == null ? "" : i.status;
                        civicData.eviItemSource = i.source.pmcId == null ? "" : i.source.pmcId;

                        CIVIC_SAVE.Add(civicData);


                    }

                    //continue;

                }

                if (i.description != null && !string.IsNullOrEmpty(i.description.ToString()))
                {
                    Descri = i.description.ToString();
                }
                else
                {
                    Descri = "";
                }


                if (i.evidenceItems != null)
                {
                    var evidenceItems = i.evidenceItems;
                    var eviNodes = evidenceItems.nodes;
                    CIVICNode(eviNodes, CIVIC_SAVE, needAnnotateData, Descri);
                }
                else
                {
                    continue;
                }



            }

            return CIVIC_SAVE;
        }


        // 解析传入的[ONCOKB]多重嵌套json节点
        public List<ONCOKB_DATA> ONCOKBNode(dynamic oncokbJson, List<ONCOKB_DATA> ONCOKB_SAVE, string refSeq, ExtractVariants.needAnnotateData needAnnotateData)
        {
            if (oncokbJson.geneExist != false || oncokbJson.variantExist != false)
            {
                
                foreach (var onco in oncokbJson.treatments)
                {
                    if (onco.drugs.Count >= 1)
                    {
                        ONCOKB_DATA oncokbData = new ONCOKB_DATA();

                        oncokbData.queryGene = needAnnotateData.geneA;
                        oncokbData.queryVariant = needAnnotateData.variant;
                        oncokbData.queryTranscript = needAnnotateData.transcript;
                        oncokbData.queryExon = needAnnotateData.exon;
                        oncokbData.queryCdsChange = needAnnotateData.cds_change;

                        oncokbData.refSeq = refSeq;
                        oncokbData.oncogenic = oncokbJson.oncogenic;
                        oncokbData.knownEffect = oncokbJson.mutationEffect.knownEffect;
                        oncokbData.mutationCitations = string.Join(" | ", oncokbJson.mutationEffect.citations.pmids);
                        oncokbData.highestSensitiveLevel = oncokbJson.highestSensitiveLevel;
                        oncokbData.highestResistanceLevel = oncokbJson.highestResistanceLevel;
                        oncokbData.highestFdaLevel = oncokbJson.highestFdaLevel;
                        oncokbData.hotspot = oncokbJson.hotspot;
                        oncokbData.geneSummary = oncokbJson.geneSummary;
                        oncokbData.variantSummary = oncokbJson.variantSummary;
                        oncokbData.dataVersion = oncokbJson.dataVersion;
                        oncokbData.lastUpdate = oncokbJson.lastUpdate;
                        // 当把以上这部分放在循环外，内容出错，
                        // Lambda表达式(或匿名方法)中所引用的外部变量称为捕获变量。而捕获变量的表达式就称为闭包。
                        // 因为捕获的变量会在[真正调用]委托时“赋值”，而不是在捕获时“赋值”，即总是使用捕获变量的[最新的值]
                        // 因此调用委托的值就都是最后一个循环的值

                        oncokbData.alteration = string.Join(" | ", onco.alterations);

                        Func<JArray, string> GetDrugName = x => string.Join(" + ", x.ToObject<List<oncokbDrugs>>().Where(m => !string.IsNullOrEmpty(m.drugName)).Select(n => n.drugName));
                        oncokbData.drugName = GetDrugName(onco.drugs);

                        Func<JArray, string> GetDrugNcit = x => string.Join(" + ", x.ToObject<List<oncokbDrugs>>().Where(m => !string.IsNullOrEmpty(m.drugName)).Select(n => n.ncitCode));
                        oncokbData.drugNcitCode = GetDrugNcit(onco.drugs);

                        Func<JArray, string> GetDrugSynonyms = x => string.Join(" + ", x.ToObject<List<oncokbDrugs>>().Where(m => !string.IsNullOrEmpty(m.drugName)).Select(n => string.Join("/",n.synonyms)));
                        oncokbData.drugSynonyms = GetDrugSynonyms(onco.drugs);

                        oncokbData.approvedIndications = string.Join(" | ", onco.approvedIndications);
                        oncokbData.treatmentLevel = onco.level;
                        oncokbData.fdaLevel = onco.fdaLevel;
                        oncokbData.levelAssociCancerType_Code = onco.levelAssociatedCancerType.code;
                        oncokbData.levelAssociCancerType_Name = onco.levelAssociatedCancerType.name;
                        oncokbData.cancerMainType = onco.levelAssociatedCancerType.mainType.name;
                        oncokbData.tissue = onco.levelAssociatedCancerType.tissue;
                        oncokbData.tumorForm = onco.levelAssociatedCancerType.tumorForm;
                        oncokbData.treatpmids = string.Join(" | ", onco.pmids);

                        ONCOKB_SAVE.Add(oncokbData);
                    }

                }


            }
            
            return ONCOKB_SAVE;

        }

        // 从oncokb获取对应基因的RefSeq
        public async Task<Dictionary<string, string>> ONCOKBGene(HttpClient hc)
        {
            string ONCOKB_BASE_URL = "https://www.oncokb.org/api/v1";
            string Token = "31ee4294-feed-473d-98f1-4084d92d0078";

            string queryGene = "/utils/cancerGeneList?";
            string queryGeneURL = ONCOKB_BASE_URL + queryGene;

            var request = new HttpRequestMessage(HttpMethod.Get, queryGeneURL);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

            Thread.Sleep(20);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            // 发送Get请求
            var response = await hc.SendAsync(request);
            var responseGene = await response.Content.ReadAsStringAsync();
            var geneJson = JsonConvert.DeserializeObject<dynamic>(responseGene);

            Func<object, string> GetRefSeq = x => x.ToString();
            Dictionary<string, string> GeneDict = new Dictionary<string, string>();
            foreach (var i in geneJson)
            {

                GeneDict.Add(GetRefSeq(i.hugoSymbol), GetRefSeq(i.grch37RefSeq));

            }
            return GeneDict;

        }


        // 对每一行SNV 组合多个数据库中注释结果
        public List<matchCombine> CombineAnno(List<TricdbMatch> TricDB_Matches, List<civicMatch> CIVIC_Matches, List<oncokbMatch> OncoKB_Matches)
        {
            
            List<ExtractVariants.matchCombine> matchList = new List<ExtractVariants.matchCombine>();
            List<string> comItem = new List<string>();

            // 找出 TricDB、CIVIC、OncoKB相同
            foreach (var i in TricDB_Matches)
            {
                foreach (var j in CIVIC_Matches)
                {
                    {
                        foreach (var o in OncoKB_Matches)
                        {
                            if (i.Drug.ToLower() == j.Drug.ToLower() && i.Drug.ToLower() == o.Drug.ToLower() && j.Drug.ToLower() == o.Drug.ToLower())
                            {
                                if (i.Observe_Alteration.ToLower() == j.Observe_Alteration.ToLower() && i.Observe_Alteration.ToLower() == o.Observe_Alteration.ToLower() && j.Observe_Alteration.ToLower() == o.Observe_Alteration.ToLower())
                                {
                                    if (i.Disease.ToLower() == j.Disease.ToLower() && i.Disease.ToLower() == o.Disease.ToLower() && j.Disease.ToLower() == o.Disease.ToLower())
                                    {

                                        comItem.Add(i.Guid);
                                        comItem.Add(j.Guid);
                                        comItem.Add(o.Guid);

                                        Func<string, string, string, string> Combine = (x, y, z) => JsonConvert.SerializeObject(new { TricDB = Convert.ToString(x), CIVIC = Convert.ToString(y), oncoKB = Convert.ToString(z) }).Replace("{","").Replace("}","").Replace(@"""","");
                                        Dictionary<string, string> diffType = new Dictionary<string, string>();

                                        ExtractVariants.matchCombine matchCombine = new ExtractVariants.matchCombine();

                                        matchCombine.Gene = i.Gene;
                                        matchCombine.Observe_Alteration = i.Annoation_Alteration;
                                        matchCombine.Observe_RefSeq = Combine(i.Observe_RefSeq,j.Observe_RefSeq,o.Observe_RefSeq);
                                        matchCombine.Observe_Exon = Combine(i.Observe_Exon, j.Observe_Exon, o.Observe_Exon);
                                        matchCombine.Observe_CdsChange = Combine(i.Observe_CdsChange, j.Observe_CdsChange, o.Observe_CdsChange);

                                        matchCombine.Variant_Type = i.Variant_Type;
                                        matchCombine.Disease = i.Disease;
                                        matchCombine.Annoation_Alteration = Combine(i.Annoation_Alteration,j.Annoation_Alteration,o.Annoation_Alteration);
                                        matchCombine.Annoation_RefSeq = Combine(i.Annoation_RefSeq,j.Annoation_RefSeq,o.Annoation_RefSeq);;
                                        matchCombine.Drug = i.Drug;
                                        matchCombine.Resource = "TricDB, CIVIC, oncoKB";
                                        matchCombine.Evidence_Level = Combine(i.Evidence_Level,j.Evidence_Level,o.treatmentLevel);
                                        matchCombine.Clinical_Significance = Combine(i.Clinical_Significance,j.Clinical_Significance, "Sensitivity");
                                        matchCombine.Reference = Combine(i.Reference,j.Reference,o.PMID);
                                        matchCombine.Tricdb_GeneFunction = i.Gene_Function ?? "";
                                        matchCombine.Tricdb_Therapy = i.Therapy ?? "";
                                        matchCombine.Clinicals = i.Clinicals + j.Reference + o.PMID ?? "";
                                        matchCombine.Tricdb_Indication = i.Indication ?? "";
                                        matchCombine.Tricdb_Dosage = i.Dosage ?? "";
                                        matchCombine.Tricdb_MutationRate = i.Mutation_Rate ?? "";
                                        matchCombine.CIVIC_VariantDesc = j.nodeDescri ?? "";
                                        matchCombine.CIVIC_TreatDesc = j.variant_description ?? "";
                                        matchCombine.OncoKB_GeneSummary = o.GeneSummary ?? "";
                                        matchCombine.OncoKB_VariantSummary = o.VariantSummary ?? "";
                                        matchCombine.Tricdb_Mechanism = i.Mechanism ?? "";
                                        matchList.Add(matchCombine);
                                    }
                                }
                            }
                        }

                    }
                }
            }


            // 找出 TricDB和CIVIC相同
            foreach (var i in TricDB_Matches)
            {
                foreach (var j in CIVIC_Matches)
                {
                    if (i.Drug.ToLower() == j.Drug.ToLower() && i.Observe_Alteration.ToLower() == j.Observe_Alteration.ToLower() && i.Disease.ToLower() == j.Disease.ToLower() && !comItem.Contains(i.Guid) && !comItem.Contains(j.Guid))
                    {

                        comItem.Add(i.Guid);
                        comItem.Add(j.Guid);

                        Func<string, string, string> Combine = (x, y) => JsonConvert.SerializeObject(new { TricDB = Convert.ToString(x), CIVIC = Convert.ToString(y)}).Replace("{", "").Replace("}", "").Replace(@"""", "");
                        Dictionary<string, string> diffType = new Dictionary<string, string>();

                        ExtractVariants.matchCombine matchCombine = new ExtractVariants.matchCombine();

                        matchCombine.Gene = i.Gene;
                        matchCombine.Observe_Alteration = i.Annoation_Alteration;
                        matchCombine.Observe_RefSeq = Combine(i.Observe_RefSeq, j.Observe_RefSeq);
                        matchCombine.Observe_Exon = Combine(i.Observe_Exon, j.Observe_Exon);
                        matchCombine.Observe_CdsChange = Combine(i.Observe_CdsChange, j.Observe_CdsChange);

                        matchCombine.Variant_Type = i.Variant_Type;
                        matchCombine.Disease = i.Disease;
                        matchCombine.Annoation_Alteration = Combine(i.Annoation_Alteration, j.Annoation_Alteration);
                        matchCombine.Annoation_RefSeq = Combine(i.Annoation_RefSeq,j.Annoation_RefSeq);
                        matchCombine.Drug = i.Drug;
                        matchCombine.Resource = "TricDB, CIVIC";
                        matchCombine.Evidence_Level = Combine(i.Evidence_Level, j.Evidence_Level);
                        matchCombine.Clinical_Significance = Combine(i.Clinical_Significance, j.Clinical_Significance);
                        matchCombine.Reference = Combine(i.Reference, j.Reference);
                        matchCombine.Tricdb_GeneFunction = i.Gene_Function ?? "";
                        matchCombine.Tricdb_Therapy = i.Therapy ?? "";
                        matchCombine.Clinicals = i.Clinicals + j.Reference ?? "";
                        matchCombine.Tricdb_Indication = i.Indication ?? "";
                        matchCombine.Tricdb_Dosage = i.Dosage;
                        matchCombine.Tricdb_MutationRate = i.Mutation_Rate ?? "";
                        matchCombine.CIVIC_VariantDesc = j.nodeDescri ?? "";
                        matchCombine.CIVIC_TreatDesc = j.variant_description ?? "";
                        matchCombine.OncoKB_GeneSummary = " ";
                        matchCombine.OncoKB_VariantSummary = " ";
                        matchCombine.Tricdb_Mechanism = i.Mechanism ?? "";
                        matchList.Add(matchCombine);
                    }
                }
            }



            // 找出TricDB和OncoKB相同
            foreach (var i in TricDB_Matches)
            {
                foreach (var o in OncoKB_Matches)
                {
                    if (i.Drug.ToLower() == o.Drug.ToLower() && i.Observe_Alteration.ToLower() == o.Observe_Alteration.ToLower() && i.Disease.ToLower() == o.Disease.ToLower() && !comItem.Contains(i.Guid) && !comItem.Contains(o.Guid))
                    {

                        comItem.Add(i.Guid);
                        comItem.Add(o.Guid);
                        Func<string, string, string> Combine = (x, y) => JsonConvert.SerializeObject(new { TricDB = Convert.ToString(x),oncoKB = Convert.ToString(y) }).Replace("{", "").Replace("}", "").Replace(@"""", "");
                        Dictionary<string, string> diffType = new Dictionary<string, string>();

                        ExtractVariants.matchCombine matchCombine = new ExtractVariants.matchCombine();

                        matchCombine.Gene = i.Gene;
                        matchCombine.Observe_Alteration = i.Annoation_Alteration;
                        matchCombine.Observe_RefSeq = Combine(i.Observe_RefSeq,o.Observe_RefSeq);
                        matchCombine.Observe_Exon = Combine(i.Observe_Exon, o.Observe_Exon);
                        matchCombine.Observe_CdsChange = Combine(i.Observe_CdsChange, o.Observe_CdsChange);

                        matchCombine.Variant_Type = i.Variant_Type;
                        matchCombine.Disease = i.Disease;
                        matchCombine.Annoation_Alteration = Combine(i.Annoation_Alteration, o.Annoation_Alteration);
                        matchCombine.Annoation_RefSeq = Combine(i.Annoation_RefSeq,o.Annoation_RefSeq);
                        matchCombine.Drug = i.Drug;
                        matchCombine.Resource = "TricDB, oncoKB"; ;
                        matchCombine.Evidence_Level = Combine(i.Evidence_Level, o.treatmentLevel);
                        matchCombine.Clinical_Significance = Combine(i.Clinical_Significance, "Unknown");
                        matchCombine.Reference = Combine(i.Reference, o.PMID);
                        matchCombine.Tricdb_GeneFunction = i.Gene_Function ?? "";
                        matchCombine.Tricdb_Therapy = i.Therapy ?? "";
                        matchCombine.Clinicals = i.Clinicals ?? "";
                        matchCombine.Tricdb_Indication = i.Indication ?? "";
                        matchCombine.Tricdb_Dosage = i.Dosage ?? "";
                        matchCombine.Tricdb_MutationRate = i.Mutation_Rate ?? "";
                        matchCombine.CIVIC_VariantDesc = " ";
                        matchCombine.CIVIC_TreatDesc = " ";
                        matchCombine.OncoKB_GeneSummary = o.GeneSummary ?? "";
                        matchCombine.OncoKB_VariantSummary = o.VariantSummary ?? "";
                        matchCombine.Tricdb_Mechanism = i.Mechanism ?? "";
                        matchList.Add(matchCombine);
                    }
                }
            }



            // 找出CIVIC和OncoKB相同
            foreach (var j in CIVIC_Matches)
            {
                foreach (var o in OncoKB_Matches)
                {
                    if (j.Drug.ToLower() == o.Drug.ToLower() && j.Observe_Alteration.ToLower() == o.Observe_Alteration.ToLower() && j.Disease.ToLower() == o.Disease.ToLower() && !comItem.Contains(j.Guid) && !comItem.Contains(o.Guid))
                    {

                        comItem.Add(j.Guid);
                        comItem.Add(o.Guid);
                        Func<string, string, string> Combine = (x, y) => JsonConvert.SerializeObject(new { CIVIC = Convert.ToString(x), oncoKB = Convert.ToString(y) }).Replace("{", "").Replace("}", "").Replace(@"""", "");
                        Dictionary<string, string> diffType = new Dictionary<string, string>();

                        ExtractVariants.matchCombine matchCombine = new ExtractVariants.matchCombine();

                        matchCombine.Gene = j.Gene;
                        matchCombine.Observe_Alteration = j.Annoation_Alteration;
                        matchCombine.Observe_RefSeq = Combine(j.Observe_RefSeq,o.Observe_RefSeq);
                        matchCombine.Observe_Exon = Combine(j.Observe_Exon, o.Observe_Exon);
                        matchCombine.Observe_CdsChange = Combine(j.Observe_CdsChange, o.Observe_CdsChange);

                        matchCombine.Variant_Type = j.Variant_Type;
                        matchCombine.Disease = j.Disease;
                        matchCombine.Annoation_Alteration = Combine(j.Annoation_Alteration, o.Annoation_Alteration);
                        matchCombine.Annoation_RefSeq = Combine(j.Annoation_RefSeq,o.Annoation_RefSeq);
                        matchCombine.Drug = j.Drug;
                        matchCombine.Resource = "CIVIC, oncoKB"; ;
                        matchCombine.Evidence_Level = Combine(j.Evidence_Level, o.treatmentLevel);
                        matchCombine.Clinical_Significance = Combine(j.Clinical_Significance, "Unknown");
                        matchCombine.Reference = Combine(j.Reference, o.PMID);
                        matchCombine.Tricdb_GeneFunction = " ";
                        matchCombine.Tricdb_Therapy = " ";
                        matchCombine.Clinicals = j.Reference;
                        matchCombine.Tricdb_Indication = " ";
                        matchCombine.Tricdb_Dosage = " ";
                        matchCombine.Tricdb_MutationRate = " ";
                        matchCombine.CIVIC_VariantDesc = " ";
                        matchCombine.CIVIC_TreatDesc = " ";
                        matchCombine.OncoKB_GeneSummary = o.GeneSummary ?? "";
                        matchCombine.OncoKB_VariantSummary = o.VariantSummary ?? "";
                        matchCombine.Tricdb_Mechanism = " ";
                        matchList.Add(matchCombine);
                    }
                }
            }

            // 剩下的都单独列出
            // TricDB单独
            foreach (var i in TricDB_Matches.Take(4))
            {
                if (!comItem.Contains(i.Guid))
                {
                    comItem.Add(i.Guid);
                    Func<string, string> Combine = x => JsonConvert.SerializeObject(new { TricDB = Convert.ToString(x) }).Replace("{", "").Replace("}", "").Replace(@"""", "");
                    Dictionary<string, string> diffType = new Dictionary<string, string>();

                    ExtractVariants.matchCombine matchCombine = new ExtractVariants.matchCombine();


                    matchCombine.Gene = i.Gene;
                    matchCombine.Observe_Alteration = i.Observe_Alteration;
                    matchCombine.Observe_RefSeq = i.Observe_RefSeq;
                    matchCombine.Observe_Exon = i.Observe_Exon;
                    matchCombine.Observe_CdsChange = i.Observe_CdsChange;

                    matchCombine.Variant_Type = i.Variant_Type;
                    matchCombine.Disease = i.Disease;
                    matchCombine.Annoation_Alteration = Combine(i.Annoation_Alteration);
                    matchCombine.Annoation_RefSeq = Combine(i.Annoation_RefSeq);
                    matchCombine.Drug = i.Drug;
                    matchCombine.Resource = "TricDB";
                    matchCombine.Evidence_Level = Combine(i.Evidence_Level);
                    if (string.IsNullOrEmpty(i.Clinical_Significance))
                    {
                        matchCombine.Clinical_Significance = Combine("Unknown");
                    }
                    
                    matchCombine.Reference = Combine(i.Reference);
                    matchCombine.Tricdb_GeneFunction = i.Gene_Function ?? "";
                    matchCombine.Tricdb_Therapy = i.Therapy ?? "";
                    matchCombine.Clinicals = i.Clinicals ?? "";
                    matchCombine.Tricdb_Indication = i.Indication ?? "";
                    matchCombine.Tricdb_Dosage = i.Dosage ?? "";
                    matchCombine.Tricdb_MutationRate = i.Mutation_Rate ?? "";
                    matchCombine.CIVIC_VariantDesc = " ";
                    matchCombine.CIVIC_TreatDesc = " ";
                    matchCombine.OncoKB_GeneSummary = " ";
                    matchCombine.OncoKB_VariantSummary = " ";
                    matchCombine.Tricdb_Mechanism = i.Mechanism ?? "";
                    matchList.Add(matchCombine);
                }
            }


            // CIVIC单独
            foreach (var j in CIVIC_Matches.Take(4))
            {
                if (!comItem.Contains(j.Guid))
                {

                    comItem.Add(j.Guid);
                    Func<string, string> Combine = x => JsonConvert.SerializeObject(new { CIVIC = Convert.ToString(x)}).Replace("{", "").Replace("}", "").Replace(@"""", "");
                    Dictionary<string, string> diffType = new Dictionary<string, string>();

                    ExtractVariants.matchCombine matchCombine = new ExtractVariants.matchCombine();

                    matchCombine.Gene = j.Gene;
                    matchCombine.Observe_Alteration = j.Observe_Alteration;
                    matchCombine.Observe_RefSeq = j.Observe_RefSeq;
                    matchCombine.Observe_Exon = j.Observe_Exon;
                    matchCombine.Observe_CdsChange = j.Observe_CdsChange;

                    matchCombine.Variant_Type = j.Variant_Type;
                    matchCombine.Disease = j.Disease;
                    matchCombine.Annoation_Alteration = Combine(j.Annoation_Alteration);
                    matchCombine.Annoation_RefSeq = Combine(j.Annoation_RefSeq);
                    matchCombine.Drug = j.Drug;
                    matchCombine.Resource = "CIVIC";
                    matchCombine.Evidence_Level = Combine(j.Evidence_Level);
                    matchCombine.Clinical_Significance = Combine(j.Clinical_Significance);
                    matchCombine.Reference = Combine(j.Reference);
                    matchCombine.Tricdb_GeneFunction = " ";
                    matchCombine.Tricdb_Therapy = " ";
                    matchCombine.Clinicals = j.Reference ?? "";
                    matchCombine.Tricdb_Indication = " ";
                    matchCombine.Tricdb_Dosage = " ";
                    matchCombine.Tricdb_MutationRate = " ";
                    matchCombine.CIVIC_VariantDesc = j.nodeDescri ?? "";
                    matchCombine.CIVIC_TreatDesc = j.variant_description ?? "";
                    matchCombine.OncoKB_GeneSummary = " ";
                    matchCombine.OncoKB_VariantSummary = " ";
                    matchCombine.Tricdb_Mechanism = " ";
                    matchList.Add(matchCombine);
                }

            }

            // OncoKB单独
            foreach (var o in OncoKB_Matches.Take(4))
            {
                if (!comItem.Contains(o.Guid))
                {
                    comItem.Add(o.Guid);
                    Func<string, string> Combine = x => JsonConvert.SerializeObject(new { oncoKB = Convert.ToString(x) }).Replace("{", "").Replace("}", "").Replace(@"""", "");
                    Dictionary<string, string> diffType = new Dictionary<string, string>();

                    ExtractVariants.matchCombine matchCombine = new ExtractVariants.matchCombine();

                    matchCombine.Gene = o.Gene;
                    matchCombine.Observe_Alteration = o.Annoation_Alteration;
                    matchCombine.Observe_RefSeq = o.Observe_RefSeq;
                    matchCombine.Observe_Exon = o.Observe_Exon;
                    matchCombine.Observe_CdsChange = o.Observe_CdsChange;

                    matchCombine.Variant_Type = " ";
                    matchCombine.Disease = o.Disease;
                    matchCombine.Annoation_Alteration = Combine(o.Annoation_Alteration);
                    matchCombine.Annoation_RefSeq = Combine(o.Annoation_RefSeq);
                    matchCombine.Drug = o.Drug;
                    matchCombine.Resource = "oncoKB";
                    matchCombine.Evidence_Level = Combine(o.treatmentLevel);
                    matchCombine.Clinical_Significance = Combine("Unknown");
                    matchCombine.Reference = Combine(o.PMID);
                    matchCombine.Tricdb_GeneFunction = " ";
                    matchCombine.Tricdb_Therapy = " ";
                    matchCombine.Clinicals = o.PMID;
                    matchCombine.Tricdb_Indication = " ";
                    matchCombine.Tricdb_Dosage = " ";
                    matchCombine.Tricdb_MutationRate = " ";
                    matchCombine.CIVIC_VariantDesc = " ";
                    matchCombine.CIVIC_TreatDesc = " ";
                    matchCombine.OncoKB_GeneSummary = o.GeneSummary ?? "";
                    matchCombine.OncoKB_VariantSummary = o.VariantSummary ?? "";
                    matchCombine.Tricdb_Mechanism = " ";
                    matchList.Add(matchCombine);

                }
            }


            return matchList;






        }









        public class SNVInfo
        {
            public string gene { get; set; }
            //public string transcript { get; set; }
            public string chromosome { get; set; }
            public string startPosition { get; set; }
            public string endPosition { get; set; }
            //public string proteinChange { get; set; }
            public string refSeq { get; set; }
            public string altSeq { get; set; }
            public string[] AAChange { get; set; }
        }

        public class needAnnotateData
        {
            public string geneA { get; set; }
            public string geneB { get; set; }
            //public string transcript { get; set; }
            public string transcript { get; set; }
            public string exon { get; set; }
            public string cds_change { get; set; }
            //public string proteinChange { get; set; }
            public string variant { get; set; }
            public string type { get; set; }
        }


        public class CIVICEviItem
        {
            public string clinicalSignificance { get; set; }
            public string description { get; set; }
            public disease disease { get; set; }
            public List<drugs> drugs { get; set; }
            public variant variant { get; set; }
            public string evidenceDirection { get; set; }
            public string evidenceLevel { get; set; }
            public int evidenceRating { get; set; }
            public string evidenceType { get; set; }
            public gene gene { get; set; }
            public int id { get; set; }
            public string link { get; set; }
            public string name { get; set; }
            public List<phenotypes> phenotypes { get; set; }
            public source source { get; set; }
            public string status { get; set; }
            public string variantOrigin { get; set; }
        }

        public class drugs
        {
            public int id { get; set; }
            public string name { get; set; }
            public string ncitId { get; set; }
            public List<string> drugAliases { get; set; }
        }

        public class disease
        {
            public int id { get; set; }
            public string name { get; set; }
            public string diseaseUrl { get; set; }
            public string displayName { get; set; }
            public List<string> diseaseAliases { get; set; }
        }

        public class variant
        {
            public int id { get; set; }
            public string name { get; set; }
            public gene gene { get; set; }
            public List<string> hgvsDescriptions { get; set; }
            public List<string> variantAliases { get; set; }
            public List<variantTypes> variantTypes { get; set; }
            public string link { get; set; }
            public string primaryCoordinates { get; set; } ///
            public string referenceBases { get; set; }
            public string variantBases { get; set; }

        }

        public class gene
        {
            public int id { get; set; }
            public int entrezId { get; set; }
            public string name { get; set; }
            public List<string> geneAliases { get; set; }
            public string officialName { get; set; }
            public List<sources> sources { get; set; }
        }

        public class sources
        {
            public string Abstract { get; set; }
            public int ascoAbstractId { get; set; }
            public string authorString { get; set; }
            public string publicationDate { get; set; }
            public string title { get; set; }
            public string pmcId { get; set; }
        }

        public class variantTypes
        {
            public string name { get; set; }
            public string description { get; set; }
            public string soid { get; set; }
        }

        public class primaryCoordinate
        {
            public int start { get; set; }
            public int stop { get; set; }
            public string chromosome { get; set; }
            public string representativeTranscript { get; set; }
        }

        public class phenotypes
        {
            public int id { get; set; }
            public string name { get; set; }
            public string link { get; set; }
            public string url { get; set; }
        }

        public class source
        {
            public string ascoAbstractId { get; set; }
            public string pmcId { get; set; }
            public List<clinicalTrials> clinicalTrials { get; set; }
        }

        public class clinicalTrials
        {
            public int id { get; set; }
            public string nctId { get; set; }
            public string name { get; set; }
        }


        public class CIVIC_DATA
        {
            public string queryGene { get; set; }
            public string queryVariant { get; set; }
            public string queryTranscript { get; set; }
            public string queryExon { get; set; }
            public string queryCdsChange { get; set; }
            public string nodeDescri { get; set; }
            public string drug_name { get; set; }
            public string drug_ncitid { get; set; }
            public string drugInteractionType { get; set; }
            public string disease { get; set; }
            public string diseaseUrl { get; set; }
            public int diseaseId { get; set; }
            public string gene { get; set; }
            public string entrezId { get; set; }
            public string gene_sources { get; set; }
            public string variant_description { get; set; }
            public string variant_name { get; set; }
            public string hgvsDescriptions { get; set; }
            public string variantTypes { get; set; }
            public string variantLink { get; set; }
            public string variantStart { get; set; }
            public string variantStop { get; set; }
            public string variantChromo { get; set; }
            public string variantRepTrans { get; set; }
            public string variantRefBase { get; set; }
            public string variantAltBase { get; set; }
            public string SV_Type { get; set; }
            public string clinicalSignificance { get; set; }
            public string eviItemLevel { get; set; }
            public string eviItemType { get; set; }
            public string eviItemRating { get; set; }
            public string eviItemDirection { get; set; }
            public string eviItemLink { get; set; }
            public string eviItemStatus { get; set; }
            public string eviItemSource { get; set; }
        }


        public class ONCOKB_DATA
        {
            public string queryGene { get; set; }
            public string queryVariant { get; set; }
            public string queryTranscript { get; set; }
            public string queryExon { get; set; }
            public string queryCdsChange { get; set; }
            public string refSeq { get; set; }
            public string knownEffect { get; set; }
            public string oncogenic { get; set; }
            public string mutationCitations { get; set; }
            public string highestSensitiveLevel { get; set; }
            public string highestResistanceLevel { get; set; }
            public string highestFdaLevel { get; set; }
            public bool hotspot { get; set; }
            public string geneSummary { get; set; }
            public string variantSummary { get; set; }
            public string alteration { get; set; }
            public string drugName { get; set; }
            public string drugNcitCode { get; set; }
            public string drugSynonyms { get; set; }
            public string approvedIndications { get; set; }
            public string treatmentLevel { get; set; }
            public string fdaLevel { get; set; }
            public string levelAssociCancerType_Code { get; set; }
            public string levelAssociCancerType_Name { get; set; }
            public string cancerMainType { get; set; }
            public string tissue { get; set; }
            public string tumorForm { get; set; }
            public string treatpmids { get; set; }
            public string dataVersion { get; set; }
            public string lastUpdate { get; set; }
        }

        public class oncokbDrugs
        {
            public string ncitCode { get; set; }
            public string drugName { get; set; }
            public string uuid { get; set; }
            public List<string> synonyms { get; set; }
        }

        public class TricdbMatch
        {
            public string Guid { get; set; }
            public string Gene { get; set; }
            public string Observe_Alteration { get; set; }
            public string Observe_RefSeq { get; set; }
            public string Observe_Exon { get; set; }
            public string Observe_CdsChange { get; set; }
            public string Annoation_Alteration { get; set; }
            public string Annoation_RefSeq { get; set; }
            //public string Transcript { get; set; }
            public string Variant_Type { get; set; }
            public string Disease { get; set; }
            public string Disease_Ncit { get; set; }
            public string Disease_Type { get; set; }
            public string Drug { get; set; }
            public string Resource { get; set; }
            public string Evidence_Level { get; set; }
            public string Clinical_Significance { get; set; }
            public string Reference { get; set; }
            public string Gene_Function { get; set; }
            public string Therapy { get; set; }
            public string Clinicals { get; set; }
            public string Indication { get; set; }
            public string Dosage { get; set; }
            public string Mechanism { get; set; }
            public string Mutation_Rate { get; set; }

        }


        public class civicMatch
        {
            public string Guid { get; set; }
            public string nodeDescri { get; set; }
            public string Gene { get; set; }
            public string Observe_Alteration { get; set; }
            public string Observe_RefSeq { get; set; }
            public string Observe_Exon { get; set; }
            public string Observe_CdsChange { get; set; }
            public string Annoation_Alteration { get; set; }
            public string Annoation_RefSeq { get; set; }
            //public string Transcript { get; set; }
            public string Variant_Type { get; set; }
            public string Variant_Link { get; set; }
            public string Disease { get; set; }
            public string Drug { get; set; }
            public string Drug_ncitid { get; set; }
            public string Drug_InteractionType { get; set; }
            public string Resource { get; set; }
            public string variant_description { get; set; }
            public string Evidence_Level { get; set; }
            public string EvidenceItem_Type { get; set; }
            public string EvidenceItem_Rating { get; set; }
            public string EvidenceItem_Direction { get; set; }
            public string EvidenceItem_Link { get; set; }
            public string EvidenceItem_Status { get; set; }
            public string Clinical_Significance { get; set; }
            public string Reference { get; set; }
        }

        public class oncokbMatch
        {
            public string Guid { get; set; }
            public string Gene { get; set; }
            public string Observe_Alteration { get; set; }
            public string Observe_RefSeq { get; set; }
            public string Observe_Exon { get; set; }
            public string Observe_CdsChange { get; set; }
            public string Annoation_Alteration { get; set; }
            public string Annoation_RefSeq { get; set; }
            //public string Transcript { get; set; }
            public string Oncogenic { get; set; }
            public string KnownEffect { get; set; }
            public string Disease { get; set; }
            public string DiseaseType_Code { get; set; }
            public string DiseaseMain_Type { get; set; }
            public string Tissue { get; set; }
            public string Drug { get; set; }
            public string Drug_ncitid { get; set; }
            public string Drug_synonyms { get; set; }
            public string Resource { get; set; }
            public string GeneSummary { get; set; }
            public string VariantSummary { get; set; }
            public string highestSensitiveLevel { get; set; }
            public string highestResistanceLevel { get; set; }
            public string highestFdaLevel { get; set; }
            public string fdaLevel { get; set; }
            public string treatmentLevel { get; set; }
            public string PMID { get; set; }
        }

        public class matchCombine
        {
            public string Gene { get; set; }
            public string Observe_Alteration { get; set; }
            public string Observe_RefSeq { get; set; }
            public string Observe_Exon { get; set; }
            public string Observe_CdsChange { get; set; }
            public string Annoation_Alteration { get; set; }
            public string Annoation_RefSeq { get; set; }
            //public string Transcript { get; set; }
            public string Variant_Type { get; set; }
            public string Disease { get; set; }
            public string Drug { get; set; }
            public string Resource { get; set; }
            public string Evidence_Level { get; set; }
            public string Clinical_Significance { get; set; }
            public string Reference { get; set; }
            public string Tricdb_GeneFunction { get; set; }
            public string Tricdb_Therapy { get; set; }
            public string Clinicals { get; set; }
            public string Tricdb_Indication { get; set; }
            public string Tricdb_Dosage { get; set; }
            public string Tricdb_Mechanism { get; set; }
            public string Tricdb_MutationRate { get; set; }
            public string CIVIC_VariantDesc { get; set; }
            public string CIVIC_TreatDesc { get; set; }
            public string OncoKB_GeneSummary { get; set; }
            public string OncoKB_VariantSummary { get; set; }
            public string OncoKB_Indication { get; set; }
        }

        //----------------------------------------------------------------------
        /// CNV
        //----------------------------------------------------------------------

        public class CNVInfo
        {
            public string GeneSymbol { get; set; }
            public string EntrezGeneId { get; set; }
            public string Locus_ID { get; set; }
            public string Cytoband { get; set; }
            public int value { get; set; }
        }

        //----------------------------------------------------------------------
        /// SV
        //----------------------------------------------------------------------

        public class SVInfo
        {
            public string GeneA { get; set; }
            public string GeneAEntrezId { get; set; }
            public string GeneB { get; set; }
            public string GeneBEntrezId { get; set; }
            public string SvType { get; set; }
        }



    }
}
