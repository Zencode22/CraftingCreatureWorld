using CraftingCreatureWorld.Entities;
using CreatureWorld;

namespace CraftingCreatureWorld.Core
{
    public class GameState
    {
        public Player Player { get; set; }
        // Trader may not be initialized immediately, allow null
        public Trader? Trader { get; set; }
        public World GameWorld { get; set; }
        public int CurrentDay { get; private set; } = 1;
        public bool IsGameOver { get; set; }
        public string? GameOverReason { get; set; }
        
        public GameState(string playerName)
        {
            Player = new Player(playerName);
            GameWorld = new World("Fantasy Lands");
            IsGameOver = false;
        }
        
        public void AdvanceDay()
        {
            CurrentDay++;
        }
    }
}