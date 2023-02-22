using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_REPORT_KHFK
    {

        [Display(Name = "客户编码"),enableExport,thWidth("120")]
        public string KHBM { get; set; }

        [Display(Name = "客户名称"), enableExport,enableSearch, thWidth("200")]
        public string KHMC { get; set; }
        
        [Display(Name = "投诉类型"), enableExport, enableSearch, thWidth("120"), UIHint("Select", "TSLX")]
        public string TSLX { get; set; }

        [Display(Name = "负责部门"), enableExport, enableSearch, thWidth("120"),UIHint("Select", "ZZBM")]
        public string FZBM { get; set; }
        
        [Display(Name = "发起人姓名"), enableExport, enableSearch, thWidth("120")]
        public string FQRYXM { get; set; }

        [Display(Name = "处理人姓名"), enableExport, enableSearch, thWidth("120")]
        public string CLRYXM { get; set; }
        
        [Display(Name = "紧急程度"), enableExport, enableSearch, thWidth("120")]
        public string JJCD { get; set; }

        [Display(Name = "当前状态"), enableExport, enableSearch, thWidth("120"), UIHint("Select", "FKZT")]
        public string FKZT { get; set; }

        [Display(Name = "客户意见"), enableExport, thWidth("200")]
        public string FKXX { get; set; }

        [Display(Name = "处理信息"), enableExport, thWidth("200")]
        public string BZ { get; set; }

        [Display(Name = "服务评分"), enableExport, enableSearch, thWidth("120")]
        public string FWPF { get; set; }

        [Display(Name = "处理日期"), enableExport, enableSearch,UIHint("Date","true"), thWidth("140")]
        public string CLRQ { get; set; }

        [Display(Name = "截止日期"), enableExport, enableSearch, UIHint("Date", "true"), thWidth("140")]
        public string JZRQ { get; set; }

        [Display(Name = "创建日期"), enableExport, enableSearch, UIHint("Date", "true"), thWidth("140")]
        public string CREATE_DATE { get; set; }
    }
}
