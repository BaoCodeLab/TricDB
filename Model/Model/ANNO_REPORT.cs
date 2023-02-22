using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model
{
    public partial class ANNO_REPORT
    {
        public string REPORT_ID { get; set; }
        public string ANNO_SAMPLEID { get; set; }
        public string REPORT_NAME{ get; set; }
        public string REPORT_DESCRIPTION { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string OPERATOR { get; set; }
        public string VERSION { get; set; }

    }
}
