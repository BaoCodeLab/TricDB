using Main.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_CMS_AD:BaseViewModel
    {
        protected string aDID=Guid.NewGuid().ToString();
        protected string aDNAME = string.Empty;
        protected string aDTEXT = string.Empty;
        protected bool iS_DELETE=false;
        protected DateTime cREATE_DATE=DateTime.Now;
        protected DateTime mODIFY_DATE=DateTime.Now;
        protected string oPERATOR;
        protected string bZ=string.Empty;

        public VM_CMS_AD()
        {
        }
        public VM_CMS_AD(Controller controller) : base(controller)
        {
        }
        [Key, Display(Name = "文本编号",AutoGenerateField =false), Required(ErrorMessage = "{0}不允许为空")]
        public string ADID
        {
            get { return aDID; }
            set { aDID = value; }
        }
        [Display(Name = "文本名称"),Required(ErrorMessage ="{0}不允许为空"),AllowModify]
        
        public string ADNAME
        {
            get { return aDNAME; }
            set { aDNAME = value; }
        }
        [Display(Name = "文本内容",AutoGenerateField =false),AllowModify]
        public string ADTEXT
        {
            get { return aDTEXT; }
            set { aDTEXT = value; }
        }
        [Display(Name = "是否删除",AutoGenerateField =false)]
        public bool IS_DELETE
        {
            get { return iS_DELETE; }
            set { iS_DELETE = value; }
        }
        [Display(Name = "创建时间")]
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
        [Display(Name = "操作员",AutoGenerateField =false)]
        public string OPERATOR
        {
            get { return oPERATOR; }
            set { oPERATOR = value; }
        }
        [Display(Name = "备注"),AllowModify]
        public string BZ
        {
            get { return bZ; }
            set { bZ = value; }
        }
    }
}
