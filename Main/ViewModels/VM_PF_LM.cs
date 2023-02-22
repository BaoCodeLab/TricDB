using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_PF_LM:BaseViewModel
    {
        public VM_PF_LM(Controller controller) : base(controller)
        {
        }
        public VM_PF_LM() { }
        [Key]
        [Required(ErrorMessage = "{0}不允许为空")]
        [Display(Name = "标识符", AutoGenerateField = false)]
        public string GID { get; set; }

        [Display(Name = "栏目名称")]
        [enableSort(Sort = "true")]
        [MaxLength(50, ErrorMessage = "栏目名至多为50")]
        [AllowModify]
        [enableSearch]
        public string NAME { get; set; }

        [Display(Name = "创建时间")]
        [enableSort(Sort = "true")]
        [AllowModify]
        [enableSearch]
        public string CREATE_DATE { get; set; }

        [Display(Name = "修改时间")]
        [enableSort(Sort = "true")]
        [AllowModify]
        [enableSearch]
        public string MODIFY_DATE { get; set; }
    }
}
