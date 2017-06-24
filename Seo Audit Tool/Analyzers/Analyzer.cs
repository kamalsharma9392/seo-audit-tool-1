using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Seo_Audit_Tool.Interfaces;

namespace Seo_Audit_Tool.Analyzers
{
    public class Analyzer : IAnalyzer
    {
        private string pageTitle;
        private string pageUrl;
        private string keyword;
        private string pageSource;
        private HtmlDocument document;

        // TODO check for favicon (not very important but nice to have for branding)
        // TODO count broken links, make a list of broken links, count images without alt attribute, list images without alt attribute
        // private List<String> brokenLinksList;
        // private int brokenLinksCount;
        // private List<string> imagesWithoutAlt;
        // private int imagesWithoutAltCount;
        private int internalLinksCount;
        private int externalLinksCount;
        private int domainLength;
        private int socialLinksCount;


        public bool KeywordInTitle;
        public bool KeywordInDescription;
        public bool KeywordInHeadings;
        public bool KeywordInUrl;

        public Analyzer(string url, string keyword)
        {
            this.pageUrl = url;
            this.pageTitle = "";
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
            try
            {
                this.pageTitle = document.DocumentNode.SelectNodes("/html/head/title").First().InnerText;
                return pageTitle.ToLower().Contains(keyword.ToLower());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return false;
            }
        }

        public bool HasKeywordInDescription()
        {
            try
            {
                var metaDescriptionNode = document.DocumentNode.SelectSingleNode("/html/head/meta[@name=\"description\"]");
                return metaDescriptionNode.GetAttributeValue("content", "").ToLower().Contains(keyword.ToLower());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return false;
            }
        }

        public bool[] HasKeywordInHeadings()
        {
            bool[] keywordInHeadings = { false, false, false, false, false, false };
            try
            {
                var headingsXPath = "//*[self::h1 or self::h2 or self::h3 or self::h4 or self::h5 or self::h6]";
                foreach (var heading in document.DocumentNode.SelectNodes(headingsXPath))
                {
                    if (heading.InnerText.ToLower().Contains(keyword.ToLower()))
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
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return keywordInHeadings;
            }
        }

        public bool HasKeywordInUrl()
        {
            return pageUrl.ToLower().Contains(keyword.ToLower());
        }

        public int CountInternalLinks()
        {
            try
            {
                var allNodes = document.DocumentNode.SelectNodes("//a[@href]");
                var internalLinkNodes = allNodes.Where(x => !x.GetAttributeValue("href", "").Contains("http"));
                return internalLinkNodes.Count();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return 0;
            }
        }

        public int CountExternalLinks()
        {
            try
            {
                var allNodes = document.DocumentNode.SelectNodes("//a[@href]");
                var externalLinkNodes = allNodes.Where(x => x.GetAttributeValue("href", "").Contains("http"));
                return externalLinkNodes.Count();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return 0;
            }
        }

        public int MeasureDomainLength()
        {
            var domain = pageUrl.Substring(0, pageUrl.IndexOf("/", 8));
            if (!domain.Contains("www."))
            {
                domain = domain.Substring(domain.IndexOf("//") + 2);
                domain = domain.Substring(0, domain.LastIndexOf("."));
            }
            else
            {
                domain = domain.Substring(domain.IndexOf(".") + 1);
                domain = domain.Substring(0, domain.LastIndexOf("."));
            }
            return domain.Length;
        }

        public void Analyze()
        {
            this.KeywordInTitle = HasKeywordInTitle();
            this.KeywordInDescription = HasKeywordInDescription();
            this.KeywordInHeadings = HasKeywordInHeadings().Contains(true);
            this.KeywordInUrl = HasKeywordInUrl();

            this.internalLinksCount = CountInternalLinks();
            this.externalLinksCount = CountExternalLinks();
            this.domainLength = MeasureDomainLength();
        }

        // TODO implement ComputeScore() -> make a grading system (1-100) to calculate how optimized the page is

        private sealed class PageTitleRelationalComparer : Comparer<Analyzer>
        {
            public override int Compare(Analyzer x, Analyzer y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                return string.Compare(x.pageTitle, y.pageTitle, StringComparison.Ordinal);
            }
        }

        public string GetPageTitle()
        {
            return this.pageTitle;
        }

        public string GetPageUrl()
        {
            return this.pageUrl;
        }
        
    }

}