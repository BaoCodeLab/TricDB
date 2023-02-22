using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.ViewModels.Annotation
{
    public class VM_ANNO_ALL : BaseViewModel
    {
        public VM_ANNO_ALL(Controller controller) : base(controller)
        {

        }
        public VM_ANNO_ALL() { }


        [Display(Name = "唯一码", AutoGenerateField = false), Key]
        public string ANNO_SAMPLEID { get; set; }

        [Display(Name = "用户编号"), enableShow("false"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("100")]
        public string USER_ID { get; set; }

        [Display(Name = "项目编号"), enableShow("false"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("100")]
        public string PROJECT_ID { get; set; }

        [Display(Name = "病人编号"), enableShow("false"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("100")]
        public string PATIENT_ID { get; set; }

        [Display(Name = "项目编号"), enableShow("false"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("100")]
        public string REPORT_ID { get; set; }

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




        [Display(Name = "项目名称"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string PROJECT_NAME { get; set; }

        [Display(Name = "项目描述"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string PROJECT_DESCRIP { get; set; }

        [Display(Name = "文件类型"), AllowModify, enableSort, enableSearch, thMinWidth("150")]
        public string FILE_TYPE { get; set; }

        [Display(Name = "分析类型"), AllowModify, enableSort, enableSearch, thMinWidth("150")]
        public string ANALYSIS_TYPE { get; set; }

        [Display(Name = "报告名称"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("100")]
        public string REPORT_NAME { get; set; }

        [Display(Name = "报告描述"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("100")]
        public string REPORT_DESCRIPTION { get; set; }


        [Display(Name = "标识符")]
        [Required(ErrorMessage = "{0}为必填字段")]
        public string GID { get; set; }


        [Display(Name = "关联GID")]
        [Required(ErrorMessage = "{0}为必填字段")]
        public string WGID { get; set; }


        [Display(Name = "文件名称")]
        [enableSearch]
        [Required(ErrorMessage = "{0}为必填字段")]
        //[JsonProperty(PropertyName =""]
        public string FILENAME { get; set; }


        [Display(Name = "文件地址")]
        [Required(ErrorMessage = "{0}为必填字段")]
        public string FILEURI { get; set; }


        [Display(Name = "文件类型")]
        public string TYPE { get; set; }

        [Display(Name = "文件分类"), enableSort, AllowModify, UIHint("Radio")]
        public string LX { get; set; }

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
