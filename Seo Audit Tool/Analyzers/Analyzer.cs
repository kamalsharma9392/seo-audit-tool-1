using System;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Seo_Audit_Tool.Interfaces;

namespace Seo_Audit_Tool.Analyzers
{
    public class Analyzer : IDomAnalyzer
    {
        private string pageUrl;
        private string keyword;
        private string pageSource;
        private HtmlDocument document;

        // TODO check for favicon (not very important but nice to have for branding)
        //TODO count broken links, make a list of broken links, count images without alt attribute, list images without alt attribute
        // private List<String> brokenLinksList;
        // private int brokenLinksCount;
        // private List<string> imagesWithoutAlt;
        // private int imagesWithoutAltCount;

        public bool KeywordInTitle;
        public bool KeywordInDescription;
        public bool KeywordInHeadings;
        public bool KeywordInUrl;

        public Analyzer(string url, string keyword)
        {
            this.pageUrl = url;
            this.document = new HtmlDocument();
            this.keyword = keyword.ToLower();

            this.KeywordInTitle = false;
            this.KeywordInDescription = false;
            this.KeywordInHeadings = false;
            this.KeywordInUrl = false;

            try
            {
                WebClient webClient = new WebClient();
                // TODO add webclient proxy option
                pageSource = webClient.DownloadString(url);
                document.LoadHtml(pageSource);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }
        public bool HasKeywordInTitle()
        {
            var titleNode = document.DocumentNode.SelectNodes("/html/head/title").First();
            return titleNode.InnerText.ToLower().Contains(keyword);
        }

        public bool HasKeywordInDescription()
        {
            var metaDescriptionNode = document.DocumentNode.SelectSingleNode("/html/head/meta[@name=\"description\"]");
            return metaDescriptionNode.GetAttributeValue("content", "").ToLower().Contains(keyword);
        }

        public bool[] HasKeywordInHeadings()
        {
            bool[] keywordInHeadings = { false, false, false, false, false, false };
            var heagingsXpath = "//*[self::h1 or self::h2 or self::h3 or self::h4 or self::h5 or self::h6]";
            foreach (var heading in document.DocumentNode.SelectNodes(heagingsXpath))
            {
                if (heading.InnerText.ToLower().Contains(keyword))
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

        public bool HasKeywordInUrl()
        {
            return pageUrl.ToLower().Contains(keyword);
        }

        public void Analyze()
        {
            this.KeywordInTitle = HasKeywordInTitle();
            this.KeywordInDescription = HasKeywordInDescription();
            this.KeywordInHeadings = HasKeywordInHeadings().Contains(true);
        }

        // TODO implement ComputeScore() -> make a grading system (1-100) to calculate how optimized the page is
    }
}