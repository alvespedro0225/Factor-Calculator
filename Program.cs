using Factoring.Models;
using Npgsql;

var connectionString = Environment.GetEnvironmentVariable("DbConnectionString");
using NpgsqlConnection connection = new(connectionString);
connection.Open();
var factor = new Factor(connection);

while (true)
{
    Console.Write("Enter a number: ");
    var input = Console.ReadLine()?.Trim().Replace(",", "");
    if (!string.IsNullOrEmpty(input) && "/q".StartsWith(input))
    {
        break;
    }

    if (int.TryParse(input, out var number) && number > 1)
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
    else
        Console.WriteLine($"Invalid number! Must be an integer between 2 and {int.MaxValue}");
}