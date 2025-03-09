using System;
using System.IO;
using System.Linq;

public class Program
{
    private static string rootDirectory = "";
    private static string sourceDirectory = "";
    private static string backupDirectory = "";
    private static string logFilePath = "";

    public static void Main()
    {
        Console.WriteLine("Enter the root directory or press Enter to use the current directory:");
        string? input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
        {
            rootDirectory = input;
        }
        else
        {
            rootDirectory = Directory.GetCurrentDirectory();
        }

        sourceDirectory = Path.Combine(rootDirectory, "source");
        backupDirectory = Path.Combine(rootDirectory, "backup");
        logFilePath = Path.Combine(backupDirectory, "backup.log");

        // Confirm the source and backup directories exist.
        if (!Directory.Exists(sourceDirectory))
        {
            Console.WriteLine("Source directory does not exist: " + sourceDirectory);
            return;
        }
        if (!Directory.Exists(backupDirectory))
        {
            Directory.CreateDirectory(backupDirectory);
        }

        using FileSystemWatcher watcher = new FileSystemWatcher(sourceDirectory)
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
        };

        watcher.Changed += OnChanged;
        watcher.Created += OnChanged;
        watcher.EnableRaisingEvents = true;

        Console.WriteLine("Monitoring directory: " + sourceDirectory);
        Console.WriteLine("Press 'r' to restore a file, or 'q' to quit.");

        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.KeyChar == 'q')
            {
                break;
            }
            else if (key.KeyChar == 'r')
            {
                RestoreFile();
            }
        }
    }

    private static void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (File.Exists(e.FullPath))
        {
            Console.WriteLine($"Detected change: {e.FullPath}");
            try
            {
                BackupFile(e.FullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during backup: " + ex.Message);
            }
        }
    }

    private static void BackupFile(string filePath)
    {
        string fileName = Path.GetFileName(filePath);
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
        string backupFileName = $"{fileName}.{timestamp}";
        string backupFilePath = Path.Combine(backupDirectory, backupFileName);

        File.Copy(filePath, backupFilePath, true);

        FileInfo fileInfo = new FileInfo(filePath);
        string fileSize = FormatFileSize(fileInfo.Length);

        string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Backup created: {fileName}\n" +
                          $"Original: {filePath}\n" +
                          $"Backup: {backupFilePath}\n" +
                          $"Size: {fileSize}\n";
        AppendLog(logEntry);
        Console.WriteLine("Backup created: " + backupFileName);

        CleanOldBackups(fileName);
    }

    private static void CleanOldBackups(string fileName)
    {
        var backups = Directory.GetFiles(backupDirectory, fileName + ".*")
            .OrderByDescending(f => File.GetCreationTime(f))
            .ToList();

        if (backups.Count > 3)
        {
            var backupsToDelete = backups.Skip(3);
            foreach (var backup in backupsToDelete)
            {
                try
                {
                    File.Delete(backup);
                    string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Old version removed: {Path.GetFileName(backup)}\n";
                    AppendLog(logEntry);
                    Console.WriteLine("Old backup removed: " + Path.GetFileName(backup));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error deleting old backup: " + ex.Message);
                }
            }
        }
    }

    private static void RestoreFile()
    {
        Console.WriteLine("Enter the file name to restore (e.g., file.txt):");
        string? fileName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(fileName))
        {
            Console.WriteLine("Invalid file name.");
            return;
        }

        var backups = Directory.GetFiles(backupDirectory, fileName + ".*")
            .OrderByDescending(f => File.GetCreationTime(f))
            .ToList();

        if (backups.Count == 0)
        {
            Console.WriteLine("No backups found for " + fileName);
            return;
        }

        Console.WriteLine("Available backups:");
        for (int i = 0; i < backups.Count; i++)
        {
            string backupName = Path.GetFileName(backups[i]);
            Console.WriteLine($"{i + 1}: {backupName}");
        }

        Console.WriteLine("Enter the number of the backup to restore:");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= backups.Count)
        {
            string selectedBackup = backups[choice - 1];
            string destinationPath = Path.Combine(sourceDirectory, fileName);
            try
            {
                File.Copy(selectedBackup, destinationPath, true);
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] File restored: {fileName}\n" +
                                  $"Restored from: {selectedBackup}\n" +
                                  $"Destination: {destinationPath}\n";
                AppendLog(logEntry);
                Console.WriteLine("File restored successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error restoring file: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }
    }

    private static void AppendLog(string entry)
    {
        try
        {
            File.AppendAllText(logFilePath, entry + "\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error writing to log file: " + ex.Message);
        }
    }

    private static string FormatFileSize(long bytes)
    {
        double size = bytes;
        string[] units = { "B", "KB", "MB", "GB", "TB" };
        int unit = 0;
        while (size >= 1024 && unit < units.Length - 1)
        {
            size /= 1024;
            unit++;
        }
        return $"{size:0.##} {units[unit]}";
    }
}

