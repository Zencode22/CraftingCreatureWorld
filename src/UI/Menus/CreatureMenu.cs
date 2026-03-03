using System;
using System.Collections.Generic;
using System.Linq;
using CraftingCreatureWorld.Core;
using CraftingCreatureWorld.UI.Display;
using CreatureWorld;

namespace CraftingCreatureWorld.UI.Menus
{
    public class CreatureMenu
    {
        private readonly GameState _state;
        
        public CreatureMenu(GameState state)
        {
            _state = state;
        }
        
        public void Show()
        {
            bool inMenu = true;
            
            while (inMenu && _state.Player.Creatures.Any())
            {
                ConsoleHelper.Clear();
                ConsoleDisplay.ShowHeader("MANAGE YOUR CREATURES");
                
                DisplayCreatureList();
                
                Console.WriteLine($"\n{_state.Player.Creatures.Count + 1}. Return to Main Menu");
                Console.Write("\nSelect a creature to view details: ");
                
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == _state.Player.Creatures.Count + 1)
                    {
                        inMenu = false;
                    }
                    else if (choice > 0 && choice <= _state.Player.Creatures.Count)
                    {
                        var creature = _state.Player.Creatures[choice - 1];
                        ViewCreatureDetails(creature);
                    }
                    else
                    {
                        ConsoleDisplay.ShowError("Invalid choice.");
                        InputHandler.WaitForKey();
                    }
                }
            }
            
            if (!_state.Player.Creatures.Any())
            {
                ConsoleDisplay.ShowWarning("You don't have any creatures yet!");
                InputHandler.WaitForKey();
            }
        }
        
        private void DisplayCreatureList()
        {
            for (int i = 0; i < _state.Player.Creatures.Count; i++)
            {
                var creature = _state.Player.Creatures[i];
                string happinessBar = GetBar(creature.Happiness, 10);
                string healthBar = GetBar(creature.Health, 10);
                
                Console.WriteLine($"\n{i + 1}. {creature.Name} the {creature.Type}");
                Console.WriteLine($"   Health: {healthBar} {creature.Health}%");
                Console.WriteLine($"   Happiness: {happinessBar} {creature.Happiness}%");
                Console.WriteLine($"   Hunger: {creature.HungerLevel}/100");
                Console.WriteLine($"   Daily Value: {creature.DailyCurrency:C}");
            }
        }
        
        private void ViewCreatureDetails(Creature creature)
        {
            ConsoleHelper.Clear();
            ConsoleDisplay.ShowHeader(creature.Name);
            
            creature.DisplayInfo();
            Console.WriteLine($"\nSpecial Ability: {creature.GetSpecialAbility()}");
            Console.WriteLine($"Daily Currency: {creature.DailyCurrency:C}");
            
            InputHandler.WaitForKey();
        }
        
        private string GetBar(int value, int length)
        {
            int filled = (value * length) / 100;
            return new string('#', filled) + new string('-', length - filled);
        }
    }
}