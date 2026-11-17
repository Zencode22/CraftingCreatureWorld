using CraftingCreatureWorld.Core;
using CreatureWorld;
using Microsoft.Xna.Framework.Graphics;

namespace CraftingCreatureWorld.GameUI
{
    /// <summary>
    /// Draws the persistent HUD: title bar with day/currency and a row of
    /// creature cards showing health/happiness bars. Shared by all screens so
    /// the game state is always visible while clicking around.
    /// </summary>
    public static class Hud
    {
        public const int TopBarHeight = 52;
        public const int CardRowHeight = 108;
        public const int ContentTop = TopBarHeight + CardRowHeight + 14;

        public static void Draw(ScreenContext ctx, GameLogic logic)
        {
            var r = ctx.Renderer;
            var vp = r.Viewport;

            // ---- top bar ----
            r.FillRect(new Rectangle(0, 0, vp.Width, TopBarHeight), Theme.Panel);
            r.FillRect(new Rectangle(0, TopBarHeight - 2, vp.Width, 2), Theme.Accent * 0.5f);

            r.Text($"CREATURE CRAFT", 16, 14, Color.White, bold: true);
            r.Text($"Day {logic.CurrentDay}", 220, 16, Theme.Accent);
            r.Text($"{logic.Player.Name}", 320, 16, Theme.TextDim);

            var money = $"$ {logic.Player.Currency:N2}";
            var size = r.Measure(money, bold: true);
            r.Text(money, vp.Width - size.X - 16, 14, Theme.Gold, bold: true);

            // ---- creature cards ----
            int count = logic.Creatures.Count;
            if (count == 0) return;

            float margin = 12;
            float w = (vp.Width - margin * (count + 1)) / count;
            for (int i = 0; i < count; i++)
            {
                var rect = new Rectangle((int)(margin + i * (w + margin)), TopBarHeight + 8, (int)w, CardRowHeight - 12);
                DrawCreatureCard(r, logic.Creatures[i], rect, index: i + 1);
            }
        }

        private static void DrawCreatureCard(Renderer r, Creature c, Rectangle rect, int index)
        {
            r.FillRoundRect(rect, 10, Theme.Panel);

            // low-stat warning glow
            if (c.Health <= 0 || c.Happiness <= 0)
                r.StrokeRect(rect, 2, Theme.Red * 0.8f);
            else
                r.StrokeRect(rect, 1, Color.White * 0.08f);

            r.Text($"{index}. {c.Name}", rect.X + 12, rect.Y + 8, Color.White, bold: true);
            r.Text(c.Type.ToString(), rect.Right - r.Measure(c.Type.ToString()).X - 12, rect.Y + 10, TypeColor(c));

            string flavor = GameLogic.FlavorFor(c);
            if (flavor.Length > 34) flavor = flavor[..34];
            r.Text(flavor, rect.X + 12, rect.Y + 30, Theme.TextDim, scale: 0.75f);

            // health bar
            var hb = new Rectangle(rect.X + 12, rect.Y + 52, rect.Width - 70, 12);
            r.ProgressBar(hb, c.Health / 100f, Theme.HealthBar, Theme.PanelDisabled);
            r.Text($"{c.Health}%", hb.Right + 6, hb.Y - 3, c.Health <= 0 ? Theme.Red : Color.White, scale: 0.8f);

            // happiness bar
            var hpb = new Rectangle(rect.X + 12, rect.Y + 72, rect.Width - 70, 12);
            r.ProgressBar(hpb, c.Happiness / 100f, Theme.HappinessBar, Theme.PanelDisabled);
            r.Text($"{c.Happiness}%", hpb.Right + 6, hpb.Y - 3, c.Happiness <= 0 ? Theme.Red : Color.White, scale: 0.8f);

            r.Text($"+{c.DailyCurrency:C}/day", rect.X + 12, rect.Bottom - 22, Theme.Gold, scale: 0.8f);
        }

        public static Color TypeColor(Creature c) => c.Type switch
        {
            CreatureType.Dragon => new Color(255, 130, 90),
            CreatureType.Fairy => new Color(190, 150, 255),
            CreatureType.Goblin => new Color(130, 220, 130),
            _ => Color.White
        };
    }
}
