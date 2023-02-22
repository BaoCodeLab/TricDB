using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class CMS_CHANNEL
    {
        public CMS_CHANNEL()
        {
            CMS_CHANNELNavigation = new HashSet<CMS_ARTICLE>();
        }

        public string CHANNELID { get; set; }
        public string PRECHANNELNAME { get; set; }
        public string CHANNELNAME { get; set; }
        public string CHANNELREDIRECT { get; set; }
        public bool ISPUB { get; set; }
        public bool ISMENU { get; set; }
        public bool IS_DELETE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public string BZ { get; set; }
        public string PARENTCHANNELID { get; set; }
        public int CHANNELXH { get; set; }

        public ICollection<CMS_ARTICLE> CMS_CHANNELNavigation { get; set; }
    }
}
