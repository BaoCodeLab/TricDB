using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;


namespace Main.ViewModels
{
    public class VM_BUS_CyScape : BaseViewModel
    {
        public VM_BUS_CyScape(Controller controller): base(controller)
        {
        }
        public VM_BUS_CyScape() { }
        [Display(Name = "疾病编码"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("100")]
        public string DISEASECODE { get; set; }
        [Display(Name = "药物编码"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("150")]
        public string DRUGCODE { get; set; }
        [Display(Name = "疾病名称"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("150")]
        public string DISEASE { get; set; }
        [Display(Name = "基因ID"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("150")]
        public string TARGETID { get; set; }
        [Display(Name = "基因名称"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("150")]
        public string TARGET { get; set; }
        [Display(Name = "药物名称"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("150")]
        public string DRUG_NAME { get; set; }
        [Display(Name = "ENTREZ_GENEID"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("150")]
        public string ENTREZ_GENEID { get; set; }
        [Display(Name = "样本编号"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("150")]
        public string SAMPLE_ID { get; set; }
        public int GENECOUNT { get; set; }
    }
}
