using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public partial class VM_PF_MENU
    {
        [Key, Display(Name = "编码")]
        public string GID { get; set; }
        [Required(ErrorMessage = "{0}为必填字段"), AllowModify, Display(Name = "标题")]
        public string TITLE { get; set; }

        [Required(ErrorMessage = "{0}为必填字段"), AllowModify, Display(Name = "类型")]
        public string TYPE { get; set; }

        [AllowModify, Display(Name = "上级")]
        public string SUPER { get; set; }

        [AllowModify, Display(Name = "图标")]
        public string ICON { get; set; }

        [AllowModify, Display(Name = "排序")]
        public int ORDERD { get; set; }

        [AllowModify, Display(Name = "关联地址")]
        public string URL { get; set; }

        [Display(Name = "权限字段")]
        public string PERMISSION_CODE { get; set; }
        [Display(Name = "创建日期")]
        public DateTime CREATE_DATE { get; set; }
        [Display(Name = "修改日期")]
        public DateTime MODIFY_DATE { get; set; }
        [Display(Name = "创建人")]
        public string OPERATOR { get; set; }
    }
}
