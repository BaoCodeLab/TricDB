using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.ViewModels.Annotation
{
    public class VM_ANNO_REPORT : BaseViewModel
    {
        public VM_ANNO_REPORT(Controller controller) : base(controller)
        {

        }
        public VM_ANNO_REPORT() { }

        [Display(Name = "唯一码", AutoGenerateField = false), Key]
        public string REPORT_ID { get; set; }

        [Display(Name = "样本编号"), enableShow("false"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("100")]
        public string ANNO_SAMPLEID { get; set; }

        [Display(Name = "报告名称"),  AllowModify, enableSort, enableSearch, enableExport, thMinWidth("100")]
        public string REPORT_NAME { get; set; }

        [Display(Name = "报告描述"),  AllowModify, enableSort, enableSearch, enableExport, thMinWidth("100")]
        public string REPORT_DESCRIPTION { get; set; }

        [Display(Name = "创建时间"), enableShow("false")]
        public DateTime CREATE_DATE { get; set; }
        [Display(Name = "修改时间"), enableShow("false")]
        public DateTime MODIFY_DATE { get; set; }
        [Display(Name = "操作人员"), enableShow("false")]
        public string OPERATOR { get; set; }
        [Display(Name = "是否删除"), enableShow("false")]
        public bool IS_DELETE { get; set; }
        [Display(Name = "是否公开"), AllowModify, Required(ErrorMessage = "{0}为必填字段"), enableExport]
        public bool IS_PUB { get; set; }
        [Display(Name = "版本"), enableShow(false), AllowModify, Required(ErrorMessage = "{0}为必填字段")]
        public string VERSION { get; set; }


    }
}
