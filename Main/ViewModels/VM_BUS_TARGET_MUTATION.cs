using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_BUS_TARGET_MUTATION : BaseViewModel
    {
        public VM_BUS_TARGET_MUTATION(Controller controller) : base(controller)
        {
        }
        public VM_BUS_TARGET_MUTATION() { }
        [Display(Name = "靶点"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("100")]
        public string TARGET { get; set; }
        [Display(Name = "基因突变"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("150")]
        public string ALTERATION { get; set; }
        [Display(Name = "突变类型"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("150")]
        public string VARIANT_CLASSIFICATION { get; set; }
        [Display(Name = "突变位置"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("150")]
        public string PROTEIN_POSITION { get; set; }
        public int Count { get; set; }
    }
}
