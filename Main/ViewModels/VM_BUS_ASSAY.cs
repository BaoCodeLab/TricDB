using Main.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public partial class VM_BUS_ASSAY : BaseViewModel
    {
        public VM_BUS_ASSAY(Controller controller) : base(controller)
        {
        }
        public VM_BUS_ASSAY() { }
        [Display(Name = "测试平台ID", AutoGenerateField = false), Key]
        public string SEQ_ASSAY_ID { get; set; }
        [Display(Name = "平台编码"), AllowModify, enableSort, enableSearch]
        public string ASSAYCODE { get; set; }
        [Display(Name = "是否双端测序"), AllowModify, enableSort, enableSearch]
        public bool? IS_PAIRED_END { get; set; }
        [Display(Name = "文库选择"), AllowModify, enableSort, enableSearch]
        public string LIBRARY_SELECTION { get; set; }
        [Display(Name = "文库方法"), AllowModify, enableSort, enableSearch]
        public string LIBRARY_STRATEGY { get; set; }
        [Display(Name = "测序平台"), AllowModify, enableSort, enableSearch]
        public string PLATFORM { get; set; }
        [Display(Name = "读段长度"), AllowModify, enableSort, enableSearch]
        public int? READ_LENGTH { get; set; }
        [Display(Name = "靶向试剂盒"), AllowModify, enableSort, enableSearch]
        public string TARGET_CAPTURE_KIT { get; set; }
        [Display(Name = "仪器型号"), AllowModify, enableSort, enableSearch]
        public string INSTRUMENT_MODEL { get; set; }
        [Display(Name = "基因填充"), AllowModify, enableShow(false), enableSort, enableSearch]
        public int? GENE_PADDING { get; set; }
        [Display(Name = "基因数目"), AllowModify, enableShow(false), enableSort, enableSearch]
        public int? NUMBER_OF_GENES { get; set; }
        [Display(Name = "变异类别"), AllowModify, enableShow(false), enableSort, enableSearch]
        public string VARIANT_CLASSIFICATIONS { get; set; }
        [Display(Name = "实验中心"), AllowModify, enableShow(false), enableSort, enableSearch]
        public string CENTER { get; set; }
        [Display(Name = "突变类型"), AllowModify, enableShow(false), enableSort, enableSearch]
        public string ALTERATION_TYPES { get; set; }
        [Display(Name = "样本保存技术"), AllowModify, enableShow(false), enableSort, enableSearch]
        public string PRESERVATION_TECHNIQUE { get; set; }
        [Display(Name = "标本肿瘤细胞"), AllowModify, enableShow(false), enableSort, enableSearch]
        public string SPECIMEN_TUMOR_CELLULARITY { get; set; }
        [Display(Name = "召集方法"), AllowModify, enableShow(false), enableSort, enableSearch]
        public string CALLING_STRATEGY { get; set; }
        [Display(Name = "覆盖范围"), AllowModify, enableShow(false), enableSort, enableSearch]
        public string COVERAGE { get; set; }
        [Display(Name = "测序流程ID"), AllowModify, enableShow(false), enableSort, enableSearch]
        public string SEQ_PIPELINE_ID { get; set; }
        [Display(Name = "创建时间"), enableShow(false), AllowModify, enableSort, enableSearch]
        public DateTime CREATE_DATE { get; set; }
        [Display(Name = "修改时间"), enableShow(false), AllowModify, enableSort, enableSearch]
        public DateTime? MODIFY_DATE { get; set; }
        [Display(Name = "操作者"), enableShow(false), AllowModify, enableSort, enableSearch]
        public string OPERATOR { get; set; }
        [Display(Name = "是否发布"), enableShow(false), AllowModify, enableSort, enableSearch]
        public bool IS_PUB { get; set; }
        [Display(Name = "是发删除"), enableShow(false), AllowModify, enableSort, enableSearch]
        public bool IS_DELETE { get; set; }
        [Display(Name = "版本"), enableShow(false), AllowModify, enableSort, enableSearch]
        public string VERSION { get; set; }
    }
}
