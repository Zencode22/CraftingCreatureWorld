using CraftingEngine;

namespace CraftingCreatureWorld.Entities
{
    public sealed class Trader(string name, Recipe featuredRecipe) : Person(name)
    {
        public Recipe FeaturedRecipe { get; private set; } = featuredRecipe;
    }
}