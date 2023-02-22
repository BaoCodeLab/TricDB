using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;
using System;

namespace Main.ViewModels
{
    public partial class VM_PF_STATE:BaseViewModel
    {
        public VM_PF_STATE(Controller controller) : base(controller)
        {
        }
        public VM_PF_STATE() { }

        [Key]
        [Display(Name ="数据编码", AutoGenerateField =false)]
        public string GID { get; set; }

        [Display(Name = "状态类型", AutoGenerateField = false)]
        [enableSearch,enableSort,AllowModify,Required]
        [StringLength(10,ErrorMessage ="{0}长度在2~10之间",MinimumLength =2)]
        public string TYPE { get; set; }

        [Display(Name = "状态值")]
        [enableSearch,Required,AllowModify]
        public string CODE { get; set; }


        [Display(Name = "名称")]
        [Required,AllowModify]
        public string NAME { get; set; }
        [Display(Name = "排序值")]
        [Required, AllowModify]
        public int ORDERS { get; set; }

        [Display(Name = "创建日期")]
        [enableSort]
        public DateTime CREATE_DATE { get; set; }


        [Display(Name = "修改日期")]
        public DateTime MODIFY_DATE { get; set; }


        [Display(Name = "创建人")]
        public string OPERATOR { get; set; }
    }
}
