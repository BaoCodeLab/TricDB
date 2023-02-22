using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;
using System;

namespace Main.ViewModels
{
    public partial class VM_PF_FILE : BaseViewModel
    {
        public VM_PF_FILE(Controller controller) : base(controller)
        {
        }
        public VM_PF_FILE() { }
        [Key]
        [Display(Name = "标识符")]
        [Required(ErrorMessage = "{0}为必填字段")]
        public string GID { get; set; }

        [Display(Name = "关联GID")]
        [Required(ErrorMessage = "{0}为必填字段")]
        public string WGID { get; set; }


        [Display(Name = "文件名称")]
        [enableSearch]
        [Required(ErrorMessage = "{0}为必填字段")]
        //[JsonProperty(PropertyName =""]
        public string FILENAME { get; set; }


        [Display(Name = "文件地址")]
        [Required(ErrorMessage = "{0}为必填字段")]
        public string FILEURI { get; set; }


        [Display(Name = "文件类型")]
        public string TYPE { get; set; }

        [Display(Name = "MD5")]
        public string MD5 { get; set; }

        [Display(Name = "IP")]
        public string IP { get; set; }

        [Display(Name = "文件分类"), enableSort, AllowModify, UIHint("Radio")]
        public string LX { get; set; }

        [Display(Name = "创建时间")]
        [enableSort]
        public DateTime CREATE_DATE { get; set; }
        [Display(Name = "备注")]
        [enableSort]
        public string BZ { get; set; }

        [Display(Name = "修改时间")]
        [enableSort]
        public DateTime MODIFY_DATE { get; set; }

        [Display(Name = "操作用户")]
        [enableSearch]
        public string OPERATOR { get; set; }


    }
}
