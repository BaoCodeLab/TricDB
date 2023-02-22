using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Main.ViewModels
{
    public partial class VM_PF_LOG
    {
        [Key]
        [Required(ErrorMessage ="{0}不允许为空")]
        [Display(Name ="标识符",AutoGenerateField =false)]
        public string GID { get; set; }


        [Display(Name = "日志类型")]
        public string RZLX { get; set; }


        [Display(Name = "操作对象")]
        public string CZDX { get; set; }



        [Display(Name = "类型名")]
        public string LXM { get; set; }



        [Display(Name = "日志内容")]
        public string RZNR { get; set; }



        [Display(Name = "日志时间")]
        public DateTime RZSJ { get; set; }


    

    }
}
