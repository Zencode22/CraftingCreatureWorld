using System.Collections.Generic;
using CraftingCreatureWorld.Core;
using CraftingEngine;

using Microsoft.Xna.Framework;

namespace CraftingCreatureWorld.GameUI.Screens
{
    /// <summary>
    /// Recipe list. Each recipe is a button; crafting consumes ingredients and
    /// advances the day. A pantry panel on the right shows current stock.
    /// </summary>
    public sealed class CraftingScreen : IScreen
    {
        private List<Recipe> _recipes = new();
        private int _version = -1;

        public void Update(ScreenContext ctx)
        {
        }

        public void Draw(ScreenContext ctx)
        {
            var r = ctx.Renderer!;
            var vp = r.Viewport;
            Hud.Draw(ctx, ctx.Logic);

            if (_version != ctx.Logic.CurrentDay)
            {
                _recipes = new List<Recipe>(ctx.Logic.Recipes);
                _version = ctx.Logic.CurrentDay;
            }

            float top = Hud.ContentTop + 8;
            r.Text("CRAFTING STATION", 24, top - 30, Color.White, bold: true);

            float leftW = Math.Min(560, vp.Width - 320);
            float x = 24;
            const float h = 62, gap = 8;

            for (int i = 0; i < _recipes.Count && i < 9; i++)
            {
                var recipe = _recipes[i];
                bool canCraft = ctx.Logic.CanCraft(recipe);
                var rect = new Rectangle((int)x, (int)(top + i * (h + gap)), (int)leftW, (int)h);

                string needs = string.Join(", ",
                    System.Linq.Enumerable.Select(recipe.Ingredients, ing => $"{ing.Value}"));

                if (r.Button(rect, $"Craft: {recipe.Name}", ctx.Input, enabled: canCraft,
                        subLabel: $"Makes {recipe.Result}  |  Needs: {needs}"))
                {
                    CraftOne(ctx, recipe);
                    return;
                }
            }

            // ---- pantry panel ----
            float px = x + leftW + 20;
            float pw = vp.Width - px - 16;
            var panel = new Rectangle((int)px, (int)top - 30, (int)pw, (int)(vp.Height - top - 20));
            r.FillRoundRect(panel, 10, Theme.Panel);
            r.StrokeRect(panel, 1, Color.White * 0.08f);
            r.Text("PANTRY", panel.X + 14, panel.Y + 10, Color.White, bold: true);

            float py = panel.Y + 40;
            foreach (var (item, amount) in ctx.Logic.InventoryItems())
            {
                if (py > panel.Bottom - 24) break;
                r.Text($"{amount:0.##} x {item.Name}", panel.X + 14, py, Theme.TextDim, scale: 0.85f);
                py += 22;
            }

            r.Text("Food ready to feed:", panel.X + 14, py + 10, Theme.Accent, scale: 0.85f, bold: true);
            py += 32;
            if (ctx.Logic.CraftedFood.Count == 0)
                r.Text("(empty)", panel.X + 14, py, Theme.TextDim, scale: 0.85f);
            foreach (var food in ctx.Logic.CraftedFood)
            {
                if (py > panel.Bottom - 24) break;
                r.Text($"{food}", panel.X + 14, py, Theme.Green, scale: 0.85f);
                py += 22;
            }

            var backBtn = new Rectangle(24, vp.Height - 58, 200, 42);
            if (r.Button(backBtn, "Back to Menu", ctx.Input))
                ctx.Navigate?.Invoke(new MainMenuScreen());
        }

        private void CraftOne(ScreenContext ctx, Recipe recipe)
        {
            if (!ctx.Logic.Craft(recipe))
                return; // toast raised by logic

            var report = new DayReport
            {
                DayEnded = ctx.Logic.CurrentDay,
                ActionDescription = $"crafted {recipe.Name}",
                GameOver = ctx.Logic.IsGameOver,
                GameOverReason = ctx.Logic.GameOverReason
            };

            if (report.GameOver)
                ctx.Navigate?.Invoke(new GameOverScreen(ctx.Logic.GameOverReason ?? "Game over"));
            else
                ctx.Navigate?.Invoke(new DayReportScreen(report, new CraftingScreen()));
        }
    }
}
