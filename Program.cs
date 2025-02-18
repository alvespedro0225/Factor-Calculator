using Factoring.Models;

const string path = "/home/pedro/Documents/Code/C#/Factor-Calculator/primes.csv";
var factor = new Factor(path);

while (true)
{
    Console.Write("Enter a number: ");
    var input = Console.ReadLine()?.Trim().Replace(",", "");
    if (!string.IsNullOrEmpty(input) && "quit".StartsWith(input))
    {
        break;
    }       
    if (int.TryParse(input, out var number))
    {
        try
        {
            var factors = factor.FindFactors(number);
            Console.Write("[");
            for (var i = 0; i < factors.Length; i++)
            {
                Console.Write(factors[i]);
                 if (i < factors.Length - 1)
                 {
                     Console.Write(", ");
                 }
            }
            Console.WriteLine("]");
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine($"Invalid number! Must be an integer between 2 and {int.MaxValue}");
            
        }
    }
    else
        Console.WriteLine($"Invalid input! Must be an integer between 2 and {int.MaxValue}");
}