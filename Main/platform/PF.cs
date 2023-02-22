
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.platform
{
    public class PF
    {
        public static string GetLinkFromGene(string format, string gene)
        {
            string[] sdata = gene.Split(';');
            string ret = string.Empty;
            for (int i = 0; i < sdata.Length; i++)
            {
                ret += string.Format(format, sdata[i]);
            }
            return ret;
        }
    }
}
