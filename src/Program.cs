using System;
using CraftingCreatureWorld.Core;

namespace CraftingCreatureWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Title = "Creature Craft";
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@"   _____                _                   _____            __ _   
  / ____|              | |                 / ____|          / _| |  
 | |     _ __ ___  __ _| |_ _   _ _ __ ___| |     _ __ __ _| |_| |_ 
 | |    | '__/ _ \/ _` | __| | | | '__/ _ \ |    | '__/ _` |  _| __|
 | |____| | |  __/ (_| | |_| |_| | | |  __/ |____| | | (_| | | | |_ 
  \_____|_|  \___|\__,_|\__|\__,_|_|  \___|\_____|_|  \__,_|_|  \__|
                                                                    
                                                                    ");
                Console.ResetColor();
                Console.WriteLine("\nWelcome to a world where creatures help you gather resources!");
                Console.WriteLine("Collect creatures, earn currency, buy ingredients, and craft food to improve your creatures.\n");
                
                Console.Write("Enter your name: ");
                string playerName = Console.ReadLine()?.Trim() ?? "Adventurer";
                if (string.IsNullOrWhiteSpace(playerName)) playerName = "Adventurer";
                
                var game = new Game(playerName);
                game.Run();
                
                Console.WriteLine("\nThanks for playing Creature Craft!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nAn error occurred: {ex.Message}");
            }
        }
    }
}