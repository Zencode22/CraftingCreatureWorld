using System;

namespace CreatureWorld
{
    public class Elf : Creature
    {
        public BowType BowType { get; set; }
        
        public Elf(string name, int age, int health, BowType bowType) 
            : base(name, age, health)
        {
            Type = CreatureType.Elf;
            BowType = bowType;
        }
        
        public override void MakeSound()
        {
            Console.WriteLine($"{Name} speaks in melodic Elvish: 'Nae saian luume!'");
        }
        
        public override void Move()
        {
            Console.WriteLine($"{Name} moves silently through the forest with grace and precision.");
        }
        
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Bow Type: {BowType}");
        }
        
        public override string GetSpecialAbility()
        {
            return "Master Archer - Finds rare ingredients when exploring";
        }
        
        public void ShootArrow()
        {
            Console.WriteLine($"{Name} fires an arrow from their {BowType} with deadly accuracy!");
        }
        
        protected override void CalculateDailyCurrency()
        {
            base.CalculateDailyCurrency();
            if (Health > 90)
            {
                DailyCurrency = Math.Round(DailyCurrency * 1.3m, 2);
            }
        }
    }
}