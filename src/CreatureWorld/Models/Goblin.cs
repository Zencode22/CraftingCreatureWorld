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
            base.DisplayInfo();
            Console.WriteLine($"Goblin Type: {GoblinType}");
        }
        
        public override string GetSpecialAbility()
        {
            return "Stealthy Sneak - A cunning and resourceful creature";
        }
        
        public void Steal()
        {
            Console.WriteLine($"{Name} attempts to steal something shiny!");
        }
    }
}