using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using TDSCoreLib;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Main.Extensions;

namespace Main.Extensions
{
    public class KEGG
    {

        string _hsa_url = "http://rest.kegg.jp/get/hsa:";
        public string hsa_url
        {
            get { return this._hsa_url; } 
            set { this._hsa_url = value; }
        }

        string _pathway_url = "http://rest.kegg.jp/get/";
        public string pathway_url
        {
            get { return this._pathway_url; }
            set { this._pathway_url = value; }
        }


        public List<object> get_hsa(string hsa)
        {
            List<object> hsaJS = new List<object>();
            var getHsaUrl = hsa_url + hsa;
            string res = HttpClientHelper.GetResponse(getHsaUrl) ?? "No data";
            var hsaMatches = Regex.Matches(res, @"hsa\d{5}\s+.*\n");
            foreach (var i in hsaMatches)
            {
                string[] hsaContent = i.ToString().Split("  ");
                var hsaData = new {pathway = hsaContent[0], name = hsaContent[1]};
                hsaJS.Add(hsaData);
            }
            return hsaJS;
        }

        



 

    }
}
