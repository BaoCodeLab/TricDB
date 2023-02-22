using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class FD_BO_FIELD
    {
        public string ID { get; set; }
        public string BO_ID { get; set; }
        public string NAME { get; set; }
        public string CODE { get; set; }
        public string FIELDNAME { get; set; }
        public string DATATYPE { get; set; }
        public int ATTRLENGTH { get; set; }
        public int PRECISION { get; set; }
        public string FORMAT { get; set; }
        public string DESC { get; set; }
        public string DEFVALUE { get; set; }
        public string ISNULL { get; set; }
        public string BZ { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public DateTime MODIFY_DATE { get; set; }
        public string OPERATOR { get; set; }
        public bool IS_DELETE { get; set; }
    }
}
