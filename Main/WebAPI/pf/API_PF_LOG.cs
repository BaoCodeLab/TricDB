using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Model;
using Main.ViewModels;
using AutoMapper;
using System.Linq.Dynamic.Core;
using TDSCoreLib;

namespace Main.WebAPI
{
    [Produces("application/json")]
    [Route("api/cda/log")]
    public class API_PF_LOG : Controller
    {
        private readonly drugdbContext _context;

        public API_PF_LOG(drugdbContext context)
        {
            _context = context;
        }

      
        [HttpGet,Route("GetLOG")]
        public ResultList<VM_PF_LOG> GetLOG([FromQuery]int page = 1, int limit = 5, string searchfield = "CZDX", string searchword = "", string field = "RZSJ", string order = "ASC")
        {
            //1、设定检索默认值
            searchfield = string.IsNullOrEmpty(searchfield) ? "CZDX" : searchfield;
            searchword = string.IsNullOrEmpty(searchword) ? "" : searchword;
            //2、执行查询
            var queryResult = _context.PF_LOG
            .Where((searchfield + ".Contains(@0)"), searchword) //like查找
            .OrderBy(field + " " + order)//按条件排序 desc asc
            .Skip((page - 1) * limit) //跳过前x项
            .Take(limit)//从当前位置开始取前x项
            .ToList();//将结果转为List类型
            //3、返回结果
            return new ResultList<VM_PF_LOG>
            {
                TotalCount = _context.PF_LOG.Where(searchfield + ".Contains(@0) ", searchword).Count(),
                Results = Mapper.Map<List<PF_LOG>, List<VM_PF_LOG>>(queryResult)
            };
        }

        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        [HttpPost,Route("saveLogs")]
        public async Task<IActionResult> saveLOG(PF_LOG log)
        {
            _context.PF_LOG.Add(log);
            await _context.SaveChangesAsync();
            return Ok();
        }


        //// GET: api/API_LOG/5
        [HttpGet,Route("GetLOGs")]
        public async Task<IActionResult> GetLOGs(string gid)
   
        {

         
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PF_LOG pF_LOG = await _context.PF_LOG.SingleOrDefaultAsync(m => m.GID == gid);

            if (pF_LOG == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<PF_LOG, VM_PF_LOG>(pF_LOG));
        }



        //private bool PF_LOGExists(string id)
        //{
        //    return _context.PF_LOG.Any(e => e.GID == id);
        //}








    }


}