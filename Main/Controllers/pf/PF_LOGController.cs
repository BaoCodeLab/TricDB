using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;

namespace Main.Controllers
{
    [Route("log")]
    public class PF_LOGController : Controller
    {
        readonly VM_PF_LOG viewModel = new VM_PF_LOG();
        // GET:LOG
        public IActionResult LogIndex()
        {
            return View("LogIndex", viewModel);
        }



        //[HttpGet, Route("detail")]
        //public ActionResult LogDetail(string GID)
        //{
        //    VM_PF_LOG p = new VM_PF_LOG();
        //    p.GID = GID;

        //    return View("LogDetail", p);
        //}

        //ฯ๊ว้
        [HttpGet, Route("details")]
        public ActionResult Details(string GID)
        {
            VM_PF_LOG viewModel = new VM_PF_LOG
            {
                GID = GID
            };
            return View("/Views/PF_LOG/LogDetail.cshtml", viewModel);
        }





    }
}
