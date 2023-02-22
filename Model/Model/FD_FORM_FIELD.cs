using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class FD_FORM_FIELD
    {
        public string ID { get; set; }
        public string FORM_ID { get; set; }
        public string NAME { get; set; }
        public string LABEL { get; set; }
        public string SHOWNAME { get; set; }
        public string DESC { get; set; }
        public string FIELD_TYPE { get; set; }
        public string FIELD_OPTIONS { get; set; }
        public string FIELD_NAME { get; set; }
        public string DATATYPE { get; set; }
        public string VERSION { get; set; }
        public int ORDER { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
