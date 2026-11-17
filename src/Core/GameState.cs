
namespace CraftingCreatureWorld.Core
{
    /// <summary>
    /// Shared per-frame context handed to every screen so they can read game
    /// state, draw with the renderer, and navigate to other screens.
    /// </summary>
    public sealed class ScreenContext
    {
        public GameLogic Logic { get; set; }
        public Renderer Renderer { get; }
        public InputState Input { get; }
        public ToastQueue Toasts { get; }

        public ScreenContext(GameLogic logic, Renderer renderer, InputState input, ToastQueue toasts)
        {
            Logic = logic;
            Renderer = renderer;
            Input = input;
            Toasts = toasts;
        }

        /// <summary>Replace the current screen with a new one.</summary>
        public Action<IScreen>? Navigate { get; set; }

        /// <summary>Close the current (modal) screen and return to the previous one.</summary>
        public Action? CloseModal { get; set; }

        /// <summary>Show a transient notification on screen.</summary>
        public void Toast(string message, ToastKind kind = ToastKind.Info) => Toasts.Push(message, kind);
    }

    /// <summary>Anything drawn and updated each frame implements this.</summary>
    public interface IScreen
    {
        string HotkeyHelp { get; }
        void Update(ScreenContext ctx);
        void Draw(ScreenContext ctx);
    }
}
