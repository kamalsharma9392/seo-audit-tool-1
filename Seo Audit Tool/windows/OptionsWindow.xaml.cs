using System;
using System.Configuration;
using System.Windows.Forms;

namespace Seo_Audit_Tool.windows
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow
    {
        public OptionsWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        public void LoadSettings()
        {
            try
            {
                var reportsFolder = ConfigurationManager.AppSettings["reportsFolder"].Equals("Desktop")
                        ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                        : ConfigurationManager.AppSettings["reportsFolder"];
                var alwaysGeneratePDFReports = ConfigurationManager.AppSettings["alwaysGeneratePDFReports"];
                AlwaysGeneratePdfCheckBox.IsChecked = alwaysGeneratePDFReports.Equals("true");
                ReportsFolderTextBox.Text = reportsFolder;
            }
            catch (ConfigurationErrorsException exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }

        public void SaveUserSettings()
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["alwaysGeneratePDFReports"].Value = (AlwaysGeneratePdfCheckBox.IsChecked.GetValueOrDefault(false) == false) ? "false" : "true";
                config.AppSettings.Settings["reportsFolder"].Value = ReportsFolderTextBox.Text;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (ConfigurationErrorsException exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }

        private void SaveSettings(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveUserSettings();
        }

        private void changeReportsFolderButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Choose the folder where to save reports";
                folderDialog.ShowDialog();
                ReportsFolderTextBox.Text = folderDialog.SelectedPath.Equals("") ? ConfigurationManager.AppSettings["reportsFolder"] : folderDialog.SelectedPath;
            }
        }
    }
}
