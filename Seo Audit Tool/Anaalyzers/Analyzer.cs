using System;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Seo_Audit_Tool.Interfaces;

namespace Seo_Audit_Tool.Anaalyzers
{
    public class Analyzer : IDomAnalyzer
    {
        private string pageUrl;
        private string keyword;
        private string pageSource;
        private HtmlDocument document;

        public Analyzer(string url, string keyword)
        {
            this.pageUrl = url;
            this.document = new HtmlDocument();
            this.keyword = keyword;

            WebClient webClient = new WebClient();
            // TODO add webclient proxy option
            pageSource = webClient.DownloadString(url);
            document.LoadHtml(pageSource);
        }
        public bool HasKeywordInTitle()
        {
            var titleNode = document.DocumentNode.SelectNodes("/html/head/title").First();
            return titleNode.InnerText.Contains(keyword);
        }

        public bool HasKeywordInDescription()
        {
            var metaDescriptionNode = document.DocumentNode.SelectSingleNode("/html/head/meta[@name=\"description\"]");
            return metaDescriptionNode.GetAttributeValue("content", "").Contains(keyword);
        }

        public bool[] HasKeywordInHeadings()
        {
            bool[] keywordInHeadings = { false, false, false, false, false, false };
            var heagingsXpath = "//*[self::h1 or self::h2 or self::h3 or self::h4 or self::h5 or self::h6]";
            foreach (var heading in document.DocumentNode.SelectNodes(heagingsXpath))
            {
                if (heading.InnerText.Contains(keyword))
                {
                    if (heading.Name.Equals("h1"))
                    {
                        keywordInHeadings[0] = true;
                    }
                    if (heading.Name.Equals("h2"))
                    {
                        keywordInHeadings[1] = true;
                    }
                    if (heading.Name.Equals("h3"))
                    {
                        keywordInHeadings[2] = true;
                    }
                    if (heading.Name.Equals("h4"))
                    {
                        keywordInHeadings[3] = true;
                    }
                    if (heading.Name.Equals("h5"))
                    {
                        keywordInHeadings[4] = true;
                    }
                    if (heading.Name.Equals("h6"))
                    {
                        keywordInHeadings[5] = true;
                    }
                }
            }
            return keywordInHeadings;
        }
    }
}
