using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CraftingEngine
{
    public static class ItemRegistry
    {
        private static readonly Dictionary<Guid, Item> _lookup = new();

        public static void Register(Item item) => _lookup[item.Id] = item;

        public static bool TryGet(Guid id, [NotNullWhen(true)] out Item? item)
        {
            return _lookup.TryGetValue(id, out item);
        }
        
        public static Item Get(Guid id)
        {
            if (_lookup.TryGetValue(id, out var item))
                return item;
            throw new KeyNotFoundException($"Item with ID {id} not found in registry.");
        }
    }
}