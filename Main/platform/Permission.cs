using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Model.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Main.platform
{
    public class UserORG
    {
        public UserORG(string org_gid, string org_name, string org_path, string bz2)
        {
            ORG_GID = org_gid;
            ORG_NAME = org_name;
            ORG_PATH = org_path;
            BZ2 = bz2;
        }
        public string ORG_GID { get; set; }
        public string ORG_NAME { get; set; }
        public string ORG_PATH { get; set; }
        public string BZ2 { get; set; }
    }
    public class Permission
    {

        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            //还需在startup中注入：Permission.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext HttpContext => _httpContextAccessor.HttpContext;

        public static Dictionary<string, List<string>> PDICTIONARY = new Dictionary<string, List<string>>();
        //检查当前用户是否有权限
        public static bool check(HttpContext HttpContext, String RoleOrPermission)
        {
            int count = HttpContext.User.Claims.Where(claim => claim.Value == RoleOrPermission).Count();
            if (count > 0)
            {
                return true;
            }
            //检测权限
            List<string> permissions = getCurrentUserPermissions();
            if (permissions != null && permissions.Contains(RoleOrPermission))
            {
                return true;
            }
            return false;
        }
        //获取当前用户
        public static string getCurrentUser()
        {
            Claim claim_ = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Sid);
            if (claim_ == null)
            {
                return "Anonymous";
            }
            return claim_.Value;
        }
        //获取用户
        public static User getCurrentUserObj()
        {
            User user = new User
            {
                username = getCurrentUser(),
                roles = getCurrentUserRoles(),
                permissions = getCurrentUserPermissions(),
                PF_PROFILE = getPF_PROFILE()
            };
            return user;
        }
        //获取当前用户组织机构信息
        public static UserORG getCurrentUserOrg()
        {
            string org_gid = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "ORG_GID").Value;
            string org_name = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "ORG_NAME").Value;
            string org_path = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "ORG_PATH").Value;
            string bz2 = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "BZ2").Value;
            UserORG uo = new UserORG(org_gid, org_name, org_path, bz2);
            return uo;

        }
        //获取用户档案
        public static PF_PROFILE getPF_PROFILE()
        {
            if (HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "DA_CODE") == null)
            {
                return null;
            }
            PF_PROFILE PF_PROFILE = new PF_PROFILE
            {
                CODE = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "DA_CODE").Value,
                ZW = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "DA_ZW").Value,
                NAME = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "DA_NAME").Value,
                SEX = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "DA_SEX").Value,
                AGE = Convert.ToByte(HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "DA_AGE").Value),
                PHONE = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "DA_PHONE").Value,
                MAIL = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "DA_MAIL").Value,
                BZ = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "DA_BZ").Value,
                DLZH = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "DA_DLZH").Value,
                TXDZ = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "DA_TXDZ").Value,
                GRAH = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "DA_GRAH").Value,
                SR = DateTime.ParseExact(HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "DA_SR").Value, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture)
            };
            return PF_PROFILE;
        }
        //获取当前用户所有的角色
        public static List<string> getCurrentUserRoles()
        {
            var claims = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.Role);
            if (claims == null)
            {
                return null;
            }
            List<string> roles = new List<string>();

            foreach (var claim in claims)
            {
                roles.Add(claim.Value);
            }
            return roles;

        }
        /// <summary>
        /// 根据角色获取全部具有该角色的用户名
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="ROLE"></param>
        /// <returns></returns>
        public static List<string> getUsersByRole(drugdbContext _context, string ROLE)
        {
            var r = from a in _context.PF_ROLE join b in _context.PF_USER_ROLE on a.GID equals b.ROLE_GID join c in _context.PF_USER on b.USER_GID equals c.GID where a.CODE == ROLE select c.USERNAME;
            return r.ToList();
        }

        /// <summary>
        /// 获取角色名称
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static string getRoleName(string role)
        {

            var _context = HttpContext.RequestServices.GetService(typeof(drugdbContext)) as drugdbContext;
            var result = _context.PF_ROLE.First(s => s.CODE == role).NAME;
            return result;
        }
        /// <summary>
        /// 获取当前用户所有的权限
        /// </summary>
        /// <returns></returns>
        public static List<string> getCurrentUserPermissions()
        {
            var _context = HttpContext.RequestServices.GetService(typeof(drugdbContext)) as drugdbContext;

            string username = getCurrentUser();
            if (PDICTIONARY.ContainsKey(username))
            {
                List<string> permissions = PDICTIONARY[username];
                return permissions;
            }
            else
            {
                //获取权限
                var pf_permissions = from s in _context.PF_PERMISSION
                                     join a in _context.PF_ROLE_PERMISSION on s.GID equals a.PER_GID
                                     join b in _context.PF_ROLE on a.ROLE_GID equals b.GID
                                     join c in _context.PF_USER_ROLE on b.GID equals c.ROLE_GID
                                     join d in _context.PF_USER on c.USER_GID equals d.GID
                                     where s.IS_DELETE == false
                                     && a.IS_DELETE == false
                                     && b.IS_DELETE == false
                                     && c.IS_DELETE == false
                                     && d.IS_DELETE == false
                                     && d.USERNAME == username
                                     select new
                                     {
                                         code = s.CODE,
                                         name = s.NAME,
                                     };
                List<string> claims = new List<string>();
                if (pf_permissions != null)
                {
                    foreach (var permission in pf_permissions)
                    {
                        claims.Add(permission.code);
                        //identity.AddClaim(new Claim(ClaimTypes.Name, permission.name));
                    }
                    if (Permission.PDICTIONARY.ContainsKey(username))
                    {
                        Permission.PDICTIONARY.Remove(username);
                    }
                    Permission.PDICTIONARY.Add(username, claims);
                }
            }

            if (PDICTIONARY.ContainsKey(username))
            {
                List<string> permissions = PDICTIONARY[username];
                return permissions;
            }
            return new List<string>();
        }
        public static List<string> GetFGSYWY()
        {
            var _context = HttpContext.RequestServices.GetService(typeof(drugdbContext)) as drugdbContext;

            var result = _context.PF_USER_ORG.Where(s => s.ORG_GID == getCurrentUserOrg().ORG_GID).Select(g => g.USER_NAME);
            return result.ToList();
        }
        public static bool PasswordStrength(string password)
        {
            //空字符串强度值为0
            if (password == "") return false;
            //字符统计
            int iNum = 0, iLtt = 0, iSym = 0;
            foreach (char c in password)
            {
                if (c >= '0' && c <= '9') iNum++;
                else if (c >= 'a' && c <= 'z') iLtt++;
                else if (c >= 'A' && c <= 'Z') iLtt++;
                else iSym++;
            }
            if (iLtt == 0 && iSym == 0) return false; //纯数字密码
            if (iNum == 0 && iLtt == 0) return false; //纯符号密码
            if (iNum == 0 && iSym == 0) return false; //纯字母密码
            if (password.Length <= 8) return false; //长度不大于6的密码
            if (iLtt == 0) return true; //数字和符号构成的密码
            if (iSym == 0) return true; //数字和字母构成的密码
            if (iNum == 0) return true; //字母和符号构成的密码
            if (password.Length <= 12) return true; //长度不大于10的密码
            return true; //由数字、字母、符号构成的密码
        }
    }
}
