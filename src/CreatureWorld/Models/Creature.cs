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
            decimal baseRate = 15.0m;
            
            // Apply penalties for zero stats
            if (Health <= 0 && Happiness <= 0)
            {
                DailyCurrency = 0m;
            }
            else if (Health <= 0 || Happiness <= 0)
            {
                DailyCurrency = Math.Round(baseRate * 0.5m, 2);
            }
            else
            {
                DailyCurrency = baseRate;
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
        
        public void EndOfDay()
        {
            CalculateDailyCurrency();
        }
        
        public bool IsAliveAndHappy()
        {
            return Health > 0 || Happiness > 0;
        }
        
        public override string ToString() => $"{Name} the {Type}";
    }
}