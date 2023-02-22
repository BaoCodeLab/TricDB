using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;
using System;
namespace Main.ViewModels
{
    public class VM_PF_PROFILE:BaseViewModel
    {
        private string _GId = Guid.NewGuid().ToString();
        public VM_PF_PROFILE(Controller controller) : base(controller)
        {
        }
        public VM_PF_PROFILE() { }
        [Key,Display(Name = "编码",AutoGenerateField =false),enableSort(Sort = "true")]
        public string GID { get { return this._GId; } set { this._GId = value; } }

        [Required(ErrorMessage = "{0}为必填字段"),AllowModify,Display(Name = "人员编码"),MaxLength(50, ErrorMessage = "编码至多为50"),enableSearch]
        public string CODE { get; set; }


        [Required(ErrorMessage = "{0}为必填字段"),AllowModify,enableSearch,Display(Name = "姓名"),MaxLength(50, ErrorMessage = "姓名至多为50"),MinLength(2, ErrorMessage = "姓名至少为两位中文")]
        public string NAME { get; set; }

        [Required(ErrorMessage = "{0}为必填字段"),Display(Name = "职务"),MaxLength(50, ErrorMessage = "组织至多为50"),AllowModify,enableSort(Sort = "true")]
        public string ZW { get; set; }

        [Required(ErrorMessage = "{0}为必填字段"),AllowModify,Display(Name = "性别")]
        public string SEX { get; set; }
        
        [AllowModify,Display(Name = "年龄",AutoGenerateField =false)]
        public int AGE { get; set; }

        [Required(ErrorMessage = "{0}为必填字段"),AllowModify,Display(Name = "电话")]
        public string PHONE { get; set; }
        
        [Display(Name = "邮箱"),AllowModify]
        public string MAIL { get; set; }
        
        [Display(Name = "通讯地址",AutoGenerateField =false),AllowModify,MaxLength(200, ErrorMessage = "地址至多为200")]
        public string TXDZ { get; set; }

        //[Required(ErrorMessage = "{0}为必填字段")]
        [Display(Name = "个人爱好",AutoGenerateField =false),AllowModify,MaxLength(200, ErrorMessage = "爱好至多为200")]
        public string GRAH { get; set; }
        
        [Display(Name = "生日",AutoGenerateField =false),AllowModify]
        public string  SR { get; set; }
        
        [AllowModify,Display(Name = "删除", AutoGenerateField = false)]
        public bool IS_DELETE { get; set; }

        [Display(Name = "登陆账户",AutoGenerateField =false)]
        public string DLZH { get; set; }
    }
}
