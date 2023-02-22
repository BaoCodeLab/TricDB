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
    public class VM_CMS_CHANNEL : BaseViewModel
    {
        protected string cHANNELID = Guid.NewGuid().ToString();
        protected string cHANNELNAME = string.Empty;
        protected string cHANNELREDIRECT = string.Empty;
        protected bool iSPUB = true;
        protected bool iSMENU = true;
        protected bool iS_DELETE = false;
        protected DateTime cREATE_DATE = DateTime.Now;
        protected DateTime mODIFY_DATE = DateTime.Now;
        protected string oPERATOR = string.Empty;
        protected string bZ = string.Empty;
        protected string pARENTCHANNELID = "root";
        protected int cHANNELXH = 1;
        protected string pRECHANNELNAME = string.Empty;
        public VM_CMS_CHANNEL()
        {
        }
        public VM_CMS_CHANNEL(Controller controller) : base(controller)
        {
        }
        [Display(Name = "栏目编号", AutoGenerateField = false), Required(ErrorMessage = "{0}不允许为空")]
        public string CHANNELID
        {
            get { return cHANNELID; }
            set { cHANNELID = value; }
        }
        [Display(Name = "前置字符"), AllowModify]
        public string PRECHANNELNAME
        {
            get { return pRECHANNELNAME; }
            set { pRECHANNELNAME = value; }
        }
        [Display(Name = "栏目名称"), Required(ErrorMessage = "{0}不允许为空"), AllowModify]
        public string CHANNELNAME
        {
            get { return cHANNELNAME; }
            set { cHANNELNAME = value; }
        }
        [Display(Name = "跳转地址"), AllowModify]
        public string CHANNELREDIRECT
        {
            get { return cHANNELREDIRECT; }
            set { cHANNELREDIRECT = value; }
        }
        [Display(Name = "菜单显示"), AllowModify]
        public bool ISMENU
        {
            get { return iSMENU; }
            set { iSMENU = value; }
        }
        [Display(Name = "是否发布"), AllowModify]
        public bool ISPUB
        {
            get { return iSPUB; }
            set { iSPUB = value; }
        }
        [Display(Name = "是否删除")]
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
        [Display(Name = "操作员")]
        public string OPERATOR
        {
            get { return oPERATOR; }
            set { oPERATOR = value; }
        }
        [Display(Name = "备注"), AllowModify]
        public string BZ
        {
            get { return bZ; }
            set { bZ = value; }
        }
        [Display(Name = "父栏目编号"), AllowModify]
        public string PARENTCHANNELID
        {
            get { return pARENTCHANNELID; }
            set { pARENTCHANNELID = value; }
        }
        [Display(Name = "栏目排序号"), AllowModify]
        public int CHANNELXH
        {
            get { return cHANNELXH; }
            set { cHANNELXH = value; }
        }
    }
}