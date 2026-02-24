# Crafting Creature World

A text-based adventure game where you collect creatures, earn currency, buy ingredients, and craft food to improve your creatures.

## Game Overview

In Crafting Creature World, you start with three creatures (Dragon, Elf, Goblin) that generate currency daily. You can use this currency to buy crafting ingredients from a trader, craft food items, and feed your creatures to keep them happy and healthy.

## Features

- **Creature Management**: Care for your dragons, elves, and goblins
- **Currency System**: Creatures earn money based on their happiness and health
- **Trading System**: Buy ingredients from the traveling merchant
- **Crafting System**: Combine ingredients to create food and potions
- **Feeding System**: Feed crafted items to improve creature stats
- **Day/Night Cycle**: Creatures' needs change over time
- **Random Events**: Discover ingredients or find extra currency

## How to Play

1. **Manage Creatures**: Check your creatures' health, happiness, and hunger
2. **Earn Currency**: Creatures generate money each day
3. **Visit the Trader**: Buy ingredients with your earned currency
4. **Craft Items**: Combine ingredients to make food
5. **Feed Creatures**: Use crafted food to improve creature stats

## Items and Recipes

### Ingredients
- Milk (0.50 per cup)
- Chocolate Chips (1.20 per cup)
- Flour (0.30 per cup)
- Water (Free)
- Yeast (0.80 per cup)
- Herb (0.40 per piece)
- Sugar (0.60 per cup)

### Crafting Recipes
- **Hot Chocolate**: 4 cups Milk + 0.5 cup Chocolate Chips → 12 oz Hot Chocolate
  - Effect: +20 Happiness to all creatures
  
- **Bread**: 3 cups Flour + 1.5 cups Water + 0.02 cup Yeast → 1 loaf Bread
  - Effect: -30 Hunger to all creatures
  
- **Healing Potion**: 2 pieces Herb + 0.5 cup Water → 1 bottle Healing Potion
  - Effect: +25 Health to a single creature

## Creatures

### Dragon
- Base daily value: 3.0
- Special: Extra currency when happy (>80 happiness)
- Element types: Fire, Ice, Lightning, Earth

### Elf
- Base daily value: 2.5
- Special: Extra currency when healthy (>90 health)
- Bow types: Shortbow, Longbow, Crossbow

### Goblin
- Base daily value: 2.0
- Special: Extra currency when not hungry (<30 hunger)
- Types: Cave, Forest, Mountain

## Getting Started

1. Clone the repository
2. Open `CraftingCreatureWorld.sln` in Visual Studio
3. Build and run the project
4. Enter your name when prompted
5. Follow the on-screen menus to play!

## Building from Source

```bash
dotnet build
dotnet run --project src/CraftingCreatureWorld.csproj