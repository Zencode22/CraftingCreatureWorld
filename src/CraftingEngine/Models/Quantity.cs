using System;
using System.Globalization;

namespace CraftingEngine
{
    public sealed class Quantity : IEquatable<Quantity>
    {
        public Item Item { get; }
        public decimal Amount { get; }

        public Quantity(Item item, decimal amount)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
            if (amount <= 0) throw new ArgumentException("Amount must be > 0", nameof(amount));
            Amount = amount;
        }

        public override string ToString()
            => $"{Amount.ToString(CultureInfo.InvariantCulture)} {Item.Unit} {Item.Name}";

        public bool Equals(Quantity? other) =>
            other != null && Item.Id == other.Item.Id && Amount == other.Amount;

        public override bool Equals(object? obj) => Equals(obj as Quantity);
        public override int GetHashCode() => HashCode.Combine(Item.Id, Amount);
    }
}