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
            base.DisplayInfo();
            Console.WriteLine($"Bow Type: {BowType}");
        }
        
        public override string GetSpecialAbility()
        {
            return "Forest Magic - A delicate and magical creature";
        }
        
        public void ShootArrow()
        {
            Console.WriteLine($"{Name} fires a tiny arrow from their {BowType} with surprising accuracy!");
        }
    }
}