using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.ViewModels
{
    public class VM_CHANNEL_DIR
    {
        public VM_CHANNEL_DIR()
        {

        }
        public string id { get; set; }
        public string parentid { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public string href { get; set; }
        public string redirect { get; set; }
        public bool spread { get; set; }
        public List<VM_CHANNEL_DIR> children { get; set; }
    }
}
