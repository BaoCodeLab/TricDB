using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public partial class VM_PF_ORG
    {
        public string GID { get; set; }
        [Display(Name = "机构名称"), Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableSort, AllowModify]
        public string TITLE { get; set; }
        [Display(Name = "机构别名"), Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableSort, AllowModify]
        public string CODE { get; set; }
        [Display(Name = "机构类型"), UIHint("Select"), Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableSort, AllowModify]
        public string TYPE { get; set; }
        [Display(Name = "上级部门"), Required(ErrorMessage = "{0}为必填字段"), enableSearch, AllowModify]
        public string SUPER { get; set; }
        [Display(Name = "排序值"), Required(ErrorMessage = "{0}为必填字段"), enableSort, AllowModify]
        public double ORDER { get; set; }
        [Display(Name = "深度", AutoGenerateField = false)]
        public int DEPTH { get; set; }
        [Display(Name = "路径", AutoGenerateField = false),AllowModify]
        public string PATH { get; set; }
        [Display(Name = "备注1", AutoGenerateField = false), AllowModify]
        public string BZ1 { get; set; }
        [Display(Name = "备注2", AutoGenerateField = false), AllowModify]
        public string BZ2 { get; set; }
        [Display(Name = "备注3", AutoGenerateField = false), AllowModify]
        public string BZ3 { get; set; }
        [Display(Name = "创建日期", AutoGenerateField = false)]
        public DateTime CREATE_DATE { get; set; }
        [Display(Name = "上次修改", AutoGenerateField = false), AllowModify]
        public DateTime MODIFY_DATE { get; set; }
        [Display(Name = "维护人", AutoGenerateField = false)]
        public string OPERATOR { get; set; }
        [Display(Name = "删除标记", AutoGenerateField = false)]
        public bool IS_DELETE { get; set; }
        public List<VM_PF_ORG> children { get; set; }
        [Display(Name = "是否展开", AutoGenerateField = false), JsonProperty(PropertyName = "spread")]
        public bool SPREAD { get; set; }
        public string title { get; set; }
        public string id { get; set; }
    }
}
