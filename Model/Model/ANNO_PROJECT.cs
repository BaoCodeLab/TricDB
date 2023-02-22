using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Model
{
    public partial class ANNO_PROJECT
    {

        public string PROJECT_ID { get; set; }
        public string USER_ID { get; set; }
        public int ORDER { get; set; }
        public string PROJECT_NAME { get; set; }
        public string PROJECT_DESCRIP { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public bool IS_PUB { get; set; }
        public bool IS_DELETE { get; set; }
        public string OPERATOR { get; set; }
        public string VERSION { get; set; }


    }
}
