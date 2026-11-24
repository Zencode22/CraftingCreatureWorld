using CraftingCreatureWorld.Core;

using Microsoft.Xna.Framework;
namespace CraftingCreatureWorld.GameUI.Screens
{
    /// <summary>
    /// Hub screen. Every option is a big clickable button.
    /// </summary>
    public sealed class MainMenuScreen : IScreen
    {
        public void Update(ScreenContext ctx)
        {
            // handled in Draw via immediate-mode buttons
        }

        public void Draw(ScreenContext ctx)
        {
            var r = ctx.Renderer!;
            var vp = r.Viewport;
            Hud.Draw(ctx, ctx.Logic);

            float top = Hud.ContentTop + 10;
            float w = Math.Min(560, vp.Width - 60);
            float x = (vp.Width - w) / 2;
            const float h = 74;
            const float gap = 16;

            r.CenteredText("MAIN MENU", vp.Width / 2f, top - 34, Color.White, bold: true, scale: 1.3f);

            int i = 0;
            Rectangle Row() => new((int)x, (int)(top + i * (h + gap)), (int)w, (int)h);

            if (r.Button(Row(), "Manage Your Creatures", ctx.Input, accent: true,
                    subLabel: "View stats, feed and play with your creatures"))
                ctx.Navigate?.Invoke(new CreatureListScreen());
            i++;

            if (r.Button(Row(), $"Visit {(ctx.Logic.State.Trader?.Name ?? "the Merchant")}", ctx.Input,
                    subLabel: "Buy ingredients (rotation changes weekly)"))
                ctx.Navigate?.Invoke(new TraderScreen());
            i++;

            if (r.Button(Row(), "Crafting Station", ctx.Input,
                    subLabel: "Combine ingredients into creature food & potions"))
                ctx.Navigate?.Invoke(new CraftingScreen());
            i++;

            if (r.Button(Row(), "End Day Early", ctx.Input,
                    subLabel: "Skip to tomorrow - creatures still earn, but stats decay"))
                ctx.Navigate?.Invoke(new ConfirmScreen(
                    "END DAY EARLY",
                    "Your creatures will still earn currency, but their health and\nhappiness will decrease. Continue?",
                    onYes: c =>
                    {
                        var report = c.Logic.EndDayEarly();
                        c.Navigate?.Invoke(new DayReportScreen(report, new MainMenuScreen()));
                    }));
            i++;

            if (r.Button(Row(), "Quit Game", ctx.Input,
                    subLabel: "Thanks for playing!"))
                ctx.Logic.QuitGame();
        }
    }
}
