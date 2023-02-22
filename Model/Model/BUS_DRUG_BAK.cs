using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class BUS_DRUG_BAK
    {
        public string DID { get; set; }
        public string DRUGNAME { get; set; }
        public string DRUGDATE { get; set; }
        public string DRUGBRAND { get; set; }
        public string DISEASE { get; set; }
        public string DISEASEDEVELOPMENT { get; set; }
        public string MUTANTGENE { get; set; }
        public string CLINICALTRNUM { get; set; }
        public string DOSE { get; set; }
        public string SOURCEURL { get; set; }
        public string PRINCIPAL { get; set; }
        public int? HIT { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool? IS_DELETE { get; set; }
        public bool? IS_PUB { get; set; }
        public string DRUGVERSION { get; set; }
        public string DRUGCODE { get; set; }
    }
}
