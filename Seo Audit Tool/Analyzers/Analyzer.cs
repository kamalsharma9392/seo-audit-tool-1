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
        private string _pageTitle;
        private string _pageUrl;
        private string _keyword;
        private string _pageSource;
        private HtmlDocument _document;

        // TODO check for favicon (not very important but nice to have for branding)
        // TODO count broken links, make a list of broken links, count images without alt attribute, list images without alt attribute
        // private List<String> brokenLinksList;
        // private int brokenLinksCount;
        // private List<string> imagesWithoutAlt;
        // private int imagesWithoutAltCount;
        public List<string> InternalLinks { get; private set; }
        public List<string> ExternalLinks { get; private set; }
        private int _domainLength;
        public List<string> SocialLinks { get; private set; }


        public bool KeywordInTitle;
        public bool KeywordInDescription;
        public bool KeywordInHeadings;
        public bool KeywordInUrl;

        public Analyzer(string url, string keyword)
        {
            this._pageUrl = url;
            this._pageTitle = "";
            this._document = new HtmlDocument();
            this._keyword = keyword.ToLower();
            this.SocialLinks = new List<string>();

            this.KeywordInTitle = false;
            this.KeywordInDescription = false;
            this.KeywordInHeadings = false;
            this.KeywordInUrl = false;

            try
            {
                WebClient webClient = new WebClient();
                // TODO add webclient proxy option
                _pageSource = webClient.DownloadString(new Uri(url));
                _document.LoadHtml(_pageSource);
                webClient.Dispose();
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
                this._pageTitle = _document.DocumentNode.SelectNodes("/html/head/title").First().InnerText;
                return _pageTitle.ToLower().Contains(_keyword.ToLower());
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
                var metaDescriptionNode = _document.DocumentNode.SelectSingleNode("/html/head/meta[@name=\"description\"]");
                return metaDescriptionNode.GetAttributeValue("content", "").ToLower().Contains(_keyword.ToLower());
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
                foreach (var heading in _document.DocumentNode.SelectNodes(headingsXPath))
                {
                    if (heading.InnerText.ToLower().Contains(_keyword.ToLower()))
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
            return _pageUrl.ToLower().Contains(_keyword.ToLower());
        }

        public int GetInternalLinksCount()
        {
            return InternalLinks.Count;
        }

        public int GetExternalLinksCount()
        {
            return ExternalLinks.Count;
        }

        public int MeasureDomainLength()
        {
            var domain = _pageUrl.Substring(0, _pageUrl.IndexOf("/", 8));
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
            this._domainLength = MeasureDomainLength();

            GetInternalLinks();
            GetExternalLinks();
            GetSocialLinks();
        }

        // TODO implement ComputeScore() -> make a grading system (1-100) to calculate how optimized the page is
        
        public string GetPageTitle()
        {
            return this._pageTitle;
        }

        public string GetPageUrl()
        {
            return this._pageUrl;
        }

        public string GetKeyword()
        {
            return this._keyword;
        }

        public void GetInternalLinks()
        {
            try
            {
                var allNodes = _document.DocumentNode.SelectNodes("//a[@href]");
                var internalLinkNodes = allNodes.Where(x => !x.GetAttributeValue("href", "").Contains("http"));
                InternalLinks = internalLinkNodes.Select(x => x.GetAttributeValue("href", null)).ToList();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }

        public void GetExternalLinks()
        {
            try
            {
                var allNodes = _document.DocumentNode.SelectNodes("//a[@href]");
                var externalLinkNodes = allNodes.Where(x => x.GetAttributeValue("href", "").Contains("http"));
                ExternalLinks = externalLinkNodes.Select(x => x.GetAttributeValue("href", null)).ToList();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }

        public void GetSocialLinks()
        {
            foreach (var link in this.ExternalLinks)
            {
                if (link.Contains("facebook.com") || link.Contains("twitter.com") || link.Contains("instagram.com") ||
                    link.Contains("pinterest.com"))
                {
                    this.SocialLinks.Add(link);   // keep only the unique ones?? -- it may have links to multiple pages on same social network
                }
            }
        }
        // TODO pie chart of internal/external links, also for live/dead links
    }

}