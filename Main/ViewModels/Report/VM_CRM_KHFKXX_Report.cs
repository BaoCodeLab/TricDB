using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public partial class VM_CRM_KHFKXX_Report
    {
        [Display(Name = "客户编码")]
        [enableSort]
        [enableSearch]
        public string KHBM { get; set; }


        [Display(Name = "客户名称")]
        [enableSort]
        [enableSearch]
        public string KHMC { get; set; }



        [Display(Name = "投诉类型")]
        [enableSort]
        [enableSearch]
        [UIHint("Select", "TSLX")]
        public string TSLX { get; set; }



        [Display(Name = "负责部门")]
        [enableSort]
        [enableSearch]
        [UIHint("Select", "ZZBM")]
        public string FZBM { get; set; }



        [Display(Name = "发起人编码")]
        public string FQRYBM { get; set; }


        [Display(Name = "发起人")]
        [enableSort]
        [enableSearch]
        public string FQRYXM { get; set; }
        


        [Display(Name = "处理人")]
        [enableSort]
        [enableSearch]
        public string CLRYXM { get; set; }


        [Display(Name = "紧急程度")]
        [enableSort]
        [enableSearch]
        public string JJCD { get; set; }
        


        [Display(Name = "反馈状态")]
        [enableSort]
        [enableSearch]
        [UIHint("Select", "FKZT")]
        public string FKZT { get; set; }
        


        [Display(Name = "服务评分")]
        [enableSort]
        [enableSearch]
        public string FWPF { get; set; }

       


        [Display(Name = "处理日期")]
        [enableSort]
        [enableSearch]
        [UIHint("Date","true")]
        public string CLRQ { get; set; }



        [Display(Name = "截止日期")]
        [enableSort]
        [enableSearch]
        [UIHint("Date", "true")]
        public string JZRQ { get; set; }
    }
}
