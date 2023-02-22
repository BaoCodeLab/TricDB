using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class PF_MSG_REPLY
    {
        public string GID { get; set; }
        public string MSG_GID { get; set; }
        public string USERNAME { get; set; }
        public string CONTENT { get; set; }
        public string URL { get; set; }
        public bool IS_READ { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
