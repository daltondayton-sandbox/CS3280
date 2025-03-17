using System;
using System.Collections.Generic;

public interface INotifiable
{
    string NotificationChannel { get; }
    void ReceiveNotification(string message);
}

public class EmailNotification : INotifiable
{
    public string NotificationChannel { get; }

    public EmailNotification(string emailAddress = "john@email.com")
    {
        NotificationChannel = emailAddress;
    }

    public void ReceiveNotification(string message)
    {
        Console.WriteLine($"EMAIL NOTIFICATION");
        Console.WriteLine($"To {NotificationChannel}");
        Console.WriteLine($"Message: {message}");
        Console.WriteLine($"Time: [{DateTime.Now:yyyy-MM-dd HH:mm:ss}]");
        Console.WriteLine($"-------------");
    }
}

public class SMSNotification : INotifiable
{
    public string NotificationChannel { get; }

    public SMSNotification(string phoneNumber = "+1234567890")
    {
        NotificationChannel = phoneNumber;
    }

    public void ReceiveNotification(string message)
    {
        Console.WriteLine($"SMS NOTIFICATION");
        Console.WriteLine($"To {NotificationChannel}");
        Console.WriteLine($"Message: {message}");
        Console.WriteLine($"Time: [{DateTime.Now:yyyy-MM-dd HH:mm:ss}]");
        Console.WriteLine($"-------------");
    }
}

public class NotificationCenter
{
    private List<INotifiable> subscribers = new List<INotifiable>();
    private List<string> logs = new List<string>();

    public event Action<string>? notification;

    public void Subscribe(INotifiable subscriber)
    {
        subscribers.Add(subscriber);
        notification += subscriber.ReceiveNotification;
        Console.WriteLine($"Added {subscriber.GetType().Name.Replace("Notification", "")} Subscriber ({subscriber.NotificationChannel})");
    }

    public void Unsubscribe(INotifiable subscriber)
    {
        subscribers.Remove(subscriber);
        notification -= subscriber.ReceiveNotification;
        Console.WriteLine($"Removed {subscriber.GetType().Name.Replace("Notification", "")} Subscriber ({subscriber.NotificationChannel})");
    }

    public void SendNotification(string message)
    {
        Console.WriteLine($"\nSending Notification: \"{message}\"");
        Console.WriteLine($"-------------");
        notification?.Invoke(message);

        int logCount = logs.Count + 1;
        foreach (var subscriber in subscribers)
        {
            logs.Add($"{logCount}. [{DateTime.Now:HH:mm:ss}] {subscriber.GetType().Name.Replace("Notification", "")} sent to {subscriber.NotificationChannel}");
            logCount++;
        }
    }

    public void PrintLogs()
    {
        Console.WriteLine("\nNotification Log:");
        foreach (var log in logs)
        {
            Console.WriteLine(log);
        }
    }
}

class Program
{
    static void Main()
    {
        EmailNotification email = new EmailNotification();
        SMSNotification sms = new SMSNotification();
        NotificationCenter center = new NotificationCenter();

        Console.WriteLine("=== Notification System Demo ===");
        Console.WriteLine("\nInitializing Notification System...");
        center.Subscribe(email);
        center.Subscribe(sms);

        center.SendNotification("System maintenance scheduled for tomorrow");
        center.SendNotification("Update completed successfully");

        center.PrintLogs();
    }
}

