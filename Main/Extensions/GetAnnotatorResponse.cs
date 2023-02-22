using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Resources.NetStandard;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading;
using System.Net;

namespace Main.Extensions
{
    public class GetAnnotatorResponse
    {

        private static readonly string current_path = System.IO.Directory.GetCurrentDirectory();
        private static readonly ResXResourceReader queryS = new ResXResourceReader(current_path + "/Extensions/CIVIC_GraphQL.resx");

        [HttpGet, Route("GetCivicData")]
        public async Task<List<ExtractVariants.CIVIC_DATA>> GetCivicData(HttpClient hc, ExtractVariants.needAnnotateData needAnnotateData)
        {

            string CIVIC_BASE_URL = "https://civicdb.org/api/graphql";

            string x = "";
            foreach (DictionaryEntry d in queryS)
            {
                if ((string)d.Key == "GetVariantsEvidence")
                {
                    x = d.Value.ToString();
                }

            }

            // CIVIC的ABL1查询不带1
            if (!string.IsNullOrEmpty(needAnnotateData.geneB))
            {
                needAnnotateData.variant = needAnnotateData.variant.Replace("ABL1","ABL");
            }


            var format = @"""{0}""";
            var variant = string.Format(format, needAnnotateData.variant);
            // 字符串格式化需要{}
            // 而graphQL查询存在多重{}嵌套，识别时存在错误，因此先将左右{}改为M N，传递参数后再改回来
            var one = x.Replace("{", "<").Replace("}", ">").Replace("[", "{").Replace("]", "}");
            var two = string.Format(one, variant);
            var three = two.Replace("<", "{").Replace(">", "}");

            var postData = new { query = three };

            List<ExtractVariants.CIVIC_DATA> civicQueryResult = new List<ExtractVariants.CIVIC_DATA>();
            //string GetAllEvidenceItems = HttpClientHelper.PostResponse(CIVIC_BASE_URL, postData) ?? "No data";
            Thread.Sleep(30);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var response = await hc.PostAsJsonAsync(CIVIC_BASE_URL, postData);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(responseBody))
            {
                return civicQueryResult;
            }

            // 不需要构造对应实体类，配合Json.net可以实现不用定义实体类的json转dynamic类型对象
            var civicJSON = JsonConvert.DeserializeObject<dynamic>(responseBody);

            if (civicJSON == null)
            {
                return civicQueryResult;
            }

            if (civicJSON.data == null)
            {
                return civicQueryResult;
            }


            var civicNodes = civicJSON.data.variants.nodes;
            //var civicNodes = civicJSON["data"]["variants"]["nodes"];

            

            ExtractVariants Variants = new ExtractVariants();
            List<ExtractVariants.CIVIC_DATA> CIVIC_SAVE = new List<ExtractVariants.CIVIC_DATA>();
            

            civicQueryResult = Variants.CIVICNode(civicNodes, CIVIC_SAVE, needAnnotateData);

            return civicQueryResult;

        }



        [HttpGet, Route("GetOncokbDataAsync")]
        public async Task<List<ExtractVariants.ONCOKB_DATA>> GetOncokbDataAsync(HttpClient hc, ExtractVariants.needAnnotateData needAnnotateData, string SrefSeq)
        {
            string ONCOKB_BASE_URL = "https://www.oncokb.org/api/v1";
            string Token = "31ee4294-feed-473d-98f1-4084d92d0078";

            string queryProteinChange = $"/annotate/mutations/byProteinChange?hugoSymbol={needAnnotateData.geneA}&alteration={needAnnotateData.variant}";
            string queryProteinChangeURL = ONCOKB_BASE_URL + queryProteinChange;

            bool isFunctionFusion = true;
            if (needAnnotateData.geneA == needAnnotateData.geneB)
            {
                isFunctionFusion = false;
                needAnnotateData.type = "deletion";
            }

            string querySV = $"/annotate/structuralVariants?hugoSymbolA={needAnnotateData.geneA}&hugoSymbolB={needAnnotateData.geneB}&structuralVariantType={needAnnotateData.type.ToUpper()}&isFunctionalFusion={isFunctionFusion}&referenceGenome=GRCh37";
            string querySVURL = ONCOKB_BASE_URL + querySV;

            // 配置请求对象
            var request = new HttpRequestMessage(HttpMethod.Get, queryProteinChangeURL);
            if (!string.IsNullOrEmpty(needAnnotateData.geneB))
            {
                request = new HttpRequestMessage(HttpMethod.Get, querySVURL);
            }


            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

            // 添加请求头
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");

            List<ExtractVariants.ONCOKB_DATA> oncokbQueryResult = new List<ExtractVariants.ONCOKB_DATA>();
            // 发送Get请求
            Thread.Sleep(20);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var response = await hc.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var ProteinChangeString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(ProteinChangeString))
            {
                return oncokbQueryResult;
            }

            var ProteinChangeJson = JsonConvert.DeserializeObject<dynamic>(ProteinChangeString);

            ExtractVariants Variants = new ExtractVariants();
            List<ExtractVariants.ONCOKB_DATA> ONCOKB_SAVE = new List<ExtractVariants.ONCOKB_DATA>();
            oncokbQueryResult = Variants.ONCOKBNode(ProteinChangeJson, ONCOKB_SAVE, SrefSeq, needAnnotateData);
            
            return oncokbQueryResult;

        }


        public List<ExtractVariants.civicMatch> CIVIC_Match(List<Task<List<ExtractVariants.CIVIC_DATA>>> civicResponse)
        {
            List<ExtractVariants.civicMatch> MatchData = new List<ExtractVariants.civicMatch>();
            for (int i = 0; i < civicResponse.Count; i++)
            {
                var civicTaskResult = civicResponse[i].Result;
                foreach (var c in civicTaskResult)
                {
                    if (c.queryGene.ToLower().Contains(c.gene.ToLower()) && c.variant_name.ToLower().Contains(c.queryVariant.ToLower()))
                    {
                        Regex r = new Regex(@"NM_\d+");
                        var transp = r.Match(c.hgvsDescriptions).Value.ToString(); // 获取civic的RefSeq

                        var civicMatch = new ExtractVariants.civicMatch
                        {
                            Guid = Guid.NewGuid().ToString(),
                            Gene = c.gene,
                            Observe_Alteration = (c.queryVariant.Replace("::","-").Replace("ABL","ABL1") + " " + c.SV_Type).Trim(),
                            Observe_RefSeq = c.queryTranscript,
                            Observe_Exon = c.queryExon,
                            Observe_CdsChange = c.queryCdsChange,
                            Annoation_Alteration = c.variant_name,
                            Annoation_RefSeq = transp,
                            //Transcript = transcript,
                            Variant_Type = c.variantTypes,
                            Variant_Link = c.variantLink,
                            Disease = c.disease,
                            Drug = c.drug_name,
                            Drug_ncitid = c.drug_ncitid,
                            Drug_InteractionType = c.drugInteractionType,
                            Resource = "CIVIC",
                            Evidence_Level = c.eviItemLevel,
                            EvidenceItem_Type = c.eviItemType,
                            EvidenceItem_Rating = c.eviItemRating,
                            EvidenceItem_Direction = c.eviItemDirection,
                            EvidenceItem_Link = c.eviItemLink,
                            EvidenceItem_Status = c.eviItemStatus,
                            Clinical_Significance = c.clinicalSignificance,
                            Reference = c.eviItemSource,
                        };

                        MatchData.Add(civicMatch);

                    }
                }

            }

            return MatchData;
        }

        public List<ExtractVariants.oncokbMatch> ONCOKB_Match(List<Task<List<ExtractVariants.ONCOKB_DATA>>> oncokbResponse)
        {
            List<ExtractVariants.oncokbMatch> MatchData = new List<ExtractVariants.oncokbMatch>();
            for (int o = 0; o < oncokbResponse.Count; o++)
            {
                var oncokbTaskResult = oncokbResponse[o].Result;
                foreach (var i in oncokbTaskResult)
                {
                    if (i.alteration.ToLower().Contains(i.queryVariant.ToLower()))
                    {
                        var oncokbMatch = new ExtractVariants.oncokbMatch
                        {
                            Guid = Guid.NewGuid().ToString(),
                            Gene = i.queryGene,
                            Observe_Alteration = i.queryVariant,
                            Observe_RefSeq = i.queryTranscript,
                            Observe_Exon = i.queryExon,
                            Observe_CdsChange = i.queryCdsChange,
                            Annoation_Alteration = i.alteration,
                            Annoation_RefSeq = i.refSeq,
                            //Transcript = transcript,
                            Oncogenic = i.oncogenic,
                            KnownEffect = i.knownEffect,
                            Disease = i.levelAssociCancerType_Name == "" ? i.cancerMainType : i.levelAssociCancerType_Name,
                            DiseaseType_Code = i.levelAssociCancerType_Code,
                            DiseaseMain_Type = i.cancerMainType,
                            Tissue = i.tissue,
                            Drug = i.drugName,
                            Drug_ncitid = i.drugNcitCode,
                            Drug_synonyms = i.drugSynonyms,
                            GeneSummary = i.geneSummary,
                            VariantSummary = i.variantSummary,
                            Resource = "OncoKB",
                            highestSensitiveLevel = i.highestSensitiveLevel,
                            highestResistanceLevel = i.highestResistanceLevel,
                            highestFdaLevel = i.highestFdaLevel,
                            fdaLevel = i.fdaLevel,
                            treatmentLevel = i.treatmentLevel,
                            PMID = i.treatpmids

                        };

                        MatchData.Add(oncokbMatch);

                    }
                }

            }
            return MatchData;
        }



        public class TaskCollect
        {
            public List<ExtractVariants.TricdbMatch> TricDB_Matches { get; set; }
            public List<Task<List<ExtractVariants.CIVIC_DATA>>> civicResponse { get; set; }
            public List<Task<List<ExtractVariants.ONCOKB_DATA>>> oncokbResponse { get; set; }

        }







    }
}
