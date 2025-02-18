namespace Factoring.Models;

public static class GenerateFile
{

    private static int[] GetPrimes(int start, int limit)
    {
        int[] primes = [];
        var tasks = new List<Thread>();
        var results = new List<List<int>>();
        while (start <= limit)
        {
            var startPoint = start;
            var endPoint = start + 250_000_000 >= limit || start > start + 250_000_000 ? limit : start + 250_000_000;
            var list = new List<int>();
            results.Add(list);
            var thread = new Thread(() => PrimeGenerator.GeneratePrimes(startPoint, endPoint, ref list));
            tasks.Add(thread);
            start = start + 250_000_000 > start ? start + 250_000_000 : int.MaxValue;
            if (start == int.MaxValue) break;
        }
        
        tasks.ForEach(t => t.Start());
        tasks.ForEach(t => t.Join());
        results.ForEach(t => primes = primes.Concat(t).ToArray());
        return primes;
    }

    public static void WritePrimes(int start, int limit, string path)
    {
        var primes = GetPrimes(start, limit);
        using StreamWriter fileWriter = new (Directory.GetParent(Directory.GetCurrentDirectory())!
            .Parent!.Parent!.FullName + $"/{path}");
        foreach (var prime in primes)
        {
            fileWriter.WriteLine(prime.ToString());
        }
    }
}