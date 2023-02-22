using System;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_FD_FORM_CASE
    {
        [Key,Display(Name ="编号",AutoGenerateField =false)]
        public string ID { get; set; }
        [Display(Name = "表单模板编号",AutoGenerateField =false)]
        public string FORM_ID { get; set; }
        [Display(Name = "表单模板关键字", AutoGenerateField = false)]
        public string FORM_KEY { get; set; }
        [Display(Name = "名称",AutoGenerateField =false)]
        public string FORM_NAME { get; set; }
        [Display(Name = "状态", AutoGenerateField = false)]
        public string STATE { get; set; }
        [Display(Name = "部门"),enableSearch,enableSort,thMinWidth("150")]
        public string ORGNAME { get; set; }
        [Display(Name = "创建时间"), enableSearch, enableSort, thMinWidth("160")]
        public string CREATE_DATE { get; set; }
        [Display(Name = "上次修改"), thMinWidth("160")]
        public DateTime MODIFY_DATE { get; set; }
        [Display(Name = "创建人"),enableSearch,enableSort, thMinWidth("100")]
        public string OPERATOR { get; set; }
    }
}
