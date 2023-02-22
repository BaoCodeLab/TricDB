using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class PF_USER_ORG
    {
        public string GID { get; set; }
        public string USER_GID { get; set; }
        public string USER_NAME { get; set; }
        public string ORG_GID { get; set; }
        public string ORG_NAME { get; set; }
        public string ORG_PATH { get; set; }
        public string BZ1 { get; set; }
        public string BZ2 { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
