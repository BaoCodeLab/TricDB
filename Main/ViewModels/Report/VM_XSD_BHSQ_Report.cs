using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_XSD_BHSQ_Report
    {
        

        [Display(Name = "申请编号"), enableSearch, enableSort, thWidth("130")]
        public string SQBH { get; set; }
        
        [Display(Name = "订单号"),enableSearch,enableSort,thWidth("110")]
        public string DDH { get; set; }
        [Display(Name = "客户编码"), enableSearch, enableSort, thWidth("110")]
        public string KHBM { get; set; }

        [Display(Name = "客户名称"), enableSearch, enableSort, thWidth("300")]
        public string KHMC { get; set; }

        [Display(Name = "交货日期"), enableSearch, enableSort, thWidth("110"),UIHint("Date","true")]
        public string JHRQ { get; set; }

        [Display(Name = "交货工厂"), enableSearch, enableSort, thWidth("100")]
        public string JHGC { get; set; }
        
        [Display(Name = "业务员"), enableSearch, thWidth("100")]
        public string YWYMC { get; set; }

        [Display(Name = "申请状态"), enableSearch, UIHint("Select", "销售订单状态"), thWidth("100")]
        public string SQZT { get; set; }

        [Display(Name = "申请类型"), enableSearch, UIHint("Select", "销售申请类型"), thWidth("100")]
        public string SQLX { get; set; }

        [Display(Name = "创建时间"), enableSearch, enableSort, thWidth("110"), UIHint("Date", "true")]
        public string CREATE_DATE { get; set; }
        
        [Display(Name = "备注"), enableSearch,thWidth("300")]
        public string BZ { get; set; }
    }
}
