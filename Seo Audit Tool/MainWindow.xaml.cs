﻿using System.Windows;
using Seo_Audit_Tool.Anaalyzers;

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
            Analyzer analyzer = new Analyzer(urlTextBox.Text, keywordTextBox.Text);
            analyzer.Analyze();
        }
    }
}