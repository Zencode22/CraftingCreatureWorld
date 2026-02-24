namespace CraftingEngine
{
    public sealed class Result
    {
        public Quantity Value { get; }

        public Result(Item item, decimal amount) => Value = new Quantity(item, amount);

        public Item Item => Value.Item;
        public decimal Amount => Value.Amount;
        public override string ToString() => Value.ToString();
    }
}