using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public partial class VM_PF_PRINT_TMPL
    {
        public string GID { get; set; }
        [Display(Name = "模板文件",AutoGenerateField =false)]
        public string FGID { get; set; }
        [Display(Name ="模板名称"), Required(ErrorMessage = "{0}为必填字段"), enableSearch,enableSort,AllowModify]
        public string TITLE { get; set; }
        [Display(Name = "模板代码"), Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableSort, AllowModify]
        public string CODE { get; set; }
        [Display(Name = "上级分类"), Required(ErrorMessage = "{0}为必填字段"), enableSearch, AllowModify]
        public string SUPER { get; set; }
        [Display(Name = "排序值"), Required(ErrorMessage = "{0}为必填字段"), enableSort, AllowModify]
        public double ORDER { get; set; }
        [Display(Name = "深度",AutoGenerateField =false)]
        public int DEPTH { get; set; }
        [Display(Name = "备注1", AutoGenerateField = false), AllowModify]
        public string BZ1 { get; set; }
        [Display(Name = "备注2", AutoGenerateField = false), AllowModify]
        public string BZ2 { get; set; }
        [Display(Name = "创建日期", AutoGenerateField = false)]
        public DateTime CREATE_DATE { get; set; }
        [Display(Name = "上次修改", AutoGenerateField = false), AllowModify]
        public DateTime MODIFY_DATE { get; set; }
        [Display(Name = "维护人", AutoGenerateField = false)]
        public string OPERATOR { get; set; }
        [Display(Name = "删除标记", AutoGenerateField = false)]
        public bool IS_DELETE { get; set; }
        public List<VM_PF_PRINT_TMPL> children { get; set; }
        [Display(Name = "是否展开", AutoGenerateField = false)]
        public bool spread { get; set; }
        public string id { get; set; }
        public string title { get; set; }
    }
}
