using System.Collections.Generic;
using System.Linq;
using CraftingCreatureWorld.Core;
using CraftingEngine;

namespace CraftingCreatureWorld.Services
{
    public class CraftingService
    {
        private readonly GameState _state;
        private readonly List<Recipe> _availableRecipes;
        
        public CraftingService(GameState state)
        {
            _state = state;
            _availableRecipes = RecipeCatalog.LoadStarterRecipes();
        }
        
        public List<Recipe> GetAvailableRecipes() => _availableRecipes;
        
        public List<Recipe> GetCraftableRecipes()
        {
            return _availableRecipes.Where(r => r.CanCraft(_state.Player.Inventory)).ToList();
        }
        
        public bool CraftRecipe(Recipe recipe)
        {
            if (recipe.Craft(_state.Player.Inventory))
            {
                _state.Player.AddCraftedItem(recipe.Result.Item, recipe.Result.Amount);
                return true;
            }
            return false;
        }
        
        public string GenerateCraftingInstructions(Recipe recipe)
        {
            var instructions = new System.Text.StringBuilder();
            
            instructions.AppendLine("=".PadRight(60, '='));
            instructions.AppendLine("           CRAFTING INSTRUCTIONS");
            instructions.AppendLine("=".PadRight(60, '='));
            instructions.AppendLine();
            instructions.AppendLine($"RECIPE: {recipe.Name}");
            instructions.AppendLine(new string('-', 40));
            instructions.AppendLine();
            instructions.AppendLine("CRAFTING RESULT:");
            instructions.AppendLine($"   {recipe.Result}");
            instructions.AppendLine();
            instructions.AppendLine("INGREDIENTS REQUIRED:");
            
            foreach (var ing in recipe.Ingredients)
            {
                instructions.AppendLine($"   - {ing}");
            }
            
            instructions.AppendLine();
            instructions.AppendLine("STEP-BY-STEP INSTRUCTIONS:");
            instructions.AppendLine();
            
            int stepNumber = 1;
            instructions.AppendLine($"   Step {stepNumber++}: Gather all required ingredients");
            instructions.AppendLine($"   Step {stepNumber++}: Verify you have all ingredients");
            instructions.AppendLine($"   Step {stepNumber++}: Prepare your crafting workspace");
            instructions.AppendLine($"   Step {stepNumber++}: Combine the ingredients carefully");
            instructions.AppendLine($"   Step {stepNumber++}: Complete the crafting process");
            
            instructions.AppendLine();
            instructions.AppendLine("=".PadRight(60, '='));
            instructions.AppendLine("           HAPPY CRAFTING!");
            instructions.AppendLine("=".PadRight(60, '='));
            
            return instructions.ToString();
        }
    }
}