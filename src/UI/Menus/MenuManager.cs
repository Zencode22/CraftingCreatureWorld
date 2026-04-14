using CraftingCreatureWorld.Core;
using CraftingCreatureWorld.Services;
using CraftingCreatureWorld.UI.Display;

namespace CraftingCreatureWorld.UI.Menus
{
    public class MenuManager
    {
        private readonly GameState _state;
        private readonly CreatureMenu _creatureMenu;
        private readonly TraderMenu _traderMenu;
        private readonly CraftingMenu _craftingMenu;
        private readonly DayService _dayService;
        
        public MenuManager(GameState state)
        {
            _state = state;
            _dayService = new DayService(state);
            _creatureMenu = new CreatureMenu(state, _dayService);
            _traderMenu = new TraderMenu(state, _dayService);
            _craftingMenu = new CraftingMenu(state, _dayService);
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
                Console.WriteLine("4. Exit Game");
                
                Console.Write("\nEnter your choice (1-4): ");
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
                        _state.IsGameOver = true;
                        inMenu = false;
                        break;
                    default:
                        ConsoleDisplay.ShowError("Invalid choice. Please try again.");
                        InputHandler.WaitForKey();
                        break;
                }
            }
        }
    }
}