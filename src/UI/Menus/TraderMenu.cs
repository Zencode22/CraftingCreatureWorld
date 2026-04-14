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
        private readonly DayService _dayService;
        
        public TraderMenu(GameState state, DayService dayService)
        {
            _state = state;
            _dayService = dayService;
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
                            bool purchased = BuyItem(item, price);
                            if (purchased)
                            {
                                inShop = false;
                            }
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
        
        private bool BuyItem(Item item, decimal price)
        {
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
                        ConsoleDisplay.ShowError("Insufficient currency or item out of stock!");
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
        
        private void DisplayRecipes()
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
                    Console.WriteLine($"   🎯 Best for: {recipe.TargetCreature}");
                    Console.WriteLine($"   🍽️ Effect when fed to {recipe.TargetCreature}: {recipe.GetEffectDescription()}");
                }
                else
                {
                    Console.WriteLine($"   🍽️ Effect: {recipe.GetEffectDescription()}");
                }
                
                Console.WriteLine($"   Ingredients: {string.Join(", ", recipe.Ingredients.Select(i => i.ToString()))}");
            }
            
            InputHandler.WaitForKey();
        }
    }
}