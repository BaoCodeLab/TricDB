using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;

namespace Main.ViewModels
{
    public partial class VM_PF_SMSCODE : BaseViewModel
    {
        protected string cID=Guid.NewGuid().ToString();
        protected string cODE=string.Empty;
        protected string mOBILE=string.Empty;
        protected bool sENDRET = false;
        protected bool iS_DELETE = false;
        protected DateTime cREATE_DATE=DateTime.Now;
        protected DateTime mODIFY_DATE=DateTime.Now;
        protected string oPERATOR=string.Empty;
        public VM_PF_SMSCODE(Controller controller) : base(controller)
        {
        }
        public VM_PF_SMSCODE() { }
        [Key]
        [Display(Name ="编号")]
        [Required(ErrorMessage = "{0}为必填字段")]
        public string CID
        {
            get { return cID; }
            set { cID = value; }
        }
        [Display(Name = "验证码")]
        public string CODE
        {
            get { return cODE; }
            set { cODE = value; }
        }
        [Display(Name = "手机号码")]
        public string MOBILE
        {
            get { return mOBILE; }
            set { mOBILE = value; }
        }
        [Display(Name = "发送结果")]
        public bool SENDRET
        {
            get { return sENDRET; }
            set { sENDRET = value; }
        }
        [Display(Name = "是否删除")]
        public bool IS_DELETE
        {
            get { return iS_DELETE; }
            set { iS_DELETE = value; }
        }
        [Display(Name = "发送时间")]
        public DateTime CREATE_DATE
        {
            get { return cREATE_DATE; }
            set { cREATE_DATE = value; }
        }
        [Display(Name = "修改时间")]
        public DateTime MODIFY_DATE
        {
            get { return mODIFY_DATE; }
            set { mODIFY_DATE = value; }
        }
        [Display(Name = "创建者")]
        public string OPERATOR
        {
            get { return oPERATOR; }
            set { oPERATOR = value; }
        }
    }
}
