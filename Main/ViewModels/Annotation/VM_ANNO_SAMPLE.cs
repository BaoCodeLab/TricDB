using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;



namespace Main.ViewModels.Annotation
{
    public class VM_ANNO_SAMPLE : BaseViewModel
    {
        public VM_ANNO_SAMPLE(Controller controller) : base(controller)
        {
        }
        public VM_ANNO_SAMPLE() { }

        [Display(Name = "唯一码", AutoGenerateField = false), Key]
        public string ANNO_SAMPLEID { get; set; }

        [Display(Name = "用户编号"), enableShow("false"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("100")]
        public string USER_ID { get; set; }

        [Display(Name = "项目编号"), enableShow("false"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("100")]
        public string PROJECT_ID { get; set; }

        [Display(Name = "病人编号"), enableShow("false"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("100")]
        public string PATIENT_ID { get; set; }

        [Display(Name = "样本来源"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string SAMPLE_SOURCE { get; set; }

        [Display(Name = "样本名称"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string SAMPLE_NAME { get; set; }

        [Display(Name = "样本类型"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string SAMPLE_TYPE { get; set; }

        [Display(Name = "取样位置"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string SAMPLE_POSI { get; set; }

        [Display(Name = "取样方法"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string SAMPLE_METHOD { get; set; }

        [Display(Name = "突变类型"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string MUTATION_TYPE { get; set; }

        [Display(Name = "测序类型"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string SEQUENCE_TYPE { get; set; }

        [Display(Name = "MSI算法类型"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string MSI_Type { get; set; }

        [Display(Name = "MSI状态"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string MSI { get; set; }

        [Display(Name = "测序长度"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public double CAPTURE_SIZE { get; set; }

        [Display(Name = "取样时间"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public DateTime SAMPLE_DATE { get; set; }

        [Display(Name = "处理时间"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public DateTime ACCESSION_DATE { get; set; }

        [Display(Name = "样本诊断结果"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string SAMPLE_DIAG { get; set; }

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
