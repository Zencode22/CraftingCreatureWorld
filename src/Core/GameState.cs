
using CraftingCreatureWorld.GameUI;
namespace CraftingCreatureWorld.Core
{
    /// <summary>
    /// Shared per-frame context handed to every screen so they can read game
    /// state, draw with the renderer, and navigate to other screens.
    /// </summary>
    public sealed class ScreenContext
    {
        public GameLogic Logic { get; set; }
        /// <summary>Assigned once the graphics device and fonts are available
        /// (LoadContent). Nullable because screens are constructed before it exists.</summary>
        public Renderer? Renderer { get; set; }
        public InputState Input { get; }
        public ToastQueue Toasts { get; }

        public ScreenContext(GameLogic logic, InputState input, ToastQueue toasts)
        {
            Logic = logic;
            Input = input;
            Toasts = toasts;
        }

        /// <summary>Replace the current screen with a new one.</summary>
        public Action<IScreen>? Navigate { get; set; }

        /// <summary>Close the current (modal) screen and return to the previous one.</summary>
        public Action? CloseModal { get; set; }

        /// <summary>Show a transient notification on screen.</summary>
        public void Toast(string message, ToastKind kind = ToastKind.Info) => Toasts.Push(message, kind);

        /// <summary>Last creature the player was looking at (used to return after day reports).</summary>
        public CreatureWorld.Creature? LastViewedCreature { get; set; }
    }

    /// <summary>Anything drawn and updated each frame implements this.</summary>
    public interface IScreen
    {
        void Update(ScreenContext ctx);
        void Draw(ScreenContext ctx);
    }
}
