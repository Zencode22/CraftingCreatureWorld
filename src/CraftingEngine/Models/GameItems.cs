#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace CraftingEngine
{
    public static class GameItems
    {
        private static Item Register(Item item)
        {
            ItemRegistry.Register(item);
            return item;
        }

        // Hot Chocolate ingredients (Dragon)
        public static readonly Item Milk = Register(new Item("Milk", "cups", 0.50m));
        public static readonly Item ChocolateChip = Register(new Item("Chocolate Chips", "cup", 1.20m));
        public static readonly Item Cinnamon = Register(new Item("Cinnamon", "sticks", 0.80m));
        public static readonly Item HotChocolate = Register(new Item("Hot Chocolate", "ounces", 2.00m, category: ItemCategory.Consumable));
        
        // Bread ingredients (Fairy)
        public static readonly Item Flour = Register(new Item("Flour", "cups", 0.30m));
        public static readonly Item Yeast = Register(new Item("Yeast", "cup", 0.80m));
        public static readonly Item Honey = Register(new Item("Honey", "cups", 0.90m));
        public static readonly Item Bread = Register(new Item("Bread", "loaf", 1.50m, category: ItemCategory.Consumable));
        
        // Healing Potion ingredients (All)
        public static readonly Item Herb = Register(new Item("Herb", "pieces", 0.40m));
        public static readonly Item CrystalWater = Register(new Item("Crystal Water", "vials", 2.00m));
        public static readonly Item MoonDust = Register(new Item("Moon Dust", "pinches", 1.50m));
        public static readonly Item HealingPotion = Register(new Item("Healing Potion", "bottle", 3.00m, category: ItemCategory.Consumable));
        
        // Jelly Beans ingredients (Goblin)
        public static readonly Item Sugar = Register(new Item("Sugar", "cups", 0.60m));
        public static readonly Item Gelatin = Register(new Item("Gelatin", "packets", 1.00m));
        public static readonly Item FruitJuice = Register(new Item("Fruit Juice", "cups", 0.70m));
        public static readonly Item JellyBeans = Register(new Item("Jelly Beans", "handful", 1.80m, category: ItemCategory.Consumable));
    }
}
#pragma warning restore IDE0130