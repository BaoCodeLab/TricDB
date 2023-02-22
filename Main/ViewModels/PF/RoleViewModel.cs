using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;
using Main.ViewModels;

namespace Main.PF.ViewModels
{
    public class RoleViewModel : BaseViewModel
    {
        public RoleViewModel(Controller controller) : base(controller)
        {
        }

        public RoleViewModel() { }

        [Display(Name = "GID",AutoGenerateField = false), Key]
        [Editable(false)]
        public string GID { get; set; }


        [Required(ErrorMessage ="{0}为必填字段")]
        [Editable(false)]
        [Display(Name = "角色编码")]
        [enableSort(Sort="true")]
        [enableSearch]
        public string CODE { get; set; }



        [Required(ErrorMessage = "{0}为必填字段")]
        [AllowModify]
        [Display(Name = "角色名称")]
        [enableSort(Sort = "true")]
        [enableSearch]
        public string NAME { get; set; }


        
        [Display(Name = "创建时间")]
        [enableSort(Sort = "true")]
        public string CREATE_DATE { get; set; }



        [Display(Name = "修改时间")]
        [enableSort(Sort = "true")]
        public string MODIFY_DATE { get; set; }



        [Display(Name = "操作人")]
        public string OPERATOR { get; set; }



    }
}
