using System;
using CraftingEngine;
using Xunit;

namespace CraftingCreatureWorld.Tests.CraftingEngineTests
{
    public class ItemTests
    {
        [Fact]
        public void Item_Creation_ValidParameters_Succeeds()
        {
            // Arrange & Act
            var item = new Item("Test Item", "pieces", 1.50m);
            
            // Assert
            Assert.Equal("Test Item", item.Name);
            Assert.Equal("pieces", item.Unit);
            Assert.Equal(1.50m, item.BasePrice);
            Assert.NotEqual(Guid.Empty, item.Id);
        }
        
        [Fact]
        public void Item_ApplyDiscount_ValidDiscount_UpdatesPrice()
        {
            // Arrange
            var item = new Item("Test", "unit", 100m);
            
            // Act
            item.ApplyDiscount(20);
            
            // Assert
            Assert.Equal(80m, item.BasePrice);
        }
        
        [Fact]
        public void Item_ApplyDiscount_InvalidDiscount_ThrowsException()
        {
            // Arrange
            var item = new Item("Test", "unit", 100m);
            
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => item.ApplyDiscount(150));
        }
        
        [Fact]
        public void Item_Equals_SameId_ReturnsTrue()
        {
            // Arrange
            var item1 = new Item("Test", "unit", 10m);
            var item2 = new Item("Test", "unit", 10m);
            
            // Act - force same ID via reflection (for testing)
            var field = typeof(Item).GetProperty("Id");
            field.SetValue(item2, item1.Id);
            
            // Assert
            Assert.True(item1.Equals(item2));
        }
    }
}