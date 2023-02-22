using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class PF_REMINDER
    {
        public string GID { get; set; }
        public string USERS { get; set; }
        public string ROLES { get; set; }
        public string STATUS { get; set; }
        public string TITLE { get; set; }
        public string CONTENT { get; set; }
        public string API { get; set; }
        public string RULE { get; set; }
        public DateTime SDATE { get; set; }
        public DateTime EDATE { get; set; }
        public string PARAM1 { get; set; }
        public string PARAM2 { get; set; }
        public string PARAM3 { get; set; }
        public string PARAM4 { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
