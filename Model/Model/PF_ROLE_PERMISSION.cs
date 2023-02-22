using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class PF_ROLE_PERMISSION
    {
        public string GID { get; set; }
        public string PER_GID { get; set; }
        public string ROLE_GID { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
