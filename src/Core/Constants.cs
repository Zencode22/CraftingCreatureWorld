namespace CraftingCreatureWorld.Core
{
    public static class Constants
    {
        public const string RECIPE_OUTPUT_FILE = "crafting_instructions.txt";
        
        public static class CreatureStats
        {
            public const int MAX_HEALTH = 100;
            public const int MAX_HAPPINESS = 100;
            public const int BASE_HAPPINESS = 70;
        }
        
        public static class Pricing
        {
            // Tier 1: Milk = Flour = Sugar = Herb
            public const decimal MILK_PRICE = 0.50m;
            public const decimal FLOUR_PRICE = 0.50m;
            public const decimal SUGAR_PRICE = 0.50m;
            public const decimal HERB_PRICE = 1.00m;  // Double for potion
            
            // Tier 2: Chocolate Chips = Yeast = Gelatin = Crystal Water
            public const decimal CHOCOLATE_CHIP_PRICE = 1.20m;
            public const decimal YEAST_PRICE = 1.20m;
            public const decimal GELATIN_PRICE = 1.20m;
            public const decimal CRYSTAL_WATER_PRICE = 2.40m;  // Double for potion
            
            // Tier 3: Cinnamon = Honey = Fruit Juice = Moon Dust
            public const decimal CINNAMON_PRICE = 0.80m;
            public const decimal HONEY_PRICE = 0.80m;
            public const decimal FRUIT_JUICE_PRICE = 0.80m;
            public const decimal MOON_DUST_PRICE = 1.60m;  // Double for potion
        }
    }
}