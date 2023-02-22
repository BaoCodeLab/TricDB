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
    public class msiAnnotator
    {



        public List<ExtractVariants.TricdbMatch> TricDB_Match(string MSI)
        {
            
            List<ExtractVariants.TricdbMatch> MatchData = new List<ExtractVariants.TricdbMatch>();

            var msiList = MSI.Split(':');
            if (msiList.Length <= 1 )
            {
                return MatchData;
            }

            string msiStatus = "" ;
            float score;

            if (msiList[0] == "MSIsensor")
            {
                score = float.Parse(msiList[1]);
                if (score > 3.5)
                {
                    msiStatus = "MSI-H";
                } else { msiStatus = "MSS"; }
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
                return MatchData;
            }


            var bus_all = MySQLDB.GetSimpleTFromQuery<VM_BUS_ALL>("select * from bus_all where target like '%MSI%' or alteration like '%MSI%'").AsQueryable();
            var matches = from n in bus_all
                          select new ExtractVariants.TricdbMatch
                          {
                              Guid = n.RELATIONID,
                              Gene = n.TARGET,
                              Observe_Alteration = msiStatus,
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
                              Mutation_Rate = n.MUTATION_RATE
                          };

            foreach (var tric in matches)
            {
                MatchData.Add(tric);
            }


            return MatchData;
        }


        public List<Task<List<ExtractVariants.ONCOKB_DATA>>> oncokbResponse(HttpClient hc, string MSI)
        {
            List<Task<List<ExtractVariants.ONCOKB_DATA>>> oncokbRequestSave = new List<Task<List<ExtractVariants.ONCOKB_DATA>>>();

            var msiList = MSI.Split(':');
            if (msiList.Length <= 1)
            {
                return oncokbRequestSave;
            }

            string msiStatus = "";
            float score;

            if (msiList[0] == "MSIsensor")
            {
                score = float.Parse(msiList[1]);
                if (score > 3.5)
                {
                    msiStatus = "Microsatellite Instability-High";
                }
                else { msiStatus = "MSS"; }
            }

            if (msiList[0] == "mSINGS")
            {
                score = float.Parse(msiList[1]);
                if (score > 0.2)
                {
                    msiStatus = "Microsatellite Instability-High";
                }
                else { msiStatus = "MSS"; }
            }

            if (msiList[0] == "MANTIS")
            {
                score = float.Parse(msiList[1]);
                if (score > 0.4)
                {
                    msiStatus = "Microsatellite Instability-High";
                }
                else { msiStatus = "MSS"; }
            }

            if (msiList[0] == "Direct")
            {
                msiStatus = msiList[1];
            }

            if (msiStatus == "MSS")
            {
                return oncokbRequestSave;
            }



            GetAnnotatorResponse GetResponse = new GetAnnotatorResponse();

            var needAnnotateData = new ExtractVariants.needAnnotateData
            {
                geneA = "Other Biomarkers",
                transcript = "",
                exon = "",
                cds_change = "",
                variant = msiStatus,
                type = ""
            };

            var oncokbResult = GetResponse.GetOncokbDataAsync(hc, needAnnotateData, "");
            oncokbRequestSave.Add(oncokbResult);

            return oncokbRequestSave;
        }




    }
}
