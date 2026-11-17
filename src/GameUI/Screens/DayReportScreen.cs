using CraftingCreatureWorld.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CraftingCreatureWorld.GameUI.Screens
{
    /// <summary>
    /// Shown after any action that advances the day. Summarises earnings and
    /// per-creature stat changes. Hot keys: [Enter]/[Space] continue, [Esc] to menu.
    /// </summary>
    public sealed class DayReportScreen : IScreen
    {
        private readonly DayReport _report;
        private readonly IScreen _returnTo;

        public DayReportScreen(DayReport report, IScreen returnTo)
        {
            _report = report;
            _returnTo = returnTo;
        }

        public string HotkeyHelp => "[Enter] Continue   [Esc] Main menu";

        public void Update(ScreenContext ctx)
        {
            if (ctx.Input.WasPressed(Keys.Enter) || ctx.Input.WasPressed(Keys.Space))
                ctx.Navigate?.Invoke(_returnTo);
            else if (ctx.Input.WasPressed(Keys.Escape))
                ctx.Navigate?.Invoke(new MainMenuScreen());
        }

        public void Draw(ScreenContext ctx)
        {
            var r = ctx.Renderer;
            var vp = r.Viewport;
            Hud.Draw(ctx, ctx.Logic);

            float top = Hud.ContentTop + 8;
            float w = Math.Min(760, vp.Width - 60);
            float x = (vp.Width - w) / 2;

            r.CenteredText($"END OF DAY {_report.DayEnded}", vp.Width / 2f, top - 34, Color.White, bold: true, scale: 1.25f);
            r.CenteredText($"You {_report.ActionDescription}.", vp.Width / 2f, top - 4, Theme.TextDim, scale: 0.9f);

            float y = top + 30;
            var panel = new Rectangle((int)x, (int)y, (int)w, (int)(vp.Height - y - 90));
            r.FillRoundRect(panel, 10, Theme.Panel);
            r.StrokeRect(panel, 1, Color.White * 0.08f);

            r.Text($"Earnings today: {_report.Earnings:C}", panel.X + 16, panel.Y + 12, Theme.Gold, bold: true);
            y = panel.Y + 44;

            foreach (var c in _report.Changes)
            {
                if (y > panel.Bottom - 60) break;

                string line = $"{c.Name} ({c.Type}):  Health {c.OldHealth}->{c.NewHealth}   " +
                              $"Happiness {c.OldHappiness}->{c.NewHappiness}";
                bool bad = c.NewHealth <= c.OldHealth && c.NewHappiness <= c.OldHappiness;
                r.Text(line, panel.X + 16, y, bad ? Theme.TextDim : Color.White, scale: 0.9f);
                y += 24;

                foreach (var warning in c.Warnings)
                {
                    if (y > panel.Bottom - 40) break;
                    r.Text("! " + warning, panel.X + 32, y, Theme.Red, scale: 0.85f);
                    y += 22;
                }
            }

            foreach (var ev in _report.EventLines)
            {
                if (y > panel.Bottom - 30) break;
                r.Text(ev, panel.X + 16, y, Theme.Accent, scale: 0.9f);
                y += 24;
            }

            foreach (var line in _report.InteractionLines)
            {
                if (y > panel.Bottom - 30) break;
                r.Text(line, panel.X + 16, y, Theme.Green, scale: 0.9f);
                y += 24;
            }

            var contBtn = new Rectangle(vp.Width / 2 - 130, vp.Height - 78, 260, 44);
            if (r.Button(contBtn, "Continue", ctx.Input, Keys.Enter, accent: true))
                ctx.Navigate?.Invoke(_returnTo);

            r.CenteredText(HotkeyHelp, vp.Width / 2f, vp.Height - 28, Theme.TextDim, scale: 0.9f);
        }
    }
}
