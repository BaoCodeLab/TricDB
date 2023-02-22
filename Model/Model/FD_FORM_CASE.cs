using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class FD_FORM_CASE
    {
        public string ID { get; set; }
        public string FORM_ID { get; set; }
        public string FORM_KEY { get; set; }
        public string FORM_NAME { get; set; }
        public string FORM_DATA { get; set; }
        public string BO_ID { get; set; }
        public string BODATA { get; set; }
        public string STATE { get; set; }
        public string PERMISSIONS { get; set; }
        public string ORGID { get; set; }
        public string ORGNAME { get; set; }
        public string ORGPATH { get; set; }
        public string VERSION { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
