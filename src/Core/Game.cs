using CraftingCreatureWorld.Entities;
using CraftingCreatureWorld.Services;
using CraftingCreatureWorld.UI.Menus;

namespace CraftingCreatureWorld.Core
{
    public class Game
    {
        public GameState State { get; private set; }
        private readonly GameService _gameService;
        private readonly MenuManager _menuManager;
        
        public Game(string playerName)
        {
            State = new GameState(playerName);
            _gameService = new GameService(State);
            _menuManager = new MenuManager(State);
        }
        
        public void Run()
        {
            InitializeGame();
            
            while (!State.IsGameOver)
            {
                _menuManager.DisplayMainMenu();
                
                // if the player chose to exit the game inside the menu we
                // want to break out immediately and never run another day.
                if (State.IsGameOver)
                    break;

                _gameService.ProcessDay();
                State.AdvanceDay();
            }
        }
        
        private void InitializeGame()
        {
            // Add starter creatures
            _gameService.AddStarterCreatures();
            
            // Initialize trader with random recipe
            _gameService.InitializeTrader();
        }
    }
}