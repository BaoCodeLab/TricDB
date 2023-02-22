using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_FOR_TEST
    {
        [Key, Display(Name = "编号"), StringLength(50)]
        public string GID { get; set; }
        [Display(Name = "单个日期"), enableSort, AllowModify, Required, thWidth("100"),UIHint("Date","false")]
        public string DATE_TYPE { get; set; }
        [Display(Name = "范围日期"), enableSort, AllowModify, enableSearch, Required, thWidth("100"), UIHint("Date","true")]
        public string DATERANGE { get; set; }
        [Display(Name = "字符串"), enableSort, DataType(DataType.Text), Required, thWidth("200")]
        public string STRING_TYPE { get; set; }
        [Display(Name = "数值"), enableSort, AllowModify, enableSearch, Required, Range(0, 100, ErrorMessage = "需在{1}~{2}范围内"), thWidth("300")]
        public int? INT_TYPE { get; set; }
        [Display(Name = "布尔值"), enableSort, AllowModify, enableSearch, Required, thWidth("100")]
        public bool? BOOL_TYPE { get; set; }
        [Display(Name = "状态"), enableSort, AllowModify, enableSearch, Required, UIHint("select", "潜在客户状态")]
        public string STATE { get; set; }
        [Display(Name = "组织"), enableSort, AllowModify, enableSearch, Required, UIHint("Org")]
        public string ORGNAME { get; set; }
        [Display(Name = "组织路径",AutoGenerateField =false), AllowModify, Required]
        public string ORGPATH{ get; set; }
        [Display(Name = "用户"), AllowModify, Required,enableSearch]
        public string USERNAME { get; set; }

    }
}
