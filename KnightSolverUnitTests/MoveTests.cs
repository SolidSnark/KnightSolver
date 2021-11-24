using NUnit.Framework;

using KnightMazeSolver;

namespace KnightSolverUnitTests
{
    public class MoveBoardLocationTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Move_Equals_Equal()
        {
            // Setup
            BoardLocation locationA = new BoardLocation(1, 2);
            BoardLocation locationB = new BoardLocation(4, 3);
            Move moveA = new Move(locationA, locationB);

            BoardLocation locationC = new BoardLocation(1, 2);
            BoardLocation locationD = new BoardLocation(4, 3);
            Move moveB = new Move(locationC, locationD);

            // Act/Assert
            Assert.IsTrue(moveA.Equals(moveA, moveB), $"Result does not equal expected ({moveA.StartingLocation.X},{moveA.StartingLocation.Y} -> {moveA.EndingLocation.X},{moveA.EndingLocation.Y}) != ({moveB.StartingLocation.X},{moveB.StartingLocation.Y} -> {moveB.EndingLocation.X},{moveB.EndingLocation.Y})");
        }

        [Test]
        public void BoardLocation_AltEquals_NotEqual()
        {
            // Setup
            BoardLocation locationA = new BoardLocation(1, 2);
            BoardLocation locationB = new BoardLocation(4, 3);
            Move moveA = new Move(locationA, locationB);

            BoardLocation locationC = new BoardLocation(1, 2);
            BoardLocation locationD = new BoardLocation(4, 4);
            Move moveB = new Move(locationC, locationD);

            // Act/Assert
            Assert.IsFalse(moveA.Equals(moveA, moveB), $"Result does not equal expected ({moveA.StartingLocation.X},{moveA.StartingLocation.Y} -> {moveA.EndingLocation.X},{moveA.EndingLocation.Y}) == ({moveB.StartingLocation.X},{moveB.StartingLocation.Y} -> {moveB.EndingLocation.X},{moveB.EndingLocation.Y})");
        }

        [Test]
        public void BoardLocation_GetHashCode_DoesNotThrow()
        {
            // Setup
            BoardLocation locationA = new BoardLocation(1, 2);
            BoardLocation locationB = new BoardLocation(3, 3);
            Move move = new Move(locationA, locationB);

            // Act/Assert
            Assert.DoesNotThrow(() => move.GetHashCode(move), "GetHashCode threw an Exception");
        }
    }
}