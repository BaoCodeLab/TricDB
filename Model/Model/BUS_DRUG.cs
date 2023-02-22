
using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class BUS_DRUG
    {
        public string DRUGID { get; set; }
        public string DRUGCODE { get; set; }
        public string DRUG_NAME { get; set; }
        public string DRUG_TYPE { get; set; }
        public string DRUG_TARGET { get; set; }
        public string BRAND_NAME { get; set; }
        public string COMPANY { get; set; }
        public string COMPANY_ALIAS { get; set; }
        public string MECHANISM_OF_ACTION { get; set; }
        public string STRUCTURE { get; set; }
        public string STRUCTURE_INFO { get; set; }
        public int? HIT { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string OPERATOR { get; set; }
        public string VERSION { get; set; }
    }
}
