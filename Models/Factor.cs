using Npgsql;

namespace Factoring.Models;

public class Factor(NpgsqlConnection database)
{
    private readonly int[] _primes = GetPrimes(database);

    public int[] FindFactors(int target)
    {
        return IsPrime(target, out var first, out var beginning) ?
            [1, target] :
            Helper(target, first, beginning);


        bool IsPrime(int number, out int firstFactor, out int start)
        {
            var sqrt = (int) Math.Sqrt(target);
            for (var i = 0; _primes[i] <= sqrt; i++)
            {
                var currentPrime = _primes[i];
                if (number % currentPrime != 0) continue;
                firstFactor = currentPrime;
                start = i;
                return false;
            }
            firstFactor = -1;
            start = -1;
            return true;
        }

        int[] Helper(int number, int firstFactor, int start)
        {
            List<int> factors = [firstFactor];
            number /= firstFactor;
            while (number > 1)
            {
                if (number % _primes[start] != 0)
                {
                    start++;
                    continue;
                }
                factors.Add(_primes[start]);
                number /= _primes[start];
            }
            return factors.ToArray();
        }
    }

    private static int[] GetPrimes(NpgsqlConnection connection)
    {
        var primes = new int[105_097_565];
        using var cmd = new NpgsqlCommand("SELECT * FROM primes ORDER BY number", connection);
        using var reader = cmd.ExecuteReader();
        var i = 0;
        while (reader.Read())
        {
            primes[i++] = reader.GetInt32(0);
        }
        Console.WriteLine($"{primes.Length}, {primes[0]}, {primes[1]}");
        connection.Close();
        return primes;
    }
}