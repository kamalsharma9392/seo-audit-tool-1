using System;
using System.Configuration;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Seo_Audit_Tool.Analyzers;

namespace Seo_Audit_Tool.Files
{
    public static class PdfGenerator
    {
        public static void GeneratePdfReport(string pageTitle, string pageUrl, Analyzer analyzer)
        {
            try
            {
                var date = DateTime.Now.ToString("dd-MM-yyyy hh-mm");
                var filepath = ConfigurationManager.AppSettings["reportsFolder"];
                var fileName = CleanFileName($"{pageTitle} - {date}.pdf");
                var fs = new FileStream( filepath + "\\" + fileName, FileMode.Create);
                var document = new Document(PageSize.A4, 25, 25, 30, 30);
                var writer = PdfWriter.GetInstance(document, fs);

                document.AddAuthor("SEO Audit Tool");
                document.AddKeywords("On-page SEO report");
                document.AddTitle($"Report for: {pageUrl} {Environment.NewLine}");

                document.Open();

                var header = new Header("header", $"Report for: {pageUrl} {Environment.NewLine}Date: {DateTime.Now:dd-MM-yyyy hh:mm}");
                document.Add(header);

                var firstParagraph = new Paragraph("Search Engine Optimization Report");
                firstParagraph.Alignment = 1;
                firstParagraph.SpacingAfter = 10;
                LineSeparator separator = new LineSeparator();

                var pageParagraph = new Paragraph($"Page: {analyzer.GetPageUrl()}");
                var keywordParagraph = new Paragraph($"Keyword: {analyzer.GetKeyword()}");

                PdfPTable table = new PdfPTable(2);
                table.SpacingBefore = 20;

                table.AddCell("Keyword in title");
                table.AddCell(analyzer.KeywordInTitle ? "Found" : "Not found");
                table.AddCell("Keyword in description");
                table.AddCell(analyzer.KeywordInDescription ? "Found" : "Not found");
                table.AddCell("Keyword in headings");
                table.AddCell(analyzer.KeywordInHeadings ? "Found" : "Not found");
                table.AddCell("Keyword in URL");
                table.AddCell(analyzer.KeywordInUrl ? "Found" : "Not found");

                document.Add(firstParagraph);
                document.Add(separator);
                document.Add(pageParagraph);
                document.Add(keywordParagraph);
                document.Add(table);

                document.Close();
                writer.Close();
                fs.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }

        public static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }
    }
}
