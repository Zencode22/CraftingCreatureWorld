namespace CraftingEngine
{
    public sealed class Ingredient
    {
        public Quantity Value { get; }
        public Ingredient(Item item, decimal amount) => Value = new Quantity(item, amount);
        public Item Item => Value.Item;
        public decimal Amount => Value.Amount;
        public override string ToString() => Value.ToString();
    }
}