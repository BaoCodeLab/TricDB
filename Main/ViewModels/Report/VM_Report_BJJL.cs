using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    /// <summary>
    /// 询价报表
    /// </summary>
    public partial class VM_Report_BJJL : BaseViewModel
    {
        [Display(Name = "序号")]
        public string SN { get; set; }
        
        [Display(Name = "客户名称")]
        [enableSearch]
        public string KHMC { get; set; }

        [Display(Name = "客户编码")]
        [enableSearch]
        public string KHBM { get; set; }
        
        [Display(Name = "物料编号"), enableSearch]
        public string WLBM { get; set; }

        [Display(Name = "物料描述"), enableSearch]
        public string WLMC { get; set; }

        [Display(Name = "新产品型号"), enableSearch]
        public string CPXH { get; set; }

        [Display(Name = "使用车型")]
        public string SYCX { get; set; }

        [Display(Name = "使用部位")]
        public string SYBW { get; set; }

        [Display(Name = "报价日期")]
        [enableSort]
        public string BJRQ { get; set; }

        [Display(Name = "月需求总量")]
        public string YXQZL { get; set; }
        
        [Display(Name = "最终报价")]
        [AllowModify]
        public string JYBJ { get; set; }
        
        [Display(Name = "交货方式", AutoGenerateField = false)]
        [AllowModify]
        public string JHFS { get; set; }

        [Display(Name = "货币")]
        [AllowModify]
        public string DW { get; set; }
    }
}
