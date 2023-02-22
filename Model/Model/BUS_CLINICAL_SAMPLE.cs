using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class BUS_CLINICAL_SAMPLE
    {
        public string SAMPLE_ID { get; set; }
        public string SAMPLECODE { get; set; }
        public string PATIENT_ID { get; set; }
        public string AGE_AT_SEQ_REPORT { get; set; }
        public string ONCOTREE_CODE { get; set; }
        public string SAMPLE_TYPE { get; set; }
        public string SEQ_ASSAY_ID { get; set; }
        public string CANCER_TYPE { get; set; }
        public string CANCER_TYPE_DETAILED { get; set; }
        public string SAMPLE_TYPE_DETAILED { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string VERSION { get; set; }

        public BUS_PATIENT BUS_PATIENT { get; set; }
    }
}
