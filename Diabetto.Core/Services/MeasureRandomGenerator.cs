using System;
using System.Linq;
using System.Threading;
using Diabetto.Core.Models;

namespace Diabetto.Core.Services
{
    public static class TagRandomGenerator
    {
        private static string _abc = "qwertyuiopasdfghjklzxcvbnm ";
        private static readonly Random _random = new Random();

        private static int _id = 1;

        public static Tag Next()
        {
            return new Tag
            {
                Id = Interlocked.Increment(ref _id),
                Name = new string(Enumerable
                    .Range(2, _random.Next(4, 10))
                    .Select(_ => _abc[_random.Next(0, _abc.Length)])
                    .ToArray())
            };
        }
    }

    public static class MeasureRandomGenerator
    {
        private static readonly Random _random = new Random();

        private static int _id = 1;

        public static Measure Next()
        {
            return new Measure
            {
                Id = Interlocked.Increment(ref _id),
                BreadUnits = _random.Next(0, 100) / 10.0f,
                Date = DateTime.UtcNow
                    .Date
                    .AddDays(-_random.Next(0, 3))
                    .AddHours(_random.Next(0, 24))
                    .AddMinutes(_random.Next(0, 60)),
                Level = _random.Next(40, 60),
                ShortInsulin = _random.Next(0, 3),
                LongInsulin = _random.Next(0, 20) == 0
                    ? _random.Next(5, 10)
                    : 0
            };
        }
    }
}