using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_FD_FORM
    {
        [Key,Display(Name ="编号",AutoGenerateField =false)]
        public string id { get; set; }
        [Display(Name = "表单名称"),enableSearch]
        public string name { get; set; }
        [Display(Name = "表单简介"), enableSearch]
        public string desc { get; set; }
        [Display(Name = "表单标识",AutoGenerateField =false)]
        public string key { get; set; }
        [Display(Name = "创建时间", AutoGenerateField =false)]
        public string CREATE_DATE { get; set; }
        [Display(Name = "上次修改", AutoGenerateField = false)]
        public string MODIFY_DATE { get; set; }
        [Display(Name = "创建人", AutoGenerateField = false)]
        public string OPERATOR { get; set; }
    }
}
