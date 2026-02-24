using System;
using System.Linq;
using CraftingCreatureWorld.Core;
using CraftingCreatureWorld.Services;
using CraftingCreatureWorld.UI.Display;
using CraftingEngine;

namespace CraftingCreatureWorld.UI.Menus
{
    public class CraftingMenu
    {
        private readonly GameState _state;
        private readonly CraftingService _craftingService;
        
        public CraftingMenu(GameState state)
        {
            _state = state;
            _craftingService = new CraftingService(state);
        }
        
        public void Show()
        {
            bool inCrafting = true;
            
            while (inCrafting)
            {
                ConsoleHelper.Clear();
                ConsoleDisplay.ShowHeader("CRAFTING STATION");
                
                DisplayInventory();
                
                Console.WriteLine("\nAvailable Recipes:\n");
                
                var recipes = _craftingService.GetAvailableRecipes();
                
                for (int i = 0; i < recipes.Count; i++)
                {
                    var recipe = recipes[i];
                    bool canCraft = recipe.CanCraft(_state.Player.Inventory);
                    
                    Console.Write($"{i + 1}. {recipe.Name} - {recipe.Result}");
                    
                    if (canCraft)
                        ConsoleDisplay.ShowSuccess("(Craftable)");
                    else
                        ConsoleDisplay.ShowError("(Missing ingredients)");
                    
                    Console.WriteLine($"   Needs: {string.Join(", ", recipe.Ingredients.Select(i => i.ToString()))}");
                }
                
                Console.WriteLine($"\n{recipes.Count + 1}. Return to Main Menu");
                Console.Write("\nSelect a recipe to craft: ");
                
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == recipes.Count + 1)
                    {
                        inCrafting = false;
                    }
                    else if (choice > 0 && choice <= recipes.Count)
                    {
                        var selectedRecipe = recipes[choice - 1];
                        AttemptCraft(selectedRecipe);
                    }
                    else
                    {
                        ConsoleDisplay.ShowError("Invalid choice.");
                        InputHandler.WaitForKey();
                    }
                }
            }
        }
        
        private void DisplayInventory()
        {
            Console.WriteLine("Your Ingredients:");
            bool hasItems = false;
            
            foreach (var kvp in _state.Player.Inventory.Contents.Where(k => k.Value > 0))
            {
                var item = ItemRegistry.Get(Guid.Parse(kvp.Key));
                Console.WriteLine($"   {kvp.Value} {item.Unit} of {item.Name}");
                hasItems = true;
            }
            
            if (!hasItems)
            {
                Console.WriteLine("   (No ingredients)");
            }
        }
        
        private void AttemptCraft(Recipe recipe)
        {
            ConsoleHelper.Clear();
            ConsoleDisplay.ShowHeader($"CRAFTING: {recipe.Name}");
            
            Console.WriteLine($"Result: {recipe.Result}");
            Console.WriteLine("\nRequired Ingredients:");
            
            foreach (var ing in recipe.Ingredients)
            {
                bool hasIng = _state.Player.Inventory.Has(ing.Item.Id.ToString(), ing.Amount);
                string status = hasIng ? "[OK]" : "[MISSING]";
                Console.WriteLine($"   {status} {ing}");
            }
            
            Console.WriteLine();
            
            string confirm = InputHandler.GetConfirmation("Craft this item?");
            
            if (confirm == "Y")
            {
                if (_craftingService.CraftRecipe(recipe))
                {
                    ConsoleDisplay.ShowSuccess($"Successfully crafted {recipe.Result}!");
                    
                    // Generate instructions
                    string instructions = _craftingService.GenerateCraftingInstructions(recipe);
                    string filePath = "crafting_instructions.txt";
                    System.IO.File.WriteAllText(filePath, instructions);
                    ConsoleDisplay.ShowInfo($"Crafting instructions saved to {filePath}");
                }
                else
                {
                    ConsoleDisplay.ShowError("Failed to craft. Missing ingredients?");
                }
            }
            else
            {
                ConsoleDisplay.ShowInfo("Crafting cancelled.");
            }
            
            InputHandler.WaitForKey();
        }
    }
}