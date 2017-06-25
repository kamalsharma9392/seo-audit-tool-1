using System;
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
                PdfGenerator.GeneratePdfReport(analyzer.GetPageTitle(), analyzer.GetPageUrl(), analyzer);  // pdf report
                UpdateLastReportPath(analyzer);
            }

        }

        public static void GenerateReport(Analyzer analyzer)
        {
            PdfGenerator.GeneratePdfReport(analyzer.GetPageTitle(), analyzer.GetPageUrl(), analyzer);  // pdf report
            UpdateLastReportPath(analyzer);
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

        public static void UpdateLastReportPath(Analyzer analyzer)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["lastGeneratedReport"].Value =
                    PdfGenerator.CleanFileName($"{analyzer.GetPageTitle()} - {DateTime.Now:dd-MM-yyyy hh-mm}.pdf");
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error updating last report path!" + Environment.NewLine + exception.Message);
            }
        }
    }
}
