using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model
{
    public partial class ANNO_SAMPLE
    {

        public string ANNO_SAMPLEID { get; set; }
        public string USER_ID { get; set; }
        public string PATIENT_ID { get; set; }
        public string SAMPLE_NAME { get; set; }
        public string SAMPLE_SOURCE { get; set; }
        public string SAMPLE_TYPE { get; set; }
        public string SAMPLE_POSI { get; set; }
        public string SAMPLE_METHOD { get; set; }
        public string MUTATION_TYPE { get; set; }
        public string SEQUENCE_TYPE { get; set; }
        public string MSI { get; set; }
        public double CAPTURE_SIZE { get; set; }
        public DateTime SAMPLE_DATE { get; set; }
        public DateTime ACCESSION_DATE { get; set; }
        public string SAMPLE_DIAG { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string OPERATOR { get; set; }
        public string VERSION { get; set; }

    }
}
