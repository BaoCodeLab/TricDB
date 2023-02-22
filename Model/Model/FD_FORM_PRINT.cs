using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class FD_FORM_PRINT
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string FORMKEY { get; set; }
        public string DESC { get; set; }
        public string CONTENT { get; set; }
        public string HTML { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
