using System;
using System.Windows;
using League_Account_Manager.views;
using NLog;

namespace League_Account_Manager;

/// <summary>
///     Interaction logic for Window1.xaml
/// </summary>
public partial class Window1 : Window
{
    public Window1()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            this.DialogResult = true;
            Close();
        }
        catch (Exception exception)
        {
            LogManager.GetCurrentClassLogger().Error(exception, "Error relogging");
        }
    }

    private void Button_Close(object sender, RoutedEventArgs e)

    {
        try
        {
            Close();
        }
            catch (Exception exception)
        {
            LogManager.GetCurrentClassLogger().Error(exception, "Error relogging");
        }
    }
}