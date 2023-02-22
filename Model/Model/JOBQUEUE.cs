using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class JOBQUEUE
    {
        public int ID { get; set; }
        public int JOBID { get; set; }
        public string QUEUE { get; set; }
        public DateTime? FETCHEDAT { get; set; }
        public string FETCHTOKEN { get; set; }
    }
}
