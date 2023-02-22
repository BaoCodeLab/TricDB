using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;
using Main.ViewModels;
using System;

namespace Main.PF.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        public UserViewModel(Controller controller) : base(controller)
        {
        }

        public UserViewModel() { }


        [Display(Name = "GID", AutoGenerateField = false)]
        [Editable(false)]
        public string GID { get; set; }


        [Required(ErrorMessage ="{0}为必填字段")]
        [Display(Name = "用户名")]
        [enableSort(Sort="true")]
        [enableSearch]
        public string USERNAME { get; set; }
        [Display(Name ="姓名",AutoGenerateField =false),enableSort(Sort ="true"),enableSearch]
        public string NAME
        {
            get;set;
        }
        [Display(Name = "档案编码", AutoGenerateField = false), enableSort(Sort = "true"), enableSearch]
        public string RYBM
        {
            get; set;
        }
        [Display(Name = "用户别名"), enableSort(Sort = "true"), enableSearch]
        public string XMBM { get; set; }
        [Required(ErrorMessage = "{0}为必填字段")]
        [AllowModify]
        [Display(Name = "密码",AutoGenerateField =false)]
        [MinLength(6,ErrorMessage ="密码至少为6位")]
        public string PASSWORD { get; set; }
        
        [Display(Name = "创建时间")]
        [Editable(false)]
        [enableSort(Sort = "true")]
        [Verify("number")]
        public string CREATE_DATE { get; set; }

        [Required(ErrorMessage = "{0}为必填字段")]
        [AllowModify]
        [Display(Name = "修改时间")]
        [enableSort(Sort = "true")]
        [Verify("required")]
        public string MODIFY_DATE { get; set; }

        [Display(Name = "操作人")]
        public string OPERATOR { get; set; }
        [Display(Name = "手机号码"),enableSort(Sort = "true"),enableSearch]
        public string SJHM { get; set; }
        [Display(Name = "用户状态"), enableSort(Sort = "true"), enableSearch]
        public string YHZT { get; set; }
    }
}
