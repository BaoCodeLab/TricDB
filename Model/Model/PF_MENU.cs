using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class PF_MENU
    {
        public string GID { get; set; }
        public string TITLE { get; set; }
        public string TYPE { get; set; }
        public string SUPER { get; set; }
        public string ICON { get; set; }
        public double ORDERD { get; set; }
        public string URL { get; set; }
        public string PERMISSION_CODE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
