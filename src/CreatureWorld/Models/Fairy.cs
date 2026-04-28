using System;

namespace CreatureWorld
{
    public class Fairy : Creature
    {
        public BowType BowType { get; set; }
        
        public Fairy(string name, int age, int health, BowType bowType) 
            : base(name, age, health)
        {
            Type = CreatureType.Fairy;
            BowType = bowType;
            CalculateDailyCurrency();
        }
        
        public override void MakeSound()
        {
            Console.WriteLine($"{Name} giggles with a tinkling sound like tiny bells: 'Tee-hee-hee!'");
        }
        
        public override void Move()
        {
            Console.WriteLine($"{Name} flutters through the air leaving a trail of sparkling fairy dust.");
        }
        
        public override void DisplayInfo()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Type: {BowType} Fairy");
            Console.WriteLine($"Age: {Age} years");
            Console.WriteLine($"Health: {Health}/100");
            Console.WriteLine($"Happiness: {Happiness}/100");
            Console.WriteLine($"Daily Income: {DailyCurrency:C}");
        }
        
        public void ShootArrow()
        {
            Console.WriteLine($"{Name} fires a tiny arrow from their {BowType} with surprising accuracy!");
        }
    }
}