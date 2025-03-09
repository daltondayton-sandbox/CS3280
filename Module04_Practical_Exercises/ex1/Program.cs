using System;

public class Program
{
    public static int Add(int a, int b)
    {
        return a + b;
    }

    public static double Add(double a, double b)
    {
        return a + b;
    }

    public static decimal Add(decimal a, decimal b)
    {
        return a + b;
    }

    public static void DivideWithRemainder(int dividend, int divisor, out int quotient, out int remainder)
    {
        quotient = dividend / divisor;
        remainder = dividend % divisor;
    }

    public static void SwapNumbers(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }

    public static double CalculateAverage(params double[] numbers)
    {
        double sum = 0;
        foreach (double number in numbers)
        {
            sum += number;
        }
        return sum / numbers.Length;
    }

    public static double CalculatePower(double baseNumber, int exponent = 2)
    {
        return Math.Pow(baseNumber, exponent);
    }

    public static void Main()
    {
        // Exercise 1
        System.Console.WriteLine("Math Operations Test");
        System.Console.WriteLine("--------------------");
        System.Console.WriteLine($"Addition: 15 + 25 = {Add(15, 25)}");
        System.Console.WriteLine($"Decimal Addition: 15.5 + 25.7 = {Add(15.5, 25.7)}");

        int quotient, remainder;
        DivideWithRemainder(17, 5, out quotient, out remainder);
        System.Console.WriteLine($"Division: 17 ÷ 5 = {quotient} remainder {remainder}");

        int a = 10;
        int b = 20;
        SwapNumbers(ref a, ref b);
        System.Console.WriteLine($"Swapped Numbers: a = {a}, b = {b}");

        System.Console.WriteLine($"Average: (1, 2, 3, 4, 5) = {CalculateAverage(1, 2, 3, 4, 5)}");
        System.Console.WriteLine($"Power calculation: 2^3 = {CalculatePower(2, 3)}");
    }
}
