using System;
using CraftingCreatureWorld.Entities;
using CraftingCreatureWorld.Services;
using CraftingCreatureWorld.UI.Menus;
using CraftingCreatureWorld.UI.Display;

namespace CraftingCreatureWorld.Core
{
    public class Game
    {
        public GameState State { get; private set; }
        private readonly GameService _gameService;
        private readonly MenuManager _menuManager;
        
        public Game(string playerName)
        {
            State = new GameState(playerName);
            _gameService = new GameService(State);
            _menuManager = new MenuManager(State);
        }
        
        public void Run()
        {
            InitializeGame();
            
            while (!State.IsGameOver)
            {
                _menuManager.DisplayMainMenu();
                
                // if the player chose to exit the game inside the menu we
                // want to break out immediately.
                if (State.IsGameOver)
                    break;
            }
            
            // Show game over message if applicable
            if (State.GameOverReason == "all_creatures_lost")
            {
                ShowGameOverMessage();
            }
        }
        
        private void InitializeGame()
        {
            // Safe clear screen before showing intro
            SafeClearConsole();
            
            ConsoleDisplay.ShowHeader("WELCOME TO CREATURE CRAFT");
            Console.WriteLine($"\nWelcome, {State.Player.Name}!");
            Console.WriteLine("\nGet ready to care for your creatures and craft amazing items!\n");
            
            // Add starter creatures
            _gameService.AddStarterCreatures();
            
            // Initialize trader with random recipe
            _gameService.InitializeTrader();
            
            Console.WriteLine("\n");
            ConsoleDisplay.ShowInfo("Press any key to begin your adventure...");
            InputHandler.WaitForKey();
        }
        
        private void ShowGameOverMessage()
        {
            SafeClearConsole();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
   _____                         ____                 
  / ____|                       / __ \                
 | |  __  __ _ _ __ ___   ___  | |  | |_   _____ _ __ 
 | | |_ |/ _` | '_ ` _ \ / _ \ | |  | \ \ / / _ \ '__|
 | |__| | (_| | | | | | |  __/ | |__| |\ V /  __/ |   
  \_____|\__,_|_| |_| |_|\___|  \____/  \_/ \___|_|   
                                                      
            ");
            Console.ResetColor();
            
            Console.WriteLine("\n");
            ConsoleDisplay.ShowHeader("A SAD ENDING");
            Console.WriteLine("\n");
            
            Console.WriteLine("As the last ember of joy fades from your creatures' eyes,");
            Console.WriteLine("you realize the terrible truth of what you've done.");
            Console.WriteLine();
            Console.WriteLine("In your relentless pursuit of coins and crafting,");
            Console.WriteLine("you forgot the most important ingredient of all: CARE.");
            Console.WriteLine();
            Console.WriteLine("Your creatures, once vibrant and full of life, now lie still.");
            Console.WriteLine("Their health depleted. Their happiness extinguished.");
            Console.WriteLine("The crafting station sits silent. The merchant has packed up and left.");
            Console.WriteLine();
            Console.WriteLine("The lesson echoes through the empty fantasy lands:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  \"Sustainability is not just about what you take,");
            Console.WriteLine("   but what you give back to those who give to you.\"");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Perhaps in another life, you'll remember to balance");
            Console.WriteLine("productivity with compassion, profit with care.");
            Console.WriteLine();
            ConsoleDisplay.ShowInfo($"\nYou survived for {State.CurrentDay} days.");
            ConsoleDisplay.ShowInfo($"You earned a total of {State.Player.Currency:C}.");
            Console.WriteLine();
            
            // Check if we can read from console
            if (!Console.IsInputRedirected)
            {
                Console.WriteLine("\nPress any key to exit...");
                SafeReadKey();
            }
            else
            {
                Console.WriteLine("\nGame Over - Exiting...");
            }
        }
        
        private void SafeReadKey()
        {
            try
            {
                if (!Console.IsInputRedirected)
                {
                    Console.ReadKey(true);
                }
            }
            catch (InvalidOperationException)
            {
                // Console input is redirected or unavailable, just continue
            }
            catch (Exception)
            {
                // Any other exception, just continue
            }
        }
        
        private void SafeClearConsole()
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