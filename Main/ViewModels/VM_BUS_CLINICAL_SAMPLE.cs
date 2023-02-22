using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_BUS_CLINICAL_SAMPLE: BaseViewModel
    {
        public VM_BUS_CLINICAL_SAMPLE(Controller controller) : base(controller)
        {
        }
        public VM_BUS_CLINICAL_SAMPLE() { }
        [Display(Name = "唯一码", AutoGenerateField = false), Key]
        public string SAMPLE_ID { get; set; }
        [Display(Name = "样本编码", AutoGenerateField = false), Key]
        public string SAMPLECODE { get; set; }
        [Display(Name = "病人编号"), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public string PATIENT_ID { get; set; }
        [Display(Name = "测序时患者年龄"), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public string AGE_AT_SEQ_REPORT { get; set; }
        [Display(Name = "Onco Tree编码"), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public string ONCOTREE_CODE { get; set; }
        [Display(Name = "样本类型"), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public string SAMPLE_TYPE { get; set; }
        [Display(Name = "测序平台ID"), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public string SEQ_ASSAY_ID { get; set; }
        [Display(Name = "癌症类型"), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public string CANCER_TYPE { get; set; }
        [Display(Name = "癌症类型明细"), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public string CANCER_TYPE_DETAILED { get; set; }
        [Display(Name = "样本类型"), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public string SAMPLE_TYPE_DETAILED { get; set; }
        [Display(Name = "创建时间"), enableShow(false), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public DateTime? CREATE_DATE { get; set; }
        [Display(Name = "修改时间"), enableShow(false), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public DateTime? MODIFY_DATE { get; set; }
        [Display(Name = "操作员"), enableShow(false), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public string OPERATOR { get; set; }
        [Display(Name = "是否公布"), enableShow(false), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public bool IS_PUB { get; set; }
        [Display(Name = "是否删除"), enableShow(false), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public bool IS_DELETE { get; set; }
        [Display(Name = "版本"), enableShow(false), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public string VERSION { get; set; }
    }
}
