using System;
using CraftingEngine;

namespace CraftingCreatureWorld.Entities
{
    public abstract class Person
    {
        public string Name { get; }
        public Inventory Inventory { get; }

        protected Person(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Inventory = new Inventory();
        }
    }
}