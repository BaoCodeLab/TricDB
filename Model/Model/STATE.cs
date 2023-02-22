using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class STATE
    {
        public int ID { get; set; }
        public int JOBID { get; set; }
        public string NAME { get; set; }
        public string REASON { get; set; }
        public DateTime CREATEDAT { get; set; }
        public string DATA { get; set; }

        public JOB JOB { get; set; }
    }
}
