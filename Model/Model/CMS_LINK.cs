using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class CMS_LINK
    {
        public string LINKID { get; set; }
        public string LINKTEXT { get; set; }
        public int LINKXH { get; set; }
        public bool ISPUB { get; set; }
        public string LINKURL { get; set; }
        public string INDEXPIC { get; set; }
        public string PRECSS { get; set; }
        public string LINKTYPE { get; set; }
        public bool IS_DELETE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public string BZ { get; set; }
    }
}
