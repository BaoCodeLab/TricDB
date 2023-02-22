using AutoMapper;
using Main.PF.ViewModels;
using Main.platform;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Reflection;
using TDSCoreLib;

namespace Main.WebAPI.PF
{
    [Produces("application/json")]
    [Route("api/pf/vm")]
    public class ViewModelAPI : Controller
    {

        /// <summary>
        /// 获取ViewModel的基本信息，并支持附带示例数据
        /// </summary>
        /// <param name="ViewModelClass">ViewModel的类名</param>
        /// <param name="ViewModelDemoData">ViewModel的示例数据（Json）</param>
        /// <returns></returns>
        [HttpGet, Authorize]
        public IActionResult ViewModelInfo([Required]string ViewModelClass,string ViewModelDemoData="")
        {
            Type ViewModel_Head = Assembly.Load(new AssemblyName("Main")).GetType("Main.ViewModels." + ViewModelClass);
            //将ViewModel对象转换为键值对列表，键为属性名，值为属性的Attribute
            Dictionary<string, VM_ViewModel> viewmodel_head_field = ViewModel_Head.GetProperties()
                .Select(s => new VM_ViewModel
                {
                    NAME = ((DisplayAttribute)s.GetCustomAttribute(typeof(DisplayAttribute))).Name,
                    CODE = ((JsonPropertyAttribute)s.GetCustomAttribute(typeof(JsonPropertyAttribute))) == null ? s.Name : ((JsonPropertyAttribute)s.GetCustomAttribute(typeof(JsonPropertyAttribute))).PropertyName,
                    EXPORT = ((enableExportAttribute)s.GetCustomAttribute(typeof(enableExportAttribute))) == null ? "不允许" : "允许",
                    TYPE = "表头"
                }).ToDictionary(d => d.CODE);
            //表头
            JObject JData = (JObject)JsonConvert.DeserializeObject(WebUtility.UrlDecode(ViewModelDemoData));
            List<VM_ViewModel> Head = new List<VM_ViewModel>();
            foreach (KeyValuePair<string, JToken> JH in JData)
            {
                //表头属性
                if (viewmodel_head_field.ContainsKey(JH.Key))
                {
                    var temp = viewmodel_head_field[JH.Key];
                    if (temp.EXPORT == "允许")
                    {
                        Head.Add(new VM_ViewModel { CODE = JH.Key, NAME = temp.NAME, EXPORT = temp.EXPORT, TYPE = temp.TYPE, VALUE = JH.Value.ToString() });
                    }
                }

            }
            return Ok(JsonConvert.SerializeObject(Head));
        }
    }
}


