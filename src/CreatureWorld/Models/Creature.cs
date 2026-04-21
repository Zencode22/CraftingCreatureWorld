using System;

namespace CreatureWorld
{
    public abstract class Creature
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int Health { get; set; }
        public int Happiness { get; set; } = 70;
        public CreatureType Type { get; protected set; }
        public decimal DailyCurrency { get; protected set; }
        
        // Properties to store old values for display purposes
        public int OldHealth { get; set; }
        public int OldHappiness { get; set; }
        
        public Creature(string name, int age, int health)
        {
            Name = name;
            Age = age;
            Health = health;
            CalculateDailyCurrency();
        }
        
        public virtual void CalculateDailyCurrency()
        {
            // Fixed base rates per creature type
            decimal baseRate = Type switch
            {
                CreatureType.Dragon => 15.0m,
                CreatureType.Fairy => 15.0m,  // All creatures should have the same base rate
                CreatureType.Goblin => 15.0m, // All creatures should have the same base rate
                _ => 15.0m
            };
            
            // Apply penalties for zero stats
            if (Health <= 0 && Happiness <= 0)
            {
                DailyCurrency = 0m; // No income when both are zero
            }
            else if (Health <= 0 || Happiness <= 0)
            {
                DailyCurrency = Math.Round(baseRate * 0.5m, 2); // 50% penalty if either is zero
            }
            else
            {
                DailyCurrency = baseRate; // Full income when both stats are above 0
            }
        }
        
        public virtual void MakeSound()
        {
            Console.WriteLine($"{Name} makes a generic creature sound.");
        }
        
        public virtual void Move()
        {
            Console.WriteLine($"{Name} moves slowly.");
        }
        
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Type: {Type}");
            Console.WriteLine($"Age: {Age} years");
            Console.WriteLine($"Health: {Health}/100");
            Console.WriteLine($"Happiness: {Happiness}/100");
            Console.WriteLine($"Daily Income: {DailyCurrency:C}");
        }
        
        public virtual string GetSpecialAbility()
        {
            return "A loyal companion";
        }
        
        public void EndOfDay()
        {
            // No longer reduces happiness based on health
            // Just recalculate currency for the next day
            CalculateDailyCurrency();
        }
        
        public bool IsAliveAndHappy()
        {
            return Health > 0 || Happiness > 0;
        }
        
        public override string ToString() => $"{Name} the {Type}";
    }
}