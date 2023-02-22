using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class FD_FORM_DATA
    {
        public string ID { get; set; }
        public string FORM_ID { get; set; }
        public string FORM_CASE_ID { get; set; }
        public string KEY { get; set; }
        public string VALUE { get; set; }
        public string BZ { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
