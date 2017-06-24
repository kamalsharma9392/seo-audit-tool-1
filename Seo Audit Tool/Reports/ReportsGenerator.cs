using System.Configuration;
using System.Data;
using System.Windows.Controls;
using Seo_Audit_Tool.Analyzers;
using Seo_Audit_Tool.Files;

namespace Seo_Audit_Tool.Reports
{
    static class ReportsGenerator
    {
        public static void GenerateReport(Analyzer analyzer, ref DataGrid ReportDataGrid)
        {
            var table = CreateReportTable(analyzer);
            ReportDataGrid.ItemsSource = table.DefaultView;  // datagrid report


            if (ConfigurationManager.AppSettings["alwaysGeneratePDFReports"].Equals("true"))
            {
                PdfGenerator generator = new PdfGenerator(); // pdf report
                generator.GeneratePdfReport(analyzer.GetPageTitle(), analyzer.GetPageUrl(), analyzer); 
            }
        }

        public static DataTable CreateReportTable(Analyzer analyzer)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Test");
            table.Columns.Add("Result");

            DataRow row = table.NewRow();
            row["Test"] = "Keyword in title";
            row["Result"] = analyzer.KeywordInTitle ? "Found" : "Not found";
            table.Rows.Add(row);

            row = table.NewRow();
            row["Test"] = "Keyword in description";
            row["Result"] = analyzer.KeywordInDescription ? "Found" : "Not found";
            table.Rows.Add(row);

            row = table.NewRow();
            row["Test"] = "Keyword in headings";
            row["Result"] = analyzer.KeywordInHeadings ? "Found" : "Not found";
            table.Rows.Add(row);

            row = table.NewRow();
            row["Test"] = "Keyword in URL";
            row["Result"] = analyzer.KeywordInUrl ? "Found" : "Not found";
            table.Rows.Add(row);

            return table;
        }
    }
}
