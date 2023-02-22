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
    public class cnvAnnotator
    {



        public List<ExtractVariants.TricdbMatch> TricDB_Match(ExtractVariants.CNVInfo CNVInfos)
        {
            string queryStr = "";
            List<ExtractVariants.TricdbMatch> MatchData = new List<ExtractVariants.TricdbMatch>();

            if (CNVInfos.value <= -1.5)
            {
                queryStr = "deletion";
            } 
            else if ( CNVInfos.value > -1.5 && CNVInfos.value <= -1) 
            {
                queryStr = "loss";  // loss
            }
            else if (CNVInfos.value > -1 && CNVInfos.value < 1)
            {
                return MatchData;
            }
            else if (CNVInfos.value >= 1 && CNVInfos.value < 2)
            {
                queryStr = "gain";  // gain
            }
            else
            {
                queryStr = "amplification";
            }

            var gene = CNVInfos.GeneSymbol;
            var cnvAlteration = queryStr;
            
            string wildcard = "Positive Expression, Deleterious Mutations";
            var bus_all = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select * from bus_all where target=@gene", new MySqlParameter[] { new MySqlParameter("gene", gene) }).AsQueryable();
            var matches = from n in bus_all
                          where wildcard.Contains(n.ALTERATION) || n.ALTERATION.ToLower().Contains(cnvAlteration.ToLower()) || n.ALTERATION.Replace(" ", "") == ""
                          select new ExtractVariants.TricdbMatch
                          {
                              Guid = n.RELATIONID,
                              Gene = n.TARGET,
                              Observe_Alteration = cnvAlteration,
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

        public List<Task<List<ExtractVariants.CIVIC_DATA>>> civicResponse(HttpClient hc ,ExtractVariants.CNVInfo CNVInfos)
        {
            string queryStr = "";
            List<Task<List<ExtractVariants.CIVIC_DATA>>> civicRequestSave = new List<Task<List<ExtractVariants.CIVIC_DATA>>>();

            if (CNVInfos.value <= -1.5)
            {
                queryStr = "deletion";
            }
            else if (CNVInfos.value > -1.5 && CNVInfos.value <= -1)
            {
                queryStr = "loss";  // loss
            }
            else if (CNVInfos.value > -1 && CNVInfos.value < 1)
            {
                return civicRequestSave;
            }
            else if (CNVInfos.value >= 1 && CNVInfos.value < 2)
            {
                queryStr = "gain";  // gain
            }
            else
            {
                queryStr = "amplification";
            }

            var gene = CNVInfos.GeneSymbol;
            var cnvAlteration = queryStr;

            

            GetAnnotatorResponse GetResponse = new GetAnnotatorResponse();

            var needAnnotateData = new ExtractVariants.needAnnotateData
            {
                geneA = gene,
                geneB = "",
                transcript = "",
                exon = "",
                cds_change = "",
                variant = cnvAlteration,
                type = ""
            };

            var civicQueryResult = GetResponse.GetCivicData(hc, needAnnotateData);
            civicRequestSave.Add(civicQueryResult);

            return civicRequestSave;
        }

        public List<Task<List<ExtractVariants.ONCOKB_DATA>>> oncokbResponse(HttpClient hc, ExtractVariants.CNVInfo CNVInfos, string oncokbRefSeq, List<ExtractVariants.needAnnotateData> allVariants)
        {
            List<Task<List<ExtractVariants.ONCOKB_DATA>>> oncokbRequestSave = new List<Task<List<ExtractVariants.ONCOKB_DATA>>>();
            string queryStr = "";
            if (CNVInfos.value <= -1.5)
            {
                queryStr = "deletion";
            }
            else if (CNVInfos.value > -1.5 && CNVInfos.value <= -1)
            {
                queryStr = "loss";  // loss
            }
            else if (CNVInfos.value > -1 && CNVInfos.value < 1)
            {
                return oncokbRequestSave;
            }
            else if (CNVInfos.value >= 1 && CNVInfos.value < 2)
            {
                queryStr = "gain";  // gain
            }
            else
            {
                queryStr = "amplification";
            }

            var gene = CNVInfos.GeneSymbol;
            var cnvAlteration = queryStr;

            

            GetAnnotatorResponse GetResponse = new GetAnnotatorResponse();

            var needAnnotateData = new ExtractVariants.needAnnotateData
            {
                geneA = gene,
                geneB = "",
                transcript = "",
                exon = "",
                cds_change = "",
                variant = cnvAlteration,
                type = ""
            };

            allVariants.Add(needAnnotateData);

            var oncokbResult = GetResponse.GetOncokbDataAsync(hc, needAnnotateData, oncokbRefSeq);
            oncokbRequestSave.Add(oncokbResult);

            return oncokbRequestSave;
        }

        







    }
}
