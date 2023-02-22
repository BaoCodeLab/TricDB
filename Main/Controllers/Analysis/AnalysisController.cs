using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers.Analysis
{

    [Route("analysis")]
    public class AnalysisController : Controller
    {
        [HttpGet, Route("[action]")]
        public IActionResult sjrztj()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "Visit Log of Site";
            return View("stacked_line_chart");
        }
        [HttpGet, Route("[action]")]
        public IActionResult ddtj()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "近30日订单下达统计";
            return View("stacked_line_chart");
        }
        [HttpGet, Route("[action]")]
        public IActionResult dyqd()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "打卡统计";
            return View("calendar");
        }

        [HttpGet, Route("[action]")]
        public IActionResult ptyhsl()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "平台用户数量";
            ViewBag.Icon = "layui-icon-friends";
            ViewBag.BackColor = "blue";
            return View("num_panel");
        }
        [HttpGet, Route("[action]")]
        public IActionResult khsl()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "疾病数据";
            ViewBag.Icon = "layui-icon-friends";
            ViewBag.BackColor = "blue";
            return View("num_panel");
        }
        [HttpGet, Route("[action]")]
        public IActionResult qzkhsl()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "药物数量";
            ViewBag.Icon = "layui-icon-username";
            ViewBag.BackColor = "green";
            return View("num_panel");
        }
        [HttpGet, Route("[action]")]
        public IActionResult xsddsl()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "靶点基因突变数量";
            ViewBag.Icon = "layui-icon-template-1";
            ViewBag.BackColor = "orange";
            return View("num_panel");
        }

        [HttpGet, Route("[action]")]
        public IActionResult htzssl()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "已归档合同数量";
            ViewBag.Icon = "layui-icon-form";
            ViewBag.BackColor = "blue";
            return View("num_panel");
        }
        [HttpGet, Route("[action]")]
        public IActionResult dsphtsl()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "待审批合同数量";
            ViewBag.Icon = "layui-icon-form";
            ViewBag.BackColor = "orange";
            return View("num_panel");
        }

        [HttpGet, Route("[action]")]
        public IActionResult dclkhfk()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "待处理客户反馈";
            ViewBag.Icon = "layui-icon-face-cry";
            ViewBag.BackColor = "red";
            return View("num_panel");
        }

        [HttpGet, Route("[action]")]
        public IActionResult zyszk()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "总应收账款";
            ViewBag.Icon = "layui-icon-rmb";
            ViewBag.BackColor = "blue";
            ViewBag.FontSize = "20px";
            return View("num_panel");
        }

        [HttpGet, Route("[action]")]
        public IActionResult dkhfkxx()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "客户反馈信息列表";
            return View("table_list");
        }
        [HttpGet, Route("[action]")]
        public IActionResult xthj()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "系统环境";
            ViewBag.Name = "环境项";
            ViewBag.Value = "环境值";
            return View("table_panel");
        }

        [HttpGet, Route("[action]")]
        public IActionResult dyyjh()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "月计划发布状态";
            ViewBag.Name = "属性";
            ViewBag.Value = "属性值";
            return View("table_panel");
        }
        [HttpGet, Route("[action]")]
        public IActionResult khqyfb()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "客户区域分布";
            return View("map_china");
        }
        [HttpGet, Route("[action]")]
        public IActionResult jrywyfbt()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "今日业务员分布";
            return View("map_scatter");
        }

        [HttpGet, Route("[action]")]
        public IActionResult dyjhtjjd()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "业务员月计划提交百分比";
            return View("bar_progress");
        }


        [HttpGet, Route("[action]")]
        public IActionResult GetLatest()
        {
            ViewBag.APIAction = RouteData.Values["action"];
            ViewBag.Title = "最近新闻";
            return View("tab_card");
        }
    }
}
