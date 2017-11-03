using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoisonedApples
{
    class Program
    {
        static void Main(string[] args)
        {
            var applePicker = new ApplePicker();
            Console.WriteLine(applePicker.NumberOfPoisoned());
            Console.WriteLine(applePicker.NameOfSecondMostPoisonedApples());
            Console.WriteLine(applePicker.NumberOfConsecutiveNonPoisonedReds());
            Console.WriteLine(applePicker.NumberOfGreensAfterAGreen());
            Console.Read();

        }

        
    }

    public class ApplePicker
    {
        private int numOfApples = 10000;

        public int NumberOfPoisoned()
        {
            return PickApples().Take(numOfApples).Where(apple => apple.Poisoned).Count();
        }

        public string NameOfSecondMostPoisonedApples()
        {
            return PickApples()
                .Take(numOfApples)
                .Where(apple => apple.Poisoned)
                .GroupBy(apple => apple.Colour)
                .OrderByDescending(group => group.Count())
                .Skip(1)
                .First()
                .Key;
        }


        public int NumberOfGreensAfterAGreen()
        {
            return PickApples()
                .Take(numOfApples)
                .Aggregate(Tuple.Create(new Apple(), 0), (acc, nextApple) =>
                {
                    var numberOfGreensAfterGreens = acc.Item2;
                    if (acc.Item1.Colour == nextApple.Colour && acc.Item1.Colour == "Green")
                    {
                        numberOfGreensAfterGreens++;
                    }
                    return Tuple.Create(nextApple, numberOfGreensAfterGreens);
                }).Item2;
        }

        public int NumberOfConsecutiveNonPoisonedReds()
        {
            return PickApples()
                .Take(numOfApples)
                .Aggregate(Tuple.Create(0, 0), (acc, v) => UpdateStreaks(acc, v)).Item1;
        }

        private Tuple<int, int> UpdateStreaks(Tuple<int, int> acc, Apple nextApple)
        {
            var maxStreak = acc.Item1;
            var currentStreak = acc.Item2;
            if (nextApple.Colour == "Red" && !nextApple.Poisoned)
            {
                currentStreak++;
            }
            else
            {
                currentStreak = 0;
            }
            if (currentStreak > maxStreak)
            {
                maxStreak = currentStreak;
            }
            return Tuple.Create(maxStreak, currentStreak);
        }

        private IEnumerable<Apple> PickApples()
        {
            int colourIndex = 1;
            int poisonIndex = 7;

            while (true)
            {
                yield return new Apple
                {
                    Colour = GetColour(colourIndex),
                    Poisoned = poisonIndex % 41 == 0
                };

                colourIndex += 5;
                poisonIndex += 37;
            }
        }

        private string GetColour(int colourIndex)
        {
            if (colourIndex % 13 == 0 || colourIndex % 29 == 0)
            {
                return "Green";
            }

            if (colourIndex % 11 == 0 || colourIndex % 19 == 0)
            {
                return "Yellow";
            }

            return "Red";
        }

        private class Apple
        {
            public string Colour { get; set; }
            public bool Poisoned { get; set; }

            public override string ToString()
            {
                return $"{Colour} apple{(Poisoned ? " (poisoned!)" : "")}";
            }
        }
    }
}
