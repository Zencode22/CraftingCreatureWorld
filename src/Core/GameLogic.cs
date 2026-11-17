using System;
using System.Collections.Generic;
using System.Linq;
using CraftingCreatureWorld.Entities;
using CraftingCreatureWorld.Services;
using CraftingEngine;
using CreatureWorld;

namespace CraftingCreatureWorld.Core
{
    /// <summary>
    /// Console-free facade over the game logic. Every operation a UI (MonoGame,
    /// tests, etc.) needs is exposed as a plain method returning data - no
    /// Console reads or writes anywhere in this layer.
    /// </summary>
    public sealed class GameLogic
    {
        public GameState State { get; }

        private readonly GameService _gameService;
        private readonly CreatureService _creatureService;
        private readonly CurrencyService _currencyService;
        private readonly TradingService _tradingService;
        private readonly CraftingService _craftingService;
        private readonly DayService _dayService;
        private readonly Random _random = new();

        public GameLogic(string playerName)
        {
            State = new GameState(playerName);
            _gameService = new GameService(State);
            _creatureService = new CreatureService(State);
            _currencyService = new CurrencyService(State);
            _tradingService = new TradingService(State);
            _craftingService = new CraftingService(State);
            _dayService = new DayService(State);
        }

        // ---------- setup ----------

        public void StartNewGame()
        {
            _gameService.AddStarterCreatures();
            _gameService.InitializeTrader();
        }

        // ---------- queries ----------

        public Player Player => State.Player;
        public int CurrentDay => State.CurrentDay;
        public bool IsGameOver => State.IsGameOver;
        public string? GameOverReason => State.GameOverReason;

        public IReadOnlyList<Creature> Creatures => State.Player.Creatures;
        public IReadOnlyList<Result> CraftedFood => State.Player.CraftedFood;
        public IReadOnlyList<Recipe> Recipes => _craftingService.GetAvailableRecipes();

        public List<(Item item, decimal price, string stock)> TraderItems()
            => _tradingService.GetAvailableItems();

        public bool CanCraft(Recipe recipe) => recipe.CanCraft(State.Player.Inventory);

        public IEnumerable<(Item item, decimal amount)> InventoryItems()
        {
            foreach (var kvp in State.Player.Inventory.Contents.Where(k => k.Value > 0))
            {
                if (Guid.TryParse(kvp.Key, out var id) && ItemRegistry.TryGet(id, out var item))
                    yield return (item, kvp.Value);
            }
        }

        public static bool FoodIsValidFor(Creature creature, string foodName)
        {
            if (foodName.Contains("Healing Potion", StringComparison.OrdinalIgnoreCase))
                return true;

            return (creature.Type == CreatureType.Dragon && foodName.Contains("Hot Chocolate", StringComparison.OrdinalIgnoreCase)) ||
                   (creature.Type == CreatureType.Fairy && foodName.Contains("Bread", StringComparison.OrdinalIgnoreCase)) ||
                   (creature.Type == CreatureType.Goblin && foodName.Contains("Jelly Beans", StringComparison.OrdinalIgnoreCase));
        }

        public static string FoodEffectDescription(string foodName)
        {
            if (foodName.Contains("Hot Chocolate")) return "+25 Health, +15 Happiness (Dragon)";
            if (foodName.Contains("Bread")) return "+30 Health, +10 Happiness (Fairy)";
            if (foodName.Contains("Jelly Beans")) return "+20 Health, +20 Happiness (Goblin)";
            if (foodName.Contains("Healing Potion")) return "Restores Health and Happiness to maximum (Any)";
            return "";
        }

        public static string FlavorFor(Creature creature)
        {
            return creature switch
            {
                Dragon d => $"{d.Name} the {d.ElementType} Dragon",
                Fairy f => $"{f.Name} the {f.BowType} Fairy",
                Goblin g => $"{g.Name} the {g.GoblinType} Goblin",
                _ => $"{creature.Name} the {creature.Type}"
            };
        }

        // ---------- actions (each advances the day) ----------

        public DayReport PlayWith(Creature creature)
        {
            return _dayService.AdvanceToNextDay($"played with {creature.Name}", creature, null);
        }

        public DayReport Feed(Creature creature, Result food)
        {
            if (!FoodIsValidFor(creature, food.Item.Name))
            {
                UiEvents.RaiseToast($"{creature.Name} the {creature.Type} won't eat {food.Item.Name}!", ToastKind.Error);
                return NoOpReport($"(invalid food for {creature.Name})");
            }

            State.Player.CraftedFood.Remove(food);
            return _dayService.AdvanceToNextDay($"fed {creature.Name} a {food.Item.Name}", creature, food);
        }

        /// <summary>Buys <paramref name="amount"/> units and advances the day.
        /// Returns the day report, or null when the purchase failed.</summary>
        public DayReport? Buy(Item item, decimal amount)
        {
            if (!_tradingService.BuyItem(item, amount))
            {
                UiEvents.RaiseToast("Insufficient currency!", ToastKind.Error);
                return null;
            }

            return _dayService.AdvanceToNextDay($"purchased {amount} {item.Unit} of {item.Name}");
        }

        public bool Craft(Recipe recipe)
        {
            if (!_craftingService.CraftRecipe(recipe))
            {
                UiEvents.RaiseToast("Missing ingredients!", ToastKind.Error);
                return false;
            }

            UiEvents.RaiseToast($"Crafted {recipe.Result}!", ToastKind.Success);
            _dayService.AdvanceToNextDay($"crafted {recipe.Name}");
            return true;
        }

        public DayReport EndDayEarly()
        {
            return _dayService.AdvanceToNextDay("ended the day early");
        }

        public void QuitGame()
        {
            State.IsGameOver = true;
            State.GameOverReason = "quit";
        }

        private DayReport NoOpReport(string action)
        {
            return new DayReport
            {
                DayEnded = State.CurrentDay,
                ActionDescription = action
            };
        }
    }
}
