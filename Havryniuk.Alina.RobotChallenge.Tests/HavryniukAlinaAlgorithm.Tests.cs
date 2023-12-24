using Xunit;
using System.Collections.Generic;
using System.Linq;
using Alina.Havryniuk.RobotChallange;
using Robot.Common;


namespace Havryniuk.Alina.RobotChallenge.Tests
{
    public class HavryniukAlinaAlgorithmTests
    {
        [Fact]
        public void GetAttackProfit_returns_1()
        {
            var algo = new HavryniukAlinaAlgorithm();
            var movingRobot = new Robot.Common.Robot()
            {
                Energy = 200,
                Position = new Position(0, 0)
            };
            var enemy = new Robot.Common.Robot()
            {
                Energy = 400,
                Position = new Position(3, 0)
            };
            var expected = enemy.Energy * 0.1 - 9 - 30;
            var result = algo.GetAttackProfit(movingRobot, enemy);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetAttackProfit_returns_ZeroWhenSameRobot()
        {
            var algo = new HavryniukAlinaAlgorithm();
            var robot = new Robot.Common.Robot()
            {
                Energy = 200,
                Position = new Position(0, 0)
            };
            var result = algo.GetAttackProfit(robot, robot);

            Assert.Equal(0, result);
        }

        [Fact]
        public void ReadMapInfo_WhenNoMyRobots()
        {
            var algo = new HavryniukAlinaAlgorithm();
            var map = new Map()
            {
                MinPozition = new Position(0, 0),
                MaxPozition = new Position(99, 99),
                Stations = new[]
                {
                    new EnergyStation()
                    {
                        Energy = 100,
                        Position = new Position(4, 4),
                        RecoveryRate = 25
                    }
                }
            };
            var robots = new List<Robot.Common.Robot>()
            {
                new Robot.Common.Robot()
                {
                    Energy = 50,
                    Position = new Position(0, 0),
                }
            };
            algo.DoStep(robots, 0, map);
            Assert.True(MapHelper.potentialPositionEnergy.Any());
        }

        [Fact]
        public void DoStep_KicksProfitableRobot_WhenRoundIsMidAndAllRobotsAreCreated()
        {
            var algo = new HavryniukAlinaAlgorithm();
            var map = new Map()
            {
                MinPozition = new Position(0, 0),
                MaxPozition = new Position(100, 100),
            };
            var robots = new List<Robot.Common.Robot>();
            for (int i = 0; i < 100; ++i)
            {
                robots.Add(new Robot.Common.Robot()
                {
                    Energy = 200,
                    OwnerName = "Havryniuk Alina",
                    Position = new Position(i, 0)
                });
            }
            HavryniukAlinaAlgorithm._myRobotsAmount = robots.Count;
            HavryniukAlinaAlgorithm._numOfRound = 31;
            robots.Add(new Robot.Common.Robot()
            {
                Energy = 1000,
                OwnerName = "Test Owner",
                Position = new Position(0, 2),
            });
            MapHelper.ReadMapInfo(map);

            var result = algo.DoStep(robots, 0, map);

            Assert.IsType<MoveCommand>(result);
            Assert.Equal(robots.LastOrDefault().Position, ((MoveCommand)result).NewPosition);
        }
        [Fact]
        public void CollectsEnergy_WhenNoEnemies()
        {
            var algo = new HavryniukAlinaAlgorithm();
            var map = new Map()
            {
                MinPozition = new Position(0, 0),
                MaxPozition = new Position(99, 99),
                Stations = new List<EnergyStation>()
                {
                    new EnergyStation()
                    {
                        Energy = 100,
                        Position = new Position(0, 0),
                        RecoveryRate = 25
                    }
                }
            };
            var robots = new List<Robot.Common.Robot>();
            for (int i = 0; i < 100; ++i)
            {
                robots.Add(new Robot.Common.Robot()
                {
                    Energy = 300,
                    OwnerName = "Havryniuk Alina",
                    Position = new Position(i, 0)
                });
            }

            HavryniukAlinaAlgorithm._myRobotsAmount = robots.Count;
            HavryniukAlinaAlgorithm._numOfRound = 31;
            MapHelper.ReadMapInfo(map);

            var result = algo.DoStep(robots, 0, map);

            Assert.IsType<CollectEnergyCommand>(result);
        }

        [Fact]
        public void DoStep_AttacksVolunteer_WhenRoundIsMidAndNoEnemiesToAttack()
        {
            var algo = new HavryniukAlinaAlgorithm();
            var map = new Map()
            {
                MinPozition = new Position(0, 0),
                MaxPozition = new Position(100, 100),
            };
            var robots = new List<Robot.Common.Robot>();
            for (int i = 0; i < 100; ++i)
            {
                robots.Add(new Robot.Common.Robot()
                {
                    Energy = 400,
                    OwnerName = "Havryniuk Alina",
                    Position = new Position(i, 0)
                });
            }
            HavryniukAlinaAlgorithm._numOfRound = 31;
            HavryniukAlinaAlgorithm._myRobotsAmount = robots.Count;
            MapHelper.ReadMapInfo(map);

            var result = algo.DoStep(robots, 0, map);

            Assert.IsType<MoveCommand>(result);
            Assert.Equal(robots.Skip(1).FirstOrDefault().Position, ((MoveCommand)result).NewPosition);
        }
    }
}
