using System.Linq;
using CraftingCreatureWorld.Core;
using CraftingEngine;
using CreatureWorld;

using Microsoft.Xna.Framework;

namespace CraftingCreatureWorld.GameUI.Screens
{
    /// <summary>
    /// Detail page for one creature. Play with it, feed it (opens the pantry
    /// picker) or go back to the roster.
    /// </summary>
    public sealed class CreatureDetailScreen : IScreen
    {
        private readonly Creature _creature;

        public CreatureDetailScreen(Creature creature) => _creature = creature;

        public void Update(ScreenContext ctx)
        {
        }

        private void DoPlay(ScreenContext ctx)
        {
            var report = ctx.Logic.PlayWith(_creature);
            ShowReport(ctx, report);
        }

        internal static void ShowReport(ScreenContext ctx, DayReport report)
        {
            if (report.GameOver)
                ctx.Navigate?.Invoke(new GameOverScreen(report.GameOverReason ?? "Game over"));
            else
            {
                var back = ctx.LastViewedCreature is { } c && ctx.Logic.Creatures.Contains(c)
                    ? (IScreen)new CreatureDetailScreen(c)
                    : new CreatureListScreen();
                ctx.Navigate?.Invoke(new DayReportScreen(report, back));
            }
        }

        public void Draw(ScreenContext ctx)
        {
            ctx.LastViewedCreature = _creature;
            var r = ctx.Renderer!;
            var vp = r.Viewport;
            Hud.Draw(ctx, ctx.Logic);

            float top = Hud.ContentTop + 8;
            float w = Math.Min(680, vp.Width - 60);
            float x = (vp.Width - w) / 2;

            r.Text(GameLogic.FlavorFor(_creature), x, top, Color.White, bold: true, scale: 1.2f);
            r.Text($"Type: {_creature.Type}    Age: {_creature.Age}    Earns {_creature.DailyCurrency:C}/day",
                   x, top + 36, Theme.TextDim);

            // stat bars
            var hb = new Rectangle((int)x, (int)(top + 70), (int)w, 22);
            r.ProgressBar(hb, _creature.Health / 100f, Theme.HealthBar, Theme.PanelDisabled);
            r.Text($"Health: {_creature.Health}%", hb.X + 8, hb.Y + 3, Color.White, bold: true, scale: 0.9f);

            var hpb = new Rectangle((int)x, (int)(top + 100), (int)w, 22);
            r.ProgressBar(hpb, _creature.Happiness / 100f, Theme.HappinessBar, Theme.PanelDisabled);
            r.Text($"Happiness: {_creature.Happiness}%", hpb.X + 8, hpb.Y + 3, Color.White, bold: true, scale: 0.9f);

            // favourite food hint
            string fav = _creature.Type switch
            {
                CreatureType.Dragon => "Hot Chocolate",
                CreatureType.Fairy => "Bread",
                CreatureType.Goblin => "Jelly Beans",
                _ => "Healing Potion"
            };
            r.Text($"Favourite food: {fav}   (any creature accepts Healing Potions)",
                   x, top + 136, Theme.Accent, scale: 0.9f);

            int foodCount = ctx.Logic.CraftedFood.Count;

            float btnY = top + 176;
            const float bh = 58, bw2 = (680 - 16) / 2;
            var playBtn = new Rectangle((int)x, (int)btnY, (int)Math.Min(bw2, w / 2 - 8), (int)bh);
            var feedBtn = new Rectangle(playBtn.Right + 16, (int)btnY, playBtn.Width, (int)bh);

            if (r.Button(playBtn, "Play with creature", ctx.Input, accent: true,
                    subLabel: "+Happiness, ends the day"))
                DoPlay(ctx);

            if (r.Button(feedBtn, "Feed...", ctx.Input, enabled: foodCount > 0,
                    subLabel: foodCount > 0 ? $"{foodCount} item(s) in pantry" : "Pantry empty - craft some food!"))
                ctx.Navigate?.Invoke(new FoodPickerScreen(_creature));

            var backBtn = new Rectangle(24, vp.Height - 58, 200, 42);
            if (r.Button(backBtn, "Back to Roster", ctx.Input))
                ctx.Navigate?.Invoke(new CreatureListScreen());
        }
    }
}
