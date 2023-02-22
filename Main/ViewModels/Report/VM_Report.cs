using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_Report
    {
        [Display(Name = "默认名称"), enableSearch, enableSort, UIHint("Date", "false")]
        public string date { get; set; }
        [Display(Name = "默认名称"), enableSearch, enableSort, UIHint("Select", "PF_State中的类型名")]
        public string select { get; set; }
        [Display(Name = "默认名称"), enableSearch, enableSort, UIHint("Radio", "PF_State中的类型名")]
        public string radio { get; set; }
        [Display(Name = "默认名称"), enableSearch, enableSort, UIHint("Check", "PF_State中的类型名")]
        public string check { get; set; }
        [Display(Name = "默认名称"), enableSearch, enableSort, UIHint("Org", "PF_State中的类型名")]
        public string org { get; set; }
        [Display(Name ="默认名称"), enableSearch, enableSort,UIHint("Date")]
        public string text { get; set; }
    }
}
