using Main.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public partial class VM_BUS_CNA_HG : BaseViewModel
    {
        public VM_BUS_CNA_HG(Controller controller) : base(controller)
        {
        }
        public VM_BUS_CNA_HG() { }
        [Display(Name = "唯一键", AutoGenerateField = false), Key]
        public string CNA_ID { get; set; }
        [Display(Name = "CNA编码"), AllowModify, enableSort, enableSearch]
        public string CNACODE { get; set; }
        [Display(Name = "样本编号"), AllowModify, enableSort, enableSearch]
        public string SAMPLE_ID { get; set; }
        [Display(Name = "染色体"), AllowModify, enableSort, enableSearch]
        public string CHROM { get; set; }
        [Display(Name = "起始"), AllowModify, enableSort, enableSearch]
        public string LOCASTART { get; set; }
        [Display(Name = "终止"), AllowModify, enableSort, enableSearch]
        public string LOCEND { get; set; }
        [Display(Name = "探针匹配数"), AllowModify, enableSort, enableSearch]
        public int? NUMMARK { get; set; }
        [Display(Name = "读段均值"), AllowModify, enableSort, enableSearch]
        public double? SEGMEAN { get; set; }
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
