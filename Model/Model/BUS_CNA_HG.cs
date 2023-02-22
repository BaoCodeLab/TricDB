using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class BUS_CNA_HG
    {
        public string CNA_ID { get; set; }
        public string CNACODE { get; set; }
        public string SAMPLE_ID { get; set; }
        public string CHROM { get; set; }
        public string LOCASTART { get; set; }
        public string LOCEND { get; set; }
        public int? NUMMARK { get; set; }
        public double? SEGMEAN { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string VERSION { get; set; }
    }
}
