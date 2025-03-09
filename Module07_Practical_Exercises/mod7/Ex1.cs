using System;

public class Program
{
    public static void Main()
    {
        string[] lines;
        try
        {
            lines = System.IO.File.ReadAllLines("grades.txt");
        }
        catch (System.IO.FileNotFoundException)
        {
            Console.WriteLine("Error: File not found.");
            return;
        }
        catch (System.IO.IOException)
        {
            Console.WriteLine("Error: File could not be read.");
            return;
        }
        int lineItem = 0;
        int validLines = 0;
        decimal sumGrades = 0;

        Console.WriteLine("Processing grades...");
        foreach (string line in lines)
        {
            if (decimal.TryParse(line, out decimal grade))
            {
                sumGrades += grade;
                validLines++;
            }
            else
            {
                Console.WriteLine($"Error on line {lineItem + 1}: Invalid grade format '{line}'");
            }
            lineItem++;
        }

        Console.WriteLine("\nSummary:");
        Console.WriteLine($"Total grades processed: {lineItem}");
        Console.WriteLine($"Valid grades: {validLines}");
        decimal average = sumGrades / validLines;
        Console.WriteLine($"Average grade: {average}");
    }
}
