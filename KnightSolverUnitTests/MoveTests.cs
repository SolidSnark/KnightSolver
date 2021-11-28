using NUnit.Framework;

using System;
using System.Collections.Generic;

using KnightMazeSolver;

namespace KnightSolverUnitTests
{
    public class MoveTests
    {
        private Move _tempMove;

        public static IEnumerable<TestCaseData> EqualsTestCases
        {
            get
            {
                yield return new TestCaseData(new Move(new BoardLocation(1, 2), new BoardLocation(2, 3)),
                                new Move(new BoardLocation(1, 2), new BoardLocation(2, 4)), false);
                yield return new TestCaseData(new Move(new BoardLocation(1, 2), new BoardLocation(2, 3)),
                                               new Move(new BoardLocation(1, 4), new BoardLocation(2, 3)), false);
                yield return new TestCaseData(new Move(new BoardLocation(1, 2), new BoardLocation(2, 3)), null, false);
                yield return new TestCaseData(null, new Move(new BoardLocation(1, 2), new BoardLocation(2, 3)), false);
                yield return new TestCaseData(new Move(new BoardLocation(1, 2), new BoardLocation(2, 3)),
                                                new Move(new BoardLocation(1, 2), new BoardLocation(2, 3)),
                                                true);
                yield return new TestCaseData(null, null, true);
            }
        }

        public static IEnumerable<TestCaseData> NullTestCases
        {
            get
            {
                yield return new TestCaseData(new BoardLocation(1, 2), null);
                yield return new TestCaseData(null, new BoardLocation(1, 2));
                yield return new TestCaseData(null, null);
            }
        }

        [OneTimeSetUp]
        public void Init()
        {
            _tempMove = new Move(new BoardLocation(1, 2), new BoardLocation(1, 2));
        }

        [SetUp]
        public void Setup()
        {
            
        }

        [TestCaseSource(nameof(NullTestCases))]
        public void Move_Move(BoardLocation startingLocation, BoardLocation endingLocation)
        {
            Assert.Throws<ArgumentNullException>(() => new Move(startingLocation, endingLocation));
        }

        [TestCaseSource(nameof(EqualsTestCases))]
        public void Move_Equals(Move moveA, Move moveB, bool expectedResult)
        {
            // Act/Assert
            Assert.IsTrue(_tempMove.Equals(moveA, moveB) == expectedResult, $"Result does not equal expected ({Move.ToString(moveA)} {(expectedResult ? "!=" : "==")} {Move.ToString(moveB)})");
        }

        [TestCaseSource(nameof(EqualsTestCases))]
        public void Move_AltEquals(Move moveA, Move moveB, bool expectedResult)
        {
            if (moveA == null)
            {
                return;
            }

            // Act/Assert
            Assert.IsTrue(moveA.Equals(moveB) == expectedResult, $"Result does not equal expected ({Move.ToString(moveA)} {(expectedResult ? "!=" : "==")} {Move.ToString(moveB)})");
        }

        [Test]
        public void Move_GetHashCode_DoesNotThrow()
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