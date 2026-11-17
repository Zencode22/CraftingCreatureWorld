using System;
using CraftingCreatureWorld.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CraftingCreatureWorld.GameUI.Screens
{
    /// <summary>
    /// Yes/confirm dialog. Hot keys: [Y] confirm, [N]/[Esc] cancel. Both the
    /// buttons and the keys trigger the same action.
    /// </summary>
    public sealed class ConfirmScreen : IScreen
    {
        private readonly string _title;
        private readonly string _message;
        private readonly Action<ScreenContext> _onYes;

        public ConfirmScreen(string title, string message, Action<ScreenContext> onYes)
        {
            _title = title;
            _message = message;
            _onYes = onYes;
        }

        public string HotkeyHelp => "[Y] Yes   [N] No";

        public void Update(ScreenContext ctx)
        {
            if (ctx.Input.WasPressed(Keys.N) || ctx.Input.WasPressed(Keys.Escape))
                ctx.Navigate?.Invoke(new MainMenuScreen());
            else if (ctx.Input.WasPressed(Keys.Y))
                _onYes(ctx);
        }

        public void Draw(ScreenContext ctx)
        {
            var r = ctx.Renderer;
            var vp = r.Viewport;
            Hud.Draw(ctx, ctx.Logic);

            float w = Math.Min(560, vp.Width - 80);
            float x = (vp.Width - w) / 2;
            float y = vp.Height / 2f - 140;

            var panel = new Rectangle((int)x, (int)y, (int)w, 280);
            r.FillRoundRect(panel, 12, Theme.Panel);
            r.StrokeRect(panel, 2, Theme.Accent * 0.6f);

            r.CenteredText(_title, vp.Width / 2f, panel.Y + 24, Color.White, bold: true, scale: 1.2f);
            r.WrappedText(_message, panel.X + 30, panel.Y + 70, panel.Width - 60, Theme.TextDim);

            const float bh = 54, bw = 200, gap = 24;
            var yesBtn = new Rectangle((int)(vp.Width / 2 - bw - gap / 2), panel.Bottom - 80, (int)bw, (int)bh);
            var noBtn = new Rectangle((int)(vp.Width / 2 + gap / 2), yesBtn.Y, (int)bw, (int)bh);

            if (r.Button(yesBtn, "Yes", ctx.Input, Keys.Y, accent: true))
                _onYes(ctx);

            if (r.Button(noBtn, "No", ctx.Input, Keys.N))
                ctx.Navigate?.Invoke(new MainMenuScreen());

            r.CenteredText(HotkeyHelp, vp.Width / 2f, vp.Height - 36, Theme.TextDim, scale: 0.9f);
        }
    }
}
