using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class PF_PERMISSION
    {
        public string GID { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
