using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot.Common;

namespace Alina.Havryniuk.RobotChallange
{
    
    public static class DistanceHelper
    {

        private static readonly int _distance = 2;
        // перелік всіх клітинок які перетне вектор двох позицій
/*        public static List<Position> GetOptimalVector(Position p1, Position p2)
        {
            var destinations = new List<Position>();
            var maxX = Math.Max(p1.X, p2.X);
            for (var x = Math.Min(p1.X, p2.X) + 1; x <= maxX; ++x)
            {
                var y = (x - p1.X) / (p2.X - p1.X) * (p2.Y - p1.Y) + p1.Y;
                destinations.Add(new Position(x, y));
            }

            return destinations;
        }*/


        // чи може одна позиція стягнути енергію з другої позиції (перевірка чи дістає)
        public static bool IsCollision(Position p1, Position p2)
        {
            var x = Math.Abs(p1.X - p2.X);
            var y = Math.Abs(p1.Y - p2.Y);
            return x <= _distance && y <= _distance;
        }
    }
}
