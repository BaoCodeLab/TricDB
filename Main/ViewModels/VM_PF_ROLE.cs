using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public partial class VM_PF_ROLE
    {
        [Key,Display(Name ="编码",AutoGenerateField =false)]
        public string GID { get; set; }
        [Display(Name ="角色名称"), enableSearch,enableSort]
        public string NAME { get; set; }
        [Display(Name = "角色代码"), enableSearch, enableSort]
        public string CODE { get; set; }
    }
}
