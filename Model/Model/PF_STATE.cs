﻿using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class PF_STATE
    {
        public string GID { get; set; }
        public string TYPE { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public int ORDERS { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
