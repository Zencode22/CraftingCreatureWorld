using CraftingCreatureWorld.Core;
using CreatureWorld;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CraftingCreatureWorld.GameUI.Screens
{
    /// <summary>
    /// Creature roster. Each creature row is a button with a number hot key
    /// [1..9]; clicking or pressing the number opens its detail page where you
    /// can interact ([P] play) or feed ([F] then pick food).
    /// </summary>
    public sealed class CreatureListScreen : IScreen
    {
        public string HotkeyHelp => "[1-9] Select creature   [Esc] Back to menu";

        public void Update(ScreenContext ctx)
        {
            if (ctx.Input.WasPressed(Keys.Escape))
                ctx.Navigate?.Invoke(new MainMenuScreen());
        }

        public void Draw(ScreenContext ctx)
        {
            var r = ctx.Renderer!;
            var vp = r.Viewport;
            Hud.Draw(ctx, ctx.Logic);

            float top = Hud.ContentTop + 8;
            r.Text("YOUR CREATURES", 24, top - 30, Color.White, bold: true);

            var creatures = ctx.Logic.Creatures;
            if (creatures.Count == 0)
            {
                r.CenteredText("No creatures yet!", vp.Width / 2f, vp.Height / 2f, Theme.TextDim);
                return;
            }

            const float h = 64, gap = 10;
            float w = Math.Min(720, vp.Width - 60);
            float x = (vp.Width - w) / 2;

            for (int i = 0; i < creatures.Count && i < 9; i++)
            {
                var c = creatures[i];
                var rect = new Rectangle((int)x, (int)(top + i * (h + gap)), (int)w, (int)h);
                var hotkey = Keys.D1 + i;

                string status = c.Health <= 0 || c.Happiness <= 0 ? "CRITICAL!"
                              : c.Health < 30 || c.Happiness < 30 ? "Needs attention"
                              : "Content";

                if (r.Button(rect, GameLogic.FlavorFor(c), ctx.Input, hotkey,
                        subLabel: $"Health {c.Health}%  |  Happiness {c.Happiness}%  |  {status}"))
                {
                    ctx.Navigate?.Invoke(new CreatureDetailScreen(c));
                    return;
                }

                // small colored dot indicating overall wellbeing
                var dotColor = c.Health <= 0 || c.Happiness <= 0 ? Theme.Red
                             : c.Health < 30 || c.Happiness < 30 ? Theme.Gold
                             : Theme.Green;
                r.FillRoundRect(new Rectangle(rect.Right - 34, rect.Y + rect.Height / 2 - 8, 16, 16), 8, dotColor);
            }

            r.CenteredText(HotkeyHelp, vp.Width / 2f, vp.Height - 36, Theme.TextDim, scale: 0.9f);
        }
    }
}
