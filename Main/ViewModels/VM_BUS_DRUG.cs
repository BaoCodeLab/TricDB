using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_BUS_DRUG : BaseViewModel
    {
        public VM_BUS_DRUG(Controller controller) : base(controller)
        {
        }
        public VM_BUS_DRUG() { }
        [Display(Name = "唯一码", AutoGenerateField = false), Key]
        public string DRUGID { get; set; }
        [Display(Name = "药物编码"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, thMinWidth("100")]
        public string DRUGCODE { get; set; }
        [Display(Name = "药物名称"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, thMinWidth("150")]
        public string DRUG_NAME { get; set; }
        [Display(Name = "品牌名"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, thMinWidth("150")]
        public string BRAND_NAME { get; set; }
        [Display(Name = "公司"), AllowModify, enableSort, enableSearch, thMinWidth("150")]
        public string COMPANY { get; set; }
        [Display(Name = "公司简称"), AllowModify, enableSort, enableSearch, thMinWidth("150")]
        public string COMPANY_ALIAS { get; set; }
        [Display(Name = "药物靶点"), AllowModify, enableSort, enableSearch, thMinWidth("150")]
        public string DRUG_TARGET { get; set; }
        [Display(Name = "药物类型"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, thMinWidth("150")]
        public string DRUG_TYPE { get; set; }
        [Display(Name = "作用机制"), AllowModify, enableShow("false"), enableSearch, thMinWidth("150")]
        public string MECHANISM_OF_ACTION { get; set; }
        [Display(Name = "结构"), AllowModify, enableShow("false"), enableSearch, thMinWidth("150")]
        public string STRUCTURE { get; set; }
        [Display(Name = "结构其他信息"), AllowModify, enableShow("false"), enableSearch, thMinWidth("150")]
        public string STRUCTURE_INFO { get; set; }
        [Display(Name = "创建时间"), enableShow("false")]
        public DateTime CREATE_DATE { get; set; }
        [Display(Name = "修改时间"), enableShow("false")]
        public DateTime MODIFY_DATE { get; set; }
        [Display(Name = "操作人员"), enableShow("false")]
        public string OPERATOR { get; set; }
        [Display(Name = "是否删除"), enableShow("false")]
        public bool IS_DELETE { get; set; }
        [Display(Name = "是否公开"), enableShow(false), AllowModify, Required(ErrorMessage = "{0}为必填字段")]
        public bool IS_PUB { get; set; }
        [Display(Name = "点击率"), enableShow(false), enableSort, Required(ErrorMessage = "{0}为必填字段")]
        public int HIT { get; set; }
        [Display(Name = "版本"), enableShow(false), AllowModify, Required(ErrorMessage = "{0}为必填字段")]
        public string VERSION { get; set; }
    }
}
