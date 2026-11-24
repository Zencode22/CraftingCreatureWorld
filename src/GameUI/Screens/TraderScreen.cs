using System.Collections.Generic;
using CraftingCreatureWorld.Core;
using CraftingEngine;

using Microsoft.Xna.Framework;

namespace CraftingCreatureWorld.GameUI.Screens
{
    /// <summary>
    /// The merchant's shop. Every item row is a button; clicking buys one unit
    /// (which advances the day, so the end-of-day report is shown afterwards).
    /// </summary>
    public sealed class TraderScreen : IScreen
    {
        private List<(Item item, decimal price, string stock)> _items = new();
        private int _version = -1;

        public void Update(ScreenContext ctx)
        {
        }

        public void Draw(ScreenContext ctx)
        {
            var r = ctx.Renderer!;
            var vp = r.Viewport;
            Hud.Draw(ctx, ctx.Logic);

            // refresh catalogue when the day changes (stock/rotation may differ)
            if (_version != ctx.Logic.CurrentDay)
            {
                _items = ctx.Logic.TraderItems();
                _version = ctx.Logic.CurrentDay;
            }

            float top = Hud.ContentTop + 8;
            var traderName = ctx.Logic.State.Trader?.Name ?? "The Merchant";
            r.Text($"{traderName}'s Wares", 24, top - 30, Color.White, bold: true);
            r.Text("Buying advances to the next day.", 24 + r.Measure($"{traderName}'s Wares", bold: true).X + 20,
                   top - 26, Theme.TextDim, scale: 0.85f);

            const float h = 58, gap = 8;
            float w = Math.Min(720, vp.Width - 60);
            float x = (vp.Width - w) / 2;

            for (int i = 0; i < _items.Count && i < 9; i++)
            {
                var (item, price, stock) = _items[i];
                var rect = new Rectangle((int)x, (int)(top + i * (h + gap)), (int)w, (int)h);
                bool affordable = ctx.Logic.Player.Currency >= price;

                if (r.Button(rect, $"Buy {item.Unit} of {item.Name}", ctx.Input,
                        enabled: affordable,
                        subLabel: $"{price:C}  |  Stock: {stock}{(affordable ? "" : "  -  not enough coin!")}"))
                {
                    if (BuyOne(ctx, item, price)) return;
                }

                // price tag on the right
                var priceSize = r.Measure(price.ToString("C"), bold: true);
                r.Text(price.ToString("C"), rect.Right - priceSize.X - 14, rect.Y + (rect.Height - priceSize.Y) / 2,
                       affordable ? Theme.Gold : Theme.TextDim, bold: true);
            }

            var backBtn = new Rectangle(24, vp.Height - 58, 200, 42);
            if (r.Button(backBtn, "Back to Menu", ctx.Input))
                ctx.Navigate?.Invoke(new MainMenuScreen());
        }

        private bool BuyOne(ScreenContext ctx, Item item, decimal price)
        {
            // Buy() internally advances the day and returns the real report.
            var report = ctx.Logic.Buy(item, price);
            if (report == null)
                return false; // purchase failed (toast already raised by logic)

            if (report.GameOver || ctx.Logic.IsGameOver)
                ctx.Navigate?.Invoke(new GameOverScreen(ctx.Logic.GameOverReason ?? "Game over"));
            else
                ctx.Navigate?.Invoke(new DayReportScreen(report, new MainMenuScreen()));
            return true;
        }
    }
}
