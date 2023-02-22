using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.ViewModels
{
    public class VM_Article_List
    {
        public string ARTICLEID { get; set; }
        public string ARTICLETITLE { get; set; }
        public string HREF { get; set; }
        public string ARTICLEKEYWORDS { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime ARTICLETIME { get; set; }
        public int ARTICLEHIT { get; set; }
        public string CHANNELID { get; set; }
        public string ARTICLEINDEXPIC { get; set; }
    }
}
