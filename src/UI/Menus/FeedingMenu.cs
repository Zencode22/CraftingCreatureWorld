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
        private readonly DayService _dayService;
        
        public FeedingMenu(GameState state, DayService dayService)
        {
            _state = state;
            _dayService = dayService;
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
                        bool fed = FeedCreature(selectedCreature, selectedFood, foodChoice - 1);
                        if (fed)
                        {
                            inFeeding = false;
                        }
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
                    $"(Health {creature.Health}% | Happiness {creature.Happiness}%)");
            }
        }
        
        private string GetFoodEffect(string foodName)
        {
            if (foodName.Contains("Hot Chocolate", StringComparison.OrdinalIgnoreCase))
                return "[+25 Health, +15 Happiness] (Dragon only)";
            if (foodName.Contains("Bread", StringComparison.OrdinalIgnoreCase))
                return "[+30 Health, +10 Happiness] (Fairy only)";
            if (foodName.Contains("Jelly Beans", StringComparison.OrdinalIgnoreCase))
                return "[+20 Health, +20 Happiness] (Goblin only)";
            if (foodName.Contains("Healing Potion", StringComparison.OrdinalIgnoreCase))
                return "[Restores Health and Happiness to maximum] (Any creature)";
            return "";
        }
        
        private bool FeedCreature(Creature creature, Result food, int foodIndex)
        {
            string confirm = InputHandler.GetConfirmation($"Feed {food} to {creature.Name}?");
            
            if (confirm == "Y")
            {
                bool isValid = IsValidFoodForCreature(creature, food.Item.Name);
                
                if (!isValid)
                {
                    ConsoleDisplay.ShowError($"{creature.Name} the {creature.Type} won't eat {food.Item.Name}!");
                    Console.WriteLine("\nAllowed foods:");
                    Console.WriteLine("   Dragon -> Hot Chocolate");
                    Console.WriteLine("   Fairy -> Bread");
                    Console.WriteLine("   Goblin -> Jelly Beans");
                    Console.WriteLine("   Any -> Healing Potion");
                    InputHandler.WaitForKey();
                    return false;
                }
                
                _state.Player.CraftedFood.RemoveAt(foodIndex);
                
                ConsoleDisplay.ShowSuccess($"{creature.Name} has been fed!");
                
                InputHandler.WaitForKey();
                
                _dayService.AdvanceToNextDay($"fed {creature.Name}", creature, food);
                
                return true;
            }
            else
            {
                ConsoleDisplay.ShowInfo("Feeding cancelled.");
            }
            
            InputHandler.WaitForKey();
            return false;
        }
        
        private bool IsValidFoodForCreature(Creature creature, string foodName)
        {
            if (foodName.Contains("Healing Potion", StringComparison.OrdinalIgnoreCase))
                return true;
                
            return (creature.Type == CreatureType.Dragon && foodName.Contains("Hot Chocolate", StringComparison.OrdinalIgnoreCase)) ||
                   (creature.Type == CreatureType.Fairy && foodName.Contains("Bread", StringComparison.OrdinalIgnoreCase)) ||
                   (creature.Type == CreatureType.Goblin && foodName.Contains("Jelly Beans", StringComparison.OrdinalIgnoreCase));
        }
    }
}