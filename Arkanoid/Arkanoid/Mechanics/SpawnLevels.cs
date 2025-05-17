using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid.Mechanics
{
    public class SpawnLevels : GameBase
    {
        public void SpawnArea1()
        {
            for (int i = 0; i < Area.GetLength(0); i++)
            {
                for (int j = 0; j < Area.GetLength(1); j++)
                {
                    if (i <= 4)
                    {
                        Area[i, j] = Block;
                    }
                    else
                    {
                        Area[i, j] = ' ';
                    }
                }
            }
            Area[Ballposey, Ballposex] = Ball;
            Score = 0;
        }
        public void SpawnArea2()
        {
            for (int i = 0; i < Area.GetLength(0); i++)
            {
                for (int j = 0; j < Area.GetLength(1); j++)
                {
                    if (i >= 1 && i <= 3 && j >= 5 && j <= 16)
                    {
                        Area[i, j] = Block;
                    }
                    else
                    {
                        Area[i, j] = ' ';
                    }
                }
            }
            Area[Ballposey, Ballposex] = Ball;
            Score = 0;
        }
        public void SpawnArea3()
        {
            int per = 0;
            for (int i = 0; i < Area.GetLength(0); i++)
            {
                for (int j = 0; j < Area.GetLength(1); j++)
                {
                    if (i <= 10 && j >= 0 + per && j < Area.GetLength(1) - per)
                    {
                        Area[i, j] = Block;
                    }
                    else
                    {
                        Area[i, j] = ' ';
                    }
                }
            }
            Area[Ballposey, Ballposex] = Ball;
            Score = 0;
        }
    }
}
