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

        public Recipe(string name,
                      Result result,
                      IEnumerable<Ingredient> ingredients,
                      bool isStarter = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name required", nameof(name));

            Name = name;
            Result = result ?? throw new ArgumentNullException(nameof(result));
            Ingredients = ingredients?.ToList().AsReadOnly()
                         ?? throw new ArgumentNullException(nameof(ingredients));
            IsStarter = isStarter;
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

        public override string ToString()
        {
            var ingList = string.Join(", ", Ingredients.Select(i => i.ToString()));
            return $"{Name} → {Result.Amount} {Result.Item.Unit} {Result.Item.Name} (needs {ingList})";
        }
    }
}