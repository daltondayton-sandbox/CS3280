using System;

/*public class Program*/
/*{*/
/*    public static void Main()*/
/*    {*/
/*        // Get the temperature from the user*/
/*        Console.Write("Enter temperature: ");*/
/*        if (!double.TryParse(Console.ReadLine(), out double temp))*/
/*        {*/
/*            Console.WriteLine("Invalid temperature.");*/
/*            return;*/
/*        }*/
/**/
/*        // Get the scale from the user*/
/*        Console.WriteLine("Enter unit (C/F): ");*/
/*        string? scale = Console.ReadLine();*/
/**/
/*        // Check if null*/
/*        if (string.IsNullOrEmpty(scale))*/
/*        {*/
/*            Console.WriteLine("Enter a value. Cannot be null.");*/
/*            return;*/
/*        }*/
/**/
/*        // Convert to upper and confirm it's a valid option*/
/*        scale = scale.ToUpper();*/
/*        if (scale == "C" || scale == "F")*/
/*        {*/
/*            // create a new temp var for the conversion*/
/*            double new_temp = 0;*/
/**/
/*            // switch to confirm C or F*/
/*            switch (scale.ToUpper())*/
/*            {*/
/*                case "C":*/
/*                    new_temp = (temp * 9 / 5) + 32;*/
/*                    break;*/
/*                case "F":*/
/*                    new_temp = (temp - 32) * 5 / 9;*/
/*                    break;*/
/*                default:*/
/*                    // Good practice to default, but we're already handling values*/
/*                    // so this shouldn't trigger. If something's up, return after output*/
/*                    Console.WriteLine("Invalid unit.");*/
/*                    return;*/
/*            }*/
/**/
/*            // Write the old temp and the converted temp. Converted is formatted to match sample.*/
/*            // Ternary to display the opposite scale*/
/*            Console.WriteLine($"{temp}°{scale} is equal to {new_temp:F1}°{(scale == "C" ? "F" : "C")}");*/
/*        }*/
/*        else*/
/*        {*/
/*            // if scale isn't C or F*/
/*            Console.WriteLine("Invalid unit. Please enter 'C' or 'F'.");*/
/*        }*/
/*    }*/
/*}*/

public class Program
{
    public static void Main()
    {
        // create num
        int num = 0;

        // start loop, checking if num is greater than or equal to 0. Default value is.
        while (num >= 0)
        {
            // Get user input
            Console.Write("Enter a number (enter a negative number to stop): ");
            if (!int.TryParse(Console.ReadLine(), out num))
            {
                Console.WriteLine("Invalid input.\n");
                continue;
            }

            if (num > 0)
            {
                Console.WriteLine("The number is positive.");
                if (num % 2 == 0)
                {
                    Console.WriteLine("The number is even.");
                }
                else
                {
                    Console.WriteLine("The number is odd.");
                }
            }
            else if (num < 0)
            {
                Console.WriteLine("The number is negative.");
                if (num % 2 == 0)
                {
                    Console.WriteLine("The number is even.");
                }
                else
                {
                    Console.WriteLine("The number is odd.");
                }
            }
            else
            {
                Console.WriteLine("The number is zero.");
                Console.WriteLine("The number is even.");
            }

            // Display a new line for clarity between loops
            Console.Write("\n");
        }

        // Negative number entered, while loop exited
        Console.WriteLine("Program ended.");
    }
}
