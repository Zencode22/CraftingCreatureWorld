using System;

namespace CraftingEngine
{
    public enum ItemCategory { Material, Consumable, Weapon, Tool, Quest, Misc }
    public enum Rarity { Common, Uncommon, Rare, Epic, Legendary }

    public sealed class Item : IEquatable<Item>
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; init; }
        public string Description { get; init; } = "";
        public string Unit { get; init; }
        public decimal BasePrice { get; private set; }
        public bool Stackable { get; init; } = true;
        public int MaxStackSize { get; init; } = 99;
        public ItemCategory Category { get; init; } = ItemCategory.Material;
        public Rarity Rarity { get; init; } = Rarity.Common;

        public Item(string name,
                    string unit,
                    decimal basePrice,
                    bool stackable = true,
                    int maxStackSize = 99,
                    ItemCategory category = ItemCategory.Material,
                    Rarity rarity = Rarity.Common,
                    string description = "")
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
            BasePrice = basePrice;
            Stackable = stackable;
            MaxStackSize = maxStackSize;
            Category = category;
            Rarity = rarity;
            Description = description;
        }
        
        public bool Equals(Item? other)
        {
            if (other is null) return false;
            return Id == other.Id;
        }

        public override bool Equals(object? obj) => Equals(obj as Item);
        public override int GetHashCode() => Id.GetHashCode();
        public override string ToString() => $"{Name} ({Unit})";

        public void ApplyDiscount(decimal percent)
        {
            if (percent < 0m || percent > 100m)
                throw new ArgumentOutOfRangeException(nameof(percent));
            BasePrice = Math.Round(BasePrice * (1m - percent / 100m), 2);
        }
    }
}