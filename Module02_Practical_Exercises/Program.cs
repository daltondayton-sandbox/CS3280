using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        Exercise_one();
        Console.WriteLine("=================================");
        Exercise_two();
    }

    public static void Exercise_one()
    {
        Console.WriteLine("Enter the first number:");
        string? input1 = Console.ReadLine();
        if (!double.TryParse(input1, out double number1))
        {
            Console.WriteLine("Invalid input for the first number");
            return;
        }

        Console.WriteLine("Enter the second number:");
        string? input2 = Console.ReadLine();
        if (!double.TryParse(input2, out double number2))
        {
            Console.WriteLine("Invalid input for the second number");
        }

        double added = number1 + number2;
        double subtracted = number1 - number2;
        double multiplied = number1 * number2;
        double divided = number1 / number2;

        // Table of results displayed to user
        Console.WriteLine("Results:");
        Console.WriteLine($"Addition: {added}");
        Console.WriteLine($"Subtraction: {subtracted}");
        Console.WriteLine($"Multiplication: {multiplied}");
        Console.WriteLine($"Division: {divided}");
    }

    public static void Exercise_two()
    {
        int age = 20;
        double temperature = 72.5;
        char grade = 'A';
        string name = "Alice";
        bool isStudent = true;

        Console.WriteLine($"Sum of Age and Temperature: {age + temperature}");
        Console.WriteLine($"Message: {name} is {age} years old.");
        Console.WriteLine($"Age is {(age % 2 == 0 ? "even" : "odd")}");
        Console.WriteLine($"Is Student?: {isStudent}");
        Console.WriteLine($"Unicode Value of Grade: {Convert.ToInt32(grade)}");
    }
}
