using Npgsql;

namespace Factoring.Models;

public class Factor(NpgsqlConnection database)
{
    private readonly int[] _primes = GetPrimes(database);

    public int[] FindFactors(int target)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(target, 1);
        // If the number isn't a prime, it will be divisible by a prime number smaller
        // than or equal to it's square root. So in order to avoid allocating more memory
        // for storing more primes, it's better to call the function many times, as it isn't resource intensive.
        List<int> factors = new(30);
        ReadOnlySpan<int> primesSpan = _primes;
        while (!IsPrime(target, primesSpan, out var factor))
        {
            target /= factor;
            factors.Add(factor);
        }
        if (factors.Count == 0) factors.Add(1);
        factors.Add(target);
        return factors.ToArray();
        
        bool IsPrime(int number, ReadOnlySpan<int> span, out int factor)
        {
            if (number == int.MaxValue)
            {
                factor = 0;
                return true;
            }
            
            var sqrt = (int) Math.Sqrt(number);
            
            foreach (var currentPrime in span)
            {
                if (currentPrime > sqrt) break;
                if (number % currentPrime != 0) continue;
                factor = currentPrime;
                return false;
            }
            
            if (factors.Count == 0) factors.Add(1);
            factor = 0;
            
            return true;
        }


    }

    private static int[] GetPrimes(NpgsqlConnection connection)
    {
        var primes = new int[4792];
        using var cmd = new NpgsqlCommand(
            $"SELECT * FROM primes WHERE number < {(int) Math.Sqrt(int.MaxValue)} ORDER BY number",
            connection
        );
        using var reader = cmd.ExecuteReader();
        var i = 0;
        while (reader.Read())
        {
            primes[i++] = reader.GetInt32(0);
        }
        connection.Close();
        return primes.ToArray();
    }
}