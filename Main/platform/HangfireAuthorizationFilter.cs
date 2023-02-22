using Hangfire.Dashboard;

namespace Main.platform
{
    //Hangfire仪表盘的权限访问控制
    public class HangfireAuthorizationFilter: IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return Permission.check(context.GetHttpContext(), "admin");
        }
    }
}
