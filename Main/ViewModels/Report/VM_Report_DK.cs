using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    /// <summary>
    /// 打卡记录表
    /// </summary>
    public partial class VM_Report_DK : BaseViewModel
    {
        [Display(Name = "序号"),thWidth("80")]
        public string SN { get; set; }
        
        [Display(Name = "打卡人"), thWidth("120")]
        public string NAME { get; set; }

        [Display(Name = "打卡时间"),enableSearch,UIHint("Date","true"), thWidth("150")]
        public string DKSJ { get; set; }
        
        [Display(Name = "客户名称"), thWidth("200")]
        public string KHMC { get; set; }

        [Display(Name = "打卡事由"), thWidth("200")]
        public string DKSY { get; set; }

        [Display(Name = "省份"), thWidth("100")]
        public string SF { get; set; }

        [Display(Name = "详细地址"),enableSearch, thWidth("500")]
        public string DKDD { get; set; }

        [Display(Name = "备注"), thWidth("200")]
        public string BZ { get; set; }
    }
}
