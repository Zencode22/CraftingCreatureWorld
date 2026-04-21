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
            
            while (inMenu && !_state.IsGameOver)
            {
                ConsoleHelper.Clear();
                ConsoleDisplay.ShowHeader($"DAY {_state.CurrentDay} - {_state.Player.Name}'s Adventure");
                
                ConsoleDisplay.ShowPlayerStatus(_state.Player);
                
                Console.WriteLine("\n=== MAIN MENU ===");
                Console.WriteLine("1. Manage Your Creatures");
                Console.WriteLine("2. Visit the Trader");
                Console.WriteLine("3. Crafting Station");
                Console.WriteLine("4. End Day Early");
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
                        EndDayEarly();
                        break;
                    case "5":
                        // user chose to exit
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
        
        private void EndDayEarly()
        {
            ConsoleHelper.Clear();
            ConsoleDisplay.ShowHeader("END DAY EARLY");
            
            Console.WriteLine("\nYou decide to call it a day early.");
            Console.WriteLine("Your creatures will still earn currency, but their health and happiness will decrease.");
            Console.WriteLine("\nCreature Status Preview:");
            
            // Show what will happen to each creature
            foreach (var creature in _state.Player.Creatures)
            {
                Console.WriteLine($"\n  {creature.Name} the {creature.Type}:");
                Console.WriteLine($"    Current Health: {creature.Health}% → Will decrease slightly");
                Console.WriteLine($"    Current Happiness: {creature.Happiness}% → Will decrease slightly");
                Console.WriteLine($"    Daily Income: {creature.DailyCurrency:C}");
            }
            
            Console.WriteLine();
            string confirm = InputHandler.GetConfirmation("Are you sure you want to end the day early?");
            
            if (confirm == "Y")
            {
                // Advance to next day without any interaction bonuses
                _dayService.AdvanceToNextDay("ended the day early");
            }
            else
            {
                ConsoleDisplay.ShowInfo("Returning to main menu...");
                InputHandler.WaitForKey();
            }
        }
    }
}