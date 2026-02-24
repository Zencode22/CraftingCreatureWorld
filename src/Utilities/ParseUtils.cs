using System;
using System.Globalization;

namespace CraftingCreatureWorld.Utilities
{
    public static class ParseUtils
    {
        public static int ConvertStringToInteger(string input)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));
            return int.Parse(input, NumberStyles.Integer, CultureInfo.InvariantCulture);
        }

        public static double ConvertStringToDouble(string input)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));
            return double.Parse(input,
                                NumberStyles.Float | NumberStyles.AllowThousands,
                                CultureInfo.InvariantCulture);
        }

        public static float ConvertStringToFloat(string input)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));
            return float.Parse(input,
                               NumberStyles.Float | NumberStyles.AllowThousands,
                               CultureInfo.InvariantCulture);
        }

        public static decimal[] ConvertCsvToDecimalArray(string csv)
        {
            if (string.IsNullOrWhiteSpace(csv))
                return Array.Empty<decimal>();

            var parts = csv.Split(',');
            var result = new decimal[parts.Length];
            for (int i = 0; i < parts.Length; i++)
                result[i] = decimal.Parse(parts[i],
                                          NumberStyles.Float | NumberStyles.AllowThousands,
                                          CultureInfo.InvariantCulture);
            return result;
        }

        public static bool TryConvertStringToInteger(string input, out int value) =>
            int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);

        public static string NormaliseKey(string raw) =>
            raw?.Trim().ToLowerInvariant() ?? string.Empty;
    }
}