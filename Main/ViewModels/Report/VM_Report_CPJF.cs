using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    /// <summary>
    /// 产品交付情况报表
    /// </summary>
    public partial class VM_Report_CPJF
    {
        [Display(Name = "序号")]
        public string SN { get; set; }
        [Display(Name = "订单类型"), thWidth("100")]
        public string SQLX { get; set; }
        [Display(Name = "客户编码") , thMinWidth("100")]
        public string KHBM { get; set; }
        [Display(Name = "客户名称"), enableSearch, thWidth("200")]
        public string KHMC { get; set; }
        [Display(Name = "物料编码"), thWidth("200")]
        public string WLBM { get; set; }
        [Display(Name = "物料描述"),enableSearch, thWidth("200")]
        public string WLMC { get; set; }
        [Display(Name = "申请编号"), thWidth("150")]
        public string SQBH { get; set; }
        [Display(Name = "申请时间"), enableSearch,UIHint("Date","true"),thWidth("200")]
        public string SQSJ { get; set; }
        [Display(Name = "申请数量"),totalRow, thWidth("110")]
        public string SQSL { get; set; }
        [Display(Name = "申请人"), enableSearch,thWidth("110")]
        public string SQR { get; set; }
        [Display(Name = "要求交货时间"), thWidth("110")]
        public string JHRQ { get; set; }
        [Display(Name = "下达时间"), thWidth("200")]
        public string XDSJ { get; set; }
        [Display(Name = "下达数量"), totalRow, thWidth("110")]
        public string XDSL { get; set; }
        [Display(Name = "下达操作人"), thWidth("110")]
        public string XDCZR { get; set; }
        [Display(Name = "销售凭证号"), thWidth("110")]
        public string DDH { get; set; }
        [Display(Name = "备注"), thWidth("100")]
        public string BZ { get; set; }
    }
}
