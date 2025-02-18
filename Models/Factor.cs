namespace Factoring.Models;

public sealed class Factor(string path)
{
    private readonly int[] _primes = GetPrimes(path);

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
            
            factor = 0;
            return true;
        }


    }

    private static int[] GetPrimes(string path)
    {
        var primes = new int[4792];
        using var streamReader = new StreamReader(path); 
        var i = 0;
        while (!streamReader.EndOfStream)
        {
            string? line = streamReader.ReadLine();
            if (int.TryParse(line, out int prime))
                primes[i++] = prime;
        }
        return primes.ToArray();
    }
}