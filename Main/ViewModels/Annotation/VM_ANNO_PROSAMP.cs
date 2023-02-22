using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.ViewModels.Annotation
{
    public class VM_ANNO_PROSAMP : BaseViewModel
    {
        public VM_ANNO_PROSAMP(Controller controller): base(controller) 
        { 

        }
        public VM_ANNO_PROSAMP() { }


        [Display(Name = "唯一码", AutoGenerateField = false), Key]
        public string PROSAMP_ID { get; set; }


        [Display(Name = "样本编号"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("100")]
        public string ANNO_SAMPLEID { get; set; }

        [Display(Name = "项目名称"), AllowModify, enableSort, Required(ErrorMessage = "{0}为必填字段"), enableSearch, enableExport, thMinWidth("150")]
        public string PROJECT_ID { get; set; }


        [Display(Name = "创建时间")]
        public DateTime CREATE_DATE { get; set; }
        [Display(Name = "修改时间")]
        public DateTime MODIFY_DATE { get; set; }

        [Display(Name = "操作人员")]
        public string OPERATOR { get; set; }

        [Display(Name = "是否删除"), AllowModify, Required(ErrorMessage = "{0}为必填字段"), enableExport]
        public bool IS_DELETE { get; set; }

        [Display(Name = "是否公开"), AllowModify, Required(ErrorMessage = "{0}为必填字段"), enableExport]
        public bool IS_PUB { get; set; }

        [Display(Name = "版本"), AllowModify, Required(ErrorMessage = "{0}为必填字段")]
        public string VERSION { get; set; }


    }
}
