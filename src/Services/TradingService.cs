using System.Collections.Generic;
using CraftingCreatureWorld.Core;
using CraftingEngine;
using static CraftingCreatureWorld.Core.Constants;

namespace CraftingCreatureWorld.Services
{
    public class TradingService
    {
        private readonly GameState _state;
        private readonly CurrencyService _currencyService;
        private readonly System.Random _random = new();
        
        private readonly List<Recipe> _allRecipes;
        private int _currentRecipeGroup = 0;
        private int _lastRotationDay = 1;
        
        public TradingService(GameState state)
        {
            _state = state;
            _currencyService = new CurrencyService(state);
            _allRecipes = RecipeCatalog.LoadStarterRecipes();
            _currentRecipeGroup = _random.Next(3);
        }
        
        public List<(Item item, decimal price, string stock)> GetAvailableItems()
        {
            var items = new List<(Item, decimal, string)>();
            int currentDay = _state.CurrentDay;
            
            if (currentDay - _lastRotationDay >= 7)
            {
                RotateRecipe();
                _lastRotationDay = currentDay;
            }
            
            items.Add((GameItems.Herb, Pricing.HERB_PRICE, "∞"));
            items.Add((GameItems.CrystalWater, Pricing.CRYSTAL_WATER_PRICE, "∞"));
            items.Add((GameItems.MoonDust, Pricing.MOON_DUST_PRICE, "∞"));
            
            var activeRecipe = GetActiveRecipe();
            
            foreach (var ingredient in activeRecipe.Ingredients)
            {
                decimal price = GetIngredientPrice(ingredient.Item);
                items.Add((ingredient.Item, price, "∞"));
            }
            
            return items;
        }
        
        private void RotateRecipe()
        {
            var otherGroups = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                if (i != _currentRecipeGroup)
                {
                    otherGroups.Add(i);
                }
            }
            
            _currentRecipeGroup = otherGroups[_random.Next(2)];
        }
        
        private Recipe GetActiveRecipe()
        {
            var creatureRecipes = _allRecipes.FindAll(r => r.TargetCreature != "All");
            
            if (creatureRecipes.Count > 0)
            {
                return creatureRecipes[_currentRecipeGroup % creatureRecipes.Count];
            }
            
            return _allRecipes[0];
        }
        
        private static decimal GetIngredientPrice(Item item)
        {
            if (item.Id == GameItems.Milk.Id) return Pricing.MILK_PRICE;
            if (item.Id == GameItems.ChocolateChip.Id) return Pricing.CHOCOLATE_CHIP_PRICE;
            if (item.Id == GameItems.Cinnamon.Id) return Pricing.CINNAMON_PRICE;
            if (item.Id == GameItems.Flour.Id) return Pricing.FLOUR_PRICE;
            if (item.Id == GameItems.Yeast.Id) return Pricing.YEAST_PRICE;
            if (item.Id == GameItems.Honey.Id) return Pricing.HONEY_PRICE;
            if (item.Id == GameItems.Sugar.Id) return Pricing.SUGAR_PRICE;
            if (item.Id == GameItems.Gelatin.Id) return Pricing.GELATIN_PRICE;
            if (item.Id == GameItems.FruitJuice.Id) return Pricing.FRUIT_JUICE_PRICE;
            if (item.Id == GameItems.Herb.Id) return Pricing.HERB_PRICE;
            if (item.Id == GameItems.CrystalWater.Id) return Pricing.CRYSTAL_WATER_PRICE;
            if (item.Id == GameItems.MoonDust.Id) return Pricing.MOON_DUST_PRICE;
            
            return item.BasePrice;
        }
        
        public bool BuyItem(Item item, decimal amount)
        {
            decimal price = GetIngredientPrice(item);
            decimal totalCost = price * amount;
            
            if (_currencyService.CanAfford(totalCost))
            {
                _currencyService.SpendCurrency(totalCost);
                _state.Player.Inventory.Add(item.Id.ToString(), amount);
                return true;
            }
            
            return false;
        }
    }
}