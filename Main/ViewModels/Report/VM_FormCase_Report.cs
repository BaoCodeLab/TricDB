using System.ComponentModel.DataAnnotations;

namespace Main.ViewModels
{
    /// <summary>
    /// 表单设计器表单报表-检索器
    /// </summary>
    public class VM_FormCase_Report
    {
        [Display(Name = "字段"), UIHint("Select")]
        public string KEY { get; set; }

        [Display(Name = "关键字")]
        public string VALUE { get; set; }

        [Display(Name = "组织")]
        public string ORGPATH { get; set; }
        [Display(Name = "组织")]
        public string ORGNAME { get; set; }

        [Display(Name = "账号")]
        public string USERID { get; set; }

        [Display(Name = "姓名")]
        public string USERNAME { get; set; }

        [Display(Name = "角色"), UIHint("Select")]
        public string ROLE { get; set; }

        [Display(Name = "日期"),UIHint("Date","true")]
        public string CREATE_DATE { get; set; }
      
    }
}
