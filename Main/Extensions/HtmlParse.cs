using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Main.Extensions
{
    public class HtmlParse
    {
        /// <summary>
        /// 解析出内容的所有超链接的链接
        /// </summary>
        /// <param name="cnt">文本内容</param>
        /// <returns></returns>
        public static List<string> GetAHref(string cnt)
        {
            Regex reg = new Regex(@"(?is)<a[^>]*?href=(['""]?)(?<url>[^'""\s>]+)\1[^>]*>(?<text>(?:(?!</?a\b).)*)</a>");
            MatchCollection mc = reg.Matches(cnt);
            List<string> list = new List<string>();
            foreach (Match m in mc)
            {
                list.Add(m.Groups["url"].Value);
            }
            return list;
        }
        /// <summary>
        /// 解析出内容的所有超链接的链接和键面字
        /// </summary>
        /// <param name="cnt">文本内容</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAHrefAll(string cnt)
        {
            Regex reg = new Regex(@"(?is)<a[^>]*?href=(['""]?)(?<url>[^'""\s>]+)\1[^>]*>(?<text>(?:(?!</?a\b).)*)</a>");
            MatchCollection mc = reg.Matches(cnt);
            Dictionary<string, string> list = new Dictionary<string, string>();
            foreach (Match m in mc)
            {
                if (!list.ContainsKey(m.Groups["url"].Value))
                {
                    list.Add(m.Groups["url"].Value, m.Groups["text"].Value);
                }
            }
            return list;
        }
        public static string FomartUrl(string formatter, string parm)
        {
            if (parm == null || parm.Length == 0)
            {
                return parm;
            }
            else
            {
                List<string> list = parm.Replace(" ", "").Split(',').ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = string.Format(formatter, list[i]);
                }
                return string.Join(", ", list.ToArray());
            }
        }

    }
}
