using System;
using System.Collections.Generic;
using System.Linq;

namespace CraftingEngine
{
    public sealed class Recipe
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; }
        public Result Result { get; init; }
        public IReadOnlyList<Ingredient> Ingredients { get; init; }
        public bool IsStarter { get; init; }
        public string TargetCreature { get; init; }

        public Recipe(string name,
                      Result result,
                      IEnumerable<Ingredient> ingredients,
                      bool isStarter = false,
                      string targetCreature = "All")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name required", nameof(name));

            Name = name;
            Result = result ?? throw new ArgumentNullException(nameof(result));
            Ingredients = ingredients?.ToList().AsReadOnly()
                         ?? throw new ArgumentNullException(nameof(ingredients));
            IsStarter = isStarter;
            TargetCreature = targetCreature;
        }

        public bool CanCraft(Inventory inv) =>
            Ingredients.All(i => inv.Has(i.Item.Id.ToString(), i.Amount));

        public bool Craft(Inventory inv)
        {
            if (!CanCraft(inv)) return false;

            foreach (var ing in Ingredients)
                inv.Remove(ing.Item.Id.ToString(), ing.Amount);

            inv.Add(Result.Item.Id.ToString(), Result.Amount);
            return true;
        }

        public string GetEffectDescription()
        {
            return TargetCreature switch
            {
                "Dragon" => "+25 Health, +15 Happiness",
                "Fairy" => "+30 Health, +10 Happiness",
                "Goblin" => "+20 Health, +20 Happiness",
                "All" => "Restores Health and Happiness to maximum",
                _ => "Special food effect"
            };
        }

        public override string ToString()
        {
            var ingList = string.Join(", ", Ingredients.Select(i => i.ToString()));
            var creatureInfo = TargetCreature != "All" ? $" [For: {TargetCreature}]" : " [For: All Creatures]";
            var effectInfo = $" - Effects: {GetEffectDescription()}";
            return $"{Name}{creatureInfo}{effectInfo}\n   Needs: {ingList}";
        }
    }
}