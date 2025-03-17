using System;
using System.Collections.Generic;

public class Calculator
{
    public delegate double Operation(double a, double b);

    public event Action<string>? CalculationPerformed;

    public double Add(double a, double b) => a + b;
    public double Subtract(double a, double b) => a - b;
    public double Multiply(double a, double b) => a * b;
    public double Divide(double a, double b)
    {
        if (b == 0)
            throw new DivideByZeroException("Division by zero");
        return a / b;
    }

    public void Calculate(double a, double b, Operation op)
    {
        string opSymbol = GetOperationSymbol(op);
        try
        {
            double result = op(a, b);
            string msg = $"{a} {opSymbol} {b} = {result}";
            Console.WriteLine(msg);

            string logMsg = $"{GetOperationName(op)}: {msg}";
            CalculationPerformed?.Invoke(logMsg);
        }
        catch (Exception ex)
        {
            string msg = $"Error: {ex.Message}";
            Console.WriteLine(msg);

            string logMsg = $"{GetOperationName(op)}: {msg}";
            CalculationPerformed?.Invoke(logMsg);
        }
    }

    private string GetOperationSymbol(Operation op)
    {
        if (op == Add) return "+";
        if (op == Subtract) return "-";
        if (op == Multiply) return "*";
        if (op == Divide) return "/";
        return "?";
    }

    private string GetOperationName(Operation op)
    {
        if (op == Add) return "Addition";
        if (op == Subtract) return "Subtraction";
        if (op == Multiply) return "Multiplication";
        if (op == Divide) return "Division";
        return "Unknown";
    }
}

public class CalculationLogger
{
    List<string> history = new List<string>();

    public void LogCalculation(string msg)
    {
        history.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {msg}");
    }

    public void DisplayHistory()
    {
        foreach (var entry in history)
        {
            Console.WriteLine(entry);
        }
    }

    public void DisplayStatistics()
    {
        int totalCalculations = history.Count;
        int successfulCalculations = 0;
        int failedCalculations = 0;

        Dictionary<string, int> operationCounts = new Dictionary<string, int>();

        foreach (var entry in history)
        {
            if (entry.Contains("Error"))
                failedCalculations++;
            else
                successfulCalculations++;

            string[] parts = entry.Split(':');
            if (parts.Length >= 2)
            {
                string opName = parts[2].Trim().Split(' ')[1];
                if (operationCounts.ContainsKey(opName))
                    operationCounts[opName]++;
                else
                    operationCounts[opName] = 1;
            }
        }

        string mostUsedOp = "None";
        int mostUsedCount = 0;

        foreach (var pair in operationCounts)
        {
            if (pair.Value > mostUsedCount)
            {
                mostUsedCount = pair.Value;
                mostUsedOp = pair.Key;
            }
        }

        Console.WriteLine($"Total Calculations: {totalCalculations}");
        Console.WriteLine($"Successful: {successfulCalculations}");
        Console.WriteLine($"Errors: {failedCalculations}");
        Console.WriteLine($"Most Used Operation: {mostUsedOp} ({mostUsedCount} times)");
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Calculator with Event Logging Demo ===\n");
        Console.WriteLine("Initializing Calculator...");
        Calculator calc = new Calculator();
        Console.WriteLine("Calculator Ready. Available operations: Add, Subtract, Multiply, Divide\n");

        CalculationLogger logger = new CalculationLogger();
        calc.CalculationPerformed += logger.LogCalculation;

        Console.WriteLine("Performing Calculations:");
        calc.Calculate(10, 5, calc.Add);
        calc.Calculate(20, 8, calc.Subtract);
        calc.Calculate(6, 4, calc.Multiply);
        calc.Calculate(15, 3, calc.Divide);
        calc.Calculate(10, 0, calc.Divide);

        Console.WriteLine("\nCalculation History:");
        logger.DisplayHistory();

        Console.WriteLine("\nStatistics:");
        logger.DisplayStatistics();

        Console.WriteLine("\n=== End of Calculator Demo ===");
    }
}

