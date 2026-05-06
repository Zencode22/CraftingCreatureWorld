using System;
using System.Collections.Generic;
using System.Linq;

namespace CreatureWorld
{
    public class World
    {
        public string WorldName { get; set; }
        public int CurrentDay { get; private set; } = 1;
        private List<Creature> creatures;
        
        public World(string worldName)
        {
            WorldName = worldName;
            creatures = new List<Creature>();
        }
        
        public void AddCreature(Creature creature)
        {
            creatures.Add(creature);
        }
        
        public void RemoveCreature(Creature creature)
        {
            creatures.Remove(creature);
        }
        
        public void DisplayAllCreatures()
        {
            Console.WriteLine($"\n=== Creatures in {WorldName} ===\n");
            
            if (creatures.Count == 0)
            {
                Console.WriteLine("No creatures in the world yet.");
                return;
            }
            
            foreach (var creature in creatures)
            {
                creature.DisplayInfo();
                Console.WriteLine(new string('-', 30));
            }
            
            Console.WriteLine($"Total Creatures: {creatures.Count}");
        }
        
        public void PerformAllActions()
        {
            foreach (var creature in creatures)
            {
                creature.MakeSound();
                creature.Move();
                Console.WriteLine();
            }
        }
        
        public void DisplayCreatureCountByType()
        {
            Console.WriteLine($"\n=== Creature Population in {WorldName} ===\n");
            
            foreach (CreatureType type in Enum.GetValues(typeof(CreatureType)))
            {
                int count = creatures.FindAll(c => c.Type == type).Count;
                string typeName = type switch
                {
                    CreatureType.Dragon => "Dragons",
                    CreatureType.Fairy => "Fairies",
                    CreatureType.Goblin => "Goblins",
                    _ => type.ToString() + "s"
                };
                Console.WriteLine($"{typeName}: {count}");
            }
        }
        
        public void AdvanceDay()
        {
            CurrentDay++;
            
            foreach (var creature in creatures)
            {
                creature.EndOfDay();
            }
        }
        
        public List<Creature> GetAllCreatures()
        {
            return creatures.ToList();
        }
    }
}