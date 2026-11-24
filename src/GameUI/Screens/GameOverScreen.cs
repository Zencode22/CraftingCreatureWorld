using CraftingCreatureWorld.Core;

using Microsoft.Xna.Framework;

namespace CraftingCreatureWorld.GameUI.Screens
{
    /// <summary>
    /// Game-over screen. Play again returns to the title; quit closes the window.
    /// </summary>
    public sealed class GameOverScreen : IScreen
    {
        private readonly string _reason;

        public GameOverScreen(string reason) => _reason = reason;

        /// <summary>Set by the game so the UI can request the window to close.</summary>
        public static System.Action? RequestExit { get; set; }

        public void Update(ScreenContext ctx)
        {
        }

        public void Draw(ScreenContext ctx)
        {
            var r = ctx.Renderer!;
            var vp = r.Viewport;

            r.CenteredText("GAME OVER", vp.Width / 2f, vp.Height / 2f - 110, Theme.Red, bold: true, scale: 2.2f);
            r.CenteredText(DescribeReason(), vp.Width / 2f, vp.Height / 2f - 30, Color.White, scale: 1.1f);
            r.CenteredText($"You survived {ctx.Logic.CurrentDay} day(s) with {ctx.Logic.Player.Currency:C} in the bank.",
                           vp.Width / 2f, vp.Height / 2f + 4, Theme.TextDim);

            var againBtn = new Rectangle(vp.Width / 2 - 130, (int)(vp.Height / 2f + 50), 260, 52);
            if (r.Button(againBtn, "Play Again", ctx.Input, accent: true))
                ctx.Navigate?.Invoke(new TitleScreen());

            var quitBtn = new Rectangle(vp.Width / 2 - 130, (int)(vp.Height / 2f + 118), 260, 52);
            if (r.Button(quitBtn, "Quit Game", ctx.Input))
                RequestExit?.Invoke();
        }

        private string DescribeReason() => _reason switch
        {
            "quit" => "You chose to retire from creature keeping.",
            null or "" => "It's over... for now.",
            _ => _reason
        };
    }
}
