using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using League_Account_Manager.views;

namespace League_Account_Manager;

/// <summary>
///     Interaction logic for Window2.xaml
/// </summary>
public partial class Window2 : Window
{
    public Window2()
    {
        InitializeComponent();
        this.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(HandleEsc);

    }

    private void HandleEsc(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
            Close();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var lines = accountlogins.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        Page2.BulkAdd.Clear();

        if (lines.Length < 1 || string.IsNullOrWhiteSpace(lines[0]))
        {
            Close();
            return;
        }

        foreach (var line in lines)
        {
            var credentials = line.Split(":");
            if (credentials.Length >= 2)
                Page2.BulkAdd.Add(new Page2.UserNameList
                {
                    Username = credentials[0],
                    Password = credentials[1]
                });
        }

        Close();
    }
}