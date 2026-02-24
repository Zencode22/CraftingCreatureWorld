using System;
using System.Collections.Generic;

namespace CraftingEngine
{
    public sealed class Inventory
    {
        private readonly Dictionary<string, decimal> _store =
            new(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyDictionary<string, decimal> Contents => _store;

        public void Add(string itemId, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(itemId))
                throw new ArgumentException("ItemId required", nameof(itemId));
            if (amount <= 0) throw new ArgumentException("Amount must be > 0", nameof(amount));

            if (_store.ContainsKey(itemId))
                _store[itemId] += amount;
            else
                _store[itemId] = amount;
        }

        public bool Remove(string itemId, decimal amount)
        {
            if (!_store.TryGetValue(itemId, out var have) || have < amount)
                return false;

            _store[itemId] = have - amount;
            if (_store[itemId] == 0) _store.Remove(itemId);
            return true;
        }

        public bool Has(string itemId, decimal amount) =>
            _store.TryGetValue(itemId, out var have) && have >= amount;

        public decimal GetAmount(string itemId) =>
            _store.TryGetValue(itemId, out var amt) ? amt : 0m;
            
        public void Clear() => _store.Clear();
    }
}