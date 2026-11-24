using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace CraftingCreatureWorld.GameUI
{
    /// <summary>
    /// Per-frame input snapshot: mouse position/clicks, scroll wheel and
    /// edge-detected key presses (used for title-screen text entry).
    /// </summary>
    public sealed class InputState
    {
        private readonly HashSet<Keys> _pressed = new();
        private readonly HashSet<Keys> _released = new();
        private MouseState _prevMouse;

        public Point MousePosition { get; private set; }
        public bool LeftClicked { get; private set; }
        public int ScrollWheelDelta { get; private set; }

        public void Update()
        {
            var kb = Keyboard.GetState();
            _pressed.Clear();
            foreach (var key in kb.GetPressedKeys())
                if (!_released.Contains(key))
                    _pressed.Add(key);

            // "released" from last frame becomes the current frame's state
            _released.Clear();
            foreach (var key in kb.GetPressedKeys())
                _released.Add(key);

            var ms = Mouse.GetState();
            MousePosition = new Point(ms.X, ms.Y);
            LeftClicked = ms.LeftButton == ButtonState.Pressed &&
                          _prevMouse.LeftButton == ButtonState.Released;
            ScrollWheelDelta = ms.ScrollWheelValue - _prevMouse.ScrollWheelValue;
            _prevMouse = ms;
        }

        public bool WasPressed(Keys key) => _pressed.Contains(key);
    }
}
