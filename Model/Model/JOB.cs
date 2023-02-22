using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class JOB
    {
        public JOB()
        {
            JOB1 = new HashSet<JOBSTATE>();
            JOB2 = new HashSet<STATE>();
            JOBNavigation = new HashSet<JOBPARAMETER>();
        }

        public int ID { get; set; }
        public int? STATEID { get; set; }
        public string STATENAME { get; set; }
        public string INVOCATIONDATA { get; set; }
        public string ARGUMENTS { get; set; }
        public DateTime CREATEDAT { get; set; }
        public DateTime? EXPIREAT { get; set; }

        public ICollection<JOBSTATE> JOB1 { get; set; }
        public ICollection<STATE> JOB2 { get; set; }
        public ICollection<JOBPARAMETER> JOBNavigation { get; set; }
    }
}
