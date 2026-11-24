# Creature Craft

A graphical creature-care and crafting game for desktop, built with **C# / .NET 8** and **MonoGame**.

Collect creatures, earn currency, buy ingredients from a rotating trader, craft creature-specific food, and keep your companions healthy and happy. Neglect them and their income suffers - let every creature hit zero health *and* happiness and the game is over.

---

## Gameplay Loop

1. **Start a game** - you name your adventurer and receive three starter creatures: a Dragon, a Fairy, and a Goblin (each with randomized names, ages, and starting health).
2. **Earn currency** - every creature generates **$15.00 per day**.
3. **Care for creatures** - play with them for a happiness boost, or feed them food you have crafted.
4. **Visit the trader** - buy crafting ingredients (unlimited stock).
5. **Craft food** - combine ingredients into creature-specific meals and healing potions.
6. **End the day** - every action (play, feed, buy, craft) advances the day: creatures earn income, their stats decay, and a random event may fire.

Each day, every creature naturally loses **2-5 Health** and **3-8 Happiness**, so you have to keep producing food to stay ahead of the decay.

## Creatures

| Creature | Name theme | Variants | Starting stats |
|----------|------------|----------|----------------|
| Dragon | Chinese Emperors | Fire, Ice, Lightning, Earth | Age 30-300, Health 80-100 |
| Fairy | Medieval European women | Shortbow, Longbow, Crossbow | Age 50-500, Health 70-100 |
| Goblin | Famous scientists | Cave, Forest, Mountain | Age 10-100, Health 60-100 |

All creatures start at **70 Happiness**.

## Income Rules

- Full **$15.00/day** per creature while both stats are above zero.
- **50% penalty** if Health *or* Happiness reaches 0%.
- **No income** if both reach 0%.
- **Game over** when *all* creatures sit at 0% in both stats.

## Recipes

| Food | For | Ingredients | Effect | Yield |
|------|-----|-------------|--------|-------|
| Hot Chocolate | Dragon | 4 cups Milk, 0.5 cup Chocolate Chips, 2 sticks Cinnamon | +25 Health, +15 Happiness | 12 |
| Bread | Fairy | 3 cups Flour, 0.02 cup Yeast, 0.5 cup Honey | +30 Health, +10 Happiness | 1 |
| Jelly Beans | Goblin | 0.5 cup Sugar, 0.3 packet Gelatin, 0.5 cup Fruit Juice | +20 Health, +20 Happiness | 8 |
| Healing Potion | Any | 2 pieces Herb, 0.5 vial Crystal Water, 3 pinches Moon Dust | Fully restores Health and Happiness | 1 |

Feeding a creature food that is not meant for its type is rejected.

## Trader

Each game spawns a randomly named trader. Three **potion ingredients** (Herb, Crystal Water, Moon Dust) are always in stock at premium prices; the ingredients for one featured creature-food recipe are also offered, and the featured recipe **rotates every 7 days**. Ingredient prices are tiered (Milk/Flour/Sugar at $0.50, Chocolate Chips/Yeast/Gelatin at $1.20, Cinnamon/Honey/Fruit Juice at $0.80, potion ingredients at double price).

## Random Events

After each day there is a **75% chance** of one of three events:

- **Found ingredients** - a random ingredient is added to your inventory.
- **Magical breeze** - all creatures gain 10-15 Happiness.
- **Found currency** - pick up $5-19 off the ground.

---

## Building and Running

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- GPU with OpenGL support (DesktopGL)

### Run from source

```bash
dotnet tool restore
dotnet run --project src/GameUI/CreatureCraft.csproj
```

The MonoGame content pipeline (MGCB) runs automatically during the build and compiles the UI fonts in `src/GameUI/Content`.

### Publish a self-contained build

```bash
dotnet publish src/GameUI/CreatureCraft.csproj -c Release -r win-x64 --self-contained true
```

---

## Project Structure

```
CraftingCreatureWorld.sln
src/
  Core/               Game-state facade: GameLogic, DayReport, UiEvents, constants.
                      Console-free - all operations return data, never print.
  Entities/           Player, Trader, Person
  CreatureWorld/      Creature hierarchy (Dragon, Fairy, Goblin) and the World
  CraftingEngine/     Items, ingredients, quantities, recipes, inventory, recipe catalog
  Services/           Day, currency, trading, crafting, and creature services;
                      random name generators
  GameUI/             MonoGame application: game loop, screens, immediate-mode
                      renderer (buttons, progress bars, scrolling lists, toasts),
                      input handling, and the content pipeline project
```

Notes on the build layout:

- `src/CraftingCreatureWorld.csproj` compiles the game logic as a **class library** (`Core`, `Entities`, `Services`, `CraftingEngine`, `CreatureWorld`) - convenient for tests or other front-ends.
- `src/GameUI/CreatureCraft.csproj` is the executable. It links the same logic sources via `Compile Include` and adds MonoGame on top, so there is a single source of truth for the game rules.
- The logic layer communicates with any UI exclusively through return values (`DayReport`, booleans) and the `UiEvents` toast/log event bus - no console I/O anywhere below the UI.

## UI Screens

Title (player-name entry) - Main Menu - Creature List - Creature Detail - Trader - Crafting - Food Picker - Day Report - Game Over.

Navigation is mouse-driven; keyboard input is used for the player name. Transient notifications (insufficient funds, invalid food, crafted items, new-day announcements) appear as on-screen toasts.

---

## AI Assistance Disclosure

This project was developed with the help of generative AI tools, used as coding assistants throughout development:

- **[Lumo](https://proton.me/lumo)** (Proton) - AI assistant used for project documentation, code review, and architecture guidance, including the rewrite of this README and repo-cleanup recommendations.
- **[Qwen Coder](https://coder.qwen.ai)** (Alibaba) - primary AI coding tool used for day-to-day code generation, refactoring, and debugging of the game logic and MonoGame UI.

All AI-generated code was reviewed, tested, and iterated on by the repository owner ([@Zencode22](https://github.com/Zencode22)), who made the final decisions on design, features, and what was merged. AI was a tool in the process - the project concept, gameplay design, and direction are human-authored.

---

## Roadmap Ideas

- Save/load games (persistence layer)
- More creature species and recipes
- Multiple traders / supply-limited stock
- Sound and music

## Contributing

Issues and pull requests are welcome at [github.com/Zencode22/CraftingCreatureWorld](https://github.com/Zencode22/CraftingCreatureWorld).
