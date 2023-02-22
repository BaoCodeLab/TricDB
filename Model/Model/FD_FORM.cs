using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class FD_FORM
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string DESC { get; set; }
        public string KEY { get; set; }
        public string TYPEID { get; set; }
        public string MODE { get; set; }
        public string TYPENAME { get; set; }
        public string BUSID { get; set; }
        public string CODE { get; set; }
        public string ATTRS { get; set; }
        public string PERMISSIONS { get; set; }
        public string VERSION { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
