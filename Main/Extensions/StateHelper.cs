using System.Collections.Generic;
using System.Linq;
using Model.Model;
using TDSCoreLib;
using Microsoft.EntityFrameworkCore;

namespace Main.Extensions
{
    /// <summary>
    /// 根据状态名称得到状态键值列表
    /// </summary>
    public class StateHelper
    {
        private static readonly string connectionStr = AppConfigurtaionServices.Configuration["ConnectionStrings:MySQLConnection"];
        //private static readonly string connectionStr = "Server=202.114.144.115;User Id=root;Password=tms2019hb;Database=culturedb";
        private static readonly DbContextOptions<drugdbContext> o = new DbContextOptionsBuilder<drugdbContext>().UseMySql(connectionStr).Options;
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="stateType">状态类型</param>
        /// <returns></returns>
        public static List<TDS_State> getStates(string stateType)
        {
            using (var _context = new drugdbContext(o))
            {
                var states = _context.PF_STATE.Where(m => m.TYPE == stateType && m.IS_DELETE == false).OrderBy(m => m.ORDERS).Select(field => new TDS_State(field.CODE, field.NAME)).ToList();
                return states;
            }

        }
        /// <summary>
        /// 获取表单设计器表单列表
        /// </summary>
        /// <returns></returns>
        public static List<TDS_State> getForm()
        {
            using (var _context = new drugdbContext(o))
            {
                var states = (from a in _context.FD_FORM where a.IS_DELETE == false select new TDS_State(a.KEY, a.NAME)).ToList();
                return states;
            }

        }
        /// <summary>
        /// 获取表单设计器字段列表
        /// </summary>
        /// <param name="formKey">表单KEY</param>
        /// <returns></returns>
        public static List<TDS_State> getFormField(string formKey)
        {
            using (var _context = new drugdbContext(o))
            {
                var states = (from a in _context.FD_FORM join b in _context.FD_FORM_FIELD on a.ID equals b.FORM_ID where a.KEY == formKey select new TDS_State(b.NAME, b.LABEL)).ToList();
                return states;
            }

        }
        /// <summary>
        /// 获取表单可选角色（FD_FORM_PERMISSION授权的角色）
        /// </summary>
        /// <param name="formKey"></param>
        /// <returns></returns>
        public static List<TDS_State> getFormRole(string formKey)
        {
            using (var _context = new drugdbContext(o))
            {
                var states = _context.FD_FORM_PERMISSION.Where(w => w.IS_DELETE == false && w.FORM_KEY == formKey).Select(s => new TDS_State(s.ROLE_CODE, s.ROLE_NAME)).ToList();
                return states;
            }

        }

        public static string getCodeName(string stateType, string Code)
        {
            using (var _context = new drugdbContext(o))
            {
                var name = _context.PF_STATE.Where(m => m.TYPE == stateType && m.CODE == Code && m.IS_DELETE == false).Select(field => new { field.NAME }).First();
                return name.NAME;
            }
        }
        public static string getCodeByName(string stateType, string Name)
        {
            using (var _context = new drugdbContext(o))
            {
                var codes = _context.PF_STATE.Where(m => m.TYPE == stateType && m.NAME == Name && m.IS_DELETE == false);
                if (codes.Count() > 0) return codes.FirstOrDefault().CODE;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取支持的角色列表
        /// </summary>
        /// <returns></returns>
        public static List<TDS_State> getRoles()
        {
            using (var _context = new drugdbContext(o))
            {
                var states = _context.PF_ROLE.Where(m => m.IS_DELETE == false).Select(field => new TDS_State(field.CODE, field.NAME)).ToList();
                return states;
            }
        }
    }
}
