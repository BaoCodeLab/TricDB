using Main.platform;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using TDSCoreLib;

namespace Main.Extensions
{
    public class BaseNcbi
    {
        string _base_url = "https://eutils.ncbi.nlm.nih.gov/entrez/eutils/";
        public string base_url { get { return this._base_url; } set { this._base_url = value; } }
        string _api_key = "59cb69596ed88a80ec8189e2809251996e08";
        public string api_key { get { return this._api_key; } set { this._api_key = value; } }
        string _term = "cyanobacteria[Title/Abstract] OR cyanobacterium[Title/Abstract]) AND (\"Nature*\"[Journal] OR \"Science\" [Journal] OR \"Proceedings of the National Academy of Sciences of the United States of America\"[Journal] OR \"Plant physiology\"[Journal] OR \"The Plant cell\"[Journal] OR \"The New phytologist\"[Journal] OR \"Metabolic engineering\"[Journal] OR \"The ISME journal\"[Journal] OR \"Nucleic acids research\"[Journal])";
        public string term { get { return this._term; } set { this._term = value; } }

        public JObject esearch(int count)
        {
            if (count < 0) count = 10;
            if (count > 200) count = 200;
            var esearch_url = this._base_url + "esearch.fcgi?db=pubmed&api_key=" + this._api_key + "&retmode=json&retmax=" + count.ToString() + "&term=" + this._term;
            var res = HttpClientHelper.GetResponse(esearch_url);
            if (string.IsNullOrEmpty(res))
            {
                res = " ";
            }
            return (JObject)JsonConvert.DeserializeObject(res);
        }
        public string efetch(int count)
        {
            var efetch_url = this._base_url + "efetch.fcgi?db=pubmed&api_key=" + this._api_key + "&retmode=xml&id=";
            var idList = esearch(count);
            var idLists = (JArray)idList["esearchresult"]["idlist"];
            foreach (var each_list in idLists)
            {
                efetch_url = efetch_url + each_list + ",";
            }
            var res = HttpClientHelper.GetResponse(efetch_url);
            return res;
        }
        public List<ArticleInfo> getRefArticleInfo(int count)
        {
            var xml_text = efetch(count);
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xml_text);
            var tree = xDoc.SelectNodes("PubmedArticleSet/PubmedArticle");
            var all_article_list = new List<ArticleInfo>();
            foreach (XmlNode r in tree)
            {
                ArticleInfo article = new ArticleInfo();
                try
                {
                    article.pmid = r.SelectSingleNode("MedlineCitation/PMID").InnerText;
                }
                catch
                //catch (Exception ex)
                {
                    article.pmid = string.Empty;
                    //Log.Write(this.GetType(), "异常", "", ex.ToString());
                }
                try
                {
                    article.pubdate = r.SelectSingleNode("MedlineCitation/Article/Journal/JournalIssue/PubDate/Year").InnerText;
                    article.pubdate += " " + r.SelectSingleNode("MedlineCitation/Article/Journal/JournalIssue/PubDate/Month").InnerText;
                    article.pubdate += " " + r.SelectSingleNode("MedlineCitation/Article/Journal/JournalIssue/PubDate/Day").InnerText;
                }
                catch
                //catch (Exception ex)
                {
                    article.pubdate = string.Empty;
                    //Log.Write(this.GetType(), "异常", "", ex.ToString());
                }
                try
                {
                    article.title = r.SelectSingleNode("MedlineCitation/Article/ArticleTitle").InnerText.Replace("                ", "").Replace("\n", "");
                }
                catch
                //catch (Exception ex)
                {
                    article.title = string.Empty;
                    //Log.Write(this.GetType(), "异常", "", ex.ToString());
                }
                try
                {
                    article.magazine = r.SelectSingleNode("MedlineCitation/Article/Journal/Title").InnerText;
                }
                catch
                //catch (Exception ex)
                {
                    article.magazine = string.Empty;
                    //Log.Write(this.GetType(), "异常", "", ex.ToString());
                }
                try
                {
                    article.medlineTA = r.SelectSingleNode("MedlineCitation/MedlineJournalInfo/MedlineTA").InnerText;
                }
                catch
                //catch (Exception ex)
                {
                    article.medlineTA = string.Empty;
                    //Log.Write(this.GetType(), "异常", "", ex.ToString());
                }
                try
                {
                    XmlNode vol = r.SelectSingleNode("MedlineCitation/Article/Journal/JournalIssue/Volume");
                    XmlNode page = r.SelectSingleNode("MedlineCitation/Article/Pagination/MedlinePgn");
                    if (vol != null && page != null)
                    {
                        article.page = vol.InnerText + ":" + page.InnerText + ".";
                    }
                    else if (page != null)
                    {
                        article.page = vol.InnerText + ".";
                    }
                    else
                    {
                        article.page = "";
                    }
                }
                catch
                //catch (Exception ex)
                {
                    article.page = string.Empty;
                    //Log.Write(this.GetType(), "异常", "", ex.ToString());
                }
                try
                {
                    XmlNodeList nodeAuthor = r.SelectNodes("MedlineCitation/Article/AuthorList/Author");
                    List<string> list = new List<string>();
                    foreach (XmlNode xNode in nodeAuthor)
                    {
                        string LastName = xNode.SelectSingleNode("LastName").InnerText;
                        string ForceName = xNode.SelectSingleNode("ForeName").InnerText;
                        string Initials = xNode.SelectSingleNode("Initials").InnerText;
                        list.Add(LastName + " " + Initials);
                    }
                    article.author = string.Join(",", list.ToArray());
                }
                catch
                //catch (Exception ex)
                {
                    article.author = string.Empty;
                    //Log.Write(this.GetType(), "异常", "", ex.ToString());
                }
                try
                {
                    article.sabstract = r.SelectSingleNode("MedlineCitation/Article/Abstract/AbstractText").InnerText;
                    if (article.sabstract.Length > 150)
                    {
                        article.sabstract = article.sabstract.Substring(0, 200) + "..";
                    }
                }
                catch
                //catch (Exception ex)
                {
                    article.sabstract = string.Empty;
                    //Log.Write(this.GetType(), "异常", "", ex.ToString());
                }
                try
                {
                    XmlNodeList ArticleIdList = r.SelectNodes("PubmedData/ArticleIdList/ArticleId");
                    article.doi = string.Empty;
                    foreach (XmlNode xNode in ArticleIdList)
                    {
                        if (xNode.Attributes["IdType"].Value == "doi")
                        {
                            article.doi = xNode.InnerText;
                        }
                        if (xNode.Attributes["IdType"].Value == "pii")
                        {
                            article.pii = xNode.InnerText;
                        }
                    }
                }
                catch
                //catch (Exception ex)
                {
                    article.doi = string.Empty;
                    //Log.Write(this.GetType(), "异常", "", ex.ToString());
                }
                all_article_list.Add(article);
            }
            return all_article_list;
        }
    }
    public class ArticleInfo
    {
        public string pmid { get; set; }
        public string pii { get; set; }
        public string page { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string medlineTA { get; set; }
        public string pubdate { get; set; }
        public string magazine { get; set; }
        public string sabstract { get; set; }
        public string doi { get; set; }
    }
}