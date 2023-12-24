using Xunit;
using System.Collections.Generic;
using System.Linq;
using Alina.Havryniuk.RobotChallange;
using Robot.Common;

namespace Havryniuk.Alina.RobotChallenge.Tests
{
    public class MapHelperTests
    {
        [Fact]
        public void GetPositionPotentialEnergy_Returns_50()
        {
            var map = new Map()
            {
                Stations = new List<EnergyStation>()
                {
                    new EnergyStation()
                    {
                        Position = new Position(0, 0),
                        RecoveryRate = 20
                    },
                    new EnergyStation()
                    {
                        Position = new Position(1, 1),
                        RecoveryRate = 30
                    }
                }
            };
            var p = new Position(2, 2);
            var expected = map.Stations.Sum(x => x.RecoveryRate);
            var result = MapHelper.GetPositionPotentialEnergy(p, map);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateCosts_returns_13()
        {
            var p1 = new Position(0, 0);
            var p2 = new Position(2, 3);
            var expected = 13;
            var actual = MapHelper.CalculateCosts(p1, p2);
            Assert.Equal(expected, actual);
        }

        
        public void GetBestNearestPosition()
        {
            var map = new Map()
            {
                MinPozition = new Position(0, 0),
                MaxPozition = new Position(100, 100),
                Stations = new List<EnergyStation>()
                {
                    new EnergyStation()
                    {
                        Energy = 100,
                        Position = new Position(2, 2),
                        RecoveryRate = 30
                    },
                    new EnergyStation()
                    {
                        Energy = 200,
                        Position = new Position(1, 3),
                        RecoveryRate = 20
                    }
                }
            };
            var myRobot = new Robot.Common.Robot()
            {
                Energy = 50,
                OwnerName = "Havryniuk Alina",
                Position = new Position(0, 0),
            };
            HavryniukAlinaAlgorithm._myRobots = new SortedSet<int>();
            var p = new Position(2, 4);
            MapHelper.ReadMapInfo(map);

            var result = MapHelper.GetBestNearestPosition(myRobot, new List<Robot.Common.Robot>() { myRobot }, map);

            Assert.Equal(p, result);
        }
    }
}
