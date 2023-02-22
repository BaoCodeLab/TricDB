using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class AGGREGATEDCOUNTER
    {
        public int ID { get; set; }
        public string KEY { get; set; }
        public int VALUE { get; set; }
        public DateTime? EXPIREAT { get; set; }
    }
}
