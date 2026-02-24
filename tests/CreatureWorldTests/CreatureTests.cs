using CreatureWorld;
using Xunit;

namespace CraftingCreatureWorld.Tests.CreatureWorldTests
{
    public class CreatureTests
    {
        [Fact]
        public void Dragon_Creation_SetsPropertiesCorrectly()
        {
            // Arrange & Act
            var dragon = new Dragon("Smaug", 500, 100, Element.Fire);
            
            // Assert
            Assert.Equal("Smaug", dragon.Name);
            Assert.Equal(500, dragon.Age);
            Assert.Equal(100, dragon.Health);
            Assert.Equal(CreatureType.Dragon, dragon.Type);
            Assert.Equal(Element.Fire, dragon.ElementType);
        }
        
        [Fact]
        public void Elf_Creation_SetsPropertiesCorrectly()
        {
            // Arrange & Act
            var elf = new Elf("Legolas", 150, 80, BowType.Longbow);
            
            // Assert
            Assert.Equal("Legolas", elf.Name);
            Assert.Equal(150, elf.Age);
            Assert.Equal(80, elf.Health);
            Assert.Equal(CreatureType.Elf, elf.Type);
            Assert.Equal(BowType.Longbow, elf.BowType);
        }
        
        [Fact]
        public void Goblin_Creation_SetsPropertiesCorrectly()
        {
            // Arrange & Act
            var goblin = new Goblin("Grish", 50, 60, GoblinType.Cave);
            
            // Assert
            Assert.Equal("Grish", goblin.Name);
            Assert.Equal(50, goblin.Age);
            Assert.Equal(60, goblin.Health);
            Assert.Equal(CreatureType.Goblin, goblin.Type);
            Assert.Equal(GoblinType.Cave, goblin.GoblinType);
        }
        
        [Theory]
        // The dragon gets a 50% bonus when happiness is above 80, so expected results reflect that.
        [InlineData(100, 100, 4.50)] // full stats + bonus
        [InlineData(50, 100, 1.50)] // low happiness, no bonus
        [InlineData(100, 50, 2.25)] // low health but bonus applies
        public void CalculateDailyCurrency_ReturnsExpectedValue(int happiness, int health, decimal expected)
        {
            // Arrange
            var dragon = new Dragon("Test", 100, health, Element.Fire);
            dragon.Happiness = happiness;
            
            // Act
            dragon.EndOfDay(); // Triggers recalculation
            
            // Assert
            Assert.Equal(expected, dragon.DailyCurrency);
        }
    }
}