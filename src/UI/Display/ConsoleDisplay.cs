using System;
using CraftingCreatureWorld.Entities;

namespace CraftingCreatureWorld.UI.Display
{
    public static class ConsoleDisplay
    {
        public static void ShowHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"=== {title} ===");
            Console.WriteLine(new string('=', 60));
            Console.ResetColor();
        }
        
        public static void ShowPlayerStatus(Player player)
        {
            Console.WriteLine($"\n{player.Name}'s Status:");
            Console.WriteLine($"Currency: {player.Currency:C}");
            Console.WriteLine($"Creatures: {player.Creatures.Count}");
            Console.WriteLine($"Inventory Items: {player.Inventory.Contents.Count}");
            Console.WriteLine($"Crafted Food: {player.CraftedFood.Count}");
        }
        
        public static void ShowDailyEarnings(decimal amount)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nYour creatures earned you {amount:C} today!");
            Console.ResetColor();
        }
        
        public static void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        
        public static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        
        public static void ShowWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        
        public static void ShowInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}