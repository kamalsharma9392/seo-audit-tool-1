using NUnit.Framework;
using Seo_Audit_Tool.Analyzers;

namespace Seo_Audit_Tool.Tests
{
    [TestFixture]
    public class AnalyzerTest
    {
        [Test]
        public void HasKeywordInTitleTest()
        {
            var analyzer = new Analyzer("http://www.cs.ubbcluj.ro/", "de");
            Assert.IsTrue(analyzer.HasKeywordInTitle());
        }

        [Test]
        public void HasKeywordInDescriptionTest()
        {
            var analyzer = new Analyzer("http://www.cs.ubbcluj.ro/", "de");
            Assert.IsTrue(analyzer.HasKeywordInDescription());
        }

        [Test]
        public void HasKeywordInHeadingsTest()
        {
            var analyzer = new Analyzer("http://www.cs.ubbcluj.ro/", "de");
            analyzer.Analyze();
            Assert.True(analyzer.KeywordInHeadings);
        }

        [Test]
        public void HasKeywordInUrlTest()
        {
            var analyzer = new Analyzer("http://www.cs.ubbcluj.ro/", "de");
            Assert.IsFalse(analyzer.HasKeywordInUrl());
        }

        [Test]
        public void CountInternalLinksTest()
        {
            var analyzer = new Analyzer("https://potato.io/", "potato");
            Assert.AreEqual(analyzer.GetInternalLinksCount(), 0);
        }

        [Test]
        public void CountExternalLinksTest()
        {
            var analyzer = new Analyzer("https://potato.io/", "potato");
            Assert.AreEqual(analyzer.GetExternalLinksCount(), 0);
        }

        [Test]
        public void MeasureDomainLengthTest()
        {
            var analyzer = new Analyzer("https://potato.io/", "potato");
            Assert.AreEqual(analyzer.MeasureDomainLength(), 6);
        }
    }
}
