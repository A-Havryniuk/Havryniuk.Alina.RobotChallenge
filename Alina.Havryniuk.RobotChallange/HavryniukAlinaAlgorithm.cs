using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Robot.Common;

namespace Alina.Havryniuk.RobotChallange
{
    public class HavryniukAlinaAlgorithm : IRobotAlgorithm
    {
        public static int _numOfRound = 0;
        public static int _myRobotsAmount = 10;
        private static readonly int Distance = 2;
        private static readonly int RoundToAttack = 30;
        private static readonly int MaxRobotAmount = 100;
        private static readonly int StopVolunteeringProgram = 45;
        private static readonly int RunMitosis = 200;
        private static readonly int MaxRobotNumberInGroup = 5;
        public static SortedSet<int> _myRobots;

        public RobotCommand DoStep(IList<Robot.Common.Robot> robots, int robotToMoveIndex, Map map)
        {
            if(_myRobots.Count == 0)
                MapHelper.ReadMapInfo(map);
            _myRobots.Add(robotToMoveIndex);
            var movingRobot = robots[robotToMoveIndex];
            var summaryEnergy = robots.Where(r => r.OwnerName == robots[robotToMoveIndex].OwnerName).Sum(r => r.Energy);
            if (summaryEnergy > 350000)
            {
                return new MoveCommand()
                {
                    NewPosition = movingRobot.Position
                };
            }
            var currentStationEnergy = 
                map.GetNearbyResources(movingRobot.Position, Distance)
                    .Select(s => s.Energy).Sum();

            var robotsToAttack = robots.Where(r => GetAttackProfit(movingRobot, r) > 0 && r.OwnerName != movingRobot.OwnerName)
                .OrderByDescending(r1 => GetAttackProfit(movingRobot, r1)).ToList();
            var robotsToPotentialAttack = robots.Where(r => GetAttackProfit(movingRobot, r) > -100 && r.OwnerName != movingRobot.OwnerName)
                .OrderByDescending(r1 => GetAttackProfit(movingRobot, r1)).ToList();
            var myRobotsToPunch = robots.Where(r => GetAttackProfit(movingRobot, r) > 0 && r.OwnerName == movingRobot.OwnerName)
                .OrderByDescending(r1 => GetAttackProfit(movingRobot, r1)).ToList();
            var victim = robotsToAttack.FirstOrDefault();
            if (_numOfRound > RoundToAttack && _myRobotsAmount == MaxRobotAmount)
            {
                if (victim != null && GetAttackProfit(movingRobot, victim) >= currentStationEnergy)
                    return new MoveCommand()
                    {
                        NewPosition = victim.Position
                    };
                if (currentStationEnergy > 0)
                    return new CollectEnergyCommand();
                var volunteer = myRobotsToPunch.FirstOrDefault();
                if (volunteer != null && _numOfRound < StopVolunteeringProgram)
                    return new MoveCommand()
                    {
                        NewPosition = volunteer.Position
                    };
                if (robotsToPotentialAttack.Any())
                    return new MoveCommand() { NewPosition = robotsToPotentialAttack.First().Position };
            }

            if (movingRobot.Energy > RunMitosis 
                && _myRobotsAmount < MaxRobotAmount 
                && _myRobots.Count(x => x != robotToMoveIndex
                   && DistanceHelper.IsCollision(
                      robots[x].Position, movingRobot.Position)) < MaxRobotNumberInGroup)
            {
                ++_myRobotsAmount;
                return new CreateNewRobotCommand();
            }

            var bestNearestPosition = MapHelper.GetBestNearestPosition(movingRobot, robots, map);
            if (bestNearestPosition is null)
                return new CollectEnergyCommand();
            if (bestNearestPosition != movingRobot.Position && currentStationEnergy < 100 && _numOfRound < RoundToAttack)
                return new MoveCommand { NewPosition = bestNearestPosition };
            if (currentStationEnergy == 0)
            {
                if (robots.Where(r => r.OwnerName == movingRobot.OwnerName).Select(r => r.Energy).Sum() > 16000 &&
                    myRobotsToPunch.Any())
                    return new MoveCommand { NewPosition = myRobotsToPunch.First().Position };
                if (robots.Where(r => r.OwnerName == movingRobot.OwnerName).Select(r => r.Energy).Sum() > 16000 && myRobotsToPunch.Any())
                    return new MoveCommand { NewPosition = robotsToPotentialAttack.First().Position };
            }

            return new CollectEnergyCommand();
        }

        public int GetAttackProfit(Robot.Common.Robot movingRobot, Robot.Common.Robot enemy)
        {
            if (movingRobot == enemy)
            {
                return 0;
            }
            var costs = MapHelper.CalculateCosts(movingRobot.Position, enemy.Position) + 30;
            var profit = enemy.Energy * 0.1;
            return costs > movingRobot.Energy ? int.MinValue : (int)profit - costs;
        }

        public string Author => "Havryniuk Alina";

        public HavryniukAlinaAlgorithm()
        {
            _myRobots = new SortedSet<int>();
            Logger.OnLogRound += (sender, args) => ++_numOfRound;
        }
    }
}
