using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public partial class VM_PF_FILEEXPORT : BaseViewModel
    {
        public VM_PF_FILEEXPORT(Controller controller) : base(controller)
        {
        }
        public VM_PF_FILEEXPORT() { }

        [Display(Name = "附件标识")]
        [enableExport]
        public string MD5 { get; set; }

        [Display(Name = "附件名称")]
        [enableExport]
        public string FILENAME { get; set; }

        [Display(Name = "附件类型")]
        [enableExport]
        public string TYPE { get; set; }

        [Display(Name = "主键标识")]
        [enableExport]
        public string HTBS { get; set; }

    }
}
