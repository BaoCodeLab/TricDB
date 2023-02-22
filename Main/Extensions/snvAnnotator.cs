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
    public class snvAnnotator
    {



        public List<ExtractVariants.TricdbMatch> TricDB_Match(ExtractVariants.SNVInfo SNVInfos)
        {
            List<ExtractVariants.TricdbMatch> MatchData = new List<ExtractVariants.TricdbMatch>();

            var gene = SNVInfos.gene;
            var AAChanges = SNVInfos.AAChange; // 包含多个转录本和多个变异的数组
            if (AAChanges.Length > 1) // 空串Split的结果存在数组里长度为1
            {

                foreach (var aa in AAChanges) // 找到与数据库里对应的转录本突变
                {
                    string[] allChange = aa.Split(':');
                    if (allChange.Length > 4) // 若注释出的突变包含氨基酸突变
                    {

                        string transcript = allChange[1];
                        string exon = allChange[2].Replace("exon", "Exon ");
                        string cds_change = allChange[3];
                        string protein_change = allChange[4].Replace("p.", "");
                        string wildcard = "Positive Expression, Deleterious Mutations";
                        var bus_all = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select * from bus_all where target=@gene", new MySqlParameter[] { new MySqlParameter("gene", gene) }).AsQueryable();
                        var matches = from n in bus_all
                                      where wildcard.Contains(n.ALTERATION) || n.ALTERATION.ToLower().Contains(protein_change.ToLower()) || n.ALTERATION.ToLower().Contains(exon.ToLower()) || n.ALTERATION.Replace(" ", "") == ""
                                      select new ExtractVariants.TricdbMatch
                                      {
                                          Guid = n.RELATIONID,
                                          Gene = n.TARGET,
                                          Observe_Alteration = protein_change,
                                          Observe_RefSeq = transcript,
                                          Observe_Exon = exon,
                                          Observe_CdsChange = cds_change,
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




                }

            }

            return MatchData;
        }



        public List<Task<List<ExtractVariants.CIVIC_DATA>>> civicResponse(HttpClient hc, ExtractVariants.SNVInfo SNVInfos)
        {

            List<Task<List<ExtractVariants.CIVIC_DATA>>> civicRequestSave = new List<Task<List<ExtractVariants.CIVIC_DATA>>>();


            var gene = SNVInfos.gene;
            var AAChanges = SNVInfos.AAChange; // 包含多个转录本和多个变异的数组

            if (AAChanges.Length > 1) // 空串Split的结果存在数组里长度为1
            {

                foreach (var aa in AAChanges) // 找到与数据库里对应的转录本突变
                {
                    string[] allChange = aa.Split(':');
                    if (allChange.Length > 4) // 若注释出的突变包含氨基酸突变
                    {
                        string transcript = allChange[1];
                        string exon = allChange[2].Replace("exon", "Exon ");
                        string cds_change = allChange[3];
                        string protein_change = allChange[4].Replace("p.", "");

                        var needAnnotateData = new ExtractVariants.needAnnotateData
                        {
                            geneA = gene,
                            geneB = "",
                            transcript = transcript,
                            exon = exon,
                            cds_change = cds_change,
                            variant = protein_change,
                            type = ""
                        };

                        GetAnnotatorResponse GetResponse = new GetAnnotatorResponse();
                        var civicQueryResult = GetResponse.GetCivicData(hc, needAnnotateData);
                        civicRequestSave.Add(civicQueryResult);
                    }

                }
            }

            return civicRequestSave;

        }



        public List<Task<List<ExtractVariants.ONCOKB_DATA>>> oncokbResponse(HttpClient hc, ExtractVariants.SNVInfo SNVInfos, string oncokbRefSeq, List<ExtractVariants.needAnnotateData> allVariants)
        {
            List<Task<List<ExtractVariants.ONCOKB_DATA>>> oncokbRequestSave = new List<Task<List<ExtractVariants.ONCOKB_DATA>>>();

            var gene = SNVInfos.gene;
            var AAChanges = SNVInfos.AAChange; // 包含多个转录本和多个变异的数组
            if (AAChanges.Length > 1) // 空串Split的结果存在数组里长度为1
            {

                foreach (var aa in AAChanges) // 找到与数据库里对应的转录本突变
                {
                    string[] allChange = aa.Split(':');
                    if (allChange.Length > 4) // 若注释出的突变包含氨基酸突变
                    {

                        string transcript = allChange[1];
                        string exon = allChange[2].Replace("exon", "Exon ");
                        string cds_change = allChange[3];
                        string protein_change = allChange[4].Replace("p.", "");

                        var needAnnotateData = new ExtractVariants.needAnnotateData
                        {
                            geneA = gene,
                            geneB = "",
                            transcript = transcript,
                            exon = exon,
                            cds_change = cds_change,
                            variant = protein_change,
                            type = ""
                        };

                        allVariants.Add(needAnnotateData);

                        GetAnnotatorResponse GetResponse = new GetAnnotatorResponse();
                        var oncokbResult = GetResponse.GetOncokbDataAsync(hc, needAnnotateData, oncokbRefSeq);
                        oncokbRequestSave.Add(oncokbResult);

                    }

                }
  

            }


            return oncokbRequestSave;


        }




    }
}
