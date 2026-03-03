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
        
        public MenuManager(GameState state)
        {
            _state = state;
            _creatureMenu = new CreatureMenu(state);
            _traderMenu = new TraderMenu(state);
            _craftingMenu = new CraftingMenu(state);
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
                Console.WriteLine("4. Save Game");
                Console.WriteLine("5. Exit Game");
                
                Console.Write("\nEnter your choice (1-5): ");
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
                        SaveGame();
                        break;
                    case "5":
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