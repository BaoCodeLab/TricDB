using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class PF_USER
    {
        public string GID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
        public string RYBM { get; set; }
        public string YHZT { get; set; }
        public string SJHM { get; set; }
        public string XMBM { get; set; }
        public string FISRTNAME { get; set; }
        public string LASTNAME { get; set; }
    }
}
