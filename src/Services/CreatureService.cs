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
        
        public void ProcessEndOfDay()
        {
            foreach (var creature in _state.Player.Creatures)
            {
                creature.EndOfDay();
            }
            
            _state.GameWorld.AdvanceDay();
        }
        
        public List<Creature> GetAllCreatures() => _state.Player.Creatures;
        
        public Creature? GetCreatureByName(string name)
        {
            return _state.Player.Creatures.Find(c => 
                c.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}