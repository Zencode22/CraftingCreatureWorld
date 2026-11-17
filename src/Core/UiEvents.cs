using System;

namespace CraftingCreatureWorld.Core
{
    /// <summary>
    /// Types of transient messages the game logic can raise for the UI layer.
    /// </summary>
    public enum ToastKind
    {
        Info,
        Success,
        Warning,
        Error
    }

    /// <summary>
    /// A lightweight event bus that lets the (console-free) game services notify
    /// any UI implementation - MonoGame, tests, etc. - about things that happened.
    /// The logic layer never writes to a console directly.
    /// </summary>
    public static class UiEvents
    {
        /// <summary>Raised for short-lived on-screen notifications.</summary>
        public static event Action<string, ToastKind>? Toast;

        /// <summary>Raised for permanent log lines (e.g. day summaries).</summary>
        public static event Action<string>? Log;

        public static void RaiseToast(string message, ToastKind kind = ToastKind.Info)
            => Toast?.Invoke(message, kind);

        public static void RaiseLog(string message)
            => Log?.Invoke(message);
    }
}
