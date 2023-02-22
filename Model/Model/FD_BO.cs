using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class FD_BO
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string CODE { get; set; }
        public string PK { get; set; }
        public string BOTYPE { get; set; }
        public string STATUS { get; set; }
        public string DATAFORMAT { get; set; }
        public string PARENTID { get; set; }
        public string TYPEID { get; set; }
        public string OPTIONS { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
