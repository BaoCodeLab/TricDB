using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public partial class VM_PF_REMINDER
    {
        [Key,Display(Name ="编号",AutoGenerateField =false),Required]
        public string GID { get; set; }
        [Display(Name ="目标用户", AutoGenerateField = false),enableSearch,enableSort,AllowModify,DisplayFormat(NullDisplayText ="")]
        public string USERS { get; set; }
        [Display(Name = "目标组", AutoGenerateField = false), enableSearch, enableSort,AllowModify]
        public string ROLES { get; set; }
        [Display(Name ="当前状态"),UIHint("Radio"), AllowModify, enableSearch,DisplayFormat(NullDisplayText ="启用")]
        public string STATUS { get; set; }
        [Display(Name = "标题"), enableSearch,Required, AllowModify]
        public string TITLE { get; set; }
        [Display(Name = "内容", AutoGenerateField = false), enableSearch, AllowModify]
        public string CONTENT { get; set; }
        [Display(Name = "远程API", AutoGenerateField = false, Description ="使用远程API返回的结果值作为消息提醒的内容"), AllowModify]
        public string API { get; set; }
        [Display(Name = "定时规则", AutoGenerateField = false),Required(ErrorMessage ="{0}为必填字段"), AllowModify]
        public string RULE { get; set; }
        [Display(Name = "起始日期"),DataType(DataType.DateTime), Required, AllowModify,DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", NullDisplayText = "")]
        public string SDATE { get; set; }
        [Display(Name = "结束日期"), DataType(DataType.DateTime), Required, AllowModify, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", NullDisplayText = "")]
        public string EDATE { get; set; }
        [Display(Name = "参数1",AutoGenerateField =false), AllowModify,DisplayFormat(ConvertEmptyStringToNull =true)]
        public string PARAM1 { get; set; }
        [Display(Name = "参数2", AutoGenerateField = false), AllowModify, DisplayFormat(ConvertEmptyStringToNull = true)]
        public string PARAM2 { get; set; }
        [Display(Name = "参数3", AutoGenerateField = false), AllowModify, DisplayFormat(ConvertEmptyStringToNull = true)]
        public string PARAM3 { get; set; }
        [Display(Name = "参数4", AutoGenerateField = false), AllowModify, DisplayFormat(ConvertEmptyStringToNull = true)]
        public string PARAM4 { get; set; }
    }
}
