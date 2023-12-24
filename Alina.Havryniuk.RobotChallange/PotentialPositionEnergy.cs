using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robot.Common;

namespace Alina.Havryniuk.RobotChallange
{
    public class PotentialPositionEnergy
    {
        public Position Position { get; set; } = new Position(0, 0);
        public int Energy { get; set; }
    }
}
