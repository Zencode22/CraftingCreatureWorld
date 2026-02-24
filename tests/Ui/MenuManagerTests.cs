using System;
using System.IO;
using CraftingCreatureWorld.Core;
using CraftingCreatureWorld.UI.Menus;
using Xunit;

namespace CraftingCreatureWorld.Tests.Ui
{
    public class MenuManagerTests
    {
        [Fact]
        public void DisplayMainMenu_ExitOption_SetsGameOverFlag()
        {
            // Arrange: prepare a game state and redirect console input
            var state = new GameState("Tester");
            var menu = new MenuManager(state);

            // simulate entering '7' then newline to exit immediately
            var input = new StringReader("7\n");
            Console.SetIn(input);

            // Act
            menu.DisplayMainMenu();

            // Assert
            Assert.True(state.IsGameOver, "Selecting exit should mark the game as over.");
        }
    }
}
