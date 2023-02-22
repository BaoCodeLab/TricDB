using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class PF_PROFILE
    {
        public string GID { get; set; }
        public string CODE { get; set; }
        public string ZW { get; set; }
        public string NAME { get; set; }
        public string SEX { get; set; }
        public int AGE { get; set; }
        public string PHONE { get; set; }
        public string MAIL { get; set; }
        public string BZ { get; set; }
        public string DLZH { get; set; }
        public string TXDZ { get; set; }
        public string GRAH { get; set; }
        public DateTime SR { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
