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
            // Random Dragon
            Element randomElement = GetRandomElement();
            string dragonName = CreatureNames.GetRandomDragonName();
            int dragonAge = _random.Next(30, 300);
            int dragonHealth = _random.Next(80, 101);
            var dragon = new Dragon(dragonName, dragonAge, dragonHealth, randomElement);
            
            // Random Fairy
            BowType randomBow = GetRandomBowType();
            string fairyName = CreatureNames.GetRandomFairyName();
            int fairyAge = _random.Next(50, 500);
            int fairyHealth = _random.Next(70, 101);
            var fairy = new Fairy(fairyName, fairyAge, fairyHealth, randomBow);
            
            // Random Goblin
            GoblinType randomGoblinType = GetRandomGoblinType();
            string goblinName = CreatureNames.GetRandomGoblinName();
            int goblinAge = _random.Next(10, 100);
            int goblinHealth = _random.Next(60, 101);
            var goblin = new Goblin(goblinName, goblinAge, goblinHealth, randomGoblinType);
            
            var creatures = new Creature[] { dragon, fairy, goblin };
            
            foreach (var creature in creatures)
            {
                _creatureService.AddCreature(creature);
            }
            
            Console.WriteLine("\n✨ You received 3 starter creatures!");
            Console.WriteLine($"   🐉 {dragon.Name} - A {dragon.ElementType} Dragon");
            Console.WriteLine($"   🧚 {fairy.Name} - A {fairy.BowType} Fairy");
            Console.WriteLine($"   👺 {goblin.Name} - A {goblin.GoblinType} Goblin");
        }
        
        private Element GetRandomElement()
        {
            var elements = new[] { Element.Fire, Element.Ice, Element.Lightning, Element.Earth };
            return elements[_random.Next(elements.Length)];
        }
        
        private BowType GetRandomBowType()
        {
            var bows = new[] { BowType.Shortbow, BowType.Longbow, BowType.Crossbow };
            return bows[_random.Next(bows.Length)];
        }
        
        private GoblinType GetRandomGoblinType()
        {
            var types = new[] { GoblinType.Cave, GoblinType.Forest, GoblinType.Mountain };
            return types[_random.Next(types.Length)];
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