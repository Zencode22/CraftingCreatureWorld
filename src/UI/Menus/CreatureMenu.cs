using System;
using System.Collections.Generic;
using System.Linq;
using CraftingCreatureWorld.Core;
using CraftingCreatureWorld.Services;
using CraftingCreatureWorld.UI.Display;
using CraftingEngine;
using CreatureWorld;

namespace CraftingCreatureWorld.UI.Menus
{
    public class CreatureMenu
    {
        private readonly GameState _state;
        private readonly CreatureService _creatureService;
        
        public CreatureMenu(GameState state)
        {
            _state = state;
            _creatureService = new CreatureService(state);
        }
        
        public void Show()
        {
            bool inMenu = true;
            
            while (inMenu && _state.Player.Creatures.Any())
            {
                ConsoleHelper.Clear();
                ConsoleDisplay.ShowHeader("MANAGE YOUR CREATURES");
                
                DisplayCreatureList();
                
                Console.WriteLine($"\n{_state.Player.Creatures.Count + 1}. Return to Main Menu");
                Console.Write("\nSelect a creature to view details: ");
                
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == _state.Player.Creatures.Count + 1)
                    {
                        inMenu = false;
                    }
                    else if (choice > 0 && choice <= _state.Player.Creatures.Count)
                    {
                        var creature = _state.Player.Creatures[choice - 1];
                        ViewCreatureDetails(creature);
                    }
                    else
                    {
                        ConsoleDisplay.ShowError("Invalid choice.");
                        InputHandler.WaitForKey();
                    }
                }
            }
            
            if (!_state.Player.Creatures.Any())
            {
                ConsoleDisplay.ShowWarning("You don't have any creatures yet!");
                InputHandler.WaitForKey();
            }
        }
        
        private void DisplayCreatureList()
        {
            for (int i = 0; i < _state.Player.Creatures.Count; i++)
            {
                var creature = _state.Player.Creatures[i];
                string happinessBar = GetBar(creature.Happiness, 10);
                string healthBar = GetBar(creature.Health, 10);
                
                Console.WriteLine($"\n{i + 1}. {creature.Name} the {creature.Type}");
                Console.WriteLine($"   Health: {healthBar} {creature.Health}%");
                Console.WriteLine($"   Happiness: {happinessBar} {creature.Happiness}%");
                Console.WriteLine($"   Hunger: {creature.HungerLevel}/100");
                Console.WriteLine($"   Daily Value: {creature.DailyCurrency:C}");
            }
        }
        
        private void ViewCreatureDetails(Creature creature)
        {
            bool viewingDetails = true;
            
            while (viewingDetails)
            {
                ConsoleHelper.Clear();
                ConsoleDisplay.ShowHeader(creature.Name);
                
                creature.DisplayInfo();
                Console.WriteLine($"\nSpecial Ability: {creature.GetSpecialAbility()}");
                Console.WriteLine($"Daily Currency: {creature.DailyCurrency:C}");
                
                Console.WriteLine("\n=== CREATURE OPTIONS ===");
                Console.WriteLine("1. Feed Creature");
                Console.WriteLine("2. Back to Creature List");
                
                Console.Write("\nEnter your choice (1-2): ");
                string choice = Console.ReadLine()?.Trim() ?? "";
                
                switch (choice)
                {
                    case "1":
                        FeedCreature(creature);
                        break;
                    case "2":
                        viewingDetails = false;
                        break;
                    default:
                        ConsoleDisplay.ShowError("Invalid choice.");
                        InputHandler.WaitForKey();
                        break;
                }
            }
        }
        
        private void FeedCreature(Creature creature)
        {
            ConsoleHelper.Clear();
            ConsoleDisplay.ShowHeader($"FEED {creature.Name}");
            
            if (_state.Player.CraftedFood.Count == 0)
            {
                ConsoleDisplay.ShowError("You don't have any crafted food!");
                ConsoleDisplay.ShowInfo("Visit the crafting station to make some food first.");
                InputHandler.WaitForKey();
                return;
            }
            
            DisplayFoodOptions();
            Console.Write("\nSelect food to use: ");
            
            if (int.TryParse(Console.ReadLine(), out int foodChoice) && 
                foodChoice > 0 && foodChoice <= _state.Player.CraftedFood.Count)
            {
                var selectedFood = _state.Player.CraftedFood[foodChoice - 1];
                
                string confirm = InputHandler.GetConfirmation($"\nFeed {selectedFood} to {creature.Name}?");
                
                if (confirm == "Y")
                {
                    if (_creatureService.FeedCreature(creature, selectedFood))
                    {
                        _state.Player.CraftedFood.RemoveAt(foodChoice - 1);
                        
                        ConsoleDisplay.ShowSuccess($"{creature.Name} has been fed!");
                        
                        // Show updated stats
                        Console.WriteLine($"\nUpdated Stats for {creature.Name}:");
                        Console.WriteLine($"   Health: {creature.Health}%");
                        Console.WriteLine($"   Happiness: {creature.Happiness}%");
                        Console.WriteLine($"   Hunger: {creature.HungerLevel}%");
                    }
                    else
                    {
                        ConsoleDisplay.ShowError("Failed to feed creature.");
                    }
                }
                else
                {
                    ConsoleDisplay.ShowInfo("Feeding cancelled.");
                }
                
                InputHandler.WaitForKey();
            }
            else
            {
                ConsoleDisplay.ShowError("Invalid food selection.");
                InputHandler.WaitForKey();
            }
        }
        
        private void DisplayFoodOptions()
        {
            Console.WriteLine("Your Crafted Food:");
            for (int i = 0; i < _state.Player.CraftedFood.Count; i++)
            {
                var food = _state.Player.CraftedFood[i];
                string effect = GetFoodEffect(food.Item.Name);
                Console.WriteLine($"   {i + 1}. {food} {effect}");
            }
        }
        
        private string GetFoodEffect(string foodName)
        {
            if (foodName.Contains("Chocolate"))
                return "[+20 Happiness, -10 Hunger]";
            if (foodName.Contains("Bread"))
                return "[-30 Hunger, +5 Happiness]";
            if (foodName.Contains("Potion"))
                return "[+25 Health, +10 Happiness]";
            return "";
        }
        
        private string GetBar(int value, int length)
        {
            int filled = (value * length) / 100;
            return new string('#', filled) + new string('-', length - filled);
        }
    }
}