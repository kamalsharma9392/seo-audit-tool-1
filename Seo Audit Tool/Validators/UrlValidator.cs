using System;
using System.Net;
using Seo_Audit_Tool.Interfaces;

namespace Seo_Audit_Tool.Validators
{
    public class UrlValidator : IValidator
    {
        public bool IsValid(string url)
        {
            try
            {
                var request = WebRequest.Create(url);
                var response = ((HttpWebResponse)request.GetResponse()).StatusCode;
                return response.Equals(HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return false;
            }
        }
    }
}