using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class BUS_TARGET_SAMPLE_RELATION
    {
        public string TARGET_SAMPLE_RELATE_ID { get; set; }
        public string TARGETID { get; set; }
        public string SAMPLE_ID { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string OPERATOR { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
    }
}
