using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class CMS_FILELIST
    {
        public string FID { get; set; }
        public string FNAME { get; set; }
        public string FPATH { get; set; }
        public int FSIZE { get; set; }
        public string FCONTENTTYPE { get; set; }
        public string OPERATER { get; set; }
        public DateTime CREATE_DATE { get; set; }
    }
}
