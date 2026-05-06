using System;
using System.Linq;
using CraftingCreatureWorld.Core;
using CraftingCreatureWorld.UI.Display;
using CraftingCreatureWorld.UI.Menus;
using CreatureWorld;
using CraftingEngine;

namespace CraftingCreatureWorld.Services
{
    public class DayService
    {
        private readonly GameState _state;
        private readonly CurrencyService _currencyService;
        private readonly CreatureService _creatureService;
        private readonly Random _random = new();
        
        public DayService(GameState state)
        {
            _state = state;
            _currencyService = new CurrencyService(state);
            _creatureService = new CreatureService(state);
        }
        
        public void AdvanceToNextDay(string actionPerformed = "", Creature? interactedCreature = null, Result? foodUsed = null)
        {
            ConsoleHelper.Clear();
            ConsoleDisplay.ShowHeader($"END OF DAY {_state.CurrentDay}");
            
            if (!string.IsNullOrEmpty(actionPerformed))
            {
                if (actionPerformed == "ended the day early")
                {
                    ConsoleDisplay.ShowInfo($"\nYou {actionPerformed}. Your creatures rest for the remainder of the day.");
                }
                else
                {
                    ConsoleDisplay.ShowInfo($"\nYou {actionPerformed}. The day comes to an end.");
                }
            }
            
            decimal earnings = _currencyService.CollectDailyCurrency();
            ConsoleDisplay.ShowDailyEarnings(earnings);
            
            Console.WriteLine("\n=== CREATURE STATUS CHANGES ===");
            
            foreach (var creature in _state.Player.Creatures)
            {
                int oldHealth = creature.Health;
                int oldHappiness = creature.Happiness;
                
                creature.Health = Math.Max(0, creature.Health - _random.Next(2, 6));
                creature.Happiness = Math.Max(0, creature.Happiness - _random.Next(3, 8));
                
                creature.OldHealth = oldHealth;
                creature.OldHappiness = oldHappiness;
            }
            
            if (interactedCreature != null)
            {
                if (foodUsed != null)
                {
                    ApplyFeedingBonuses(interactedCreature, foodUsed);
                }
                else
                {
                    ApplyPlayingBonuses(interactedCreature);
                }
            }
            
            foreach (var creature in _state.Player.Creatures)
            {
                creature.CalculateDailyCurrency();
            }
            
            foreach (var creature in _state.Player.Creatures)
            {
                int oldHealth = creature.OldHealth;
                int oldHappiness = creature.OldHappiness;
                
                Console.WriteLine($"\n{creature.Name} the {creature.Type}:");
                
                if (creature.Health > oldHealth)
                    Console.WriteLine($"   Health: {oldHealth}% -> {creature.Health}% (+{creature.Health - oldHealth})");
                else if (creature.Health < oldHealth)
                    Console.WriteLine($"   Health: {oldHealth}% -> {creature.Health}% (-{oldHealth - creature.Health})");
                else
                    Console.WriteLine($"   Health: {creature.Health}% (no change)");
                
                if (creature.Happiness > oldHappiness)
                    Console.WriteLine($"   Happiness: {oldHappiness}% -> {creature.Happiness}% (+{creature.Happiness - oldHappiness})");
                else if (creature.Happiness < oldHappiness)
                    Console.WriteLine($"   Happiness: {oldHappiness}% -> {creature.Happiness}% (-{oldHappiness - creature.Happiness})");
                else
                    Console.WriteLine($"   Happiness: {creature.Happiness}% (no change)");
                
                if (creature.Health <= 0)
                {
                    ConsoleDisplay.ShowWarning($"   [!] {creature.Name} has no health left! Income reduced by 50%!");
                }
                if (creature.Happiness <= 0)
                {
                    ConsoleDisplay.ShowWarning($"   [!] {creature.Name} is completely unhappy! Income reduced by 50%!");
                }
                if (creature.Health <= 0 && creature.Happiness <= 0)
                {
                    ConsoleDisplay.ShowError($"   [X] {creature.Name} is generating NO income!");
                }
            }
            
            if (CheckAllCreaturesDeadAndUnhappy())
            {
                _state.IsGameOver = true;
                _state.GameOverReason = "all_creatures_lost";
                return;
            }
            
            _creatureService.ProcessEndOfDay();
            
            _state.AdvanceDay();
            
            int eventRoll = _random.Next(4);
            switch (eventRoll)
            {
                case 0:
                    TriggerFoundIngredientsEvent();
                    break;
                case 1:
                    TriggerHappinessBoostEvent();
                    break;
                case 2:
                    TriggerFoundCurrencyEvent();
                    break;
                case 3:
                    break;
            }
            
            ConsoleDisplay.ShowSuccess($"\n*** A new day begins! Welcome to Day {_state.CurrentDay}! ***");
            ConsoleDisplay.ShowInfo($"\nYour creatures are ready to earn more currency today.");
            
            InputHandler.WaitForKey();
            
            ConsoleHelper.Clear();
        }
        
        private bool CheckAllCreaturesDeadAndUnhappy()
        {
            if (_state.Player.Creatures.Count == 0)
                return false;
                
            return _state.Player.Creatures.All(c => c.Health <= 0 && c.Happiness <= 0);
        }
        
        private void ApplyPlayingBonuses(Creature creature)
        {
            int happinessBoost = _random.Next(15, 26);
            creature.Happiness = Math.Min(100, creature.Happiness + happinessBoost);
            
            string reaction = GetHappinessReaction(creature);
            ConsoleDisplay.ShowSuccess($"\n{reaction}");
        }
        
        private void ApplyFeedingBonuses(Creature creature, Result food)
        {
            if (food.Item.Name.Contains("Healing Potion", StringComparison.OrdinalIgnoreCase))
            {
                creature.Health = 100;
                creature.Happiness = 100;
                ConsoleDisplay.ShowSuccess($"\n*** {creature.Name} drinks the Healing Potion and feels completely restored! ***");
                return;
            }
            
            if (creature.Type == CreatureType.Dragon && 
                food.Item.Name.Contains("Hot Chocolate", StringComparison.OrdinalIgnoreCase))
            {
                creature.Health = Math.Min(100, creature.Health + 25);
                creature.Happiness = Math.Min(100, creature.Happiness + 15);
                ConsoleDisplay.ShowSuccess($"\n[Dragon] {creature.Name} the Dragon enjoys the hot chocolate!");
            }
            else if (creature.Type == CreatureType.Fairy && 
                     food.Item.Name.Contains("Bread", StringComparison.OrdinalIgnoreCase))
            {
                creature.Health = Math.Min(100, creature.Health + 30);
                creature.Happiness = Math.Min(100, creature.Happiness + 10);
                ConsoleDisplay.ShowSuccess($"\n[Fairy] {creature.Name} the Fairy nibbles daintily on the fresh bread!");
            }
            else if (creature.Type == CreatureType.Goblin && 
                     food.Item.Name.Contains("Jelly Beans", StringComparison.OrdinalIgnoreCase))
            {
                creature.Health = Math.Min(100, creature.Health + 20);
                creature.Happiness = Math.Min(100, creature.Happiness + 20);
                ConsoleDisplay.ShowSuccess($"\n[Goblin] {creature.Name} the Goblin munches happily on jelly beans!");
            }
        }
        
        private string GetHappinessReaction(Creature creature)
        {
            if (creature.Happiness >= 90)
            {
                switch (creature.Type)
                {
                    case CreatureType.Dragon:
                        return $"{creature.Name} purrs contentedly, sending small smoke rings into the air.";
                    case CreatureType.Fairy:
                        return $"{creature.Name} sparkles with joy, creating a beautiful light show around you.";
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
            else if (creature.Happiness > 0)
            {
                return $"{creature.Name} seems a bit happier now, but could use more attention soon.";
            }
            else
            {
                return $"{creature.Name} doesn't seem to respond to your affection anymore...";
            }
        }
        
        private void TriggerFoundIngredientsEvent()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n*** Random Event! ***");
            
            var randomItem = GetRandomItem();
            _state.Player.Inventory.Add(randomItem.Id.ToString(), 1);
            Console.WriteLine($"You found 1 {randomItem.Unit} of {randomItem.Name}!");
            
            Console.ResetColor();
        }
        
        private void TriggerHappinessBoostEvent()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n*** Random Event! ***");
            
            int happinessBoost = _random.Next(10, 16);
            
            foreach (var creature in _state.Player.Creatures)
            {
                int oldHappiness = creature.Happiness;
                creature.Happiness = Math.Min(100, creature.Happiness + happinessBoost);
                int actualIncrease = creature.Happiness - oldHappiness;
                
                Console.WriteLine($"{creature.Name} the {creature.Type}: Happiness +{actualIncrease}%");
                creature.CalculateDailyCurrency();
            }
            
            Console.WriteLine("\nAll your creatures are feeling extra happy today!");
            Console.WriteLine($"A magical breeze of joy swept through the land! (+{happinessBoost} Happiness to all)");
            
            Console.ResetColor();
        }
        
        private void TriggerFoundCurrencyEvent()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n*** Random Event! ***");
            
            decimal found = _random.Next(5, 20);
            _state.Player.Currency += found;
            Console.WriteLine($"You found {found:C} on the ground!");
            
            Console.ResetColor();
        }
        
        private CraftingEngine.Item GetRandomItem()
        {
            var items = new[] { 
                CraftingEngine.GameItems.Milk,
                CraftingEngine.GameItems.ChocolateChip,
                CraftingEngine.GameItems.Cinnamon,
                CraftingEngine.GameItems.Flour,
                CraftingEngine.GameItems.Yeast,
                CraftingEngine.GameItems.Honey,
                CraftingEngine.GameItems.Herb,
                CraftingEngine.GameItems.CrystalWater,
                CraftingEngine.GameItems.MoonDust,
                CraftingEngine.GameItems.Sugar,
                CraftingEngine.GameItems.Gelatin,
                CraftingEngine.GameItems.FruitJuice
            };
            return items[_random.Next(items.Length)];
        }
    }
}