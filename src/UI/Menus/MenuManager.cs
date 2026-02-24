using CraftingCreatureWorld.Core;
using CraftingCreatureWorld.UI.Display;


namespace CraftingCreatureWorld.UI.Menus
{
    public class MenuManager
    {
        private readonly GameState _state;
        private readonly CreatureMenu _creatureMenu;
        private readonly TraderMenu _traderMenu;
        private readonly CraftingMenu _craftingMenu;
        private readonly FeedingMenu _feedingMenu;
        
        public MenuManager(GameState state)
        {
            _state = state;
            _creatureMenu = new CreatureMenu(state);
            _traderMenu = new TraderMenu(state);
            _craftingMenu = new CraftingMenu(state);
            _feedingMenu = new FeedingMenu(state);
        }
        
        public void DisplayMainMenu()
        {
            bool inMenu = true;
            
            while (inMenu)
            {
                ConsoleHelper.Clear();
                ConsoleDisplay.ShowHeader($"DAY {_state.CurrentDay} - {_state.Player.Name}'s Adventure");
                
                ConsoleDisplay.ShowPlayerStatus(_state.Player);
                
                Console.WriteLine("\n=== MAIN MENU ===");
                Console.WriteLine("1. Manage Your Creatures");
                Console.WriteLine("2. Visit the Trader");
                Console.WriteLine("3. Crafting Station");
                Console.WriteLine("4. Feed Your Creatures");
                Console.WriteLine("5. Save Game");
                Console.WriteLine("6. Exit Game");
                
                Console.Write("\nEnter your choice (1-6): ");
                string input = Console.ReadLine()?.Trim() ?? "";
                
                switch (input)
                {
                    case "1":
                        _creatureMenu.Show();
                        break;
                    case "2":
                        _traderMenu.Show();
                        break;
                    case "3":
                        _craftingMenu.Show();
                        break;
                    case "4":
                        _feedingMenu.Show();
                        break;
                    case "5":
                        SaveGame();
                        break;
                    case "6":
                        // user chose to exit
                        _state.IsGameOver = true;   // ensure outer game loop stops
                        inMenu = false;
                        break;
                    default:
                        ConsoleDisplay.ShowError("Invalid choice. Please try again.");
                        InputHandler.WaitForKey();
                        break;
                }
            }
        }
        

        
        private void SaveGame()
        {
            // Simple save simulation
            ConsoleDisplay.ShowSuccess("Game saved successfully!");
            InputHandler.WaitForKey();
        }
    }
}