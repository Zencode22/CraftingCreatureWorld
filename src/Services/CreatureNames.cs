using System;
using CreatureWorld;

namespace CraftingCreatureWorld.Services
{
    public static class CreatureNames
    {
        private static readonly Random _random = new();
        
        // Dragon names - themed around Chinese Emperors
        private static readonly string[] DragonNames = new[]
        {
            "Qin Shi Huang",
            "Wu of Han",
            "Taizong of Tang",
            "Kangxi",
            "Qianlong",
            "Yongle",
            "Hongwu",
            "Xuanzong",
            "Gaozu of Han",
            "Wen of Sui",
            "Zhu Di",
            "Li Shimin",
            "Zhao Kuangyin",
            "Liu Bang",
            "Cao Pi"
        };
        
        // Fairy names - Famous Medieval European Women
        private static readonly string[] FairyNames = new[]
        {
            "Eleanor of Aquitaine",
            "Hildegard of Bingen",
            "Joan of Arc",
            "Christine de Pizan",
            "Matilda of Tuscany",
            "Heloise",
            "Marie de France",
            "Julian of Norwich",
            "Margery Kempe",
            "Brigid of Kildare",
            "Clare of Assisi",
            "Catherine of Siena",
            "Aethelflaed",
            "Emma of Normandy",
            "Theodora"
        };
        
        // Goblin names - Famous Scientists
        private static readonly string[] GoblinNames = new[]
        {
            "Newton",
            "Einstein",
            "Curie",
            "Darwin",
            "Tesla",
            "Galileo",
            "Hawking",
            "Feynman",
            "Sagan",
            "Turing",
            "Lovelace",
            "Faraday",
            "Maxwell",
            "Planck",
            "Bohr"
        };
        
        public static string GetRandomDragonName()
        {
            return DragonNames[_random.Next(DragonNames.Length)];
        }
        
        public static string GetRandomFairyName()
        {
            return FairyNames[_random.Next(FairyNames.Length)];
        }
        
        public static string GetRandomGoblinName()
        {
            return GoblinNames[_random.Next(GoblinNames.Length)];
        }
    }
}