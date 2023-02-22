using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class HASH
    {
        public int ID { get; set; }
        public string KEY { get; set; }
        public string FIELD { get; set; }
        public string VALUE { get; set; }
        public DateTime? EXPIREAT { get; set; }
    }
}
