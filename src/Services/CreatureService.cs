using System.Collections.Generic;
using CraftingCreatureWorld.Core;
using CreatureWorld;
using CraftingEngine;

namespace CraftingCreatureWorld.Services
{
    public class CreatureService
    {
        private readonly GameState _state;
        
        public CreatureService(GameState state)
        {
            _state = state;
        }
        
        public void AddCreature(Creature creature)
        {
            _state.Player.AddCreature(creature);
            _state.GameWorld.AddCreature(creature);
        }
        
        public bool FeedCreature(Creature creature, Result food)
        {
            if (creature == null || food == null) return false;
            
            // Check creature type and allow only their specific food
            if (creature.Type == CreatureType.Dragon && 
                food.Item.Name.Contains("Hot Chocolate", System.StringComparison.OrdinalIgnoreCase))
            {
                creature.Happiness = System.Math.Min(100, creature.Happiness + 20);
                creature.HungerLevel = System.Math.Max(0, creature.HungerLevel - 10);
                creature.Health = System.Math.Min(100, creature.Health + 5);
                return true;
            }
            else if (creature.Type == CreatureType.Elf && 
                     food.Item.Name.Contains("Bread", System.StringComparison.OrdinalIgnoreCase))
            {
                creature.HungerLevel = System.Math.Max(0, creature.HungerLevel - 30);
                creature.Happiness = System.Math.Min(100, creature.Happiness + 5);
                return true;
            }
            else if (creature.Type == CreatureType.Goblin && 
                     food.Item.Name.Contains("Jelly Beans", System.StringComparison.OrdinalIgnoreCase))
            {
                creature.HungerLevel = System.Math.Max(0, creature.HungerLevel - 25);
                creature.Happiness = System.Math.Min(100, creature.Happiness + 15);
                creature.Health = System.Math.Min(100, creature.Health + 3);
                return true;
            }
            
            return false;
        }
        
        public void ProcessEndOfDay()
        {
            foreach (var creature in _state.Player.Creatures)
            {
                creature.EndOfDay();
            }
            
            _state.GameWorld.AdvanceDay();
        }
        
        public List<Creature> GetAllCreatures() => _state.Player.Creatures;
        
        // Allow nullable return since Find may not locate a match.
        public Creature? GetCreatureByName(string name)
        {
            return _state.Player.Creatures.Find(c => 
                c.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}