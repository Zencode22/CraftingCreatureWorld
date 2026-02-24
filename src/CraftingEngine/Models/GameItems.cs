namespace CraftingEngine
{
    public static class GameItems
    {
        private static Item Register(Item item)
        {
            ItemRegistry.Register(item);
            return item;
        }

        public static readonly Item Milk = Register(new Item("Milk", "cups", 0.50m));
        public static readonly Item ChocolateChip = Register(new Item("Chocolate Chips", "cup", 1.20m));
        public static readonly Item HotChocolate = Register(new Item("Hot Chocolate", "ounces", 2.00m, category: ItemCategory.Consumable));
        public static readonly Item Flour = Register(new Item("Flour", "cups", 0.30m));
        public static readonly Item Water = Register(new Item("Water", "cups", 0.00m));
        public static readonly Item Yeast = Register(new Item("Yeast", "cup", 0.80m));
        public static readonly Item Bread = Register(new Item("Bread", "loaf", 1.50m, category: ItemCategory.Consumable));
        public static readonly Item Herb = Register(new Item("Herb", "pieces", 0.40m));
        public static readonly Item HealingPotion = Register(new Item("Healing Potion", "bottle", 3.00m, category: ItemCategory.Consumable));
        public static readonly Item Sugar = Register(new Item("Sugar", "cups", 0.60m));
    }
}