using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class PF_ORG
    {
        public string GID { get; set; }
        public string TITLE { get; set; }
        public string CODE { get; set; }
        public string TYPE { get; set; }
        public string SUPER { get; set; }
        public double ORDER { get; set; }
        public int DEPTH { get; set; }
        public string PATH { get; set; }
        public string BZ1 { get; set; }
        public string BZ2 { get; set; }
        public string BZ3 { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
