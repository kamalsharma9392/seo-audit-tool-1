using System;
using System.Data;
using System.Windows;
using Seo_Audit_Tool.Analyzers;
using Seo_Audit_Tool.Files;
using Seo_Audit_Tool.Validators;
using Seo_Audit_Tool.windows;

namespace Seo_Audit_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void analyzeButton_Click(object sender, RoutedEventArgs e)
        {
            PdfGenerator generator = new PdfGenerator();
            generator.GeneratePdfReport("Random site", "http://itextpdf.com/tags/net", new Analyzer("",""));

            var urlValidator = new UrlValidator();
            if (urlValidator.IsValid(UrlTextBox.Text))
            {
                // TODO refactor code below, remove duplicate code
                var analyzer = new Analyzer(UrlTextBox.Text, KeywordTextBox.Text);
                analyzer.Analyze();
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

                ReportDataGrid.ItemsSource = table.DefaultView;
            }
        }
        private void Options_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindow optionsWindow = new OptionsWindow();
            optionsWindow.Show();
        }

        private void MenuGuide_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("1. Fill in the url" + Environment.NewLine + 
                            "2. Fill in the keyword" + Environment.NewLine + 
                            "3. Click the Analyze button", "Guide", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This application is meant to help users learn about basic on-page Search Engine Optimization", "Guide", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}