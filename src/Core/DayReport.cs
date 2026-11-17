using System.Collections.Generic;

namespace CraftingCreatureWorld.Core
{
    /// <summary>Summary of what happened to one creature during a day change.</summary>
    public sealed class CreatureChange
    {
        public string Name { get; init; } = "";
        public string Type { get; init; } = "";
        public int OldHealth { get; init; }
        public int NewHealth { get; init; }
        public int OldHappiness { get; init; }
        public int NewHappiness { get; init; }
        public List<string> Warnings { get; } = new();
    }

    /// <summary>
    /// Everything the UI needs to render the "End of Day" screen after a day
    /// advances. Produced by <see cref="Services.DayService"/> instead of the
    /// service printing directly to a console.
    /// </summary>
    public sealed class DayReport
    {
        /// <summary>The day that just ended.</summary>
        public int DayEnded { get; set; }
        public string ActionDescription { get; set; } = "";
        public decimal Earnings { get; set; }
        public bool GameOver { get; set; }
        public string? GameOverReason { get; set; }
        public List<CreatureChange> Changes { get; } = new();
        public List<string> EventLines { get; } = new();
        public List<string> InteractionLines { get; } = new();
    }
}
