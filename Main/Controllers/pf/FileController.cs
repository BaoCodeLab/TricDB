using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Utils;
using Microsoft.AspNetCore.Mvc;
using Model.Model;
using TDSCoreLib;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.Controllers.pf
{
    [Route("files")]
    public class FileController : Controller
    {
        private readonly drugdbContext _context;
        public FileController(drugdbContext context)
        {

            _context = context;
        }
        [HttpGet("{GID}")]
        public IActionResult Index([FromRoute] string GID)
        {
            try
            {
                PF_FILE PF_FILE = _context.PF_FILE.SingleOrDefault(m => m.GID == GID && m.IS_DELETE == false);
                if (PF_FILE == null)
                {
                    return NotFound(new { msg = "文件不存在" });
                }
                string xdlj = PF_FILE.FILEURI;
                string jdlj = WebPath.FILE_ABSOLUTE;//绝对路径
                var stream = System.IO.File.OpenRead(jdlj + xdlj);
                if (stream == null)
                {
                    return NotFound(new { msg = "文件不存在" });
                }
                return File(stream, Helper.GetContentType(PF_FILE.TYPE.Replace(".", "")), PF_FILE.FILENAME + PF_FILE.TYPE);
            }
            catch {
                return Ok();
            }
        }
    }
}
