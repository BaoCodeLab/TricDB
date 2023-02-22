using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class BUS_RELATION
    {
        public string RELATIONID { get; set; }
        public string TARGETID { get; set; }
        public string DISEASEID { get; set; }
        public string DRUGID { get; set; }
        public string MUTATION_RATE { get; set; }
        public string ALTERATION_RATE { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string OPERATOR { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string FUNCTION_AND_CLINICAL_IMPLICATIONS { get; set; }
        public string SPECIFICITY { get; set; }
        public string THERAPY_INTERPRETATION { get; set; }
        public string EVIDENCE_LEVEL { get; set; }
        public string CLINICAL_TRIAL { get; set; }
        public string APPROVED { get; set; }
        public string APPROVAL_TIME { get; set; }
        public string NEGATIVE_GENOTYPES { get; set; }
        public string INDICATIONS { get; set; }
        public string DOSAGE { get; set; }
        public string REFERENCE_LINK { get; set; }
        public string DATA_TYPE { get; set; }
        public string GERMLINE_10389_RATE { get; set; }
        public string CHINESE_10000_RATE { get; set; }
    }
}
