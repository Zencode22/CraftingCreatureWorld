using CraftingCreatureWorld.Core;

namespace CraftingCreatureWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Crafting Creature World";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
   ______                 _   _               _____                _   
  / _____)               | | (_)             / ____)              | |  
 | /      _ __ _   _  ___| |_ _  ___ _   _  | |     _ __ ___  __ _| |_ 
 | |     | '__| | | |/ _ \ __| |/ __| | | | | |    | '__/ _ \/ _` | __|
 | |_____| |  | |_| |  __/ |_| | (__| |_| | | |____| | |  __/ (_| | |_ 
  \______)_|   \__,_|\___|\__|_|\___|\__,_|  \_____)_|  \___|\__,_|\__|
                                                                        
            ");
            Console.ResetColor();
            Console.WriteLine("\nWelcome to a world where creatures help you gather resources!");
            Console.WriteLine("Collect creatures, earn currency, buy ingredients, and craft food to improve your creatures.\n");
            
            Console.Write("Enter your name: ");
            string playerName = Console.ReadLine()?.Trim() ?? "Adventurer";
            if (string.IsNullOrWhiteSpace(playerName)) playerName = "Adventurer";
            
            var game = new Game(playerName);
            game.Run();
            
            // Once the game loop returns we know the player has chosen to quit.
            // Write a farewell message and allow the process to exit immediately;
            // do not block on a key press since the request was to shut down the
            // application when exiting.
            Console.WriteLine("\nThanks for playing Crafting Creature World!");
            
            // If the application is being run under a debugger the console may
            // remain open automatically; otherwise Main will simply return and the
            // process will terminate.
            return;
        }
    }
}