using Main.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public partial class VM_BUS_CNA : BaseViewModel
    {
        public VM_BUS_CNA(Controller controller) : base(controller)
        {
        }
        public VM_BUS_CNA() { }
        [Display(Name = "唯一键", AutoGenerateField = false), Key]
        public string SAMPLE_ID { get; set; }
        [Display(Name = "靶点"), AllowModify, enableSort, enableSearch]
        public string TARGET { get; set; }
        [Display(Name = "GASTIC分数"), AllowModify, enableSort, enableSearch]
        public string GASTIC_SCORE { get; set; }
        [Display(Name = "CNA类型"), AllowModify, enableSort, enableSearch]
        public string CNA_TYPE { get; set; }
    }
}
