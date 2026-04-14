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
        private readonly DayService _dayService;
        
        public CraftingMenu(GameState state, DayService dayService)
        {
            _state = state;
            _dayService = dayService;
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
                    
                    string creatureInfo = recipe.TargetCreature != "All" 
                        ? $" [Best for: {recipe.TargetCreature}]" 
                        : " [For all creatures]";
                    
                    Console.Write($"{i + 1}. {recipe.Name}{creatureInfo} - {recipe.Result}");
                    
                    if (canCraft)
                        ConsoleDisplay.ShowSuccess("(Craftable)");
                    else
                        ConsoleDisplay.ShowError("(Missing ingredients)");
                    
                    string effectDescription = recipe.GetEffectDescription();
                    Console.WriteLine($"   Effect: {effectDescription}");
                    Console.WriteLine($"   Ingredients: {string.Join(", ", recipe.Ingredients.Select(i => i.ToString()))}");
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
                        bool crafted = AttemptCraft(selectedRecipe);
                        if (crafted)
                        {
                            inCrafting = false;
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
        
        private bool AttemptCraft(Recipe recipe)
        {
            ConsoleHelper.Clear();
            ConsoleDisplay.ShowHeader($"CRAFTING: {recipe.Name}");
            
            Console.WriteLine($"Result: {recipe.Result}");
            
            if (recipe.TargetCreature != "All")
            {
                ConsoleDisplay.ShowInfo($"🎯 Best fed to: {recipe.TargetCreature}");
                Console.WriteLine($"   Effect when fed to {recipe.TargetCreature}: {recipe.GetEffectDescription()}");
            }
            else
            {
                Console.WriteLine($"   Effect: {recipe.GetEffectDescription()}");
            }
            
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
                    ConsoleDisplay.ShowSuccess($"\nSuccessfully crafted {recipe.Result}!");
                    
                    string instructions = _craftingService.GenerateCraftingInstructions(recipe);
                    string filePath = "crafting_instructions.txt";
                    System.IO.File.WriteAllText(filePath, instructions);
                    ConsoleDisplay.ShowInfo($"Crafting instructions saved to {filePath}");
                    
                    InputHandler.WaitForKey();
                    
                    _dayService.AdvanceToNextDay("crafted an item");
                    
                    return true;
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
            return false;
        }
    }
}