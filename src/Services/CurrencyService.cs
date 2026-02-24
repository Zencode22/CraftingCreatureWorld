using CraftingCreatureWorld.Core;

namespace CraftingCreatureWorld.Services
{
    public class CurrencyService
    {
        private readonly GameState _state;
        
        public CurrencyService(GameState state)
        {
            _state = state;
        }
        
        public decimal CollectDailyCurrency()
        {
            decimal total = 0;
            foreach (var creature in _state.Player.Creatures)
            {
                total += creature.DailyCurrency;
            }
            
            _state.Player.Currency += total;
            return total;
        }
        
        public bool SpendCurrency(decimal amount)
        {
            if (_state.Player.Currency >= amount)
            {
                _state.Player.Currency -= amount;
                return true;
            }
            return false;
        }
        
        public bool CanAfford(decimal amount) => _state.Player.Currency >= amount;
        
        public void AddCurrency(decimal amount)
        {
            if (amount > 0)
                _state.Player.Currency += amount;
        }
    }
}