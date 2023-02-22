using Main.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_BUS_PATIENT : BaseViewModel
    {
        public VM_BUS_PATIENT(Controller controller) : base(controller)
        {
        }
        public VM_BUS_PATIENT()
        {
        }
        [Display(Name = "唯一码", AutoGenerateField = false), Key]
        public string PATIENT_ID { get; set; }
        [Display(Name = "病人编码"), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public string PATIENTCODE { get; set; }
        [Display(Name = "性别"), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public string SEX { get; set; }
        [Display(Name = "肤色"), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public string PRIMARY_RACE { get; set; }
        [Display(Name = "种族"), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public string ETHNICITY { get; set; }
        [Display(Name = "检测中心"), AllowModify, enableSort, enableSearch, thMinWidth("100")]
        public string CENTER { get; set; }
        [Display(Name = "创建时间"), AllowModify, enableShow(false), enableSort, enableSearch, thMinWidth("100")]
        public DateTime? CREATE_DATE { get; set; }
        [Display(Name = "修改时间"), AllowModify, enableShow(false), enableSort, enableSearch, thMinWidth("100")]
        public DateTime? MODIFY_DATE { get; set; }
        [Display(Name = "操作者"), AllowModify, enableShow(false), enableSort, enableSearch, thMinWidth("100")]
        public string OPERATOR { get; set; }
        [Display(Name = "是否发布"), AllowModify, enableShow(false), enableSort, enableSearch, thMinWidth("100")]
        public bool IS_PUB { get; set; }
        [Display(Name = "是发删除"), AllowModify, enableShow(false), enableSort, enableSearch, thMinWidth("100")]
        public bool IS_DELETE { get; set; }
        [Display(Name = "版本"), AllowModify,enableShow(false), enableSort, enableSearch, thMinWidth("100")]
        public string VERSION { get; set; }
    }
}
