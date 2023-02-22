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

namespace Main.Extensions
{
    public class svAnnotator
    {

        public List<ExtractVariants.TricdbMatch> TricDB_Match(ExtractVariants.SVInfo SVInfos)
        {

            List<ExtractVariants.TricdbMatch> MatchData = new List<ExtractVariants.TricdbMatch>();

            string variant = SVInfos.GeneA + "-" + SVInfos.GeneB + " " + SVInfos.SvType;
            if (SVInfos.GeneA == SVInfos.GeneB)
            {
                variant = "deletion";
            }

            var bus_all = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select * from bus_all where target=@geneA or target=@geneB", new MySqlParameter[] { new MySqlParameter("geneA", SVInfos.GeneA), new MySqlParameter("geneB", SVInfos.GeneB) }).AsQueryable();
            var matches = from n in bus_all
                          where n.ALTERATION.ToLower().Contains(SVInfos.SvType.ToLower()) || n.ALTERATION.Replace(" ", "") == ""
                          select new ExtractVariants.TricdbMatch
                          {
                              Guid = n.RELATIONID,
                              Gene = n.TARGET,
                              Observe_Alteration = variant,
                              Observe_RefSeq = "",
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


            return MatchData;
        }


        public List<Task<List<ExtractVariants.CIVIC_DATA>>> civicResponse(HttpClient hc, ExtractVariants.SVInfo SVInfos)
        {
            List<Task<List<ExtractVariants.CIVIC_DATA>>> civicRequestSave = new List<Task<List<ExtractVariants.CIVIC_DATA>>>();


            GetAnnotatorResponse GetResponse = new GetAnnotatorResponse();

            string variant = SVInfos.GeneA + "::" + SVInfos.GeneB;
            if (SVInfos.GeneA == SVInfos.GeneB)
            {
                variant = "deletion";
            }


            var needAnnotateData = new ExtractVariants.needAnnotateData
            {
                geneA = SVInfos.GeneA,
                geneB = SVInfos.GeneB,
                transcript = "",
                exon = "",
                cds_change = "",
                variant = variant,
                type = SVInfos.SvType
            };

            var civicQueryResult = GetResponse.GetCivicData(hc, needAnnotateData);
            civicRequestSave.Add(civicQueryResult);

            return civicRequestSave;
        }


        // 根据传入的SV数据发送不同的请求
        public List<Task<List<ExtractVariants.ONCOKB_DATA>>> oncokbResponse(HttpClient hc, ExtractVariants.SVInfo SVInfos, string oncokbRefSeq, List<ExtractVariants.needAnnotateData> allVariants)
        {
            List<Task<List<ExtractVariants.ONCOKB_DATA>>> oncokbRequestSave = new List<Task<List<ExtractVariants.ONCOKB_DATA>>>();

            GetAnnotatorResponse GetResponse = new GetAnnotatorResponse();

            string variant = SVInfos.GeneA + "-" + SVInfos.GeneB + " " + SVInfos.SvType;
            if (SVInfos.GeneA == SVInfos.GeneB)
            {
                variant = "deletion";
            }

            var needAnnotateData = new ExtractVariants.needAnnotateData
            {
                geneA = SVInfos.GeneA,
                geneB = SVInfos.GeneB,
                transcript = "",
                exon = "",
                cds_change = "",
                variant = variant,
                type = SVInfos.SvType
            };

            allVariants.Add(needAnnotateData);

            var oncokbResult = GetResponse.GetOncokbDataAsync(hc, needAnnotateData, oncokbRefSeq);
            oncokbRequestSave.Add(oncokbResult);

            return oncokbRequestSave;
        }






    }
}
