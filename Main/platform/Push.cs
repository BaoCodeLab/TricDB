using System;
using System.Collections.Generic;
using System.Linq;
using Jiguang.JPush;
using Jiguang.JPush.Model;
using Model.Model;
using Main.Extensions;
using Microsoft.EntityFrameworkCore;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Main.platform.weixin;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text;
using TDSCoreLib;
using MimeKit;
using MimeKit.Text;

namespace Main.platform
{
    public class Push
    {

        private static JPushClient client = new JPushClient("0b801ce5d71b063da4361f46", "d658fd7b28549edb0cee1909");
        private static readonly string connectionStr = AppConfigurtaionServices.Configuration["ConnectionStrings:MySQLConnection"];
        private static readonly string wx_appId = AppConfigurtaionServices.Configuration["AppSettings:wx_appId"];
        private static readonly string wx_secret = AppConfigurtaionServices.Configuration["AppSettings:wx_secret"];
        private static readonly DbContextOptions<drugdbContext> o = new DbContextOptionsBuilder<drugdbContext>().UseMySql(connectionStr).Options;
        private readonly drugdbContext _context;
        private Push(drugdbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 消息推送：安卓+站内+微信
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msgContent"></param>
        /// <param name="users"></param>
        /// <param name="roles"></param>
        /// <param name="url"></param>

        public static void ExecutePush(string title, string msgContent, List<string> users, List<string> roles, string url, out List<string> finalUsers)
        {

            var _context = new drugdbContext(o);
            Dictionary<String, object> dic = new Dictionary<String, object>();
            dic.Add("url", url);
            //安卓APP-极光推送
            client.SetBaseURL(JPushClient.BASE_URL_PUSH_BEIJING);
            PushPayload pushPayload = new PushPayload()
            {
                Platform = new List<string> { "android", "ios" },
                Audience = new Jiguang.JPush.Model.Audience
                {
                    Tag = roles,
                    Alias = users
                },

                Notification = new Notification
                {
                    Alert = msgContent,

                    Android = new Android
                    {
                        Title = title,
                        Extras = dic
                    }

                },
            };
            var response = client.SendPush(pushPayload);
            //查询角色对应的用户
            var queryResult = from s in _context.PF_ROLE
                              join c in _context.PF_USER_ROLE on s.GID equals c.ROLE_GID
                              join d in _context.PF_USER on c.USER_GID equals d.GID
                              where s.IS_DELETE == false
                              && c.IS_DELETE == false
                              && d.IS_DELETE == false
                              && roles.Contains(s.CODE)
                              select new
                              {
                                  code = s.CODE,
                                  name = s.NAME,
                              };

            var PF_USERs = (from s in _context.PF_USER
                            join c in _context.PF_USER_ROLE on s.GID equals c.USER_GID
                            join d in _context.PF_ROLE on c.ROLE_GID equals d.GID
                            where s.IS_DELETE == false
                            && c.IS_DELETE == false
                            && d.IS_DELETE == false
                            && roles.Contains(d.CODE)
                            select s).ToList();
            List<string> userHJ = new List<string>();
            //合并用户
            if (users != null)
            {
                userHJ = users;
            }

            if (PF_USERs != null)
            {
                foreach (var user in PF_USERs)
                {
                    if (!userHJ.Contains(user.USERNAME))
                    {
                        userHJ.Add(user.USERNAME);
                    };
                }
            }

            finalUsers = userHJ;

            if (userHJ.Count == 0)
            {
                return;
            }
            //站内推送-插入消息头
            PF_MSG pf_msg = new PF_MSG();
            pf_msg.GID = Guid.NewGuid().ToString();
            pf_msg.USERNAME = "";
            if (users != null)
            {
                pf_msg.USERNAME = string.Join(",", users.ToArray());
            }
            if (roles != null)
            {
                pf_msg.ROLE = string.Join(",", roles.ToArray());
            }
            pf_msg.TYPE = "text";
            pf_msg.TITLE = title;
            pf_msg.CONTENT = msgContent;
            pf_msg.URL = url;
            _context.Add(pf_msg);
            //循环插入数据库 消息行项目
            foreach (string username in userHJ)
            {
                PF_MSG_STATE PF_MSG_STATE = new PF_MSG_STATE();
                PF_MSG_STATE.MSG_GID = pf_msg.GID;
                PF_MSG_STATE.TITLE = title;
                PF_MSG_STATE.CONTENT = msgContent;
                PF_MSG_STATE.USERNAME = username;
                PF_MSG_STATE.URL = url;
                _context.Add(PF_MSG_STATE);
            }
            _context.SaveChangesAsync();
        }

        /// <summary>
        /// 发送站内信和app通知，同时通过其他渠道发送消息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msgContent"></param>
        /// <param name="users"></param>
        /// <param name="roles"></param>
        /// <param name="url"></param>
        /// <param name="weixin">微信，需用户已完成绑定</param>
        /// <param name="duanxin">短信，需用户已填写手机号码</param>
        /// <param name="email">邮件，需用户已填写邮箱</param>
        public static async void ExecutePush(string title, string msgContent, List<string> users, List<string> roles, string url, bool weixin = false, bool duanxin = false, bool email = false)
        {
            var _context = new drugdbContext(o);
            List<string> finalUsers = new List<string>();
            ExecutePush(title, msgContent, users, roles, url, out finalUsers);//发送站内消息

            //微信普通消息
            if (weixin == true)
            {
                AccessTokenContainer.Register(wx_appId, wx_secret);
                var user_openid_list = _context.WEIXIN_USER.Where(w => finalUsers.Contains(w.BZ2)).Select(s => s.OPENID).ToArray();//获取绑定了微信的用户清单
                if (user_openid_list.Length > 1)
                {
                    //群发
                    GroupMessageApi.SendGroupMessageByOpenId(wx_appId, Senparc.Weixin.MP.GroupMessageType.text, msgContent, null, 10000, user_openid_list);//Senparc#page.333
                }
                else if (user_openid_list.Length == 1)
                {
                    CustomApi.SendText(wx_appId, user_openid_list.First(), msgContent);
                }
            }
            //短信
            if (duanxin == true)
            {

            }
            if (email == true)
            {
            }
        }

        /// <summary>
        /// 消息推送：微信模板消息
        /// </summary>
        /// <param name="users"></param>
        /// <param name="roles"></param>
        /// <param name="data">使用platform.weixin.TemplateMessage_*创建的消息对象</param>
        public static void ExecuteWeiXinTplPush(List<string> users, List<string> roles, dynamic data)
        {

            var _context = new drugdbContext(o);
            //查询角色对应的用户
            var queryResult = from s in _context.PF_ROLE
                              join c in _context.PF_USER_ROLE on s.GID equals c.ROLE_GID
                              join d in _context.PF_USER on c.USER_GID equals d.GID
                              where s.IS_DELETE == false
                              && c.IS_DELETE == false
                              && d.IS_DELETE == false
                              && roles.Contains(s.CODE)
                              select new
                              {
                                  code = s.CODE,
                                  name = s.NAME,
                              };

            var PF_USER = (from s in _context.PF_USER
                           join c in _context.PF_USER_ROLE on s.GID equals c.USER_GID
                           join d in _context.PF_ROLE on c.ROLE_GID equals d.GID
                           where s.IS_DELETE == false
                           && c.IS_DELETE == false
                           && d.IS_DELETE == false
                           && roles.Contains(d.CODE)
                           select s).ToList();
            List<string> userHJ = new List<string>();
            //合并用户
            if (users != null)
            {
                userHJ = users;
            }

            if (PF_USER != null)
            {
                foreach (var user in PF_USER)
                {
                    if (!userHJ.Contains(user.USERNAME))
                    {
                        userHJ.Add(user.USERNAME);
                    };
                }
            }

            if (userHJ.Count == 0)
            {
                return;
            }
            //微信模板消息
            AccessTokenContainer.Register(wx_appId, wx_secret);
            var user_openid_list = _context.WEIXIN_USER.Where(w => userHJ.Contains(w.BZ2)).Select(s => s.OPENID).ToArray();//获取绑定了微信的用户清单
            foreach (var openid in user_openid_list)
            {

                Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        Task<SendTemplateMessageResult> result = await TemplateApi.SendTemplateMessageAsync(wx_appId, openid, data);
                        Log.Write(typeof(String), "微信模板消息", "PF_MSG", result.Result.errmsg + ":" + wx_appId + ":" + openid + ":" + data);
                    }
                    catch (Exception ex)
                    {
                        Log.Write(typeof(String), "微信模板消息错误", "PF_MSG", ex.ToString());

                    }
                });
            }
        }

    }
}
