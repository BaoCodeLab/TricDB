using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class JOBPARAMETER
    {
        public int ID { get; set; }
        public int JOBID { get; set; }
        public string NAME { get; set; }
        public string VALUE { get; set; }

        public JOB JOB { get; set; }
    }
}
