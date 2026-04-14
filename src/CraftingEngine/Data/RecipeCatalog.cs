using System.Collections.Generic;

namespace CraftingEngine
{
    public static class RecipeCatalog
    {
        public static List<Recipe> LoadStarterRecipes()
        {
            var m = GameItems.Milk;
            var cc = GameItems.ChocolateChip;
            var hc = GameItems.HotChocolate;
            var f = GameItems.Flour;
            var w = GameItems.Water;
            var y = GameItems.Yeast;
            var b = GameItems.Bread;
            var h = GameItems.Herb;
            var hp = GameItems.HealingPotion;
            var s = GameItems.Sugar;
            var jb = GameItems.JellyBeans;

            var hotChocolateRecipe = new Recipe(
                name: "Hot Chocolate",
                result: new Result(hc, 12m),
                ingredients: new[]
                {
                    new Ingredient(m, 4m),
                    new Ingredient(cc, 0.5m)
                },
                isStarter: true,
                targetCreature: "Dragon");

            var breadRecipe = new Recipe(
                name: "Bread",
                result: new Result(b, 1m),
                ingredients: new[]
                {
                    new Ingredient(f, 3m),
                    new Ingredient(w, 1.5m),
                    new Ingredient(y, 0.02m)
                },
                isStarter: true,
                targetCreature: "Fairy");

            var potionRecipe = new Recipe(
                name: "Healing Potion",
                result: new Result(hp, 1m),
                ingredients: new[]
                {
                    new Ingredient(h, 2m),
                    new Ingredient(w, 0.5m)
                },
                isStarter: true,
                targetCreature: "All");

            var jellyBeansRecipe = new Recipe(
                name: "Jelly Beans",
                result: new Result(jb, 8m),
                ingredients: new[]
                {
                    new Ingredient(s, 0.5m),
                    new Ingredient(m, 0.5m),
                    new Ingredient(f, 0.3m)
                },
                isStarter: false,
                targetCreature: "Goblin");

            return new List<Recipe>
            {
                hotChocolateRecipe,
                breadRecipe,
                potionRecipe,
                jellyBeansRecipe
            };
        }
        
        public static Recipe GetRecipeByName(string name)
        {
            var recipes = LoadStarterRecipes();
            return recipes.Find(r => r.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase)) 
                   ?? recipes[0];
        }
    }
}