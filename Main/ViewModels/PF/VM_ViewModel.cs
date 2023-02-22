using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TDSCoreLib;
using Main.ViewModels;

namespace Main.ViewModels
{
    /// <summary>
    /// 用于呈现Viewmodel对象中的属性
    /// </summary>
    public class VM_ViewModel : BaseViewModel
    {
        public VM_ViewModel(Controller controller) : base(controller)
        {
        }

        public VM_ViewModel() { }


        [Display(Name = "名称")]
        public string NAME { get; set; }

        
        [Display(Name = "代码")]
        public string CODE { get; set; }
        
        /// <summary>
        /// 允许、不允许
        /// </summary>
        [Display(Name = "允许导出")]
        public string EXPORT { get; set; }
        
        /// <summary>
        /// 如：表头、明细
        /// </summary>
        [Display(Name = "展现形式")]
        public string TYPE { get; set; }

        [Display(Name = "示例数据")]
        public string VALUE { get; set; }
    }
}
