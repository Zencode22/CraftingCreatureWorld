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
        
        public TradingService(GameState state)
        {
            _state = state;
            _currencyService = new CurrencyService(state);
        }
        
        public List<(Item item, decimal price, int stock)> GetAvailableItems()
        {
            var items = new List<(Item, decimal, int)>
            {
                (GameItems.Milk, Pricing.MILK_PRICE, GetStock(GameItems.Milk)),
                (GameItems.ChocolateChip, Pricing.CHOCOLATE_CHIP_PRICE, GetStock(GameItems.ChocolateChip)),
                (GameItems.Flour, Pricing.FLOUR_PRICE, GetStock(GameItems.Flour)),
                (GameItems.Water, Pricing.WATER_PRICE, GetStock(GameItems.Water)),
                (GameItems.Yeast, Pricing.YEAST_PRICE, GetStock(GameItems.Yeast)),
                (GameItems.Herb, Pricing.HERB_PRICE, GetStock(GameItems.Herb)),
                (GameItems.Sugar, Pricing.SUGAR_PRICE, GetStock(GameItems.Sugar))
            };
            
            return items;
        }
        
        private int GetStock(Item item)
        {
            return (int)_state.Trader!.Inventory.GetAmount(item.Id.ToString());
        }
        
        public bool BuyItem(Item item, decimal amount)
        {
            decimal totalCost = item.BasePrice * amount;
            
            if (_currencyService.CanAfford(totalCost) && 
                _state.Trader!.Inventory.Has(item.Id.ToString(), amount))
            {
                _currencyService.SpendCurrency(totalCost);
                _state.Trader.Inventory.Remove(item.Id.ToString(), amount);
                _state.Player.Inventory.Add(item.Id.ToString(), amount);
                return true;
            }
            
            return false;
        }
        
        public void RestockTrader()
        {
            _state.Trader!.Inventory.Add(GameItems.Milk.Id.ToString(), 5);
            _state.Trader.Inventory.Add(GameItems.Flour.Id.ToString(), 5);
            _state.Trader.Inventory.Add(GameItems.Herb.Id.ToString(), 3);
        }
    }
}