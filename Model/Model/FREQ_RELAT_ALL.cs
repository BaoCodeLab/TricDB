using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model
{
    public partial class FREQ_RELAT_ALL
    {

        public string FREQ_RELAT_ID { get; set; }
        public string FREQ_ID { get; set; }
        public string RELATIONID { get; set; }
        public string TARGETID { get; set; }
        public string DISEASEID { get; set; }
        public string DRUGID { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string OPERATOR { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
    }
}