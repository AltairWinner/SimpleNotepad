using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleNotepad
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    
    {
        private async void App_Startup(object sender, StartupEventArgs e)
        {
            //Создаём новое окно программы
            MainWindow wnd = new MainWindow();

            //Проверяем количество аргументов
            //Пробуем открыть файл, если путь указан в аргументах
            if (e.Args.Length == 1)
            {
                try
                {
                    //Читаем весь текст в файле
                    string targetFileText = await File.ReadAllTextAsync(e.Args[0]);
                    //Записываем его в текстбокс
                    wnd.TextBox.Text = targetFileText;
                    //Записываем путь до открытого файла
                    wnd.pathToOpenedFile = e.Args[0];
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message);
                }
            }

            //Открываем приложение
            wnd.Show();
        }
    }
}
