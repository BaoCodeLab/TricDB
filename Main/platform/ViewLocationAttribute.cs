using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main
{
    /// <summary>
    /// 设置视图文件位置，如/Views/BUS_RZ/Edit.cshtml
    /// </summary>
    public class ViewLocationAttribute: ResultFilterAttribute
    {
        public string Path { get; set; }
        public ViewLocationAttribute()
        {
        }
    }
}
