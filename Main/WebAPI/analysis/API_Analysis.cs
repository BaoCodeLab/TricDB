using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Model.Model;
using Main.Extensions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Collections.Generic;
using TDSCoreLib;
using Main.platform;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Dynamic.Core;
using Main.ViewModels;
using AutoMapper;

namespace Main.WebAPI.analysis
{
    /// <summary>
    /// 提供各类数据分析接口
    /// </summary>
    [Produces("application/json")]
    [Route("api/Analysis")]
    public class API_Analysis : Controller
    {
        private readonly drugdbContext _context;

        public API_Analysis(drugdbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询当前平台用户总数
        /// </summary>
        /// <returns>查询结果</returns>
        [HttpGet, Route("[action]")]
        public object ptyhsl()
        {
            return new { result = _context.PF_USER.Where(w => w.IS_DELETE == false).Count().ToString() };
        }

        [HttpGet, Route("[action]")]
        public object xsddsl()
        {
            return new { result = _context.BUS_TARGET.Where(m => m.IS_DELETE.Equals(false) && m.IS_PUB.Equals(true)).Count() };
        }
        [HttpGet, Route("[action]")]
        public object qzkhsl()
        {
            return new { result = _context.BUS_DRUG.Where(m => m.IS_DELETE.Equals(false) && m.IS_PUB.Equals(true)).Count() };
        }
        [HttpGet, Route("[action]")]
        public object khsl()
        {
            return new { result = _context.BUS_DISEASE.Where(m => m.IS_DELETE.Equals(false) && m.IS_PUB.Equals(true)).Count() };
        }
        /// <summary>
        /// 查询当前系统环境
        /// </summary>
        /// <returns>查询结果</returns>
        [HttpGet, Route("[action]")]
        public object xthj()
        {
            string BaseDirectory = AppContext.BaseDirectory;
            string ApplicationName = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
            string ApplicationVersion = typeof(Program).GetTypeInfo().Assembly.GetName().Version.ToString();
            string RuntimeFramework = typeof(Program).GetTypeInfo().Assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName;
            string OSArchitecture = RuntimeInformation.OSArchitecture.ToString();
            string OSDescription = RuntimeInformation.OSDescription;
            string MachineName = Environment.MachineName;
            int ProcessorCount = Environment.ProcessorCount;
            int StartHours = Environment.TickCount / 3600000;
            List<object> KVlist = new List<object>
            {
                new Parameter("BaseDirectory", BaseDirectory, "文件目录"),
                //KVlist.Add(Parameter.Create("ApplicationName", ApplicationName, "应用名称"));
                //KVlist.Add(new Parameter("ApplicationVersion", ApplicationVersion, "应用版本"));
                new Parameter("RuntimeFramework", RuntimeFramework, "运行环境"),
                new Parameter("MachineName", MachineName, "服务器名"),
                new Parameter("OSArchitecture", OSArchitecture, "服务器架构"),
                new Parameter("OSDescription", OSDescription, "服务器系统"),
                new Parameter("ProcessorCount", ProcessorCount.ToString(), "CPU核数"),
                new Parameter("StartHours", StartHours.ToString(), "已运行小时"),
                new Parameter("ENVIRONMENT", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "当前环境")
            };
            return new ResultList<Parameter>
            {
                TotalCount = KVlist.Count,
                Results = KVlist
            };
        }
        /// <summary>
        /// 近15天数据日志统计
        /// </summary>
        /// <returns>查询结果</returns>
        [HttpGet, Route("[action]")]
        public object sjrztj()
        {
            //此处需增加日志条件，仅统计异常日志
            var RZLXList = new List<string> { "LOGIN"};
            int[] data = new int[15] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var series = (from a in _context.PF_LOG
                          where a.RZSJ.DayOfYear > DateTime.Now.DayOfYear - 15 && a.RZSJ.Year == DateTime.Now.Year && RZLXList.Contains(a.RZLX)
                          group a by new { a.RZLX } into g
                          select new { type = "line", smooth = false, stack = "总数", name = g.Key.RZLX, data = logDataArray((from f in g where f.RZLX == g.Key.RZLX group f by new { RZSJ = f.RZSJ.ToString("yyyy-MM-dd") } into g2 select new ArrayList { 14 - (DateTime.Now - Convert.ToDateTime(g2.Key.RZSJ)).Days, g2.Count() }).ToArray(), 15) }).ToList();
            var xAxis = new List<object>();
            var yAxis = new List<object>();
            xAxis.Add(new
            {
                type = "category",
                boundaryGap = false,
                data = new string[] { "15", "14", "13", "12", "11", "10", "9", "8", "7", "6", "5", "4", "3", "2", "1" }
            });
            yAxis.Add(new { type = "value" });
            var Main = new
            {
                tooltip = new
                {
                    trigger = "axis",
                    axisPointer = new
                    {
                        type = "cross",
                        label = new
                        {
                            background = "#6a7985"
                        }
                    }
                },
                legend = new
                {
                    data = (from a in _context.PF_LOG where RZLXList.Contains(a.RZLX) group a by a.RZLX into g select g.Key).ToArray()
                },
                toolbox = new
                {
                    feature = new
                    {
                        saveAsImage = new { }
                    }
                },
                grid = new
                {
                    left = "3%",
                    right = "4%",
                    bottom = "3%",
                    containLabel = true
                },
                xAxis,
                yAxis,
                series
            };

            return Main;
        }

        /// <summary>
        /// 当月签到结果统计
        /// </summary>
        /// <returns>查询结果</returns>
        [HttpGet, Route("[action]")]
        public object dyqd()
        {
            var series = new List<object>();
            var datas = new
            {
                id = "label",
                type = "scatter",
                coordinateSystem = "calendar",
                symbolSize = 1,
            };
            series.Add(datas);

            var Main = new
            {
                calender = new
                {
                    left = " center",
                    top = "middle",
                    cellSize = "[70, 70]",
                    range = DateTime.Now.ToString("yyyy-MM"),
                    //    cellsize = "auto",
                    orient = "vertical",
                    splitLine = new { show = true },
                    dayLabel = new { firstDay = 1, nameMap = "cn" },
                    monthLabel = new { nameMap = "cn" }

                },
                visualMap = new
                {
                    show = false,
                    min = 0,
                    max = 300,
                    calculable = true,
                    seriesIndex = "[2]",
                    orient = "horizontal",
                    left = " center",
                    bottom = 20,
                    inRange = new
                    {
                        color = "['#e0ffff', '#006edd']",
                        opacity = 0.3
                    },
                    controller = new
                    {
                        inRange = new
                        {
                            opacity = 0.5
                        }
                    }
                },

                series
            };

            return Main;
        }


        private ArrayList logDataArray(ArrayList[] input, int count)
        {
            ArrayList initArray = new ArrayList();
            for (int c = 0; c < count; c++)
            {
                initArray.Add("0");
            }
            for (int i = 0; i < input.Length; i++)
            {
                initArray[(int)input[i][0]] = input[i][1].ToString();
            }
            return initArray;
        }

        /// <summary>
        /// 经度和维度转换成城市名
        /// </summary>
        /// <param name="JD"></param>
        /// <param name="WD"></param>
        /// <returns></returns>
        private string ConvertLocationtoCityName(string JD, string WD)
        {
            string queryBaiduAPI = "http://api.map.baidu.com/cloudrgc/v1?location=" + WD + "," + JD + "&geotable_id=135675&coord_type=bd09ll&ak=yHfRk19drmAYv6YtQHOBrKtl";
            try
            {
                string queryResult = HttpClientHelper.GetResponse(queryBaiduAPI);
                JObject json = (JObject)JsonConvert.DeserializeObject(queryResult);
                if (json["message"].ToString() == "ok")
                {
                    return json["address_component"]["city"].ToString().Replace("市", "");
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                Log.Write(typeof(API_Analysis), "异常", "API_Analysis", "坐标转换成城市接口异常:" + ex.ToString());
                return "";
            }
        }
    }
}
