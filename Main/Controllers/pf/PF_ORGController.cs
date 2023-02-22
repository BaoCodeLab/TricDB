using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model.Model;
using Main.ViewModels;

namespace Main.Controllers
{
    [Route("org")]
    public class PF_ORGController : Controller
    {
        VM_PF_ORG viewModel = new VM_PF_ORG();

        private readonly drugdbContext _context;
        public PF_ORGController(drugdbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View("Index", viewModel);
        }

        /// <summary>
        /// ������ǰ�ڵ���¼��ڵ�
        /// </summary>
        /// <param name="GID">��ǰ�ڵ��GID�����������������¼��ڵ�</param>
        /// <param name="TITLE"></param>
        /// <returns></returns>
        [HttpGet("Create")]
        public IActionResult Create(string GID)
        {
            var currentORG = _context.PF_ORG.Where(w => w.IS_DELETE == false && w.GID == GID).SingleOrDefault();
            viewModel.GID = GID;
            ViewBag.TITLE = currentORG.TITLE;
            viewModel.DEPTH = currentORG.DEPTH;
            viewModel.PATH = currentORG.PATH;
            return View("Create", viewModel);
        }

        [HttpGet("Edit")]
        public IActionResult Edit(string GID)
        {
            viewModel.GID = GID;
            return View("Edit", viewModel);
        }

    }
}
