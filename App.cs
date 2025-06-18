using System;
using System.Windows;

namespace WpfApp1
{
    public class App : Application
    {
        protected override async void OnExit(ExitEventArgs e)
        {
            // Здесь можно добавить логику, выполняемую при выходе из приложения
            base.OnExit(e); // Вызов базовой реализации метода
        }
    }
}