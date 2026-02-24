using CraftingEngine;
using static CraftingCreatureWorld.Core.Constants;

namespace CraftingCreatureWorld.Entities
{
    public sealed class Trader : Person
    {
        public Recipe FeaturedRecipe { get; private set; }
        
        public Trader(string name, Recipe featuredRecipe) : base(name)
        {
            FeaturedRecipe = featuredRecipe;
            InitializeInventory();
        }
        
        private void InitializeInventory()
        {
            // Add ingredients for the featured recipe (double the required amount)
            foreach (var ingredient in FeaturedRecipe.Ingredients)
            {
                Inventory.Add(ingredient.Item.Id.ToString(), ingredient.Amount * 2);
            }
            
            // Add common items with stock
            Inventory.Add(GameItems.Milk.Id.ToString(), StartingInventory.TRADER_MILK_STOCK);
            Inventory.Add(GameItems.Water.Id.ToString(), StartingInventory.TRADER_WATER_STOCK);
            Inventory.Add(GameItems.Flour.Id.ToString(), StartingInventory.TRADER_FLOUR_STOCK);
            Inventory.Add(GameItems.Yeast.Id.ToString(), StartingInventory.TRADER_YEAST_STOCK);
            Inventory.Add(GameItems.Herb.Id.ToString(), StartingInventory.TRADER_HERB_STOCK);
            Inventory.Add(GameItems.ChocolateChip.Id.ToString(), StartingInventory.TRADER_CHOCOLATE_STOCK);
            Inventory.Add(GameItems.Sugar.Id.ToString(), StartingInventory.TRADER_SUGAR_STOCK);
        }
        
        public bool SellItem(Item item, decimal amount, Player buyer)
        {
            decimal totalCost = item.BasePrice * amount;
            
            if (buyer.Currency >= totalCost && Inventory.Has(item.Id.ToString(), amount))
            {
                buyer.Currency -= totalCost;
                Inventory.Remove(item.Id.ToString(), amount);
                buyer.Inventory.Add(item.Id.ToString(), amount);
                return true;
            }
            
            return false;
        }
    }
}