using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_BUS_DISEASE : BaseViewModel
    {
        public VM_BUS_DISEASE(Controller controller) : base(controller)
        {
        }
        public VM_BUS_DISEASE() { }

        [Display(Name = "唯一码", AutoGenerateField = false), Key]
        public string DISEASEID { get; set; }
        [Display(Name = "疾病编码"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("100")]
        public string DISEASECODE { get; set; }
        [Display(Name = "疾病"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("100")]
        public string DISEASE { get; set; }
        [Display(Name = "疾病别名"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string DISEASE_ALIAS { get; set; }
        [Display(Name = "疾病分类"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("150")]
        public string CLASSIFICATION { get; set; }
        [Display(Name = "疾病途径示意图"), AllowModify, enableSort, enableShow("false"), enableSearch, enableExport, thMinWidth("150")]
        public string DISEASE_PATHWAY { get; set; }
        [Display(Name = "疾病参考"), AllowModify, enableSort, enableShow("false"), enableSearch, enableExport, thMinWidth("150")]
        public string PATHWAY_REFER { get; set; }
        [Display(Name = "NCI编码"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string NCI_CODE { get; set; }
        [Display(Name = "Oncotree编码"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string ONCOTREE_CODE { get; set; }
        [Display(Name = "疾病等级路径"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string DISEASE_PATH { get; set; }
        [Display(Name = "NCCN疾病指南"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string NCCN_LINK { get; set; }

        [Display(Name = "NCCN参考"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string NCCN_REF { get; set; }

        [Display(Name = "NCI疾病定义"), AllowModify, enableSort, enableSearch, enableExport, thMinWidth("150")]
        public string NCI_DISEASE_DEFINITION { get; set; }
        [Display(Name = "点击率"), enableSort, Required(ErrorMessage = "{0}为必填字段"), enableExport]
        public int HIT { get; set; }
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
        [Display(Name = "版本"), AllowModify, Required(ErrorMessage = "{0}为必填字段"), enableExport]
        public string VERSION { get; set; }
    }
}
