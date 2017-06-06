using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Seo_Audit_Tool.Validators;

namespace Seo_Audit_Tool.Tests
{
    [TestFixture]
    public class UrlValidatorTest
    {
        [Test]
        public void IsValidTest()
        {
            var validator = new UrlValidator();
            Assert.IsFalse(validator.IsValid("htp://google.com"));
            Assert.IsFalse(validator.IsValid("www.google.com"));
            Assert.IsFalse(validator.IsValid("bing.com"));
            Assert.IsTrue(validator.IsValid("http://www.bing.com"));
            Assert.IsTrue(validator.IsValid("https://duckduckgo.com/"));
            Assert.IsTrue(validator.IsValid("https://abc.xyz/"));
        }
    }
}
