using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public struct logMessage
{
    public DateTime date;
    public string level;
    public string message;

    public logMessage(DateTime date, string level, string message)
    {
        this.date = date;
        this.level = level;
        this.message = message;
    }
}

public class Program
{
    /// <summary>
    /// Main method
    /// </summary>
    public static void Main()
    {
        List<logMessage> logs = new List<logMessage>();

        // Get filename from user
        Console.Write("Enter the log filename: ");
        string? filename = Console.ReadLine();

        if (string.IsNullOrEmpty(filename))
        {
            Console.WriteLine("Error: Filename cannot be empty");
            return;
        }

        // Verify file exists
        if (!File.Exists(filename))
        {
            Console.WriteLine($"Error: File '{filename}' not found");
            return;
        }

        try
        {
            // Read log file
            using (StreamReader reader = new StreamReader(filename, Encoding.UTF8))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    int dateEndIndex = line.IndexOf(']');
                    int levelEndIndex = line.IndexOf(']', dateEndIndex + 1);

                    if (dateEndIndex > 0 && levelEndIndex > dateEndIndex)
                    {
                        string dateString = line.Substring(1, dateEndIndex - 1);
                        string level = line.Substring(dateEndIndex + 3, levelEndIndex - dateEndIndex - 3);
                        string message = line.Substring(levelEndIndex + 2);

                        if (DateTime.TryParse(dateString, out DateTime date))
                        {
                            logs.Add(new logMessage(date, level, message));
                        }
                        else
                        {
                            Console.WriteLine($"Invalid date format: {dateString}");
                        }
                    }
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }

        SummaryReport(filename, logs);
    }

    /// <summary>
    /// Calculates and displays the log levels in the log file
    /// </summary>
    /// <param name="logs">The list of log messages</param>
    /// <returns>A dictionary of log levels and their counts</returns>
    public static Dictionary<string, int> CalculateLogLevels(List<logMessage> logs)
    {
        var logLevels = new Dictionary<string, int>();

        // Count log levels
        foreach (logMessage log in logs)
        {
            // Count log levels
            if (log.level != null)
            {
                if (logLevels.ContainsKey(log.level))
                {
                    logLevels[log.level]++;
                }
                else
                {
                    logLevels[log.level] = 1;
                }
            }
        }


        return logLevels;
    }

    /// <summary>
    /// Calculates the time span between the first and last log entries
    /// </summary>
    /// <param name="logs">The list of log messages</param>
    /// <returns>The time span between the first and last log entries</returns>
    public static TimeSpan CalculateTimeSpan(List<logMessage> logs)
    {
        if (logs.Count == 0)
        {
            Console.WriteLine("No logs to analyze");
            return TimeSpan.Zero;
        }

        DateTime firstEntry = logs.Min(log => log.date);
        DateTime lastEntry = logs.Max(log => log.date);
        TimeSpan span = lastEntry - firstEntry;

        return span;
    }

    /// <summary>
    /// Generates a summary report of the log file
    /// </summary>
    /// <param name="filename">The name of the log file</param>
    /// <param name="logs">The list of log messages</param>
    /// <returns>None</returns>
    public static void SummaryReport(string filename, List<logMessage> logs)
    {
        Console.WriteLine("\nLog Analysis Report");
        Console.WriteLine("-------------------");
        Console.WriteLine($"Analysis Date: {DateTime.Now}");
        Console.WriteLine($"Log File: {filename}\n");

        Console.WriteLine("Summary:");
        Console.WriteLine($"- Total Entries: {logs.Count}");

        var logLevels = CalculateLogLevels(logs);

        // Print log levels
        foreach (KeyValuePair<string, int> logLevel in logLevels)
        {
            Console.WriteLine($"- {logLevel.Key}: {logLevel.Value}");
        }

        var span = CalculateTimeSpan(logs);
        Console.WriteLine($"- Time Span: {span.Hours} hours, {span.Minutes} minutes, {span.Seconds} seconds");

        Console.WriteLine("\nError Messages:");
        int i = 0;
        foreach (logMessage log in logs)
        {
            if (log.level == "ERROR")
            {
                i++;
                Console.WriteLine($"{i}. [{log.date}] {log.message}");
            }
        }

        Console.WriteLine("\nReport generated successfully!");
    }
}

