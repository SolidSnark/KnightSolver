using NUnit.Framework;
using Moq;

using KnightMazeSolver;

namespace KnightSolverUnitTests
{
    public class BoardLocationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BoardLocation_Equals_Equal()
        { 
            // Setup
            BoardLocation locationA = new BoardLocation(1, 2);
            BoardLocation locationB = new BoardLocation(1, 2);
            
            // Act/Assert
            Assert.IsTrue(locationA.Equals(locationB), $"Result does not equal expected ({locationA.X},{locationA.Y}) != ({locationB.X},{locationB.Y})");
        }

        [Test]
        public void BoardLocation_AltEquals_Equal()
        {
            // Setup
            BoardLocation locationA = new BoardLocation(1, 2);
            BoardLocation locationB = new BoardLocation(1, 2);

            // Act/Assert
            Assert.IsTrue(locationA.Equals(locationA, locationB), $"Result does not equal expected ({locationA.X},{locationA.Y}) != ({locationB.X},{locationB.Y})");
        }

        [Test]
        public void BoardLocation_Equals_NotEqual()
        {
            // Setup
            BoardLocation locationA = new BoardLocation(1, 2);
            BoardLocation locationB = new BoardLocation(3, 2);
            
            // Act/Assert
            Assert.IsFalse(locationA.Equals(locationB), $"Result does not equal expected ({locationA.X},{locationA.Y}) == ({locationB.X},{locationB.Y})");
        }

        [Test]
        public void BoardLocation_AltEquals_NotEqual()
        {
            // Setup
            BoardLocation locationA = new BoardLocation(1, 2);
            BoardLocation locationB = new BoardLocation(3, 2);

            // Act/Assert
            Assert.IsFalse(locationA.Equals(locationA, locationB), $"Result does not equal expected ({locationA.X},{locationA.Y}) == ({locationB.X},{locationB.Y})");
        }

        [Test]
        public void BoardLocation_GetHashCode_DoesNotThrow()
        {
            // Setup
            BoardLocation location = new BoardLocation(1, 2);
            
            // Act/Assert
            Assert.DoesNotThrow(() => location.GetHashCode(location), "GetHashCode threw an Exception");            
        }
    }
}