using System;
using System.Linq;
using CraftingCreatureWorld.Core;
using CraftingCreatureWorld.Services;
using CraftingCreatureWorld.UI.Display;
using CraftingEngine;
using CreatureWorld;

namespace CraftingCreatureWorld.UI.Menus
{
    public class FeedingMenu
    {
        private readonly GameState _state;
        private readonly CreatureService _creatureService;
        
        public FeedingMenu(GameState state)
        {
            _state = state;
            _creatureService = new CreatureService(state);
        }
        
        public void Show()
        {
            if (_state.Player.CraftedFood.Count == 0)
            {
                ConsoleDisplay.ShowError("You don't have any crafted food to feed your creatures!");
                ConsoleDisplay.ShowInfo("Visit the crafting station to make some food first.");
                InputHandler.WaitForKey();
                return;
            }
            
            if (_state.Player.Creatures.Count == 0)
            {
                ConsoleDisplay.ShowError("You don't have any creatures to feed!");
                InputHandler.WaitForKey();
                return;
            }
            
            bool inFeeding = true;
            
            while (inFeeding)
            {
                ConsoleHelper.Clear();
                ConsoleDisplay.ShowHeader("FEED YOUR CREATURES");
                
                DisplayFood();
                DisplayCreatures();
                
                Console.WriteLine($"\n{Math.Max(_state.Player.CraftedFood.Count, _state.Player.Creatures.Count) + 1}. Return to Main Menu");
                
                Console.Write("\nSelect food to use: ");
                
                if (int.TryParse(Console.ReadLine(), out int foodChoice) && 
                    foodChoice > 0 && foodChoice <= _state.Player.CraftedFood.Count + 1)
                {
                    if (foodChoice == _state.Player.CraftedFood.Count + 1)
                    {
                        inFeeding = false;
                        continue;
                    }
                    
                    var selectedFood = _state.Player.CraftedFood[foodChoice - 1];
                    
                    Console.Write("Select creature to feed: ");
                    
                    if (int.TryParse(Console.ReadLine(), out int creatureChoice) && 
                        creatureChoice > 0 && creatureChoice <= _state.Player.Creatures.Count)
                    {
                        var selectedCreature = _state.Player.Creatures[creatureChoice - 1];
                        FeedCreature(selectedCreature, selectedFood, foodChoice - 1);
                    }
                    else
                    {
                        ConsoleDisplay.ShowError("Invalid creature selection.");
                        InputHandler.WaitForKey();
                    }
                }
                else
                {
                    ConsoleDisplay.ShowError("Invalid food selection.");
                    InputHandler.WaitForKey();
                }
            }
        }
        
        private void DisplayFood()
        {
            Console.WriteLine("\nYour Crafted Food:");
            for (int i = 0; i < _state.Player.CraftedFood.Count; i++)
            {
                var food = _state.Player.CraftedFood[i];
                string effect = GetFoodEffect(food.Item.Name);
                Console.WriteLine($"   {i + 1}. {food} {effect}");
            }
        }
        
        private void DisplayCreatures()
        {
            Console.WriteLine("\nYour Creatures:");
            for (int i = 0; i < _state.Player.Creatures.Count; i++)
            {
                var creature = _state.Player.Creatures[i];
                Console.WriteLine($"   {i + 1}. {creature.Name} the {creature.Type} " +
                    $"(Health {creature.Health}% | Happiness {creature.Happiness}% | Hunger {creature.HungerLevel}%)");
            }
        }
        
        private string GetFoodEffect(string foodName)
        {
            if (foodName.Contains("Hot Chocolate", System.StringComparison.OrdinalIgnoreCase))
                return "[+20 Happiness, -10 Hunger, +5 Health] (Dragon only)";
            if (foodName.Contains("Bread", System.StringComparison.OrdinalIgnoreCase))
                return "[-30 Hunger, +5 Happiness] (Elf only)";
            if (foodName.Contains("Jelly Beans", System.StringComparison.OrdinalIgnoreCase))
                return "[-25 Hunger, +15 Happiness, +3 Health] (Goblin only)";
            return "";
        }
        
        private void FeedCreature(Creature creature, Result food, int foodIndex)
        {
            string confirm = InputHandler.GetConfirmation($"Feed {food} to {creature.Name}?");
            
            if (confirm == "Y")
            {
                if (_creatureService.FeedCreature(creature, food))
                {
                    _state.Player.CraftedFood.RemoveAt(foodIndex);
                    
                    ConsoleDisplay.ShowSuccess($"{creature.Name} has been fed!");
                    
                    // Show updated stats
                    Console.WriteLine($"\nUpdated Stats for {creature.Name}:");
                    Console.WriteLine($"   Health: {creature.Health}%");
                    Console.WriteLine($"   Happiness: {creature.Happiness}%");
                    Console.WriteLine($"   Hunger: {creature.HungerLevel}%");
                }
                else
                {
                    ConsoleDisplay.ShowError($"{creature.Name} the {creature.Type} won't eat {food.Item.Name}!");
                    Console.WriteLine("\nAllowed foods:");
                    Console.WriteLine("   Dragon -> Hot Chocolate");
                    Console.WriteLine("   Elf -> Bread");
                    Console.WriteLine("   Goblin -> Jelly Beans");
                }
            }
            else
            {
                ConsoleDisplay.ShowInfo("Feeding cancelled.");
            }
            
            InputHandler.WaitForKey();
        }
    }
}