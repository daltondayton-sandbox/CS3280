using System;
using System.Collections.Generic;

public class Program
{
    static int essentialMax = 5;
    static int essentialCount = 0;

    static string[] essentialItems = new string[essentialMax];
    static List<string> regularItems = new List<string>();

    /// <summary>
    /// Display the list of items
    /// </summary>
    /// <param name="list">1 for essential items, 2 for regular items</param>
    public static void DisplayList(int list)
    {
        if (list == 1)
        {
            Console.WriteLine("Essential Items (Fixed: 5 slots):");
            for (int i = 0; i < essentialCount; i++)
            {
                Console.WriteLine($"{i + 1}. {essentialItems[i]}");
            }

            Console.WriteLine($"[{essentialMax - essentialCount} slots remaining]\n");
        }
        else if (list == 2)
        {
            Console.WriteLine("Regular Items:");
            int i = 0;
            foreach (var item in regularItems)
            {
                i++;
                Console.WriteLine($"{i}. {item}");
            }

            Console.Write("\n");
        }
        else
        {
            Console.WriteLine("Invalid List");
        }
    }

    /// <summary>
    /// Add an item to the list
    /// </summary>
    /// <param name="list">1 for essential items, 2 for regular items</param>
    /// <param name="item">The item to add</param>
    public static void Add(int list, string item)
    {
        if (list == 1)
        {
            if (essentialCount < essentialMax)
            {
                essentialItems[essentialCount] = item;
                essentialCount++;
                Console.WriteLine("Added to Essential Items!\n");
            }
            else
            {
                Console.WriteLine("Essential list is full");
            }
        }
        else if (list == 2)
        {
            regularItems.Add(item);
            Console.WriteLine("Added to Regular Items!\n");
        }
        else
        {
            Console.WriteLine("Invalid List");
        }
    }

    /// <summary>
    /// Remove an item from the list
    /// </summary>
    /// <param name="list">1 for essential items, 2 for regular items</param>
    /// <param name="item">The item to remove</param>
    public static void Search(int list, string item)
    {
        if (list == 1)
        {
            for (int i = 0; i < essentialCount; i++)
            {
                if (essentialItems[i] == item)
                {
                    Console.WriteLine($"{item} is in the essential list");
                    return;
                }
            }
            Console.WriteLine($"{item} is not in the essential list");
            return;
        }
        else if (list == 2)
        {
            if (regularItems.Contains(item))
            {
                Console.WriteLine($"{item} is in the regular list");
            }
            else
            {
                Console.WriteLine($"{item} is not in the regular list");
                return;
            }
        }
        else
        {
            Console.WriteLine("Invalid List");
        }
    }

    /// <summary>
    /// Remove an item from the regular list
    /// </summary>
    /// <param name="item">The item to remove</param>
    public static void Remove(string item)
    {
        if (regularItems.Contains(item))
        {
            regularItems.Remove(item);
        }
    }

    /// <summary>
    /// Main method
    /// </summary>
    public static void Main()
    {
        Console.WriteLine("Shopping List Manager");
        Console.WriteLine("---------------------");
        DisplayList(1);
        DisplayList(2);

        while (true)
        {
            Console.Write("Enter command (1-Add, 2-Remove, 3-Search, 4-Display, 5-Exit): ");
            int command = Convert.ToInt32(Console.ReadLine());

            switch (command)
            {
                case 1:
                    Console.Write("Enter list (1-Essential, 2-Regular): ");
                    int list = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Enter item: ");
                    string item = Console.ReadLine() ?? throw new ArgumentNullException(nameof(item));
                    Add(list, item);
                    break;
                case 2:
                    Console.WriteLine("Removing from the regular list.");
                    Console.Write("Enter item: ");
                    item = Console.ReadLine() ?? throw new ArgumentNullException(nameof(item));
                    Remove(item);
                    break;
                case 3:
                    Console.Write("Enter list (1-Essential, 2-Regular): ");
                    list = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Enter item: ");
                    item = Console.ReadLine() ?? throw new ArgumentNullException(nameof(item));
                    Search(list, item);
                    break;
                case 4:
                    Console.Write("Enter list (1-Essential, 2-Regular, 3-Both): ");
                    list = Convert.ToInt32(Console.ReadLine());
                    if (list == 3)
                    {
                        DisplayList(1);
                        DisplayList(2);
                    }
                    else
                    {
                        DisplayList(list);
                    }
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }
    }
}
