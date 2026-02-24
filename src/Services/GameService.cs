using System;
using CraftingCreatureWorld.Core;
using CraftingCreatureWorld.Entities;
using CreatureWorld;
using CraftingEngine;
using System.Linq;

namespace CraftingCreatureWorld.Services
{
    public class GameService
    {
        private readonly GameState _state;
        private readonly CreatureService _creatureService;
        private readonly CurrencyService _currencyService;
        private readonly CraftingService _craftingService;
        private readonly TradingService _tradingService;
        private readonly Random _random = new();
        
        public GameService(GameState state)
        {
            _state = state;
            _creatureService = new CreatureService(state);
            _currencyService = new CurrencyService(state);
            _craftingService = new CraftingService(state);
            _tradingService = new TradingService(state);
        }
        
        public void AddStarterCreatures()
        {
            var creatures = new Creature[]
            {
                new Dragon("Ember", 50, 100, Element.Fire),
                new Elf("Lyra", 120, 80, BowType.Longbow),
                new Goblin("Glimmer", 20, 60, GoblinType.Cave)
            };
            
            foreach (var creature in creatures)
            {
                _creatureService.AddCreature(creature);
            }
            
            Console.WriteLine("\n✨ You received 3 starter creatures!");
        }
        
        public void InitializeTrader()
        {
            var allRecipes = RecipeCatalog.LoadStarterRecipes();
            var randomRecipe = allRecipes[_random.Next(allRecipes.Count)];
            _state.Trader = new Trader("Merchant Gregory", randomRecipe);
            Console.WriteLine($"\n🧑‍🌾 Trader Gregory has arrived with his wares!");
        }
        
        public void ProcessDay()
        {
            decimal earnings = _currencyService.CollectDailyCurrency();
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n💰 Your creatures earned you {earnings:C} today!");
            Console.ResetColor();
            
            _creatureService.ProcessEndOfDay();
            
            // Random event chance
            if (_random.Next(100) < 20) // 20% chance
            {
                TriggerRandomEvent();
            }
        }
        
        private void TriggerRandomEvent()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n✨ Random Event! ✨");
            
            int eventType = _random.Next(3);
            switch (eventType)
            {
                case 0: // Found ingredients
                    var randomItem = GetRandomItem();
                    _state.Player.Inventory.Add(randomItem.Id.ToString(), 1);
                    Console.WriteLine($"You found 1 {randomItem.Unit} of {randomItem.Name}!");
                    break;
                    
                case 1: // Creature happiness boost
                    foreach (var creature in _state.Player.Creatures)
                    {
                        creature.Happiness = Math.Min(100, creature.Happiness + 10);
                    }
                    Console.WriteLine("All your creatures are feeling extra happy today!");
                    break;
                    
                case 2: // Found currency
                    decimal found = _random.Next(5, 20);
                    _state.Player.Currency += found;
                    Console.WriteLine($"You found {found:C} on the ground!");
                    break;
            }
            
            Console.ResetColor();
        }
        
        private Item GetRandomItem()
        {
            var items = new[] { GameItems.Herb, GameItems.Sugar, GameItems.Milk, GameItems.Flour };
            return items[_random.Next(items.Length)];
        }
    }
}