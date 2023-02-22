using Main.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public partial class VM_BUS_FUSION : BaseViewModel
    {
        public VM_BUS_FUSION(Controller controller) : base(controller)
        {
        }
        public VM_BUS_FUSION() { }
        [Display(Name = "唯一键", AutoGenerateField = false), Key]
        public string FUSION_ID { get; set; }
        [Display(Name = "融合编码"), AllowModify, enableSort, enableSearch]
        public string FUSIONCODE { get; set; }
        [Display(Name = "靶点"), AllowModify, enableSort, enableSearch]
        public string TARGET { get; set; }
        [Display(Name = "样本编号"), AllowModify, enableSort, enableSearch]
        public string SAMPLE_ID { get; set; }
        [Display(Name = "融合基因"), AllowModify, enableSort, enableSearch]
        public string FUSION { get; set; }
        [Display(Name = "阅读框"), AllowModify, enableSort, enableSearch]
        public string FRAME { get; set; }
        [Display(Name = "创建时间"), enableShow(false), AllowModify, enableSort, enableSearch]
        public DateTime CREATE_DATE { get; set; }
        [Display(Name = "修改时间"), enableShow(false), AllowModify, enableSort, enableSearch]
        public DateTime? MODIFY_DATE { get; set; }
        [Display(Name = "操作者"), AllowModify, enableSort, enableSearch]
        public string OPERATOR { get; set; }
        [Display(Name = "是否发布"), enableShow(false), AllowModify, enableSort, enableSearch]
        public bool IS_PUB { get; set; }
        [Display(Name = "是发删除"), enableShow(false), AllowModify, enableSort, enableSearch]
        public bool IS_DELETE { get; set; }
        [Display(Name = "版本"), enableShow(false), AllowModify, enableSort, enableSearch]
        public string VERSION { get; set; }
    }
}
