using System;

public enum ItemStatus { CheckedIn, CheckedOut }

abstract class LibraryItem
{
    public string Title { get; private set; }
    public string Author { get; private set; }
    public string ItemID { get; private set; }
    public ItemStatus Status { get; private set; }

    public LibraryItem(string title, string author, string itemID, ItemStatus status = ItemStatus.CheckedIn)
    {
        if (string.IsNullOrWhiteSpace(itemID))
        {
            throw new ArgumentException("ItemID cannot be empty.");
        }
        Title = title;
        Author = author;
        ItemID = itemID;
        Status = status;
    }

    public abstract void DisplayInfo();

    public virtual void CheckOut()
    {
        Console.WriteLine("=== Checkout Process ===");
        Console.WriteLine($"Attempting to check out {Title}...");

        if (Status == ItemStatus.CheckedOut)
        {
            Console.WriteLine("Error: Item is already checked out.");
        }
        else if (Status == ItemStatus.CheckedIn)
        {
            Status = ItemStatus.CheckedOut;
            Console.WriteLine($"{GetType().Name} has been successfully checked out.");
        }
        Console.WriteLine();
    }

    public virtual void Return()
    {
        Console.WriteLine("=== Return Process ===");
        Console.WriteLine($"Attempting to return {Title}...");

        if (Status == ItemStatus.CheckedIn)
        {
            Console.WriteLine("Error: Item is already checked in.");
        }
        else if (Status == ItemStatus.CheckedOut)
        {
            Status = ItemStatus.CheckedIn;
            Console.WriteLine($"{GetType().Name} has been successfully returned.");
        }
        Console.WriteLine();
    }
}

class Book : LibraryItem
{
    public string ISBN { get; private set; }
    public int NumberOfPages { get; private set; }


    public Book(string title, string author, string itemID, string isbn, int numberOfPages)
        : base(title, author, itemID)
    {
        if (numberOfPages <= 0)
        {
            throw new ArgumentException("NumberOfPages must be positive.");
        }
        ISBN = isbn;
        NumberOfPages = numberOfPages;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine("=== Book Information ===");
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Item ID: {ItemID}");
        Console.WriteLine($"ISBN: {ISBN}");
        Console.WriteLine($"Pages: {NumberOfPages}");
        Console.WriteLine($"Status: {Status}");
        Console.WriteLine();
    }
}

class Magazine : LibraryItem
{
    public int IssueNumber { get; private set; }
    public DateTime PublicationDate { get; private set; }

    public Magazine(string title, string author, string itemID, int issueNumber, DateTime publicationDate)
        : base(title, author, itemID)
    {
        IssueNumber = issueNumber;
        PublicationDate = publicationDate;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine("=== Magazine Information ===");
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Item ID: {ItemID}");
        Console.WriteLine($"Issue Number: {IssueNumber}");
        Console.WriteLine($"Publication Date: {PublicationDate}");
        Console.WriteLine($"Status: {Status}");
        Console.WriteLine();
    }
}

public class Program
{
    public static void Main()
    {
        Book book = new Book("The Great Gatsby", "F. Scott Fitzgerald", "B001", "978-0743273565", 180);
        book.DisplayInfo();

        Magazine magazine = new Magazine("National Geographic", "Various", "M001", 123, new DateTime(2024, 1, 1));
        magazine.DisplayInfo();

        book.CheckOut();
        book.DisplayInfo();
        book.CheckOut();
        book.Return();
        book.DisplayInfo();
        book.Return();
    }
}
