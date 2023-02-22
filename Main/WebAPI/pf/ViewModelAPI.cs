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
        /// ��ȡViewModel�Ļ�����Ϣ����֧�ָ���ʾ������
        /// </summary>
        /// <param name="ViewModelClass">ViewModel������</param>
        /// <param name="ViewModelDemoData">ViewModel��ʾ�����ݣ�Json��</param>
        /// <returns></returns>
        [HttpGet, Authorize]
        public IActionResult ViewModelInfo([Required]string ViewModelClass,string ViewModelDemoData="")
        {
            Type ViewModel_Head = Assembly.Load(new AssemblyName("Main")).GetType("Main.ViewModels." + ViewModelClass);
            //��ViewModel����ת��Ϊ��ֵ���б���Ϊ��������ֵΪ���Ե�Attribute
            Dictionary<string, VM_ViewModel> viewmodel_head_field = ViewModel_Head.GetProperties()
                .Select(s => new VM_ViewModel
                {
                    NAME = ((DisplayAttribute)s.GetCustomAttribute(typeof(DisplayAttribute))).Name,
                    CODE = ((JsonPropertyAttribute)s.GetCustomAttribute(typeof(JsonPropertyAttribute))) == null ? s.Name : ((JsonPropertyAttribute)s.GetCustomAttribute(typeof(JsonPropertyAttribute))).PropertyName,
                    EXPORT = ((enableExportAttribute)s.GetCustomAttribute(typeof(enableExportAttribute))) == null ? "������" : "����",
                    TYPE = "��ͷ"
                }).ToDictionary(d => d.CODE);
            //��ͷ
            JObject JData = (JObject)JsonConvert.DeserializeObject(WebUtility.UrlDecode(ViewModelDemoData));
            List<VM_ViewModel> Head = new List<VM_ViewModel>();
            foreach (KeyValuePair<string, JToken> JH in JData)
            {
                //��ͷ����
                if (viewmodel_head_field.ContainsKey(JH.Key))
                {
                    var temp = viewmodel_head_field[JH.Key];
                    if (temp.EXPORT == "����")
                    {
                        Head.Add(new VM_ViewModel { CODE = JH.Key, NAME = temp.NAME, EXPORT = temp.EXPORT, TYPE = temp.TYPE, VALUE = JH.Value.ToString() });
                    }
                }

            }
            return Ok(JsonConvert.SerializeObject(Head));
        }
    }
}


