using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class BUS_PATIENT
    {
        public BUS_PATIENT()
        {
            BUS_PATIENTNavigation = new HashSet<BUS_CLINICAL_SAMPLE>();
        }

        public string PATIENT_ID { get; set; }
        public string PATIENTCODE { get; set; }
        public string SEX { get; set; }
        public string PRIMARY_RACE { get; set; }
        public string ETHNICITY { get; set; }
        public string CENTER { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string VERSION { get; set; }

        public ICollection<BUS_CLINICAL_SAMPLE> BUS_PATIENTNavigation { get; set; }
    }
}
