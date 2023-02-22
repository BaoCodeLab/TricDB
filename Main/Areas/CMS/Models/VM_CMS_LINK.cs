using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Main.ViewModels
{
    public class VM_CMS_LINK: BaseViewModel
    {

        protected string lINKID=Guid.NewGuid().ToString();
        protected string lINKTEXT;
        protected bool iSPUB=true;
        protected string lINKURL;
        protected string iNDEXPIC;
        protected string pRECSS;
        protected bool iS_DELETE=false;
        protected DateTime cREATE_DATE=DateTime.Now;
        protected DateTime mODIFY_DATE=DateTime.Now;
        protected string oPERATOR= string.Empty;
        protected string bZ;
        protected string lINKTYPE = string.Empty;
        protected int lINKXH = 0;
        public VM_CMS_LINK(Controller controller) : base(controller)
        {
        }
        public VM_CMS_LINK()
        {
        }

        [Key,Display(Name = "链接编号",AutoGenerateField =false),Required(ErrorMessage = "{0}不允许为空")]
        public string LINKID
        {
            get { return lINKID; }
            set { lINKID = value; }
        }
        [Display(Name = "链接类别"), Required(ErrorMessage = "{0}不允许为空")]
        [enableSort(Sort = "true")]
        [AllowModify]
        public string LINKTYPE
        {
            get { return lINKTYPE; }
            set { lINKTYPE = value; }
        }
        [Display(Name = "链接排序"), Required(ErrorMessage = "{0}不允许为空")]
        [enableSort(Sort = "true")]
        [AllowModify]
        public int LINKXH
        {
            get { return lINKXH; }
            set { lINKXH = value; }
        }
        [Display(Name = "键面文字"),Required(ErrorMessage = "{0}不允许为空")]
        [enableSort(Sort = "true")]
        [AllowModify]
        [enableSearch]
        public string LINKTEXT
        {
            get { return lINKTEXT; }
            set { lINKTEXT = value; }
        }
        [Display(Name = "是否发布")]
        [enableSort(Sort = "true")]
        [AllowModify]
        [enableSearch]
        public bool ISPUB
        {
            get { return iSPUB; }
            set { iSPUB = value; }
        }
        [Display(Name = "链接地址", AutoGenerateField = false)]
        [enableSort(Sort = "true")]
        [AllowModify]
        [enableSearch]
        public string LINKURL
        {
            get { return lINKURL; }
            set { lINKURL = value; }
        }
        [Display(Name = "封面图片", AutoGenerateField = false)]
        [enableSort(Sort = "true")]
        [AllowModify]
        public string INDEXPIC
        {
            get { return iNDEXPIC; }
            set { iNDEXPIC = value; }
        }
        [Display(Name = "前置样式", AutoGenerateField = false)]
        [enableSort(Sort = "true")]
        [AllowModify]
        public string PRECSS
        {
            get { return pRECSS; }
            set { pRECSS = value; }
        }
        [Display(Name = "是否删除", AutoGenerateField = false)]
        [enableSort(Sort = "true")]
        [AllowModify]
        public bool IS_DELETE
        {
            get { return iS_DELETE; }
            set { iS_DELETE = value; }
        }
        [Display(Name = "创建时间", AutoGenerateField = false)]
        [enableSort(Sort = "true")]
        public DateTime CREATE_DATE
        {
            get { return cREATE_DATE; }
            set { cREATE_DATE = value; }
        }
        [Display(Name = "修改时间", AutoGenerateField = false)]
        [enableSort(Sort = "true")]
        public DateTime MODIFY_DATE
        {
            get { return mODIFY_DATE; }
            set { mODIFY_DATE = value; }
        }
        [Display(Name = "操作员", AutoGenerateField = false)]
        [enableSort(Sort = "true")]
        public string OPERATOR
        {
            get { return oPERATOR; }
            set { oPERATOR = value; }
        }
        [Display(Name = "备注", AutoGenerateField = false)]
        [AllowModify]
        public string BZ
        {
            get { return bZ; }
            set { bZ = value; }
        }
    }
}
