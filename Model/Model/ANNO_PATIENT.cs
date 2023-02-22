using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model
{
    public partial class ANNO_PATIENT
    {

        public string PATIENT_ID { get; set; }
        public string PATIENT_NAME{ get; set; }
        public string PATIENT_GENDER { get; set; }
        public int PATIENT_AGE{ get; set; }
        public string PATIENT_DIAG { get; set; }
        public string PATIENT_STAGE { get; set; }
        public string PRIOR_TREAT_HIST { get; set; }
        public string ETHNICITY { get; set; }
        public string FAMILY_HISTORY { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string OPERATOR { get; set; }
        public string VERSION { get; set; }


    }
}
