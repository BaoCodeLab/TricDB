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
    public class VM_CMS_ARTICLE : BaseViewModel
    {
        protected string aRTICLEID=Guid.NewGuid().ToString();
        protected string cHANNELID=string.Empty;
        protected string aRTICLETITLE=string.Empty;
        protected string aRTICLECONTENT=string.Empty;
        protected int aRITCLETOPNUM = 0;
        protected int aRTICLEHIT = 0;
        protected DateTime aRTICLETIME=DateTime.Now;
        protected bool iSPUB=false;
        protected string aRTICLEREDIRECT=string.Empty;
        protected string aRTICLEEDITOR=string.Empty;
        protected string aRTICLEINDEXPIC=string.Empty;
        protected string aRTICLEKEYWORDS=string.Empty;
        protected bool aRTICLEISTITLE = false;
        protected bool iS_DELETE = false;
        protected DateTime cREATE_DATE=DateTime.Now;
        protected DateTime mODIFY_DATE=DateTime.Now;
        protected string bZ=string.Empty;
        protected string oPERATOR = string.Empty;
        protected string dLink = string.Empty;
        public VM_CMS_ARTICLE()
        {
        }
        public VM_CMS_ARTICLE(Controller controller) : base(controller)
        {
        }
        [Display(Name = "文章编号",AutoGenerateField =false)]
        public string ARTICLEID
        {
            get { return aRTICLEID; }
            set { aRTICLEID = value; }
        }
        [Display(Name = "栏目编号", AutoGenerateField = false),AllowModify]
        public string CHANNELID
        {
            get { return cHANNELID; }
            set { cHANNELID = value; }
        }
        [Display(Name = "文章标题"), AllowModify,Required(AllowEmptyStrings =true,ErrorMessage ="{0}必填"),enableSearch,enableSort]
        public string ARTICLETITLE
        {
            get { return aRTICLETITLE; }
            set { aRTICLETITLE = value; }
        }
        [Display(Name = "文章内容", AutoGenerateField = false),AllowModify]
        public string ARTICLECONTENT
        {
            get { return aRTICLECONTENT; }
            set { aRTICLECONTENT = value; }
        }
        [Display(Name = "置顶天数", AutoGenerateField = false), AllowModify,Required(ErrorMessage ="{0}必填项"),enableSort, thWidth("100")]
        public int ARTICLETOPNUM
        {
            get { return aRITCLETOPNUM; }
            set { aRITCLETOPNUM = value; }
        }
        [Display(Name = "点击次数"), AllowModify, Required(ErrorMessage = "{0}必填项"), enableSort, thWidth("100")]
        public int ARTICLEHIT
        {
            get { return aRTICLEHIT; }
            set { aRTICLEHIT = value; }
        }
        [Display(Name = "发布时间"),AllowModify,enableSort, thWidth("120")]
        public DateTime ARTICLETIME
        {
            get { return aRTICLETIME; }
            set { aRTICLETIME = value; }
        }
        [Display(Name = "是否公开", AutoGenerateField = true), AllowModify, enableSearch, thWidth("100")]
        public bool ISPUB
        {
            get { return iSPUB; }
            set { iSPUB = value; }
        }
        [Display(Name = "发布人", AutoGenerateField = false),enableSearch]
        public string OPERATOR
        {
            get { return oPERATOR; }
            set { oPERATOR = value; }
        }
        [Display(Name = "跳转地址", AutoGenerateField = false),AllowModify,enableSort]
        public string ARTICLEREDIRECT
        {
            get { return aRTICLEREDIRECT; }
            set { aRTICLEREDIRECT = value; }
        }
        [Display(Name = "文章编辑"),AllowModify, thWidth("100")]
        public string ARTICLEEDITOR
        {
            get { return aRTICLEEDITOR; }
            set { aRTICLEEDITOR = value; }
        }
        [Display(Name = "封面图片", AutoGenerateField = false),AllowModify,enableSort]
        public string ARTICLEINDEXPIC
        {
            get { return aRTICLEINDEXPIC; }
            set { aRTICLEINDEXPIC = value; }
        }
        [Display(Name = "关键字", AutoGenerateField = false),AllowModify]
        public string ARTICLEKEYWORDS
        {
            get { return aRTICLEKEYWORDS; }
            set { aRTICLEKEYWORDS = value; }
        }
        [Display(Name = "标题新闻", AutoGenerateField = false),AllowModify,enableSort, thWidth("100")]
        public bool ARTICLEISTITLE
        {
            get { return aRTICLEISTITLE; }
            set { aRTICLEISTITLE = value; }
        }
        [Display(Name = "是否删除",AutoGenerateField =false)]
        public bool IS_DELETE
        {
            get { return iS_DELETE; }
            set { iS_DELETE = value; }
        }
        [Display(Name = "创建时间",AutoGenerateField =false)]
        public DateTime CREATE_DATE
        {
            get { return cREATE_DATE; }
            set { cREATE_DATE = value; }
        }
        [Display(Name = "修改时间",AutoGenerateField =false)]
        public DateTime MODIFY_DATE
        {
            get { return mODIFY_DATE; }
            set { mODIFY_DATE = value; }
        }
        [Display(Name = "备注", AutoGenerateField = false),AllowModify]
        public string BZ
        {
            get { return bZ; }
            set { bZ = value; }
        }
        [Display(Name ="附件链接",AutoGenerateField =false),enableShow(false)]
        public string DLINK
        {
            get { return dLink; }
            set { dLink = value; }
        }
    }
}