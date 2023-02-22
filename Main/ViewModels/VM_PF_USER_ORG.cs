using System;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_PF_USER_ORG
    {
        [Key, Display(Name = "主键编码",AutoGenerateField =false)]
        public string GID { get; set; }

        [Display(Name = "用户GID", AutoGenerateField = false)]
        public string USER_GID { get; set; }
        [Display(Name = "用户账号"),enableSearch,enableSort]

        public string SJHM { get; set; }
        [Display(Name = "手机号码"), enableSearch, enableSort]
        public string USER_NAME { get; set; }
        [Display(Name = "用户姓名"), enableSearch, enableSort]
        public string USER_REALNAME { get; set; }
        [Display(Name = "机构GID", AutoGenerateField = false)]
        public string ORG_GID { get; set; }
        [Display(Name = "机构名称")]
        public string ORG_NAME { get; set; }
        [Display(Name = "机构路径")]
        public string ORG_PATH { get; set; }
        [Display(Name = "备注1", AutoGenerateField = false)]
        public string BZ1 { get; set; }
        [Display(Name = "备注2", AutoGenerateField = false)]
        public string BZ2 { get; set; }
        [Display(Name = "创建时间", AutoGenerateField = false)]
        public DateTime CREATE_DATE { get; set; }
        [Display(Name = "修改时间", AutoGenerateField = false)]
        public DateTime MODIFY_DATE { get; set; }
        [Display(Name = "创建人", AutoGenerateField = false)]
        public string OPERATOR { get; set; }
        [Display(Name = "是否删除", AutoGenerateField = false)]
        public bool IS_DELETE { get; set; }
    }
}
