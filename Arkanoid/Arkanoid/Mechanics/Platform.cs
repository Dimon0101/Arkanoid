using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid.Mechanics
{
    public static class Platform
    {
        public static void MovePlatpose<T>(bool moveLeft, T key)
        {
            lock (GameBase.LockObj)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (GameBase.Platposex + i < GameBase.Area.GetLength(1))
                    {
                        GameBase.Area[GameBase.Platposey, GameBase.Platposex + i] = ' ';
                    }
                }
                if (moveLeft && GameBase.Platposex > 0)
                {
                    GameBase.Platposex--;
                }
                else if (!moveLeft && GameBase.Platposex + 2 < GameBase.Area.GetLength(1) - 1)
                {
                    GameBase.Platposex++;
                }
                for (int i = 0; i < 3; i++)
                {
                    if (GameBase.Platposex + i < GameBase.Area.GetLength(1))
                    {
                        GameBase.Area[GameBase.Platposey, GameBase.Platposex + i] = GameBase.Platform;
                    }
                }
                DrawAndClear.DrawPlatform();
            }
            key = default(T);
        }
        public static void SpawnPlat()
        {
            GameBase.Platposex = GameBase.Area.GetLength(1) / 2;
            GameBase.Platposey = GameBase.Area.GetLength(0) - 1;
            for (int i = GameBase.Platposex - 1; i <= GameBase.Platposex + 1; i++)
            {
                GameBase.Area[GameBase.Platposey, i] = GameBase.Platform;
            }
        }
    }
}
