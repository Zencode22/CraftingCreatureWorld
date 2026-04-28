using System;

namespace CreatureWorld
{
    public class Goblin : Creature
    {
        public GoblinType GoblinType { get; set; }
        
        public Goblin(string name, int age, int health, GoblinType goblinType) 
            : base(name, age, health)
        {
            Type = CreatureType.Goblin;
            GoblinType = goblinType;
            CalculateDailyCurrency();
        }
        
        public override void MakeSound()
        {
            Console.WriteLine($"{Name} cackles menacingly: 'Hehehe, precious!'");
        }
        
        public override void Move()
        {
            Console.WriteLine($"{Name} scurries quickly through the {GoblinType} tunnels.");
        }
        
        public override void DisplayInfo()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Type: {GoblinType} Goblin");
            Console.WriteLine($"Age: {Age} years");
            Console.WriteLine($"Health: {Health}/100");
            Console.WriteLine($"Happiness: {Happiness}/100");
            Console.WriteLine($"Daily Income: {DailyCurrency:C}");
        }
        
        public void Steal()
        {
            Console.WriteLine($"{Name} attempts to steal something shiny!");
        }
    }
}