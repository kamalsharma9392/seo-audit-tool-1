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
            var analyzer = new Analyzer("http://www.rainymood.com/", "relax");
            Assert.IsTrue(analyzer.HasKeywordInTitle());
        }

        [Test]
        public void HasKeywordInDescriptionTest()
        {
            var analyzer = new Analyzer("http://www.rainymood.com/", "relax");
            Assert.IsTrue(analyzer.HasKeywordInDescription());
        }

        [Test]
        public void HasKeywordInHeadingsTest()
        {
            var analyzer = new Analyzer("http://www.rainymood.com/", "rainy");
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
            var analyzer = new Analyzer("http://www.rainymood.com/", "mood");
            analyzer.Analyze();
            Assert.AreEqual(analyzer.GetInternalLinksCount(), 5);
        }

        [Test]
        public void CountExternalLinksTest()
        {
            var analyzer = new Analyzer("https://www.google.com/", "google");
            analyzer.Analyze();
            Assert.AreEqual(analyzer.GetExternalLinksCount(), 22);
        }

        [Test]
        public void MeasureDomainLengthTest()
        {
            var analyzer = new Analyzer("http://www.rainymood.com/", "rain");
            Assert.AreEqual(analyzer.MeasureDomainLength(), 9);
        }
    }
}
