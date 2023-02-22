using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class PF_NEW
    {
        public string GID { get; set; }
        public string TITLE { get; set; }
        public string CONTENT { get; set; }
        public string AUTHOR { get; set; }
        public DateTime PUBLISH_TIME { get; set; }
        public int VI_TIME { get; set; }
        public string STATE { get; set; }
        public string LM_GID { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
