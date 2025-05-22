using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Arkanoid.Mechanics
{
    public static class DrawAndClear
    {
        public static void ShowArea()
        {
            lock (GameBase.LockObj)
            {
                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < GameBase.Area.GetLength(0); i++)
                {
                    for (int j = 0; j < GameBase.Area.GetLength(1); j++)
                    {
                        Console.Write(GameBase.Area[i, j] + " ");
                    }
                    Console.Write("\n");
                }
                Console.WriteLine("Your score is: " + GameBase.Score);
                Console.WriteLine("Saves on F5");
            }
        }
        public static void ClearBall()
        {
            Console.SetCursorPosition(GameBase.Ballposex * 2, GameBase.Ballposey);
            Console.Write(' ');
        }
        public static void Drawball()
        {
            Console.SetCursorPosition(GameBase.Ballposex * 2, GameBase.Ballposey);
            Console.Write($"{GameBase.Ball}");
        }
        public static void DrawPlatform()
        {
            for (int i = 0; i < GameBase.Area.GetLength(1); i++)
            {
                Console.SetCursorPosition(i * 2, GameBase.Area.GetLength(0) - 1);
                Console.Write(GameBase.Area[GameBase.Area.GetLength(0) - 1, i]);
            }
        }
    }
}
