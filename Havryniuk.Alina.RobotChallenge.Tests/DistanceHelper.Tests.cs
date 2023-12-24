using Xunit;
using System.Collections.Generic;
using Alina.Havryniuk.RobotChallange;
using Robot.Common;

namespace Havryniuk.Alina.RobotChallenge.Tests
{

    public class DistanceHelperTests
    {
/*        [Fact]
        public void GetOptimalVector_listOfPoints()
        {
            // Arrange
            var position1 = new Position(0, 0);
            var position2 = new Position(2, 3);
            var expected = new List<Position>()
            {
                new Position(1,0),
                new Position(2,3)
            };
            //Act
            var result = DistanceHelper.GetOptimalVector(position1, position2);
            //Assert
            Assert.Equal(expected, result);
        }*/
        [Fact]

        public void IsCollision_false()
        {
            var position1 = new Position(0, 0);
            var position2 = new Position(3, 2);
            var result = DistanceHelper.IsCollision(position1, position2);

            Assert.False(result);
        }
        [Fact]
        public void IsCollision_true()
        {
            var position1 = new Position(0, 0);
            var position2 = new Position(1, 2);
            var result = DistanceHelper.IsCollision(position1, position2);

            Assert.True(result);
        }
    }
}