using System;
using System.Linq;
using CraftingCreatureWorld.Core;
using CraftingCreatureWorld.Services;
using CraftingCreatureWorld.UI.Display;
using CraftingEngine;

namespace CraftingCreatureWorld.UI.Menus
{
    public class TraderMenu(GameState state, DayService dayService)
    {
        private readonly GameState _state = state;
        private readonly DayService _dayService = dayService;
        private readonly TradingService _tradingService = new(state);
        
        public void Show()
        {
            bool inShop = true;
            
            while (inShop)
            {
                ConsoleHelper.Clear();
                ConsoleDisplay.ShowHeader($"{_state.Trader?.Name ?? "Merchant"}'S SHOP");
                
                Console.WriteLine($"Your Currency: {_state.Player.Currency:C}");
                Console.WriteLine("\n\"Fresh ingredients, every week!\"\n");
                
                var items = _tradingService.GetAvailableItems();
                
                for (int i = 0; i < items.Count; i++)
                {
                    var (item, price, _) = items[i];
                    Console.WriteLine($"{i + 1}. {item.Name,-20} {price,6:C} per {item.Unit}");
                }
                
                Console.WriteLine($"\n{items.Count + 1}. View Recipes");
                Console.WriteLine($"{items.Count + 2}. Return to Main Menu");
                
                Console.Write("\nWhat would you like to buy? ");
                
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == items.Count + 2)
                    {
                        inShop = false;
                    }
                    else if (choice == items.Count + 1)
                    {
                        ConsoleHelper.Clear();
                        DisplayRecipes();
                    }
                    else if (choice > 0 && choice <= items.Count)
                    {
                        var (item, price, _) = items[choice - 1];
                        ConsoleHelper.Clear();
                        bool purchased = BuyItem(item, price);
                        if (purchased)
                        {
                            inShop = false;
                        }
                    }
                    else
                    {
                        ConsoleDisplay.ShowError("Invalid choice.");
                        InputHandler.WaitForKey();
                    }
                }
            }
        }
        
        private bool BuyItem(Item item, decimal price)
        {
            ConsoleDisplay.ShowHeader($"BUYING: {item.Name}");
            Console.WriteLine($"\nPrice: {price:C} per {item.Unit}");
            
            Console.Write($"How many {item.Unit} of {item.Name} would you like to buy? ");
            
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                decimal totalCost = price * amount;
                string confirm = InputHandler.GetConfirmation($"\nPurchase {amount} {item.Unit} of {item.Name} for {totalCost:C}?");
                
                if (confirm == "Y")
                {
                    if (_tradingService.BuyItem(item, amount))
                    {
                        ConsoleDisplay.ShowSuccess($"\nPurchased {amount} {item.Unit} of {item.Name} for {totalCost:C}");
                        
                        InputHandler.WaitForKey();
                        
                        _dayService.AdvanceToNextDay("purchased items from the merchant");
                        
                        return true;
                    }
                    else
                    {
                        ConsoleDisplay.ShowError("Insufficient currency!");
                    }
                }
                else
                {
                    ConsoleDisplay.ShowInfo("Purchase cancelled.");
                }
            }
            else
            {
                ConsoleDisplay.ShowError("Invalid amount.");
            }
            
            InputHandler.WaitForKey();
            return false;
        }
        
        private static void DisplayRecipes()
        {
            ConsoleHelper.Clear();
            ConsoleDisplay.ShowHeader("AVAILABLE RECIPES");
            
            var recipes = RecipeCatalog.LoadStarterRecipes();
            
            foreach (var recipe in recipes)
            {
                Console.WriteLine($"\n{recipe.Name}");
                Console.WriteLine($"   Result: {recipe.Result}");
                
                if (recipe.TargetCreature != "All")
                {
                    Console.WriteLine($"   Best for: {recipe.TargetCreature}");
                }
                else
                {
                    Console.WriteLine($"   Best for: Any creature");
                }
                
                Console.WriteLine($"   Ingredients: {string.Join(", ", recipe.Ingredients.Select(i => i.ToString()))}");
            }
            
            InputHandler.WaitForKey();
        }
    }
}