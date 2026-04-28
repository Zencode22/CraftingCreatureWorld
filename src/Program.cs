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
                Console.WriteLine(@"
   _____                _                 _    _____            __ _   
  / ____|              | |               | |  / ____|          / _| |  
 | |     _ __ ___  __ _| |_ _   _ _ __ __| | | |     _ __ __ _| |_| |_ 
 | |    | '__/ _ \/ _` | __| | | | '__/ _` | | |    | '__/ _` |  _| __|
 | |____| | |  __/ (_| | |_| |_| | | | (_| | | |____| | | (_| | | | |_ 
  \_____|_|  \___|\__,_|\__|\__,_|_|  \__,_|  \_____|_|  \__,_|_|  \__|
                                                                       
            ");
                Console.ResetColor();
                Console.WriteLine("\nWelcome to a world where creatures help you gather resources!");
                Console.WriteLine("Collect creatures, earn currency, buy ingredients, and craft food to improve your creatures.\n");
                
                Console.Write("Enter your name: ");
                string playerName = Console.ReadLine()?.Trim() ?? "Adventurer";
                if (string.IsNullOrWhiteSpace(playerName)) playerName = "Adventurer";
                
                // Safe clear the screen before starting the game
                SafeClearConsole();
                
                var game = new Game(playerName);
                game.Run();
                
                // Safe clear screen for farewell message
                SafeClearConsole();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@"
   _____                _                 _    _____            __ _   
  / ____|              | |               | |  / ____|          / _| |  
 | |     _ __ ___  __ _| |_ _   _ _ __ __| | | |     _ __ __ _| |_| |_ 
 | |    | '__/ _ \/ _` | __| | | | '__/ _` | | |    | '__/ _` |  _| __|
 | |____| | |  __/ (_| | |_| |_| | | | (_| | | |____| | | (_| | | | |_ 
  \_____|_|  \___|\__,_|\__|\__,_|_|  \__,_|  \_____|_|  \__,_|_|  \__|
                                                                       
            ");
                Console.ResetColor();
                Console.WriteLine("\nThanks for playing Creature Craft!");
                Console.WriteLine("We hope you and your creatures had a wonderful adventure!\n");
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                Console.WriteLine($"\nAn error occurred: {ex.Message}");
            }
        }
        
        private static void SafeClearConsole()
        {
            try
            {
                if (!Console.IsOutputRedirected)
                {
                    Console.Clear();
                }
            }
            catch (IOException)
            {
                // Console output is redirected or handle is invalid, ignore
            }
            catch (Exception)
            {
                // Any other console-related error, ignore
            }
        }
    }
}