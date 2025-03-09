using System;

public class Program
{
    public static double getMedian(List<int> list)
    {
        int count = list.Count;
        if (count == 0)
        {
            return 0;
        }
        list.Sort();
        if (count % 2 == 0)
        {
            return (list[count / 2 - 1] + list[count / 2]) / 2.0;
        }
        else
        {
            return list[count / 2];
        }
    }

    public static void Main()
    {
        string cont = "y";

        do
        {
            Console.WriteLine("Number Statistics Calculator");
            Console.WriteLine("----------------------------");
            Console.WriteLine("Enter numbers (type 'done' when finished)");

            Queue<int> queue = new Queue<int>();
            while (true)
            {
                Console.Write("Enter number: ");
                string input = Console.ReadLine() ?? throw new InvalidOperationException("Input cannot be null");
                if (input == "done")
                {
                    break;
                }
                else
                {
                    int number;
                    if (int.TryParse(input, out number))
                    {
                        queue.Enqueue(number);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a number or 'done' to finish.");
                    }
                }
            }
            Console.WriteLine();

            HashSet<int> hash = new HashSet<int>();
            List<int> list = new List<int>();

            while (queue.Count > 0)
            {
                int number = queue.Dequeue();
                hash.Add(number);
                list.Add(number);
            }

            Console.WriteLine("Statistics:");
            Console.WriteLine("-----------");
            Console.WriteLine($"Numbers entered: {list.Count}");
            Console.WriteLine($"Unique numbers: {hash.Count}");
            Console.WriteLine($"Average: {list.Average()}");
            Console.WriteLine($"Median: {getMedian(list):F1}");
            list.Sort();
            Console.WriteLine($"Numbers in ascending order: {string.Join(", ", list)}\n");
            Console.WriteLine("Frequency Analysis:");
            foreach (var pair in list.GroupBy(x => x).OrderBy(x => x.Key))
            {
                int count = pair.Count();
                string times = count > 1 ? "times" : "time";
                Console.WriteLine($"{pair.Key} appears {pair.Count()} {times}");
            }

            Console.Write("\nWould you like to process another set? (y/n): ");
            cont = Console.ReadLine() ?? throw new InvalidOperationException("Input cannot be null");
            Console.WriteLine();
        } while (cont == "y");
    }
}
