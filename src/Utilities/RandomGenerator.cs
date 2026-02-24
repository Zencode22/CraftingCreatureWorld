using System;

namespace CraftingCreatureWorld.Utilities
{
    public static class RandomGenerator
    {
        private static readonly Random _random = new();
        
        public static int Next(int min, int max) => _random.Next(min, max);
        
        public static int Next(int max) => _random.Next(max);
        
        public static double NextDouble() => _random.NextDouble();
        
        public static bool NextBool(double probability = 0.5) => _random.NextDouble() < probability;
        
        public static T NextEnum<T>() where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            // GetValue returns object? so use null-forgiving operator after checking length.
            var obj = values.GetValue(_random.Next(values.Length));
            if (obj is null)
            {
                // Should never happen since index is in range, but guard for nullability.
                throw new InvalidOperationException("Unable to select a random enum value.");
            }
            return (T)obj;
        }
    }
}