using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_FREQ_RELAT_ALL : BaseViewModel
    {
        public VM_FREQ_RELAT_ALL(Controller controller) : base(controller)
        {
        }
        public VM_FREQ_RELAT_ALL() { }

        [Display(Name = "freq关联ID唯一码"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public string FREQ_RELAT_ID { get; set; }

        [Display(Name = "freq表ID"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public string FREQ_ID { get; set; }

        [Display(Name = "药物ID"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public string DRUGID { get; set; }

        [Display(Name = "总表关联ID"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public string RELATIONID { get; set; }

        [Display(Name = "疾病ID"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public string DISEASEID { get; set; }

        [Display(Name = "突变的基因ID"), AllowModify, enableSort, enableShow, enableSearch, enableExport, thMinWidth("150")]
        public string TARGETID { get; set; }

        [Display(Name = "创建时间"), enableShow("false")]
        public DateTime CREATE_DATE { get; set; }

        [Display(Name = "修改时间"), enableShow("false")]
        public DateTime MODIFY_DATE { get; set; }

        [Display(Name = "操作人员"), enableShow("false")]
        public string OPERATOR { get; set; }

        [Display(Name = "是否公开"), AllowModify, Required(ErrorMessage = "{0}为必填字段"), enableExport]
        public bool IS_PUB { get; set; }

        [Display(Name = "是否删除"), enableShow("false")]
        public bool IS_DELETE { get; set; }

    }
}
