using System;
using System.Linq;
using CraftingCreatureWorld.Core;
using CreatureWorld;
using CraftingEngine;

namespace CraftingCreatureWorld.Services
{
    /// <summary>
    /// Handles day progression. This version is UI-agnostic: instead of writing
    /// to the console it returns a <see cref="DayReport"/> describing everything
    /// that happened, and raises events through <see cref="UiEvents"/>.
    /// </summary>
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

        public DayReport AdvanceToNextDay(string actionPerformed = "", Creature? interactedCreature = null, Result? foodUsed = null)
        {
            var report = new DayReport
            {
                DayEnded = _state.CurrentDay,
                ActionDescription = actionPerformed
            };

            // 1. Collect daily currency from all creatures.
            report.Earnings = _currencyService.CollectDailyCurrency();

            // 2. Natural decay for every creature.
            foreach (var creature in _state.Player.Creatures)
            {
                int oldHealth = creature.Health;
                int oldHappiness = creature.Happiness;

                creature.Health = Math.Max(0, creature.Health - _random.Next(2, 6));
                creature.Happiness = Math.Max(0, creature.Happiness - _random.Next(3, 8));

                creature.OldHealth = oldHealth;
                creature.OldHappiness = oldHappiness;
            }

            // 3. Bonus for the interaction that triggered this day change.
            if (interactedCreature != null)
            {
                if (foodUsed != null)
                    ApplyFeedingBonuses(interactedCreature, foodUsed, report);
                else
                    ApplyPlayingBonuses(interactedCreature, report);
            }

            // 4. Recompute income and build per-creature change summaries.
            foreach (var creature in _state.Player.Creatures)
            {
                creature.CalculateDailyCurrency();

                var change = new CreatureChange
                {
                    Name = creature.Name,
                    Type = creature.Type.ToString(),
                    OldHealth = creature.OldHealth,
                    NewHealth = creature.Health,
                    OldHappiness = creature.OldHappiness,
                    NewHappiness = creature.Happiness
                };

                if (creature.Health <= 0 && creature.Happiness > 0)
                    change.Warnings.Add("No health left! Income reduced by 50%!");
                if (creature.Happiness <= 0 && creature.Health > 0)
                    change.Warnings.Add("Completely unhappy! Income reduced by 50%!");
                if (creature.Health <= 0 && creature.Happiness <= 0)
                {
                    change.Warnings.Clear();
                    change.Warnings.Add("Generating NO income!");
                }

                report.Changes.Add(change);
            }

            // 5. Game over check.
            if (_state.Player.Creatures.Count > 0 &&
                _state.Player.Creatures.All(c => c.Health <= 0 && c.Happiness <= 0))
            {
                _state.IsGameOver = true;
                _state.GameOverReason = "all_creatures_lost";
                report.GameOver = true;
                report.GameOverReason = _state.GameOverReason;
                return report;
            }

            _creatureService.ProcessEndOfDay();
            _state.AdvanceDay();

            // 6. Random event roll.
            switch (_random.Next(4))
            {
                case 0: TriggerFoundIngredientsEvent(report); break;
                case 1: TriggerHappinessBoostEvent(report); break;
                case 2: TriggerFoundCurrencyEvent(report); break;
            }

            UiEvents.RaiseToast($"A new day begins! Welcome to Day {_state.CurrentDay}!", ToastKind.Success);

            return report;
        }

        private void ApplyPlayingBonuses(Creature creature, DayReport report)
        {
            int happinessBoost = _random.Next(15, 26);
            creature.Happiness = Math.Min(100, creature.Happiness + happinessBoost);
            report.InteractionLines.Add(GetHappinessReaction(creature));
        }

        private void ApplyFeedingBonuses(Creature creature, Result food, DayReport report)
        {
            if (food.Item.Name.Contains("Healing Potion", StringComparison.OrdinalIgnoreCase))
            {
                creature.Health = 100;
                creature.Happiness = 100;
                report.InteractionLines.Add($"*** {creature.Name} drinks the Healing Potion and feels completely restored! ***");
                return;
            }

            if (creature.Type == CreatureType.Dragon &&
                food.Item.Name.Contains("Hot Chocolate", StringComparison.OrdinalIgnoreCase))
            {
                creature.Health = Math.Min(100, creature.Health + 25);
                creature.Happiness = Math.Min(100, creature.Happiness + 15);
                report.InteractionLines.Add($"[Dragon] {creature.Name} the Dragon enjoys the hot chocolate!");
            }
            else if (creature.Type == CreatureType.Fairy &&
                     food.Item.Name.Contains("Bread", StringComparison.OrdinalIgnoreCase))
            {
                creature.Health = Math.Min(100, creature.Health + 30);
                creature.Happiness = Math.Min(100, creature.Happiness + 10);
                report.InteractionLines.Add($"[Fairy] {creature.Name} the Fairy nibbles daintily on the fresh bread!");
            }
            else if (creature.Type == CreatureType.Goblin &&
                     food.Item.Name.Contains("Jelly Beans", StringComparison.OrdinalIgnoreCase))
            {
                creature.Health = Math.Min(100, creature.Health + 20);
                creature.Happiness = Math.Min(100, creature.Happiness + 20);
                report.InteractionLines.Add($"[Goblin] {creature.Name} the Goblin munches happily on jelly beans!");
            }
        }

        private string GetHappinessReaction(Creature creature)
        {
            if (creature.Happiness >= 90)
            {
                return creature.Type switch
                {
                    CreatureType.Dragon => $"{creature.Name} purrs contentedly, sending small smoke rings into the air.",
                    CreatureType.Fairy => $"{creature.Name} sparkles with joy, creating a beautiful light show around you.",
                    CreatureType.Goblin => $"{creature.Name} does a little jig and offers you their favorite shiny rock.",
                    _ => $"{creature.Name} snuggles up to you, happy and content."
                };
            }
            if (creature.Happiness >= 70)
                return $"{creature.Name} gives you a warm, appreciative look.";
            if (creature.Happiness > 0)
                return $"{creature.Name} seems a bit happier now, but could use more attention soon.";
            return $"{creature.Name} doesn't seem to respond to your affection anymore...";
        }

        private void TriggerFoundIngredientsEvent(DayReport report)
        {
            var randomItem = GetRandomItem();
            _state.Player.Inventory.Add(randomItem.Id.ToString(), 1);
            report.EventLines.Add($"Random Event! You found 1 {randomItem.Unit} of {randomItem.Name}!");
        }

        private void TriggerHappinessBoostEvent(DayReport report)
        {
            int happinessBoost = _random.Next(10, 16);

            foreach (var creature in _state.Player.Creatures)
            {
                int oldHappiness = creature.Happiness;
                creature.Happiness = Math.Min(100, creature.Happiness + happinessBoost);
                int actualIncrease = creature.Happiness - oldHappiness;
                report.EventLines.Add($"{creature.Name} the {creature.Type}: Happiness +{actualIncrease}%");
                creature.CalculateDailyCurrency();
            }

            report.EventLines.Add($"A magical breeze of joy swept through the land! (+{happinessBoost} Happiness to all)");
        }

        private void TriggerFoundCurrencyEvent(DayReport report)
        {
            decimal found = _random.Next(5, 20);
            _state.Player.Currency += found;
            report.EventLines.Add($"Random Event! You found {found:C} on the ground!");
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
