using System;

public class PointDistanceCalculator
{
    public void SetXYCoords(ref int x, ref int y)
    {
        Console.Write("Enter the x coordinate: ");
        x = Convert.ToInt32(Console.ReadLine());
        Console.Write("Enter the y coordinate: ");
        y = Convert.ToInt32(Console.ReadLine());
    }

    public double CalcDistFromOrigin(int x, int y)
    {
        return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
    }

    public int DetermineQuadrant(int x, int y)
    {
        if (x > 0 && y > 0)
            return 1;
        else if (x < 0 && y > 0)
            return 2;
        else if (x < 0 && y < 0)
            return 3;
        else if (x > 0 && y < 0)
            return 4;
        else
            return 0;
    }

    public void MovePoint(ref int x, ref int y)
    {
        Console.Write("Enter X offset: ");
        int xOffset = Convert.ToInt32(Console.ReadLine());
        Console.Write("Enter Y offset: ");
        int yOffset = Convert.ToInt32(Console.ReadLine());

        x += xOffset;
        y += yOffset;
    }

    public static void CalculateDetailedPosition(double x, double y, out double distanceFromXAxis, out double distanceFromYAxis)
    {
        distanceFromXAxis = Math.Abs(y);
        distanceFromYAxis = Math.Abs(x);
    }

    public static void Main()
    {
        PointDistanceCalculator pdc = new PointDistanceCalculator();

        int x = 0, y = 0;
        Console.WriteLine("Point Distance Calculator");
        Console.WriteLine("-----------------------");

        pdc.SetXYCoords(ref x, ref y);
        Console.WriteLine($"\nCurrent Position: ({x}, {y})");
        Console.WriteLine($"Distance from origin: {pdc.CalcDistFromOrigin(x, y):F1}");
        Console.WriteLine($"Point is in quadrant: {pdc.DetermineQuadrant(x, y)}");

        Console.WriteLine("\nMove point:");
        pdc.MovePoint(ref x, ref y);
        Console.WriteLine($"\nNew Position: ({x}, {y})");
        Console.WriteLine($"New distance from origin: {pdc.CalcDistFromOrigin(x, y):F1}");
        Console.WriteLine($"Point is in quadrant: {pdc.DetermineQuadrant(x, y)}");

        Console.WriteLine("\nCalculate detailed position:");
        CalculateDetailedPosition(x, y, out double distanceFromXAxis, out double distanceFromYAxis);
        Console.WriteLine($"Distance from X-axis: {distanceFromXAxis}");
        Console.WriteLine($"Distance from Y-axis: {distanceFromYAxis}");
    }
}
