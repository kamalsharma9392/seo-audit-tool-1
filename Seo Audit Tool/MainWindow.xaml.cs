using System.Data;
using System.Windows;
using Seo_Audit_Tool.Anaalyzers;
using Seo_Audit_Tool.Validators;

namespace Seo_Audit_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void analyzeButton_Click(object sender, RoutedEventArgs e)
        {
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
                row["Result"] = analyzer.keywordInTitle ? "Found" : "Not found";
                table.Rows.Add(row);

                row = table.NewRow();
                row["Test"] = "Keyword in description";
                row["Result"] = analyzer.keywordInDescription ? "Found" : "Not found";
                table.Rows.Add(row);

                row = table.NewRow();
                row["Test"] = "Keyword in headings";
                row["Result"] = analyzer.keywordInHeadings ? "Found" : "Not found";
                table.Rows.Add(row);

                row = table.NewRow();
                row["Test"] = "Keyword in URL";
                row["Result"] = analyzer.keywordInUrl ? "Found" : "Not found";
                table.Rows.Add(row);

                ReportDataGrid.ItemsSource = table.DefaultView;
            }
        }
    }
}