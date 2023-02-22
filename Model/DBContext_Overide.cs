using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TDSCoreLib;

namespace Model.Model
{
    public partial class drugdbContext : DbContext
    {
        public override int SaveChanges()
        {
            var entries = from e in this.ChangeTracker.Entries()
                          where e.State != EntityState.Unchanged
                          select e;
            foreach (var entry in entries)
            {
                PropertyInfo[] props = entry.Entity.GetType().GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    //空字符串以""代替Null
                    if (prop.GetValue(entry.Entity) == null && prop.PropertyType.Name.ToLower() == "string")
                    {
                        prop.SetValue(entry.Entity, string.Empty);
                    }
                }
                switch (entry.State)
                {
                    case EntityState.Added:
                        try
                        {
                            string primaryKeyValue = entry.Metadata.FindPrimaryKey().Properties.Select(p => entry.Property(p.Name).CurrentValue.ToString()).FirstOrDefault();
                            //主键没有值
                            if (string.IsNullOrEmpty(primaryKeyValue))
                            {
                                string primaryKeyName = entry.Metadata.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();
                                entry.Entity.GetType().GetProperty(primaryKeyName).SetValue(entry.Entity, Guid.NewGuid().ToString().ToLower());
                            }
                            entry.Entity.GetType().GetProperty("CREATE_DATE").SetValue(entry.Entity, DateTime.Now);
                            entry.Entity.GetType().GetProperty("MODIFY_DATE").SetValue(entry.Entity, DateTime.Now);
                            entry.Entity.GetType().GetProperty("IS_DELETE").SetValue(entry.Entity, false);
                            entry.Entity.GetType().GetProperty("OPERATOR").SetValue(entry.Entity, _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Sid).Value);

                        }
                        catch
                        {

                        }
                        break;
                    case EntityState.Deleted:
                        break;
                    case EntityState.Modified:
                        try
                        {
                            entry.Entity.GetType().GetProperty("MODIFY_DATE").SetValue(entry.Entity, DateTime.Now);
                            //entry.Entity.GetType().GetProperty("OPERATOR").SetValue(entry.Entity, _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Sid).Value);
                        }
                        catch
                        {

                        }
                        break;
                }
            }
            return base.SaveChanges();
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var entries = from e in this.ChangeTracker.Entries()
                          where e.State != EntityState.Unchanged
                          select e;
            foreach (var entry in entries)
            {
                PropertyInfo[] props = entry.Entity.GetType().GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    //空字符串以""代替Null
                    if (prop.GetValue(entry.Entity) == null && prop.PropertyType.Name.ToLower() == "string")
                    {
                        prop.SetValue(entry.Entity, string.Empty);
                    }
                }
                switch (entry.State)
                {
                    case EntityState.Added:
                        try
                        {
                            string primaryKeyValue = entry.Metadata.FindPrimaryKey().Properties.Select(p => entry.Property(p.Name).CurrentValue.ToString()).FirstOrDefault();
                            //主键没有值
                            if (string.IsNullOrEmpty(primaryKeyValue))
                            {
                                string primaryKeyName = entry.Metadata.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();
                                entry.Entity.GetType().GetProperty(primaryKeyName).SetValue(entry.Entity, Guid.NewGuid().ToString().ToLower());
                            }
                            entry.Entity.GetType().GetProperty("CREATE_DATE").SetValue(entry.Entity, DateTime.Now);
                            entry.Entity.GetType().GetProperty("MODIFY_DATE").SetValue(entry.Entity, DateTime.Now);
                            entry.Entity.GetType().GetProperty("IS_DELETE").SetValue(entry.Entity, false);
                            entry.Entity.GetType().GetProperty("OPERATOR").SetValue(entry.Entity, _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Sid).Value);

                        }
                        catch
                        {

                        }
                        break;
                    case EntityState.Deleted:
                        break;
                    case EntityState.Modified:
                        try
                        {
                            entry.Entity.GetType().GetProperty("MODIFY_DATE").SetValue(entry.Entity, DateTime.Now);
                            //entry.Entity.GetType().GetProperty("OPERATOR").SetValue(entry.Entity, _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Sid).Value);
                        }
                        catch
                        {

                        }
                        break;
                }
            }
            return (await base.SaveChangesAsync(true, cancellationToken));
        }

        /// <summary>
        /// 根据Viewmodel对数据进行处理，只更新AllowModify字段
        /// </summary>
        /// <typeparam name="ViewModel"></typeparam>
        /// <returns></returns>
        public int SaveChanges<ViewModel>()
        {
            var entries = from e in this.ChangeTracker.Entries()
                          where e.State != EntityState.Unchanged
                          select e;

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        try
                        {
                            entry.Context.Entry(entry.Entity).State = EntityState.Unchanged;
                            List<string> list = new List<string>();
                            PropertyInfo[] vm_props = typeof(ViewModel).GetProperties();
                            foreach (PropertyInfo p in vm_props)
                            {
                                AllowModifyAttribute isAllowModify = p.GetCustomAttribute(typeof(AllowModifyAttribute)) as AllowModifyAttribute;
                                if (isAllowModify != null)
                                {
                                    entry.Context.Entry(entry.Entity).Property(p.Name).IsModified = true;
                                }
                            }
                            entry.Entity.GetType().GetProperty("MODIFY_DATE").SetValue(entry.Entity, DateTime.Now);
                            //entry.Entity.GetType().GetProperty("OPERATOR").SetValue(entry.Entity, _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Sid).Value);

                        }
                        catch
                        {

                        }
                        break;
                }
            }
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync<ViewModel>(CancellationToken cancellationToken = default(CancellationToken))
        {
            var entries = from e in this.ChangeTracker.Entries()
                          where e.State != EntityState.Unchanged
                          select e;

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        try
                        {
                            entry.Context.Entry(entry.Entity).State = EntityState.Unchanged;
                            List<string> list = new List<string>();
                            PropertyInfo[] vm_props = typeof(ViewModel).GetProperties();
                            foreach (PropertyInfo p in vm_props)
                            {
                                AllowModifyAttribute isAllowModify = p.GetCustomAttribute(typeof(AllowModifyAttribute)) as AllowModifyAttribute;
                                if (isAllowModify != null)
                                {
                                    entry.Context.Entry(entry.Entity).Property(p.Name).IsModified = true;
                                }
                                if (p.GetType() == typeof(string)&&p.GetValue(entry) ==DBNull.Value)
                                {
                                    p.SetValue(entry, string.Empty);
                                }
                            }
                            entry.Entity.GetType().GetProperty("MODIFY_DATE").SetValue(entry.Entity, DateTime.Now);
                            //entry.Entity.GetType().GetProperty("OPERATOR").SetValue(entry.Entity, _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Sid).Value);

                        }
                        catch
                        {

                        }
                        break;
                }
            }
            return (await base.SaveChangesAsync(true, cancellationToken));
        }
    }
}
