using System;

namespace CreatureWorld
{
    public class Dragon : Creature
    {
        public Element ElementType { get; set; }
        
        public Dragon(string name, int age, int health, Element element) 
            : base(name, age, health)
        {
            Type = CreatureType.Dragon;
            ElementType = element;
            CalculateDailyCurrency();
        }
        
        public override void MakeSound()
        {
            Console.WriteLine($"{Name} lets out a thunderous {ElementType} dragon roar!");
        }
        
        public override void Move()
        {
            Console.WriteLine($"{Name} soars through the sky with majestic {ElementType} wings.");
        }
        
        public override void DisplayInfo()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Type: {ElementType} Dragon");
            Console.WriteLine($"Age: {Age} years");
            Console.WriteLine($"Health: {Health}/100");
            Console.WriteLine($"Happiness: {Happiness}/100");
            Console.WriteLine($"Daily Income: {DailyCurrency:C}");
        }
        
        public void BreatheElement()
        {
            Console.WriteLine($"{Name} breathes a powerful stream of {ElementType}!");
        }
    }
}