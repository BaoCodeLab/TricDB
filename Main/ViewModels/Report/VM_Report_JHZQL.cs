using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    /// <summary>
    /// 计划准确率分析报表
    /// </summary>
    public partial class VM_Report_JHZQL
    {
        [Display(Name = "序号")]
        public string SN { get; set; }
        [Display(Name = "客户编码"), thMinWidth("140")]
        public string KHBM { get; set; }
        [Display(Name = "客户名称"), enableSearch, thWidth("200")]
        public string KHMC { get; set; }
        [Display(Name = "物料编码"), thWidth("200")]
        public string WLBM { get; set; }
        [Display(Name = "物料描述"), thWidth("200")]
        public string WLMC { get; set; }
        [Display(Name = "月计划数量"), totalRow, thWidth("130")]
        public string YJHSL { get; set; }
        [Display(Name = "月计划金额"), totalRow, thWidth("130")]
        public string YJHJE { get; set; }
        [Display(Name = "业务员预提数量"), totalRow, thWidth("130")]
        public string SQSL { get; set; }
        [Display(Name = "业务员预提金额"), totalRow, thWidth("130")]
        public string SQJE { get; set; }
        [Display(Name = "累计提货数量"), totalRow, thWidth("130")]
        public string XDSL { get; set; }
        [Display(Name = "累计提货金额"), totalRow, thWidth("130")]
        public string XDJE { get; set; }
        [Display(Name = "计划准确率（数量）"), thWidth("150")]
        public string JHZQL_SL { get; set; }
        [Display(Name = "年月份", AutoGenerateField = false, Prompt = "格式：201901"), enableSearch]
        public string YF { get; set; }
        [Display(Name = "组织", AutoGenerateField = false), enableSearch, UIHint("Org")]
        public string ORGPATH { get; set; }
        [Display(Name = "纳入计算的订单类型", AutoGenerateField = false), enableSearch, UIHint("Check", "销售申请类型")]
        public string ORDERTYPE { get; set; }
    }
}
