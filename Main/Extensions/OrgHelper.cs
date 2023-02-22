using Main.platform;
using Microsoft.EntityFrameworkCore;
using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Main.Extensions
{
    /// <summary>
    /// 用于组织架构及其相关业务判断功能
    /// </summary>
    public static class OrgHelper
    {
        /// <summary>
        /// BZ字段中存储的是当前业务节点的组织路径信息以及状态名称，存储形式为：组织路径1|状态名称1;组织路径2|状态名称2……
        /// 根据前置组织路径获取当前业务阶段状态
        /// </summary>
        /// <param name="currentOrgPath">当前用户组织编码</param>
        /// <param name="BZ"></param>
        /// <returns></returns>
        public static string getCurrentStatus(string currentOrgPath, string BZ)
        {
            string currentStatus = "";
            if (!string.IsNullOrEmpty(BZ))
            {
                string[] statusArray = BZ.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                string next = statusArray[statusArray.Length - 1];
                string nextOrgPath = next.Split('|')[0];//当前数据可被读取的组织路径
                if (statusArray[statusArray.Length - 2].IndexOf(currentOrgPath) > -1)//如果当前组织为前置组织
                {
                    currentStatus = next.Split('|')[1];//当前数据的状态

                }
            }
            return currentStatus;
        }
        /// <summary>
        /// 获取最近的业务状态
        /// </summary>
        /// <param name="BZ"></param>
        /// <returns></returns>
        public static string getCurrentStatus(string BZ)
        {
            string currentStatus = "";
            if (!string.IsNullOrEmpty(BZ))
            {
                string[] statusArray = BZ.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                string next = statusArray[statusArray.Length - 1];
                string nextOrgPath = next.Split('|')[0];//当前数据可被读取的组织路径
                currentStatus = next.Split('|')[1];//当前数据的状态
            }
            return currentStatus;
        }
        /// <summary>
        /// 获取当前同级兄弟部门的ORG
        /// </summary>
        /// <param name="ORGCODE">兄弟部门的ORGCODE</param>
        /// <returns></returns>
        public static UserORG getSiblingORG(string ORGCODE, UserORG currentORG)
        {
            string connectionStr = AppConfigurtaionServices.Configuration["ConnectionStrings:MySQLConnection"];
            DbContextOptions<drugdbContext> o = new DbContextOptionsBuilder<drugdbContext>().UseSqlServer(connectionStr).Options;
            var _context = new drugdbContext(o) ;
            try
            {
                string super = _context.PF_ORG.Where(w => w.PATH == currentORG.ORG_PATH).First().SUPER;
                var d = _context.PF_ORG.Where(w => w.DEPTH == currentORG.ORG_PATH.Split('.',StringSplitOptions.RemoveEmptyEntries).Length && w.IS_DELETE == false && w.SUPER == super && w.CODE == ORGCODE).Select(f => new UserORG(f.GID, f.TITLE, f.PATH, f.BZ2)).First();
                return d;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                _context.Dispose();
            }
        }
        /// <summary>
        /// 根据组织路径获取其中全部用户
        /// </summary>
        /// <param name="org_path"></param>
        /// <returns></returns>
        public static List<string> getUsersByOrgPath(string org_path)
        {
            string connectionStr = AppConfigurtaionServices.Configuration["ConnectionStrings:MySQLConnection"];
            DbContextOptions<drugdbContext> o = new DbContextOptionsBuilder<drugdbContext>().UseMySql(connectionStr).Options;
            var _context = new drugdbContext(o);
            try
            {
                var result = _context.PF_USER_ORG.Where(w => w.IS_DELETE == false && w.ORG_PATH == org_path)
                    .Select(s => s.USER_NAME).ToList();
                return result;
            }
            catch (Exception) { return null; }
            finally
            {
                _context.Dispose();
            }

        }
        /// <summary>
        /// 根据组织路径获取全部当前或下级的全部用户
        /// </summary>
        /// <param name="org_path"></param>
        /// <returns></returns>
        public static List<string> getSubUsersByOrgPath(string org_path)
        {
            string connectionStr = AppConfigurtaionServices.Configuration["ConnectionStrings:MySQLConnection"];
            DbContextOptions<drugdbContext> o = new DbContextOptionsBuilder<drugdbContext>().UseSqlServer(connectionStr).Options;
            var _context = new drugdbContext(o);
            try
            {
                var result = _context.PF_USER_ORG.Where(w => w.IS_DELETE == false && w.ORG_PATH.Contains(org_path))
                    .Select(s => s.USER_NAME).ToList();
                return result;
            }
            catch (Exception) { return null; }
            finally
            {
                _context.Dispose();
            }

        }

        /// <summary>
        /// 根据用户GID获取组织路径
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string getOrgPathByUser(string user)
        {
            try
            {
                string connectionStr = AppConfigurtaionServices.Configuration["ConnectionStrings:MySQLConnection"];
                DbContextOptions<drugdbContext> o = new DbContextOptionsBuilder<drugdbContext>().UseSqlServer(connectionStr).Options;
                using (var _context = new drugdbContext(o))
                {
                    var result = _context.PF_USER_ORG.Where(w => w.IS_DELETE == false && w.USER_GID == user)
                    .First().ORG_PATH;
                    return result;
                }
            }
            catch (Exception) { return string.Empty; }
        }
    }
}
