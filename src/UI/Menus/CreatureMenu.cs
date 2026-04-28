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
        private readonly DayService _dayService;
        private readonly Random _random = new();
        
        public CreatureMenu(GameState state, DayService dayService)
        {
            _state = state;
            _dayService = dayService;
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
                        ConsoleHelper.Clear();
                        bool shouldReturnToMain = ViewCreatureDetails(creature);
                        if (shouldReturnToMain)
                        {
                            inMenu = false;
                        }
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
                Console.WriteLine($"   Daily Value: {creature.DailyCurrency:C}");
                
                // Show warnings for critical stats
                if (creature.Health <= 0)
                {
                    ConsoleDisplay.ShowWarning($"   ⚠️ {creature.Name} has no health! Income reduced by 50%!");
                }
                if (creature.Happiness <= 0)
                {
                    ConsoleDisplay.ShowWarning($"   ⚠️ {creature.Name} is completely unhappy! Income reduced by 50%!");
                }
                if (creature.Health <= 0 && creature.Happiness <= 0)
                {
                    ConsoleDisplay.ShowError($"   ❌ {creature.Name} is generating NO income!");
                }
            }
        }
        
        private bool ViewCreatureDetails(Creature creature)
        {
            bool viewingDetails = true;
            
            while (viewingDetails)
            {
                ConsoleHelper.Clear();
                ConsoleDisplay.ShowHeader(creature.Name);
                
                // Display creature info
                creature.DisplayInfo();
                
                Console.WriteLine("\n=== CREATURE OPTIONS ===");
                Console.WriteLine("1. Feed Creature");
                Console.WriteLine("2. Play with Creature");
                Console.WriteLine("3. Back to Creature List");
                
                Console.Write("\nEnter your choice (1-3): ");
                string choice = Console.ReadLine()?.Trim() ?? "";
                
                switch (choice)
                {
                    case "1":
                        ConsoleHelper.Clear();
                        bool fed = FeedCreature(creature);
                        if (fed)
                        {
                            return true;
                        }
                        break;
                    case "2":
                        ConsoleHelper.Clear();
                        bool played = PlayWithCreature(creature);
                        if (played)
                        {
                            return true;
                        }
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
            
            return false;
        }
        
        private bool PlayWithCreature(Creature creature)
        {
            ConsoleHelper.Clear();
            ConsoleDisplay.ShowHeader($"PLAYING WITH {creature.Name.ToUpper()}");
            
            Console.WriteLine($"\nYou spend time playing with {creature.Name} the {creature.Type}...");
            
            // Different play interactions based on creature type
            string playMessage = GetPlayMessage(creature);
            Console.WriteLine(playMessage);
            
            InputHandler.WaitForKey();
            
            // Advance to next day after playing
            _dayService.AdvanceToNextDay($"played with {creature.Name}", creature, null);
            
            return true;
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
                    
                case CreatureType.Fairy:
                    playActivities = new[]
                    {
                        $"{creature.Name} teaches you to dance on flower petals without crushing them.",
                        $"You watch {creature.Name} create beautiful patterns with fairy dust in the moonlight.",
                        $"{creature.Name} leads you to a secret fairy ring where you play hide and seek.",
                        $"You listen to {creature.Name} tell stories of ancient forest magic.",
                        $"{creature.Name} shows you how to make wishes on dandelion seeds."
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
        
        private bool FeedCreature(Creature creature)
        {
            ConsoleHelper.Clear();
            ConsoleDisplay.ShowHeader($"FEED {creature.Name}");
            
            if (_state.Player.CraftedFood.Count == 0)
            {
                ConsoleDisplay.ShowError("You don't have any crafted food!");
                ConsoleDisplay.ShowInfo("Visit the crafting station to make some food first.");
                InputHandler.WaitForKey();
                return false;
            }
            
            DisplayFoodOptions();
            Console.Write("\nSelect food to use (or 0 to cancel): ");
            
            if (int.TryParse(Console.ReadLine(), out int foodChoice))
            {
                if (foodChoice == 0)
                {
                    ConsoleDisplay.ShowInfo("Feeding cancelled.");
                    InputHandler.WaitForKey();
                    return false;
                }
                
                if (foodChoice > 0 && foodChoice <= _state.Player.CraftedFood.Count)
                {
                    var selectedFood = _state.Player.CraftedFood[foodChoice - 1];
                    
                    string confirm = InputHandler.GetConfirmation($"\nFeed {selectedFood} to {creature.Name}?");
                    
                    if (confirm == "Y")
                    {
                        // Check if the food is appropriate for this creature type (or if it's a healing potion)
                        bool isValidFood = IsValidFoodForCreature(creature, selectedFood.Item.Name);
                        
                        if (!isValidFood)
                        {
                            ConsoleDisplay.ShowError($"{creature.Name} the {creature.Type} won't eat {selectedFood.Item.Name}!");
                            Console.WriteLine("\nAllowed foods:");
                            Console.WriteLine("   Dragon -> Hot Chocolate");
                            Console.WriteLine("   Fairy -> Bread");
                            Console.WriteLine("   Goblin -> Jelly Beans");
                            Console.WriteLine("   Any -> Healing Potion");
                            InputHandler.WaitForKey();
                            return false;
                        }
                        
                        // Remove the food from inventory
                        _state.Player.CraftedFood.RemoveAt(foodChoice - 1);
                        
                        ConsoleDisplay.ShowSuccess($"\n{creature.Name} enjoys the {selectedFood.Item.Name}!");
                        
                        InputHandler.WaitForKey();
                        
                        // Advance to next day after feeding
                        _dayService.AdvanceToNextDay($"fed {creature.Name}", creature, selectedFood);
                        
                        return true;
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
            
            return false;
        }
        
        private bool IsValidFoodForCreature(Creature creature, string foodName)
        {
            // Healing potion works for all creatures
            if (foodName.Contains("Healing Potion", StringComparison.OrdinalIgnoreCase))
                return true;
                
            // Creature-specific foods
            return (creature.Type == CreatureType.Dragon && foodName.Contains("Hot Chocolate", StringComparison.OrdinalIgnoreCase)) ||
                   (creature.Type == CreatureType.Fairy && foodName.Contains("Bread", StringComparison.OrdinalIgnoreCase)) ||
                   (creature.Type == CreatureType.Goblin && foodName.Contains("Jelly Beans", StringComparison.OrdinalIgnoreCase));
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
            if (foodName.Contains("Hot Chocolate"))
                return "[+25 Health, +15 Happiness] (Dragon)";
            if (foodName.Contains("Bread"))
                return "[+30 Health, +10 Happiness] (Fairy)";
            if (foodName.Contains("Jelly Beans"))
                return "[+20 Health, +20 Happiness] (Goblin)";
            if (foodName.Contains("Healing Potion"))
                return "[Restores Health and Happiness to maximum] (Any)";
            return "";
        }
        
        private string GetBar(int value, int length)
        {
            int filled = (value * length) / 100;
            return new string('#', filled) + new string('-', length - filled);
        }
    }
}