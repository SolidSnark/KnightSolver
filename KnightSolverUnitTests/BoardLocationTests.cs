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
                yield return new TestCaseData(new BoardLocation(3, 4), (BoardLocation)null, false);
                yield return new TestCaseData(new BoardLocation(1, 2), new BoardLocation(2, 3), false);
                yield return new TestCaseData(new BoardLocation(1, 2), (BoardLocation)null, false);
                yield return new TestCaseData(new BoardLocation(1, 2), new BoardLocation(1, 2), true);
                yield return new TestCaseData(new BoardLocation(2, 3), new BoardLocation(2, 3), true);
            }
        }

        public static IEnumerable<TestCaseData> AltEqualsTestCases
        {
            get
            {
                yield return new TestCaseData(new BoardLocation(1, 2), new BoardLocation(2, 3), false);
                yield return new TestCaseData((BoardLocation)null, new BoardLocation(1, 2), false);
                yield return new TestCaseData(new BoardLocation(2, 3), new BoardLocation(2, 3), true);
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
            // Act/Assert
            Assert.That(boardLocationA.Equals(boardLocationB), Is.EqualTo(expectedValue), $"Result does not equal expected {BoardLocation.ToString(boardLocationA)} != {BoardLocation.ToString(boardLocationB)}");            
        }
        
        [TestCaseSource(nameof(AltEqualsTestCases))]
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