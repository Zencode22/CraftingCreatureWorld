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
            base.DisplayInfo();
            Console.WriteLine($"Element: {ElementType}");
        }
        
        public override string GetSpecialAbility()
        {
            return $"Breathes {ElementType} - Generates more currency when happy";
        }
        
        public void BreatheElement()
        {
            Console.WriteLine($"{Name} breathes a powerful stream of {ElementType}!");
        }
        
        protected override void CalculateDailyCurrency()
        {
            base.CalculateDailyCurrency();
            if (Happiness > 80)
            {
                DailyCurrency = Math.Round(DailyCurrency * 1.5m, 2);
            }
        }
    }
}