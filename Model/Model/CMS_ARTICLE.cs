using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class CMS_ARTICLE
    {
        public string ARTICLEID { get; set; }
        public string CHANNELID { get; set; }
        public string ARTICLETITLE { get; set; }
        public string ARTICLECONTENT { get; set; }
        public int ARTICLEHIT { get; set; }
        public int ARTICLETOPNUM { get; set; }
        public DateTime ARTICLETIME { get; set; }
        public bool ISPUB { get; set; }
        public string ARTICLEREDIRECT { get; set; }
        public string ARTICLEEDITOR { get; set; }
        public string ARTICLEINDEXPIC { get; set; }
        public string ARTICLEKEYWORDS { get; set; }
        public bool ARTICLEISTITLE { get; set; }
        public bool IS_DELETE { get; set; }
        public string OPERATOR { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string BZ { get; set; }

        public CMS_CHANNEL CMS_CHANNEL { get; set; }
    }
}
