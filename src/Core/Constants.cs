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
            public const decimal MILK_PRICE = 0.50m;
            public const decimal CHOCOLATE_CHIP_PRICE = 1.20m;
            public const decimal FLOUR_PRICE = 0.30m;
            public const decimal WATER_PRICE = 0.00m;
            public const decimal YEAST_PRICE = 0.80m;
            public const decimal HERB_PRICE = 0.40m;
            public const decimal SUGAR_PRICE = 0.60m;
        }
        
        public static class StartingInventory
        {
            public const int TRADER_MILK_STOCK = 20;
            public const int TRADER_WATER_STOCK = 30;
            public const int TRADER_FLOUR_STOCK = 25;
            public const int TRADER_YEAST_STOCK = 15;
            public const int TRADER_HERB_STOCK = 20;
            public const int TRADER_CHOCOLATE_STOCK = 15;
            public const int TRADER_SUGAR_STOCK = 20;
        }
    }
}