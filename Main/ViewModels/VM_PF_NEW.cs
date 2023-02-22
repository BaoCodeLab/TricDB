using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_PF_NEW:BaseViewModel
    {
        public VM_PF_NEW(Controller controller) : base(controller)
        {
        }
        public VM_PF_NEW() { }
        [Key]
        [Required(ErrorMessage = "{0}不允许为空")]
        [Display(Name = "标识符", AutoGenerateField = false)]
        public string GID { get; set; }

  //      [Required(ErrorMessage = "{0}为必填字段")]
        [Display(Name = "标题")]
        [enableSort(Sort = "true")]
        [MaxLength(50, ErrorMessage = "标题至多为50")]
        [AllowModify]
        [enableSearch]
        public string TITLE { get; set; }

        [Display(Name = "内容")]
        [enableSort(Sort = "true")]
        [AllowModify]
        [enableSearch]
        public string CONTENT { get; set; }

        [Display(Name = "作者")]
        [enableSort(Sort = "true")]
        [MaxLength(50, ErrorMessage = "作者至多为50")]
        [AllowModify]
        [enableSearch]
        public string AUTHOR { get; set; }

        [Display(Name = "文章状态")]
        [enableSort(Sort = "true")]
        [AllowModify]
        [enableSearch]
        public string STATE { get; set; }

        [Display(Name = "栏目ID")]
        [enableSort(Sort = "true")]
        [AllowModify]
        [enableSearch]
        public string LM_GID { get; set; }

        [Display(Name = "发布时间")]
        [enableSort(Sort = "true")]
        [AllowModify]
        [enableSearch]
        public String PUBLISH_TIME { get; set; }

        [Display(Name = "浏览次数")]
        [enableSort(Sort = "true")]
        [AllowModify]
        [enableSearch]
        public int VI_TIME { get; set; }

        [Display(Name = "创建时间")]
        [enableSort(Sort = "true")]
        [AllowModify]
        [enableSearch]
        public DateTime CREATE_DATE { get; set; }

        [Display(Name = "修改时间")]
        [enableSort(Sort = "true")]
        [AllowModify]
        [enableSearch]
        public DateTime MODIFY_DATE { get; set; }

        [Display(Name = "操作者")]
        [enableSort(Sort = "true")]
        [AllowModify]
        [enableSearch]
        public string OPERATOR { get; set; }

        [Display(Name = "删除", AutoGenerateField = false) ]
        public bool IS_DELETE { get; set; }
    }
}
