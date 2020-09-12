using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimpleNotepad
{
    /// <summary>
    /// Логика взаимодействия для ExitDialog.xaml
    /// </summary>
    public partial class ExitDialog : Window
    {
        public bool WantsSaving { get; private set; }
        public ExitDialog()
        {
            InitializeComponent();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void QuitNoSaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            WantsSaving = false;

            Close();
        }

        private void SaveAndQuitButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            WantsSaving = true;
            Close();
        }
    }
}
