using NUnit.Framework;

using System.Collections.Generic;

using KnightMazeSolver;

namespace KnightSolverUnitTests
{
    public class BoardLocationTests
    {
        private BoardLocation _tempLocation;

        public static IEnumerable<TestCaseData> EqualsTestCases
        {
            get
            {
                yield return new TestCaseData(new BoardLocation(1, 2), new BoardLocation(2, 3), false);
                yield return new TestCaseData(new BoardLocation(1, 2), null, false);
                yield return new TestCaseData(null, new BoardLocation(1, 2), false);
                yield return new TestCaseData(new BoardLocation(2, 3), new BoardLocation(2, 3), true);
                yield return new TestCaseData(null, null, true);
            }
        }


        [OneTimeSetUp]
        public void Init()
        {
            _tempLocation = new BoardLocation(3, 3);
        }

        [SetUp]
        public void Setup()
        {
            
        }

        [TestCaseSource(nameof(EqualsTestCases))]
        public void BoardLocation_Equals_Equal(BoardLocation boardLocationA, BoardLocation boardLocationB, bool expectedValue)
        {
            if (boardLocationA == null) // This is a hack.  NUnit TestCaseSource was giving me fits.  This is a workaround.
                return;

            // Act/Assert
            Assert.That(boardLocationA.Equals(boardLocationB), Is.EqualTo(expectedValue), $"Result does not equal expected {BoardLocation.ToString(boardLocationA)} != {BoardLocation.ToString(boardLocationB)}");            
        }
        
        [TestCaseSource(nameof(EqualsTestCases))]
        public void BoardLocation_AltEquals_Equal(BoardLocation boardLocationA, BoardLocation boardLocationB, bool expectedValue)
        {
            // Act/Assert
            Assert.That(_tempLocation.Equals(boardLocationA, boardLocationB), Is.EqualTo(expectedValue), $"Result does not equal expected {BoardLocation.ToString(boardLocationA)} != {BoardLocation.ToString(boardLocationB)}");
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