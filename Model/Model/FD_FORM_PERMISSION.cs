using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class FD_FORM_PERMISSION
    {
        public string GID { get; set; }
        public string TYPE { get; set; }
        public string FORM_KEY { get; set; }
        public string FORM_NAME { get; set; }
        public string ROLE_CODE { get; set; }
        public string ROLE_NAME { get; set; }
        public string ORGPATH { get; set; }
        public string ORGNAME { get; set; }
        public string FORM_KEY_FIELD { get; set; }
        public string BZ1 { get; set; }
        public string BZ2 { get; set; }
        public string OPERATOR { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
