using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_FD_FORM_PERMISSION
    {
        [Key,Display(Name ="记录编号",AutoGenerateField =false)]
        public string GID { get; set; }
        [Display(Name = "表单标识", AutoGenerateField = false),AllowModify, Required(ErrorMessage = "{0}为必填字段")]
        public string FORM_KEY { get; set; }
        [Display(Name = "表单名称"),enableSort,enableSearch,AllowModify, Required(ErrorMessage = "{0}为必填字段")]
        public string FORM_NAME { get; set; }
        [Display(Name = "授权类型"), enableSort, enableSearch, AllowModify, Required(ErrorMessage = "{0}为必填字段")]
        public string TYPE { get; set; }
        [Display(Name = "角色标识", AutoGenerateField = false), AllowModify,Required]
        public string ROLE_CODE { get; set; }
        [Display(Name = "角色名称"), enableSort, enableSearch,AllowModify, Required(ErrorMessage = "{0}为必填字段")]
        public string ROLE_NAME { get; set; }
        [Display(Name = "授权组织", AutoGenerateField = false), AllowModify]
        public string ORGPATH { get; set; }
        [Display(Name = "授权组织", AutoGenerateField = false), AllowModify]
        public string ORGNAME { get; set; }
        [Display(Name = "授权字段", AutoGenerateField = false), AllowModify, Required(ErrorMessage = "{0}为必填字段")]
        public string FORM_KEY_FIELD { get; set; }
        [Display(Name = "备注1", AutoGenerateField = false), AllowModify]
        public string BZ1 { get; set; }
        [Display(Name = "备注2", AutoGenerateField = false), AllowModify]
        public string BZ2 { get; set; }
        [Display(Name = "操作人"), enableSort, enableSearch, AllowModify]
        public string OPERATOR { get; set; }
        [Display(Name = "创建时间", AutoGenerateField = false)]
        public string CREATE_DATE { get; set; }
        [Display(Name = "上次修改", AutoGenerateField = false), AllowModify]
        public string MODIFY_DATE { get; set; }
    }
}
