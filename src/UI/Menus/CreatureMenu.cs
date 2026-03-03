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
        private readonly Random _random = new();
        
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
                Console.WriteLine($"Happiness: {creature.Happiness}%");
                Console.WriteLine($"Hunger: {creature.HungerLevel}%");
                
                Console.WriteLine("\n=== CREATURE OPTIONS ===");
                Console.WriteLine("1. Feed Creature");
                Console.WriteLine("2. Play with Creature");
                Console.WriteLine("3. Back to Creature List");
                
                Console.Write("\nEnter your choice (1-3): ");
                string choice = Console.ReadLine()?.Trim() ?? "";
                
                switch (choice)
                {
                    case "1":
                        FeedCreature(creature);
                        break;
                    case "2":
                        PlayWithCreature(creature);
                        break;
                    case "3":
                        viewingDetails = false;
                        break;
                    default:
                        ConsoleDisplay.ShowError("Invalid choice.");
                        InputHandler.WaitForKey();
                        break;
                }
            }
        }
        
        private void PlayWithCreature(Creature creature)
        {
            ConsoleHelper.Clear();
            ConsoleDisplay.ShowHeader($"PLAYING WITH {creature.Name.ToUpper()}");
            
            Console.WriteLine($"\nYou spend time playing with {creature.Name} the {creature.Type}...");
            
            // Different play interactions based on creature type
            string playMessage = GetPlayMessage(creature);
            Console.WriteLine(playMessage);
            
            // Calculate happiness boost (random between 5-15)
            int happinessBoost = _random.Next(5, 16);
            int oldHappiness = creature.Happiness;
            creature.Happiness = Math.Min(100, creature.Happiness + happinessBoost);
            
            // Small chance to reduce hunger from playing (10% chance)
            if (_random.Next(100) < 10)
            {
                int hungerReduction = _random.Next(5, 11);
                creature.HungerLevel = Math.Max(0, creature.HungerLevel - hungerReduction);
                ConsoleDisplay.ShowSuccess($"\n{creature.Name} got so distracted playing that they forgot they were hungry! (-{hungerReduction} hunger)");
            }
            
            Console.WriteLine($"\nHappiness increased by {happinessBoost}%!");
            Console.WriteLine($"{oldHappiness}% → {creature.Happiness}%");
            
            // Special reaction based on creature type
            string reaction = GetHappinessReaction(creature);
            Console.WriteLine($"\n{reaction}");
            
            // Recalculate daily currency since happiness changed
            creature.GetType()
                .GetMethod("CalculateDailyCurrency", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(creature, null);
            
            ConsoleDisplay.ShowSuccess($"\n{creature.Name}'s daily value is now {creature.DailyCurrency:C}");
            
            InputHandler.WaitForKey();
        }
        
        private string GetPlayMessage(Creature creature)
        {
            string[] playActivities;
            
            switch (creature.Type)
            {
                case CreatureType.Dragon:
                    playActivities = new[]
                    {
                        $"You play fetch with a burning log - {creature.Name} catches it mid-air!",
                        $"{creature.Name} teaches you to fly, though you mostly just hang on.",
                        $"You have a contest to see who can breathe the biggest fireball. {creature.Name} wins, of course.",
                        $"You polish {creature.Name}'s scales until they sparkle like gems.",
                        $"{creature.Name} shows you their treasure hoard and lets you hold the shiniest coin."
                    };
                    break;
                    
                case CreatureType.Elf:
                    playActivities = new[]
                    {
                        $"{creature.Name} teaches you an ancient Elvish dance under the starlight.",
                        $"You have an archery contest - {creature.Name} generously only beats you by 10 points.",
                        $"You listen to {creature.Name} sing hauntingly beautiful Elvish songs.",
                        $"{creature.Name} shows you how to walk without leaving footprints.",
                        $"You play a game of riddles with {creature.Name} in the ancient forest."
                    };
                    break;
                    
                case CreatureType.Goblin:
                    playActivities = new[]
                    {
                        $"You play hide and seek with {creature.Name}. They're suspiciously good at hiding.",
                        $"{creature.Name} challenges you to a game of " + '"' + "who can collect the most shiny rocks." + '"',
                        $"You tell each other scary stories. {creature.Name}'s stories are actually pretty creepy.",
                        $"You play tag through the caves. {creature.Name} cheats by using secret tunnels.",
                        $"{creature.Name} shows you their collection of " + '"' + "found" + '"' + " items."
                    };
                    break;
                    
                default:
                    playActivities = new[]
                    {
                        $"You play with {creature.Name} in the meadow.",
                        $"{creature.Name} seems much happier after spending time with you.",
                        $"You teach {creature.Name} a new game, and they catch on quickly."
                    };
                    break;
            }
            
            return playActivities[_random.Next(playActivities.Length)];
        }
        
        private string GetHappinessReaction(Creature creature)
        {
            if (creature.Happiness >= 90)
            {
                switch (creature.Type)
                {
                    case CreatureType.Dragon:
                        return $"{creature.Name} purrs contentedly, sending small smoke rings into the air.";
                    case CreatureType.Elf:
                        return $"{creature.Name} hums a happy tune that makes the flowers bloom around you.";
                    case CreatureType.Goblin:
                        return $"{creature.Name} does a little jig and offers you their favorite shiny rock.";
                    default:
                        return $"{creature.Name} snuggles up to you, happy and content.";
                }
            }
            else if (creature.Happiness >= 70)
            {
                return $"{creature.Name} gives you a warm, appreciative look.";
            }
            else
            {
                return $"{creature.Name} seems a bit happier now, but could use more attention soon.";
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