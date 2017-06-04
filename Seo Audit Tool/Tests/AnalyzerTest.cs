using NUnit.Framework;
using Seo_Audit_Tool.Analyzers;

namespace Seo_Audit_Tool.Tests
{
    [TestFixture]
    class AnalyzerTest
    {
        [Test]
        public void HasKeywordInTitleTest()
        {
            Analyzer analyzer = new Analyzer("http://www.cs.ubbcluj.ro/", "de");
            Assert.IsTrue(analyzer.HasKeywordInTitle());
        }

        [Test]
        public void HasKeywordInDescriptionTest()
        {
            Analyzer analyzer = new Analyzer("http://www.cs.ubbcluj.ro/", "de");
            Assert.IsTrue(analyzer.HasKeywordInDescription());
        }

        [Test]
        public void HasKeywordInHeadingsTest()
        {
            Analyzer analyzer = new Analyzer("http://www.cs.ubbcluj.ro/", "de");
            analyzer.Analyze();
            Assert.True(analyzer.KeywordInHeadings);
        }

        [Test]
        public void HasKeywordInUrlTest()
        {
            Analyzer analyzer = new Analyzer("http://www.cs.ubbcluj.ro/", "de");
            Assert.IsFalse(analyzer.HasKeywordInUrl());
        }
    }
}
