using CraftingCreatureWorld.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace CraftingCreatureWorld.GameUI
{
    /// <summary>A single on-screen notification with a fade-out timer.</summary>
    public readonly struct Toast
    {
        public string Message { get; init; }
        public ToastKind Kind { get; init; }
        public float TimeLeft { get; init; }

        public Toast WithTime(float t) => new() { Message = Message, Kind = Kind, TimeLeft = t };
    }

    /// <summary>Queue of transient messages shown at the top of the screen.</summary>
    public sealed class ToastQueue
    {
        private const float Duration = 3.5f;
        private readonly List<Toast> _items = new();

        public IReadOnlyList<Toast> Items => _items;

        public void Push(string message, ToastKind kind)
        {
            _items.Add(new Toast { Message = message, Kind = kind, TimeLeft = Duration });
            if (_items.Count > 4) _items.RemoveAt(0);
        }

        public void Update(float dt)
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                float t = _items[i].TimeLeft - dt;
                if (t <= 0) _items.RemoveAt(i);
                else _items[i] = _items[i].WithTime(t);
            }
        }

        public void Draw(Renderer r)
        {
            var vp = r.Viewport;
            float y = 64;
            foreach (var toast in _items)
            {
                var color = toast.Kind switch
                {
                    ToastKind.Success => Theme.Green,
                    ToastKind.Warning => Theme.Gold,
                    ToastKind.Error => Theme.Red,
                    _ => Theme.Accent
                };

                var size = r.Measure(toast.Message);
                var bounds = new Rectangle((int)(vp.Width / 2 - size.X / 2 - 14), (int)y,
                                           (int)size.X + 28, (int)size.Y + 14);
                float alpha = Math.Clamp(toast.TimeLeft / 0.6f, 0f, 1f);
                r.FillRoundRect(bounds, 8, Color.Black * 0.75f * alpha);
                r.StrokeRect(bounds, 1, color * 0.9f * alpha);
                r.Text(toast.Message, bounds.X + 14, bounds.Y + 7, color * alpha);
                y += bounds.Height + 8;
            }
        }
    }
}
