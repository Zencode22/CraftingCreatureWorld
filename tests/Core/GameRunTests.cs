using System;
using System.IO;
using CraftingCreatureWorld.Core;
using Xunit;

namespace CraftingCreatureWorld.Tests.Core
{
    public class GameRunTests
    {
        [Fact]
        public void Run_ImmediateExit_DoesNotAdvanceDay()
        {
            // Arrange: prepare a game instance and redirect console input so the
            // main menu immediately gets the "exit" choice (now option 6 after
            // statistics were removed).
            var game = new Game("Tester");
            Console.SetIn(new StringReader("6\n"));

            // Act
            game.Run();

            // Assert: the player should still be on day one and the game state
            // should be marked as over.  This also verifies that we didn't run
            // ProcessDay/AdvanceDay after exiting from the menu.
            Assert.True(game.State.IsGameOver);
            Assert.Equal(1, game.State.CurrentDay);
        }
    }
}
