using System;
using System.Collections.Generic;
using System.Linq;
using CraftingCreatureWorld.Core;
using CraftingCreatureWorld.Services;
using CraftingCreatureWorld.UI.Display;
using CraftingEngine;

namespace CraftingCreatureWorld.UI.Menus
{
    public class TraderMenu
    {
        private readonly GameState _state;
        private readonly TradingService _tradingService;
        
        public TraderMenu(GameState state)
        {
            _state = state;
            _tradingService = new TradingService(state);
        }
        
        public void Show()
        {
            bool inShop = true;
            
            while (inShop)
            {
                ConsoleHelper.Clear();
                ConsoleDisplay.ShowHeader("MERCHANT GREGORY'S SHOP");
                
                Console.WriteLine($"Your Currency: {_state.Player.Currency:C}\n");
                Console.WriteLine("Available Ingredients:\n");
                
                var items = _tradingService.GetAvailableItems();
                
                for (int i = 0; i < items.Count; i++)
                {
                    var (item, price, stock) = items[i];
                    string stockDisplay = stock > 0 ? stock.ToString() : "OUT";
                    Console.WriteLine($"{i + 1}. {item.Name,-15} {price,6:C} per {item.Unit} [Stock: {stockDisplay}]");
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
                        DisplayRecipes();
                    }
                    else if (choice > 0 && choice <= items.Count)
                    {
                        var (item, price, stock) = items[choice - 1];
                        if (stock > 0)
                        {
                            BuyItem(item, price);
                        }
                        else
                        {
                            ConsoleDisplay.ShowError("This item is out of stock!");
                            InputHandler.WaitForKey();
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
        
        private void BuyItem(Item item, decimal price)
        {
            Console.Write($"How many {item.Unit} of {item.Name} would you like to buy? ");
            
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                if (_tradingService.BuyItem(item, amount))
                {
                    ConsoleDisplay.ShowSuccess($"Purchased {amount} {item.Unit} of {item.Name} for {price * amount:C}");
                }
                else
                {
                    ConsoleDisplay.ShowError("Insufficient currency or item out of stock!");
                }
            }
            else
            {
                ConsoleDisplay.ShowError("Invalid amount.");
            }
            
            InputHandler.WaitForKey();
        }
        
        private void DisplayRecipes()
        {
            ConsoleHelper.Clear();
            ConsoleDisplay.ShowHeader("AVAILABLE RECIPES");
            
            var recipes = RecipeCatalog.LoadStarterRecipes();
            
            foreach (var recipe in recipes)
            {
                Console.WriteLine($"\n{recipe.Name}");
                Console.WriteLine($"   Result: {recipe.Result}");
                Console.WriteLine($"   Ingredients: {string.Join(", ", recipe.Ingredients.Select(i => i.ToString()))}");
                
                // Show effect
                if (recipe.Name.Contains("Chocolate"))
                    Console.WriteLine("   Effect: +20 Happiness to all creatures");
                else if (recipe.Name.Contains("Bread"))
                    Console.WriteLine("   Effect: -30 Hunger to all creatures");
                else if (recipe.Name.Contains("Potion"))
                    Console.WriteLine("   Effect: +25 Health to a single creature");
            }
            
            InputHandler.WaitForKey();
        }
    }
}