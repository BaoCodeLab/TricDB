using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class WEIXIN_USER
    {
        public string GID { get; set; }
        public string CITY { get; set; }
        public string PROVINCE { get; set; }
        public string COUNTRY { get; set; }
        public string HEADIMGURL { get; set; }
        public string NICKNAME { get; set; }
        public string OPENID { get; set; }
        public string SEX { get; set; }
        public string UNIONID { get; set; }
        public string BZ1 { get; set; }
        public string BZ2 { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
