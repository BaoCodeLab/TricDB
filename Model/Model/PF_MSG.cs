using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class PF_MSG
    {
        public string GID { get; set; }
        public string USERNAME { get; set; }
        public string ROLE { get; set; }
        public string TYPE { get; set; }
        public string TITLE { get; set; }
        public string CONTENT { get; set; }
        public string URL { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
