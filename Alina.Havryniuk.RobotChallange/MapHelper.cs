using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Robot.Common;

namespace Alina.Havryniuk.RobotChallange
{
    public static class MapHelper
    {
        private static Map _map;
        private static int _distance = 2;
        public static List<PotentialPositionEnergy> potentialPositionEnergy = new List<PotentialPositionEnergy>();

        public static void ReadMapInfo(Map map)
        {
            _map = map;
            potentialPositionEnergy = GetPositionSignificance();
        }

        private static List<PotentialPositionEnergy> GetPositionSignificance()
        {
            Position position;
            for(var x = _distance; x < _map.MaxPozition.X - _distance; ++x)
            for (var y = _distance; y < _map.MaxPozition.Y - _distance; ++y)
            {
                position = new Position(x, y);
                potentialPositionEnergy.Add(new PotentialPositionEnergy()
                {
                    Position = position,
                    Energy = GetPositionPotentialEnergy(position, _map)
                });
            }
            return potentialPositionEnergy.Where(x => x != null && x.Energy > 0)
                .OrderByDescending(x => x.Energy).ToList();
        }

        public static int GetPositionPotentialEnergy(Position position, Map map)
        {
            return map.GetNearbyResources(position, _distance).Sum(s => s.RecoveryRate);
        }

        public static int CalculateCosts(Position p1, Position p2)
        {
            return Min2D(p1.X, p2.X) + Min2D(p1.Y, p2.Y);
        }
        private static int Min2D(int x1, int x2)
        {
            return new[]
            {
                (int)Math.Pow(x1 - x2, 2.0),
                (int)Math.Pow(x1 - x2 + 100, 2.0),
                (int)Math.Pow(x1 - x2 - 100, 2.0)
            }.Min();
        }

        public static Position GetBestNearestPosition(Robot.Common.Robot movingRobot, IList<Robot.Common.Robot> robots, Map map)
        {
            var positions = GetReachablePositions(movingRobot, robots);
            var currentEnergy = GetPositionPotentialEnergy(movingRobot.Position, map);
/*
            if (positions.Count <= 1 && currentEnergy == 0)
            {
                var list = CommonValuesHelper.PositionsEnergy.Where(p => p.Energy > CommonValuesHelper.PositionsEnergy.First().Energy * 0.85)
                    .OrderBy(p => CommonCalculationsHelper.CalculateLoss(myRobot.Position, p.Position)).ToList();
                return DistanceHelper.GetAllUsefulDestinations(myRobot.Position,
                        list.Take(15).ElementAt(random.Next(Math.Min(15, list.Count))).Position)
                    .Where(p => CommonCalculationsHelper.CalculateLoss(myRobot.Position, p) <= myRobot.Energy / 2).ToList().Last();
            }*/
            if (positions.Count == 2)
            {
                return currentEnergy >= positions[0].Energy ? movingRobot.Position : positions[0].Position;
            }

            var list1 = positions.Where(p =>
                    p.Energy > positions.Max(np => np.Energy) * 0.85 || p.Energy >= 250)
                .ToList();
            list1.Sort((p1, p2) => CalculateCosts(movingRobot.Position, p1.Position)
                .CompareTo(CalculateCosts(movingRobot.Position, p2.Position)));
            return list1.Any(p => p.Position == movingRobot.Position) ? movingRobot.Position : list1.LastOrDefault()?.Position;
        }

        private static List<PotentialPositionEnergy> GetReachablePositions(
            Robot.Common.Robot movingRobot, IList<Robot.Common.Robot> robots)
        {
            List<PotentialPositionEnergy> positions = new List<PotentialPositionEnergy>();
            // k - коефіцієнт енергії, яку робот готовий витратити за хід
            for (var k = 0.5; k <= 1; k += 0.1)
            {
                var energyPos = potentialPositionEnergy;
                positions = energyPos.Where(p =>
                    MapHelper.CalculateCosts(movingRobot.Position, p.Position) < movingRobot.Energy * k
                    && !HavryniukAlinaAlgorithm._myRobots.Where(x => robots[x].Position != movingRobot.Position)
                        .Any(x => DistanceHelper.IsCollision(robots[x].Position, p.Position))).ToList();
                if (positions.Any())
                    return positions;
            }
            return positions;
        }
    }
}
