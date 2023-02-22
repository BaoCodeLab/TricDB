using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class PF_ZDYDA
    {
        public string GID { get; set; }
        public string DAY { get; set; }
        public string TYPE { get; set; }
        public string DKEY { get; set; }
        public string VALUE { get; set; }
        public string XGDA1 { get; set; }
        public string XGDA2 { get; set; }
        public string XGDA3 { get; set; }
        public string BZ1 { get; set; }
        public string BZ2 { get; set; }
        public string BZ3 { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
