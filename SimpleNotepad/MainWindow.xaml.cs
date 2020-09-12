using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace SimpleNotepad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isSaved = true;

        private bool spellCheckEnabled = true;

        public string pathToOpenedFile;
        public MainWindow()
        {
            InitializeComponent();
            if (pathToOpenedFile == null)
            {
                SetWindowTitle();
                pathToOpenedFile = null;
            }
        }

        //Обработка выходов из приложения
        private void ExitButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (!HandleExit())
                e.Cancel = true;
        }

        //Обработка меню File
        private void SaveAsButton_OnClick(object sender, RoutedEventArgs e)
        {
            SaveAs();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private async void Save()
        {
            if (pathToOpenedFile != null)
            {
                await File.WriteAllTextAsync(pathToOpenedFile, TextBox.Text);
            }
            else
            {
                SaveAs();
            }
        }

        private async void OpenFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            //Если открытый файл не сохранён
            if (!isSaved)
            {
                //Спрашиваем пользователя, что делать дальше
                if (!HandleExit())
                    return;
            }

            //Запрашиваем открытие файла
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open File";
            openFileDialog.DefaultExt = "txt";
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    //Читаем весь текст в файле
                    string targetFileText =
                        await File.ReadAllTextAsync(openFileDialog.FileName);
                    //Записываем его в текстбокс
                    TextBox.Text = targetFileText;
                    //Записываем путь до открытого файла
                    pathToOpenedFile = openFileDialog.FileName;
                    isSaved = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message);
                }
            }
        }

        private void NewFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            //Если открытый файл не сохранён
            if (!isSaved)
            {
                //Спрашиваем пользователя, что делать дальше
                if (!HandleExit())
                {
                    return;
                }
            }

            //Открытие нового файла
            TextBox.Clear();
            isSaved = true;
            pathToOpenedFile = null;
            SetWindowTitle();
        }


        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            isSaved = false;
            SetWindowTitle();
        }

        private void SetWindowTitle()
        {
            //Если файл открыт
            if (pathToOpenedFile != null)
            {
                Title = $"Simple Notepad - { Path.GetFileName(pathToOpenedFile) }";
                
            }
            else
            {
                Title = "Simple Notepad - New file";
            }

            if (!isSaved)
                Title += "*";
        }

        /// <summary>
        /// Метод обрабатывает закрытие текстового файла перед тем, как приложение закрывается либо открывает другой файл.
        /// </summary>
        /// <returns>Возвращает true, если файл был сохранён, и можно выполнять дальнейшие действия. Возвращает false, если пользователь не хочет делать переход и возвращается к открытому файлу.</returns>
        private bool HandleExit()
        {
            //Если файл сохранён - возвращаем true. Если нет - спрашиваем пользователя, что он хочет сделать
            if (isSaved)
                return true;
            //Если текст пуст, и при этом это не сохранённый файл на диске - то пропускаем остальное
            if (TextBox.Text.Length == 0 && pathToOpenedFile == null)
                return true;
            
            ExitDialog exitDialog = new ExitDialog();
            if (exitDialog.ShowDialog() == true) 
            {
                if (exitDialog.WantsSaving) //Если пользователь хочет сохранить файл - сохраняем
                {
                    //Сохраняем
                    if (pathToOpenedFile != null)
                        Save();
                    else
                    {
                        SaveAs();
                    }

                }

                //Выходим в любом случае
                return true;
            }

            //Пользователь нажал на кнопку отмены
            return false;
        }

        private async void SaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.Title = "Save As..";
            saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.FileName = "New Text File";
            if (saveFileDialog.ShowDialog() == true)
            {
                pathToOpenedFile = saveFileDialog.FileName;
                await File.WriteAllTextAsync(pathToOpenedFile, TextBox.Text);
                isSaved = true;
            }

            SetWindowTitle();
        }

        private void EnableSpellCheckCheckbox_OnClick(object sender, RoutedEventArgs e)
        {
            TextBox.SpellCheck.IsEnabled = EnableSpellCheckCheckbox.IsChecked;
        }
    }
}
