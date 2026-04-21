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
                if (!Console.IsOutputRedirected)
                {
                    Console.Clear();
                }
            }
            catch (IOException)
            {
                // occurs when no console is attached, ignore
            }
            catch (System.Security.SecurityException)
            {
                // ignore if permissions forbid clearing
            }
        }
        
        public static void SafeReadKey()
        {
            try
            {
                if (!Console.IsInputRedirected)
                {
                    Console.ReadKey(true);
                }
            }
            catch (InvalidOperationException)
            {
                // Console input is redirected or unavailable, skip the wait
            }
            catch (Exception)
            {
                // Any other exception, skip the wait
            }
        }
    }
}