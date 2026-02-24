using System;
using System.Collections.Generic;
using CraftingEngine;
using CreatureWorld;

namespace CraftingCreatureWorld.Entities
{
    public sealed class Player : Person
    {
        public decimal Currency { get; set; }
        public List<Creature> Creatures { get; private set; }
        public List<Result> CraftedFood { get; private set; }
        public int TotalItemsCrafted => CraftedFood.Count;
        
        public Player(string name) : base(name)
        {
            Currency = 50m; // Starting currency
            Creatures = new List<Creature>();
            CraftedFood = new List<Result>();
        }
        
        public void AddCreature(Creature creature)
        {
            Creatures.Add(creature);
        }
        
        public decimal CollectCreatureCurrency()
        {
            decimal total = 0m;
            
            foreach (var creature in Creatures)
            {
                total += creature.DailyCurrency;
            }
            
            Currency += total;
            return total;
        }
        
        public void AddCraftedItem(Item item, decimal amount)
        {
            CraftedFood.Add(new Result(item, amount));
        }
        
        public bool RemoveCraftedItem(Result food)
        {
            return CraftedFood.Remove(food);
        }
    }
}