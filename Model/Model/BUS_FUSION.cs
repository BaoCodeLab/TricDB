using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class BUS_FUSION
    {
        public string FUSION_ID { get; set; }
        public string FUSIONCODE { get; set; }
        public string TARGET { get; set; }
        public string SAMPLE_ID { get; set; }
        public string FUSION { get; set; }
        public string FRAME { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime? MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string VERSION { get; set; }
    }
}
