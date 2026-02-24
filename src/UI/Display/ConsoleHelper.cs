using System;

namespace CraftingCreatureWorld.UI.Display
{
    /// <summary>
    /// Provides utility methods for interacting with the console that
    /// gracefully handle environments where no real console is available
    /// (like unit test runners).
    /// </summary>
    public static class ConsoleHelper
    {
        public static void Clear()
        {
            try
            {
                Console.Clear();
            }
            catch (IOException)
            {
                // occurs when no console is attached, ignore in tests
            }
            catch (System.Security.SecurityException)
            {
                // ignore if permissions forbid clearing
            }
        }
    }
}
