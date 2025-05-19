using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid.Mechanics
{
    public static class SpawnLevels
    {
        public static void SpawnArea1()
        {
            for (int i = 0; i < GameBase.Area.GetLength(0); i++)
            {
                for (int j = 0; j < GameBase.Area.GetLength(1); j++)
                {
                    if (i <= 4)
                    {
                        GameBase.Area[i, j] = GameBase.Block;
                    }
                    else
                    {
                        GameBase.Area[i, j] = ' ';
                    }
                }
            }
            GameBase.Area[GameBase.Ballposey, GameBase.Ballposex] = GameBase.Ball;
            GameBase.Score = 0;
        }
        public static void SpawnArea2()
        {
            for (int i = 0; i < GameBase.Area.GetLength(0); i++)
            {
                for (int j = 0; j < GameBase.Area.GetLength(1); j++)
                {
                    if (i >= 1 && i <= 3 && j >= 5 && j <= 16)
                    {
                        GameBase.Area[i, j] = GameBase.Block;
                    }
                    else
                    {
                        GameBase.Area[i, j] = ' ';
                    }
                }
            }
            GameBase.Area[GameBase.Ballposey, GameBase.Ballposex] = GameBase.Ball;
            GameBase.Score = 0;
        }
        public static void SpawnArea3()
        {
            int per = 0;
            for (int i = 0; i < GameBase.Area.GetLength(0); i++)
            {
                for (int j = 0; j < GameBase.Area.GetLength(1); j++)
                {
                    if (i <= 10 && j >= 0 + per && j < GameBase.Area.GetLength(1) - per)
                    {
                        GameBase.Area[i, j] = GameBase.Block;
                    }
                    else
                    {
                        GameBase.Area[i, j] = ' ';
                    }
                }
            }
            GameBase.Area[GameBase.Ballposey, GameBase.Ballposex] = GameBase.Ball;
            GameBase.Score = 0;
        }
    }
}
