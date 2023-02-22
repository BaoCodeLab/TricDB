using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class PF_FILE
    {
        public string GID { get; set; }
        public string WGID { get; set; }
        public string FILENAME { get; set; }
        public string FILEURI { get; set; }
        public int PX { get; set; }
        public string TYPE { get; set; }
        public string MD5 { get; set; }
        public string IP { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
        public string LX { get; set; }
        public string BZ { get; set; }
    }
}
