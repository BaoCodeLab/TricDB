using Main.Extensions;
using Microsoft.EntityFrameworkCore;
using Model.Model;
using System;

namespace Main.platform
{
    public class Log
    {
        public static void Write(Type classType, string tablename, string content)
        {
            try
            {
                Write(classType, "", tablename, content);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async void Write(Type classType, string type, string tablename, string content)
        {
            string connectionStr = AppConfigurtaionServices.Configuration["ConnectionStrings:MySQLConnection"];
            DbContextOptions<drugdbContext> o = new DbContextOptionsBuilder<drugdbContext>().UseMySql(connectionStr).Options;
            var _context = new drugdbContext(o);

            try
            {

                PF_LOG pf_log = new PF_LOG
                {
                    RZLX = type,
                    CZDX = tablename,
                    LXM = classType.Namespace + ":" + classType.Name,
                    RZNR = content,
                    RZSJ = DateTime.Now
                };

                _context.PF_LOG.Add(pf_log);

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }


        public static void Write(drugdbContext context, Type classType, string type, string tablename, string content)
        {
            try
            {

                PF_LOG pf_log = new PF_LOG
                {
                    RZLX = type,
                    CZDX = tablename,
                    LXM = classType.Namespace + ":" + classType.Name,
                    RZNR = content,
                    RZSJ = DateTime.Now
                };

                context.PF_LOG.Add(pf_log);

            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
