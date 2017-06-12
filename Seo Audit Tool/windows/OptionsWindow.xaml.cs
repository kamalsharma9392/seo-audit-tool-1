using System;
using System.Configuration;

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
        }

        public void LoadSettings()
        {
            var reportsPath = ConfigurationManager.AppSettings["reportsFolder"].Equals("Desktop")
                ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                : ConfigurationManager.AppSettings["reportsFolder"];

        }


    }
}
