using System;
using System.Configuration;
using System.Data;
using System.Windows;
using Seo_Audit_Tool.Analyzers;
using Seo_Audit_Tool.Files;
using Seo_Audit_Tool.Reports;
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
            // TODO  make a function using the code below
            var urlValidator = new UrlValidator();
            if (urlValidator.IsValid(UrlTextBox.Text))
            {
                var analyzer = new Analyzer(UrlTextBox.Text, KeywordTextBox.Text);
                analyzer.Analyze();
                ReportsGenerator.GenerateReport(analyzer, ref ReportDataGrid);
            }
            else
            {
                MessageBox.Show(this, "Please enter valid url/keyword!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
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

        private void MenuOpenReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var reportsFolder = ConfigurationManager.AppSettings["reportsFolder"];
                var lastReport = ConfigurationManager.AppSettings["lastGeneratedReport"];
                System.Diagnostics.Process.Start(reportsFolder + "\\" + lastReport);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            // TODO  same as analyzeButton_Click ^
            var urlValidator = new UrlValidator();
            if (urlValidator.IsValid(UrlTextBox.Text))
            {
                var analyzer = new Analyzer(UrlTextBox.Text, KeywordTextBox.Text);
                analyzer.Analyze();
                ReportsGenerator.GenerateReport(analyzer);
            }
            else
            {
                MessageBox.Show(this, "Please enter valid url/keyword!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
    }
}