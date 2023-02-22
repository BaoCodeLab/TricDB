using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_BUS_RELATION : BaseViewModel
    {
        public VM_BUS_RELATION(Controller controller) : base(controller)
        {
        }
        public VM_BUS_RELATION() { }

        [Display(Name = "唯一码", AutoGenerateField = false), Key]
        public string RELATIONID { get; set; }
        [Display(Name = "靶基因编号"), enableShow("false"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("100")]
        public string TARGETID { get; set; }
        [Display(Name = "靶基因"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("100")]
        public string TARGET { get; set; }
        [Display(Name = "基因突变"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("100")]
        public string ALTERATION { get; set; }
        [Display(Name = "疾病编号"), enableShow("false"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("150")]
        public string DISEASEID { get; set; }
        [Display(Name = "疾病"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string DISEASE { get; set; }
        [Display(Name = "药物编号"), enableShow("false"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("150")]
        public string DRUGID { get; set; }
        [Display(Name = "药物"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string DRUG_NAME { get; set; }

        [Display(Name = "疾病突变率"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string MUTATION_RATE { get; set; }
        [Display(Name = "10389种系突变率"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string GERMLINE_10389_RATE { get; set; }
        [Display(Name = "中国万人体细胞突变率"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string CHINESE_10000_RATE { get; set; }

        [Display(Name = "突变率"), enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string ALTERATION_RATE { get; set; }
        [Display(Name = "基因功能和突变分析"), enableShow("false"), AllowModify, enableSearch, enableExport]
        public string FUNCTION_AND_CLINICAL_IMPLICATIONS { get; set; }
        [Display(Name = "潜在治疗方案"), AllowModify, enableShow("false"), enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string THERAPY_INTERPRETATION { get; set; }
        [Display(Name = "特异性"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string SPECIFICITY { get; set; }
        [Display(Name = "证据等级"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, thMinWidth("120")]
        public string EVIDENCE_LEVEL { get; set; }
        [Display(Name = "临床试验号"), AllowModify, enableShow(false), enableSort, enableSearch, thMinWidth("120")]
        public string CLINICAL_TRIAL { get; set; }
        [Display(Name = "批准机构"), AllowModify, enableShow(false), enableSort, enableSearch, thMinWidth("120")]
        public string APPROVED { get; set; }
        [Display(Name = "批准时间"), AllowModify, enableShow(false), enableSort, enableSearch, thMinWidth("120")]
        public string APPROVAL_TIME { get; set; }
        [Display(Name = "负基因型"), AllowModify, enableShow(false), enableSort, enableSearch, thMinWidth("120")]
        public string NEGATIVE_GENOTYPES { get; set; }
        [Display(Name = "适应症"), AllowModify, enableShow(false), enableSort, enableSearch, thMinWidth("150")]
        public string INDICATIONS { get; set; }
        [Display(Name = "剂量"), AllowModify, enableShow(false), enableSort, enableSearch, thMinWidth("150")]
        public string DOSAGE { get; set; }
        [Display(Name = "数据类型"), AllowModify, enableShow(false), enableSort, enableSearch, thMinWidth("150")]
        public string DATA_TYPE { get; set; }
        [Display(Name = "参考地址"), AllowModify, enableShow(false), enableSort, enableSearch, thMinWidth("150")]
        public string REFERENCE_LINK { get; set; }
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
    }
}
