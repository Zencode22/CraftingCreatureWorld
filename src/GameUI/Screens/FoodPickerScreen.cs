using CraftingCreatureWorld.Core;
using CreatureWorld;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CraftingCreatureWorld.GameUI.Screens
{
    /// <summary>
    /// Pantry picker shown when feeding a creature. Each food item is a button
    /// with a number hot key [1..9]. Feeding advances the day.
    /// </summary>
    public sealed class FoodPickerScreen : IScreen
    {
        private readonly Creature _creature;

        public FoodPickerScreen(Creature creature) => _creature = creature;

        public string HotkeyHelp => "[1-9] Feed item   [Esc] Back";

        public void Update(ScreenContext ctx)
        {
            if (ctx.Input.WasPressed(Keys.Escape))
                ctx.Navigate?.Invoke(new CreatureDetailScreen(_creature));
        }

        public void Draw(ScreenContext ctx)
        {
            var r = ctx.Renderer;
            var vp = r.Viewport;
            Hud.Draw(ctx, ctx.Logic);

            float top = Hud.ContentTop + 8;
            r.Text($"Feed {_creature.Name}", 24, top - 30, Color.White, bold: true);

            var foods = ctx.Logic.CraftedFood;
            const float h = 62, gap = 8;
            float w = Math.Min(720, vp.Width - 60);
            float x = (vp.Width - w) / 2;

            for (int i = 0; i < foods.Count && i < 9; i++)
            {
                var food = foods[i];
                bool valid = GameLogic.FoodIsValidFor(_creature, food.Item.Name);
                var rect = new Rectangle((int)x, (int)(top + i * (h + gap)), (int)w, (int)h);

                string effect = GameLogic.FoodEffectDescription(food.Item.Name);
                string sub = valid ? effect : $"{_creature.Type} won't eat this!";

                if (r.Button(rect, food.Item.Name, ctx.Input, Keys.D1 + i, accent: valid, subLabel: sub))
                {
                    FeedWith(ctx, food);
                    return;
                }
            }

            if (foods.Count == 0)
                r.CenteredText("Your pantry is empty - craft some food first!",
                               vp.Width / 2f, vp.Height / 2f, Theme.TextDim);

            r.CenteredText(HotkeyHelp, vp.Width / 2f, vp.Height - 36, Theme.TextDim, scale: 0.9f);
        }

        private void FeedWith(ScreenContext ctx, CraftingEngine.Result food)
        {
            bool wasValid = GameLogic.FoodIsValidFor(_creature, food.Item.Name);
            var report = ctx.Logic.Feed(_creature, food);

            if (!wasValid)
            {
                // invalid food: logic raised a toast and did not advance the day
                ctx.Navigate?.Invoke(new FoodPickerScreen(_creature));
                return;
            }

            if (report.GameOver || ctx.Logic.IsGameOver)
                ctx.Navigate?.Invoke(new GameOverScreen(ctx.Logic.GameOverReason ?? "Game over"));
            else
                ctx.Navigate?.Invoke(new DayReportScreen(report, new CreatureDetailScreen(_creature)));
        }
    }
}
