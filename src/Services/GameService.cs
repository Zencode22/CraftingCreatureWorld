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
                new Fairy("Lyra", 120, 80, BowType.Longbow),
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
    }
}