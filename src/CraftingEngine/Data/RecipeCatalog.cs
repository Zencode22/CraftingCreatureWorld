#pragma warning disable IDE0130 // Namespace does not match folder structure
using System.Collections.Generic;

namespace CraftingEngine
{
    public static class RecipeCatalog
    {
        public static List<Recipe> LoadStarterRecipes()
        {
            var m = GameItems.Milk;
            var cc = GameItems.ChocolateChip;
            var ci = GameItems.Cinnamon;
            var hc = GameItems.HotChocolate;
            var f = GameItems.Flour;
            var y = GameItems.Yeast;
            var ho = GameItems.Honey;
            var b = GameItems.Bread;
            var h = GameItems.Herb;
            var cw = GameItems.CrystalWater;
            var md = GameItems.MoonDust;
            var hp = GameItems.HealingPotion;
            var s = GameItems.Sugar;
            var g = GameItems.Gelatin;
            var fj = GameItems.FruitJuice;
            var jb = GameItems.JellyBeans;

            var hotChocolateRecipe = new Recipe(
                name: "Hot Chocolate",
                result: new Result(hc, 12m),
                ingredients: [new Ingredient(m, 4m), new Ingredient(cc, 0.5m), new Ingredient(ci, 2m)],
                isStarter: true,
                targetCreature: "Dragon");

            var breadRecipe = new Recipe(
                name: "Bread",
                result: new Result(b, 1m),
                ingredients: [new Ingredient(f, 3m), new Ingredient(y, 0.02m), new Ingredient(ho, 0.5m)],
                isStarter: true,
                targetCreature: "Fairy");

            var potionRecipe = new Recipe(
                name: "Healing Potion",
                result: new Result(hp, 1m),
                ingredients: [new Ingredient(h, 2m), new Ingredient(cw, 0.5m), new Ingredient(md, 3m)],
                isStarter: true,
                targetCreature: "All");

            var jellyBeansRecipe = new Recipe(
                name: "Jelly Beans",
                result: new Result(jb, 8m),
                ingredients: [new Ingredient(s, 0.5m), new Ingredient(g, 0.3m), new Ingredient(fj, 0.5m)],
                isStarter: false,
                targetCreature: "Goblin");

            return [hotChocolateRecipe, breadRecipe, potionRecipe, jellyBeansRecipe];
        }
        
        public static Recipe? GetRecipeByName(string name)
        {
            var recipes = LoadStarterRecipes();
            return recipes.Find(r => r.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase)) 
                   ?? recipes[0];
        }
    }
}
#pragma warning restore IDE0130