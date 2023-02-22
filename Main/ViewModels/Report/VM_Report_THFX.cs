using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    /// <summary>
    /// 采购产品合格率及PPM报表
    /// </summary>
    public partial class VM_Report_THFX
    {
        [Key, Display(Name = "序号")]
        public string SN { get; set; }
        [Display(Name = "创建人"), thWidth("130")]
        public string OPERATOR { get; set; }
        [Display(Name = "订单类型") , thMinWidth("100")]
        public string DDLX { get; set; }
        [Display(Name = "客户编码"), thWidth("130")]
        public string KHBM { get; set; }
        [Display(Name = "客户名称"),enableSearch, thWidth("200")]
        public string KHMC { get; set; }
        [Display(Name = "订单号"), thWidth("110")]
        public string DDH { get; set; }
        [Display(Name = "物料编码"), thWidth("200")]
        public string WLBM { get; set; }
        [Display(Name = "物料描述"), enableSearch, thWidth("150")]
        public string WLMC { get; set; }
        [Display(Name = "业务员创建数量"), totalRow, thWidth("150")]
        public string XQSL { get; set; }
        [Display(Name = "下达SAP数量"), totalRow, thWidth("150")]
        public string SJSL { get; set; }
        [Display(Name = "SAP下达金额汇总"), totalRow, thWidth("150")]
        public string JE { get; set; }
        [Display(Name = "退货原因"), enableSearch,thWidth("110"),UIHint("Select", "退货订单原因")]
        public string THYY { get; set; }
        [Display(Name = "日期"), enableSearch,UIHint("Date","true"), thWidth("160")]
        public string MODIFY_DATE { get; set; }
    }
}
