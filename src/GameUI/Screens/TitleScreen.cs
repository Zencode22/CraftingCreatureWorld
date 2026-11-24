using CraftingCreatureWorld.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace CraftingCreatureWorld.GameUI.Screens
{
    /// <summary>
    /// Title screen: type a player name (keyboard) and click Start.
    /// </summary>
    public sealed class TitleScreen : IScreen
    {
        private string _name = "";
        private float _blink;

        public void Update(ScreenContext ctx)
        {
            _blink += 0.05f;

            var kb = Keyboard.GetState();
            foreach (var key in kb.GetPressedKeys())
            {
                if (!ctx.Input.WasPressed(key)) continue;

                if (key == Keys.Back && _name.Length > 0)
                    _name = _name[..^1];
                else if (_name.Length < 16)
                {
                    var ch = KeyToChar(key, shift: IsShiftDown(kb));
                    if (ch != '\0') _name += ch;
                }
            }

            // clicking the start button also works
            var vp = ctx.Renderer!.Viewport;
            var startBtn = new Rectangle(vp.Width / 2 - 120, 400, 240, 56);
            if (ctx.Input.LeftClicked && startBtn.Contains(ctx.Input.MousePosition))
                StartGame(ctx);
        }

        private void StartGame(ScreenContext ctx)
        {
            var name = string.IsNullOrWhiteSpace(_name) ? "Adventurer" : _name.Trim();
            var logic = new GameLogic(name);
            logic.StartNewGame();
            ctx.Logic = logic;
            ctx.Navigate?.Invoke(new MainMenuScreen());
        }

        private static bool IsShiftDown(KeyboardState kb) =>
            kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift);

        private static char KeyToChar(Keys key, bool shift)
        {
            if (key >= Keys.A && key <= Keys.Z)
                return shift ? (char)('A' + (key - Keys.A)) : (char)('a' + (key - Keys.A));
            if (key >= Keys.D0 && key <= Keys.D9)
                return (char)('0' + (key - Keys.D0));
            return key switch
            {
                Keys.Space => ' ',
                Keys.OemMinus => shift ? '_' : '-',
                Keys.OemPeriod => shift ? '>' : '.',
                Keys.OemComma => shift ? '<' : ',',
                _ => '\0'
            };
        }

        public void Draw(ScreenContext ctx)
        {
            var r = ctx.Renderer!;
            var vp = r.Viewport;
            float cx = vp.Width / 2f;

            r.CenteredText("CREATURE CRAFT", cx, 130, Theme.Accent, bold: true, scale: 2.4f);
            r.CenteredText("Collect creatures - earn coins - craft food - keep them happy!",
                           cx, 200, Theme.TextDim);

            // name box
            var box = new Rectangle((int)cx - 180, 280, 360, 52);
            r.FillRoundRect(box, 8, Theme.Panel);
            r.StrokeRect(box, 2, Theme.Accent * 0.7f);
            r.Text("Name:", box.X + 14, box.Y + 15, Theme.TextDim);
            string display = _name.Length == 0 ? "Adventurer" : _name;
            var color = _name.Length == 0 ? Theme.TextDim * 0.6f : Color.White;
            r.Text(display, box.X + 76, box.Y + 14, color);
            if ((_blink % 1f) < 0.55f)
                r.FillRect(new Rectangle(box.X + 78 + (int)r.Measure(display).X, box.Y + 12, 2, 28), Theme.Accent);

            var startBtn = new Rectangle((int)cx - 120, 400, 240, 56);
            r.Button(startBtn, "Start Adventure", ctx.Input, accent: true);

            r.CenteredText("Your three starter creatures arrive tomorrow morning...",
                           cx, 500, Theme.TextDim, scale: 0.9f);
        }
    }
}
