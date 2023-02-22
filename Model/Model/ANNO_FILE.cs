using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model
{
    public partial class ANNO_FILE
    {
        public string FILE_ID { get; set; }
        public string ANNO_SAMPLEID { get; set; }
        public string FILE_TYPE { get; set; }
        public string ANALYSIS_TYPE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string OPERATOR { get; set; }
        public string VERSION { get; set; }



    }
}
