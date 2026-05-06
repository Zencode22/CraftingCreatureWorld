using System;

namespace CraftingCreatureWorld.Services
{
    public static class TraderNames
    {
        private static readonly Random _random = new();
        
        private static readonly string[] Names = new[]
        {
            "Elon Musk",
            "Jeff Bezos",
            "Bernard Arnault",
            "Bill Gates",
            "Mark Zuckerberg",
            "Warren Buffett",
            "Larry Ellison",
            "Larry Page",
            "Sergey Brin",
            "Steve Ballmer",
            "Michael Bloomberg",
            "Jim Walton",
            "Alice Walton",
            "Rob Walton",
            "Mackenzie Scott",
            "Gautam Adani",
            "Mukesh Ambani",
            "Carlos Slim",
            "Francoise Bettencourt",
            "Amancio Ortega",
            "Phil Knight",
            "Jack Ma",
            "Pony Ma",
            "Zhang Yiming",
            "Jensen Huang"
        };
        
        public static string GetRandomName()
        {
            return Names[_random.Next(Names.Length)];
        }
    }
}