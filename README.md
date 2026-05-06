# 🧬 Creature Craft

A text-based adventure game where you collect creatures, earn currency, buy ingredients, and craft food to keep your magical companions healthy and happy.

## Game Overview

In Creature Craft, you start with three randomly generated creatures (Dragon, Fairy, Goblin) that generate currency daily. Use your earnings to buy crafting ingredients from a rotating merchant, craft food items, and care for your creatures. But beware - neglect your creatures and they'll stop producing income. If all your creatures lose their health and happiness, the game ends with a lesson about sustainability.

## Features

- **Randomly Generated Creatures**: Each game features unique creatures with random names and types
- **Creature Care**: Manage health and happiness through feeding and playing
- **Currency System**: Creatures earn $15.00 daily, with penalties for low stats
- **Rotating Trader**: Weekly ingredient rotation keeps crafting challenging
- **Crafting System**: Combine ingredients to create creature-specific foods and potions
- **Day Progression**: Each action advances the day, changing creature stats
- **Random Events**: Discover ingredients, find currency, or enjoy happiness boosts

## How to Play

1. **Manage Creatures**: Check stats, play with them, or feed them crafted food
2. **Earn Currency**: Creatures generate $15.00 each per day
3. **Visit the Trader**: Buy ingredients (unlimited supply, weekly rotation)
4. **Craft Items**: Combine 3 ingredients to make creature-specific food
5. **End Day**: Days advance after each action (craft, buy, feed, play, or end early)

## Crafting Recipes

### 🐉 Hot Chocolate (Dragon)
- 4 cups Milk
- 0.5 cup Chocolate Chips
- 2 sticks Cinnamon
- **Effect**: +25 Health, +15 Happiness

### 🧚 Bread (Fairy)
- 3 cups Flour
- 0.02 cup Yeast
- 0.5 cup Honey
- **Effect**: +30 Health, +10 Happiness

### 💚 Jelly Beans (Goblin)
- 0.5 cup Sugar
- 0.3 packet Gelatin
- 0.5 cup Fruit Juice
- **Effect**: +20 Health, +20 Happiness

### ✨ Healing Potion (Any Creature)
- 2 pieces Herb
- 0.5 vial Crystal Water
- 3 pinches Moon Dust
- **Effect**: Restores Health and Happiness to maximum

## Creature Types

### 🐉 Dragon
- Names themed after Chinese Emperors
- Elements: Fire, Ice, Lightning, Earth
- Favorite food: Hot Chocolate

### 🧚 Fairy
- Names themed after Famous Medieval European Women
- Bow types: Shortbow, Longbow, Crossbow
- Favorite food: Bread

### 👺 Goblin
- Names themed after Famous Scientists
- Types: Cave, Forest, Mountain
- Favorite food: Jelly Beans

## Trader System

- **Random Name**: Each game features a different Fortune 500 merchant
- **Weekly Rotation**: Ingredients rotate every 7 days
- **Unlimited Supply**: Buy as much as you can afford
- **Potion Ingredients**: Always available at premium prices

## Income System

- Base income: $15.00 per creature per day
- 50% penalty if Health OR Happiness reaches 0%
- 0% income if BOTH Health AND Happiness reach 0%
- Game over if all creatures reach 0% in both stats

## Getting Started

### Prerequisites
- .NET 8.0 SDK or later

### Running the Game
1. Download and extract the zip file
2. Open a terminal in the extracted folder
3. Run the game:
```bash
dotnet run --project src/CraftingCreatureWorld.csproj