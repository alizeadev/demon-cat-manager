using System.Diagnostics;
using System.IO;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Newtonsoft.Json.Linq;
using NLog;
using Notification.Wpf;

namespace League_Account_Manager;

public class Updates
{
    public static async void UpdateCheck()
    {
        var updatecheck = new HttpClient();
        if (File.Exists(Path.Combine(Environment.CurrentDirectory, "temp_update.exe")))
        {
            File.Delete(Path.Combine(Environment.CurrentDirectory, "temp_update.exe"));
            await Task.Delay(500);
            Notif.NotificationManager.Show("Update!", "Demon Cat Manager was updated successfully",
                NotificationType.Notification);
            LogManager.GetCurrentClassLogger().Info("File removed");
        }

        updatecheck.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
        {
            NoCache = true
        };

        dynamic response = await updatecheck.GetAsync("https://raw.githubusercontent.com/alizeadev/demon-cat-manager/master/Version");
        var responseBody2 = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        if (responseBody2["Version"] != Assembly.GetExecutingAssembly().GetName().Version?.ToString())
        {
            string msg = "New update " + responseBody2["Version"] +
                         " is available, click here to download the new version!";
            Notif.NotificationManager.Show("Update!", msg, NotificationType.Notification,
                "WindowArea", TimeSpan.FromSeconds(10), null, null, () => UpdateAndRestart(), "Update now!",
                () => Launchupdate(), "Go to github", NotificationTextTrimType.NoTrim, 2U, true, null, null, false);
            LogManager.GetCurrentClassLogger().Info("Update available");
        }

        updatecheck.Dispose();
    }

    public static void Launchupdate()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "https://github.com/alizeadev/demon-cat-manager/releases/latest",
            UseShellExecute = true
        });
    }

    public static void FinishUpdate()
    {
        var currentExePath = Path.Combine(Environment.CurrentDirectory, "temp_update.exe");
        while (true)
            try
            {
                var processName = "League_Account_Manager.exe";

                // Find the process by name
                var processes = Process.GetProcessesByName(processName);

                if (processes.Length > 0)
                    // Terminate the process
                    foreach (var process in processes)
                        process.Kill();
                File.Copy(currentExePath, Path.Combine(Environment.CurrentDirectory, "League_Account_Manager.exe"),
                    true);
                Process.Start(new ProcessStartInfo
                {
                    FileName = Path.Combine(Environment.CurrentDirectory, "League_Account_Manager.exe"),
                    UseShellExecute = true
                });
                break;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex, "Error loading data");
            }

        Environment.Exit(1);
    }

    private async static void UpdateAndRestart()
    {
        var downloadUrl = "https://github.com/alizeadev/demon-cat-manager/releases/latest/download/League_Account_Manager.exe";
        var downloadPath = Path.Combine(Environment.CurrentDirectory, "temp_update.exe");
        var currentExePath = Environment.ProcessPath;
        var backupExePath = currentExePath + ".backup";
        try
        {
            using (var client = new HttpClient())
            {
                var fileStream = await client.GetStreamAsync(downloadUrl);
                using FileStream outputFileStream = new(downloadPath, FileMode.CreateNew);
                await fileStream.CopyToAsync(outputFileStream);
            }

            var ps = Process.Start(new ProcessStartInfo
            {
                FileName = downloadPath,
                UseShellExecute = true
            });

            Environment.Exit(1);
        }
        catch (Exception ex)
        {
            LogManager.GetCurrentClassLogger().Error(ex, "Error loading data");
        }
    }
}