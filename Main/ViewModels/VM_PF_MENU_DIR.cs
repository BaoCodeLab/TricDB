using System.Collections.Generic;

namespace Main.ViewModels
{
    public partial class VM_PF_MENU_DIR
    {

        public string GID { get; set; }

        public string id { get; set; }

        public string title { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public string icon { get; set; }

        public string href { get; set; }

        public bool spread { get; set; }

        public List<VM_PF_MENU_DIR> children { get; set; }
    }
}
