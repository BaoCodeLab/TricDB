using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.ViewModels.Annotation
{
    public class VM_ANNO_PATIENT : BaseViewModel
    {
        public VM_ANNO_PATIENT(Controller controller) : base(controller)
        {

        }

        public VM_ANNO_PATIENT() { }


        [Display(Name = "唯一码", AutoGenerateField = false), Key]
        public string PATIENT_ID { get; set; }

        [Display(Name = "病人名字"), AllowModify, enableSort, enableSearch, thMinWidth("150")]
        public string PATIENT_NAME { get; set; }

        [Display(Name = "病人性别"), AllowModify, enableSort, enableSearch, thMinWidth("150")]
        public string PATIENT_GENDER { get; set; }

        [Display(Name = "病人年龄"), AllowModify, enableSort, enableSearch, thMinWidth("150")]
        public int PATIENT_AGE { get; set; }

        [Display(Name = "病人临床诊断结果"), AllowModify, enableSort, enableSearch, thMinWidth("150")]
        public string PATIENT_DIAG { get; set; }

        [Display(Name = "疾病分期"), AllowModify, enableSort, enableSearch, thMinWidth("150")]
        public string PATIENT_STAGE { get; set; }

        [Display(Name = "先前治疗历史"), AllowModify, enableSort, enableSearch, thMinWidth("150")]
        public string PRIOR_TREAT_HIST { get; set; }

        [Display(Name = "种族"), AllowModify, enableSort, enableSearch, thMinWidth("150")]
        public string ETHNICITY { get; set; }

        [Display(Name = "家族史"), AllowModify, enableSort, enableSearch, thMinWidth("150")]
        public string FAMILY_HISTORY { get; set; }

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
