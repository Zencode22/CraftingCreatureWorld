using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace CraftingCreatureWorld.GameUI
{
    /// <summary>
    /// Minimal immediate-mode UI built on top of MonoGame: text rendering with
    /// automatic word wrap, rounded panels, progress bars and clickable buttons
    /// that can show a hot-key badge.
    /// </summary>
    public sealed class Renderer
    {
        private readonly SpriteBatch _sb;
        private readonly Texture2D _pixel;
        private readonly SpriteFont _font;
        private readonly SpriteFont _boldFont;

        // Simple string->size cache so wrapping is cheap across frames.
        private readonly Dictionary<string, Vector2> _sizeCache = new();

        public Renderer(SpriteBatch sb, Texture2D pixel, SpriteFont font, SpriteFont boldFont)
        {
            _sb = sb;
            _pixel = pixel;
            _font = font;
            _boldFont = boldFont;
        }

        public Viewport Viewport => _sb.GraphicsDevice.Viewport;

        // ---------- primitives ----------

        public void FillRect(Rectangle r, Color color) => _sb.Draw(_pixel, r, color);

        public void FillRoundRect(Rectangle r, int radius, Color color)
        {
            if (r.Width <= 0 || r.Height <= 0) return;
            radius = Math.Min(radius, Math.Min(r.Width / 2, r.Height / 2));
            if (radius <= 0) { FillRect(r, color); return; }

            // three rectangles make a cheap rounded box
            FillRect(new Rectangle(r.X + radius, r.Y, r.Width - 2 * radius, r.Height), color);
            FillRect(new Rectangle(r.X, r.Y + radius, radius, r.Height - 2 * radius), color);
            FillRect(new Rectangle(r.Right - radius, r.Y + radius, radius, r.Height - 2 * radius), color);
            // corner squares (good enough at small radii)
            FillRect(new Rectangle(r.X, r.Y, radius, radius), color);
            FillRect(new Rectangle(r.Right - radius, r.Y, radius, radius), color);
            FillRect(new Rectangle(r.X, r.Bottom - radius, radius, radius), color);
            FillRect(new Rectangle(r.Right - radius, r.Bottom - radius, radius, radius), color);
        }

        public void StrokeRect(Rectangle r, int thickness, Color color)
        {
            FillRect(new Rectangle(r.X, r.Y, r.Width, thickness), color);
            FillRect(new Rectangle(r.X, r.Bottom - thickness, r.Width, thickness), color);
            FillRect(new Rectangle(r.X, r.Y, thickness, r.Height), color);
            FillRect(new Rectangle(r.Right - thickness, r.Y, thickness, r.Height), color);
        }

        public void ProgressBar(Rectangle r, float fraction, Color fillColor, Color backColor)
        {
            fraction = Math.Clamp(fraction, 0f, 1f);
            FillRoundRect(r, 6, backColor);
            var fill = new Rectangle(r.X + 2, r.Y + 2, (int)((r.Width - 4) * fraction), r.Height - 4);
            if (fill.Width > 0)
                FillRoundRect(fill, 4, fillColor);
            StrokeRect(r, 1, new Color(255, 255, 255) * 0.25f);
        }

        // ---------- text ----------

        public Vector2 Measure(string text, bool bold = false)
        {
            var font = bold ? _boldFont : _font;
            var key = (bold ? "B|" : "R|") + text;
            if (!_sizeCache.TryGetValue(key, out var size))
            {
                size = font.MeasureString(text);
                _sizeCache[key] = size;
            }
            return size;
        }

        public Vector2 Text(string text, float x, float y, Color color, bool bold = false, float scale = 1f)
        {
            var font = bold ? _boldFont : _font;
            _sb.DrawString(font, text, new Vector2(x, y), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            return font.MeasureString(text) * scale;
        }

        public void CenteredText(string text, float centerX, float y, Color color, bool bold = false, float scale = 1f)
        {
            var size = Measure(text, bold) * scale;
            Text(text, centerX - size.X / 2, y, color, bold, scale);
        }

        /// <summary>Draws wrapped text and returns the height used.</summary>
        public float WrappedText(string text, float x, float y, float maxWidth, Color color, float lineSpacing = 6f, bool bold = false)
        {
            var font = bold ? _boldFont : _font;
            foreach (var paragraph in text.Split('\n'))
            {
                var line = "";
                foreach (var word in paragraph.Split(' '))
                {
                    var test = line.Length == 0 ? word : line + " " + word;
                    if (font.MeasureString(test).X > maxWidth && line.Length > 0)
                    {
                        _sb.DrawString(font, line, new Vector2(x, y), color);
                        y += font.LineSpacing + lineSpacing;
                        line = word;
                    }
                    else
                    {
                        line = test;
                    }
                }
                if (line.Length > 0)
                {
                    _sb.DrawString(font, line, new Vector2(x, y), color);
                    y += font.LineSpacing + lineSpacing;
                }
            }
            return y;
        }

        // ---------- widgets ----------

        /// <summary>
        /// Draws a button and returns true when it is activated by either a
        /// mouse click or its hot key. When a hotkey is supplied a small badge
        /// showing the key is painted in the button's top-left corner.
        /// </summary>
        public bool Button(Rectangle bounds, string label, InputState input, Keys? hotkey = null,
                           bool enabled = true, bool accent = false, string? subLabel = null)
        {
            var hover = bounds.Contains(input.MousePosition) && enabled;
            var pressed = hover && input.LeftClicked;

            var back = !enabled ? Theme.PanelDisabled
                     : pressed ? Theme.ButtonPressed
                     : hover ? Theme.ButtonHover
                     : accent ? Theme.ButtonAccent
                     : Theme.Button;

            FillRoundRect(bounds, 8, back);
            if (hover && enabled)
                StrokeRect(bounds, 2, Theme.Accent);

            var textColor = enabled ? Color.White : Theme.TextDim;

            if (subLabel == null)
            {
                var size = Measure(label);
                Text(label, bounds.X + (bounds.Width - size.X) / 2, bounds.Y + (bounds.Height - size.Y) / 2, textColor);
            }
            else
            {
                var size = Measure(label, bold: true);
                var sub = Measure(subLabel);
                float totalH = size.Y + sub.Y + 2;
                Text(label, bounds.X + 14, bounds.Y + (bounds.Height - totalH) / 2, textColor, bold: true);
                Text(subLabel, bounds.X + 14, bounds.Y + (bounds.Height - totalH) / 2 + size.Y + 2,
                     enabled ? Theme.TextDim : Theme.PanelDisabled, scale: 0.8f);
            }

            // hot-key badge
            if (hotkey.HasValue)
            {
                DrawHotkeyBadge(new Point(bounds.X + 6, bounds.Y + 6), KeyName(hotkey.Value), enabled);
            }

            if (!enabled) return false;
            return pressed || input.WasPressed(hotkey);
        }

        public void DrawHotkeyBadge(Point position, string key, bool enabled = true)
        {
            var size = Measure(key) * 0.8f;
            var badge = new Rectangle(position.X, position.Y, (int)size.X + 12, (int)size.Y + 8);
            FillRoundRect(badge, 5, enabled ? Theme.HotkeyBadge : Theme.PanelDisabled);
            StrokeRect(badge, 1, enabled ? Theme.Accent * 0.9f : Theme.TextDim * 0.4f);
            Text(key, badge.X + 6, badge.Y + 3, enabled ? Color.White : Theme.TextDim, bold: true, scale: 0.8f);
        }

        public static string KeyName(Keys key) => key switch
        {
            Keys.D0 => "0", Keys.D1 => "1", Keys.D2 => "2", Keys.D3 => "3", Keys.D4 => "4",
            Keys.D5 => "5", Keys.D6 => "6", Keys.D7 => "7", Keys.D8 => "8", Keys.D9 => "9",
            Keys.Back => "Back", Keys.Enter => "Enter", Keys.Escape => "Esc",
            Keys.Space => "Space", Keys.Up => "Up", Keys.Down => "Down",
            Keys.Left => "Left", Keys.Right => "Right",
            _ => key.ToString()
        };

        /// <summary>Scrollable list of buttons; wheel + Up/Down keys move the view.</summary>
        public void BeginScrollRegion(InputState input, int itemCount, int visibleRows, ref int scrollOffset)
        {
            scrollOffset += input.ScrollWheelDelta / 120 * visibleRows;
            if (input.IsDown(Keys.Down)) scrollOffset += 1;
            if (input.IsDown(Keys.Up)) scrollOffset -= 1;
            scrollOffset = Math.Clamp(scrollOffset, 0, Math.Max(0, itemCount - visibleRows));
        }

        public void EndScrollRegion(Rectangle region, int itemCount, int visibleRows, int scrollOffset)
        {
            if (itemCount <= visibleRows) return;
            float trackH = region.Height;
            float thumbH = Math.Max(24, trackH * visibleRows / itemCount);
            float t = (float)scrollOffset / Math.Max(1, itemCount - visibleRows);
            var thumb = new Rectangle(region.Right - 10, region.Y + (int)((trackH - thumbH) * t), 6, (int)thumbH);
            FillRoundRect(thumb, 3, Theme.TextDim);
        }
    }

    public static class Theme
    {
        public static readonly Color Background = new(24, 26, 38);
        public static readonly Color Panel = new(38, 42, 60);
        public static readonly Color PanelLight = new(50, 56, 80);
        public static readonly Color PanelDisabled = new(46, 48, 58);
        public static readonly Color Button = new(58, 66, 96);
        public static readonly Color ButtonHover = new(78, 90, 130);
        public static readonly Color ButtonPressed = new(96, 112, 160);
        public static readonly Color ButtonAccent = new(64, 96, 78);
        public static readonly Color Accent = new(120, 200, 255);
        public static readonly Color Gold = new(255, 205, 90);
        public static readonly Color Red = new(235, 90, 90);
        public static readonly Color Green = new(110, 220, 130);
        public static readonly Color TextDim = new(150, 155, 175);
        public static readonly Color HotkeyBadge = new(30, 34, 48);
        public static readonly Color HealthBar = new(225, 85, 85);
        public static readonly Color HappinessBar = new(250, 200, 80);
    }
}
