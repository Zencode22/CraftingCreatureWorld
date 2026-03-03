using System;
using CraftingCreatureWorld.UI.Display;

namespace CraftingCreatureWorld.UI.Menus
{
    public static class InputHandler
    {
        public static int GetChoice(int min, int max)
        {
            while (true)
            {
                Console.Write($"\nEnter your choice ({min}-{max}): ");
                string input = Console.ReadLine()?.Trim() ?? "";
                
                if (int.TryParse(input, out int choice) && choice >= min && choice <= max)
                {
                    return choice;
                }
                
                ConsoleDisplay.ShowError($"Invalid input. Please enter a number between {min} and {max}.");
            }
        }
        
        public static decimal GetPositiveDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim() ?? "";
                
                if (decimal.TryParse(input, out decimal value) && value > 0)
                {
                    return value;
                }
                
                ConsoleDisplay.ShowError("Please enter a positive number.");
            }
        }
        
        public static string GetConfirmation(string prompt)
        {
            Console.Write($"{prompt} (Y/N): ");
            return Console.ReadLine()?.Trim().ToUpper() ?? "N";
        }
        
        public static void WaitForKey()
        {
            try
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
            }
            catch (InvalidOperationException)
            {
                // Console input is redirected or unavailable, skip the wait
            }
        }
    }
}