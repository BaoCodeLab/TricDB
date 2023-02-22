using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.platform;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Model.Model;
using Newtonsoft.Json;
using UEditorNetCore;

namespace Main.Areas.CMS.WebAPI
{
    [Route("api/cms/UEditor")]
    public class UEditorController : Controller
    {
        private UEditorService ue;
        public UEditorController(UEditorService ue)
        {
            this.ue = ue;
        }

        public void Do()
        {
            ue.DoAction(HttpContext);
        }
    }
}