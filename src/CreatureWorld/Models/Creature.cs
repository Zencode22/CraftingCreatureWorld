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
        
        public int OldHealth { get; set; }
        public int OldHappiness { get; set; }
        
        public Creature(string name, int age, int health)
        {
            Name = name;
            Age = age;
            Health = health;
            CalculateDailyCurrency();
        }
        
        protected virtual void CalculateDailyCurrency()
        {
            decimal baseRate = Type switch
            {
                CreatureType.Dragon => 3.0m,
                CreatureType.Fairy => 2.5m,
                CreatureType.Goblin => 2.0m,
                _ => 1.0m
            };
            
            decimal happinessModifier = Happiness / 100m;
            decimal healthModifier = Health / 100m;
            
            DailyCurrency = Math.Round(baseRate * happinessModifier * healthModifier, 2);
            
            if (Health <= 0 && Happiness <= 0)
            {
                DailyCurrency = 0m;
            }
            else if (Health <= 0 || Happiness <= 0)
            {
                DailyCurrency = Math.Round(DailyCurrency * 0.5m, 2);
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
        }
        
        public virtual string GetSpecialAbility()
        {
            return "Basic creature ability";
        }
        
        public void EndOfDay()
        {
            if (Health < 30)
            {
                Happiness = Math.Max(0, Happiness - 3);
            }
            
            CalculateDailyCurrency();
        }
        
        public bool IsAliveAndHappy()
        {
            return Health > 0 || Happiness > 0;
        }
        
        public override string ToString() => $"{Name} the {Type}";
    }
}