using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Arkanoid;
using static System.Formats.Asn1.AsnWriter;

namespace Arkanoid.Mechanics
{
    public static class Ball
    {
        public static void SpawnBall()
        {
            GameBase.Ballposex = GameBase.Area.GetLength(1) / 2;
            GameBase.Ballposey = GameBase.Area.GetLength(0) - 2;
        }

        public static void RicohetCheck(ref int Nexty, ref int Nextx)
        {
            if (GameBase.Area[Nexty, GameBase.Ballposex] == GameBase.Block)
            {
                if (GameBase.Area[Nexty, Nextx] == GameBase.Block)
                {
                    Console.SetCursorPosition(Nextx * 2, Nexty);
                    Console.Write(' ');
                    GameBase.Area[Nexty, Nextx] = ' ';
                    GameBase.Score += 500;
                }
                Console.SetCursorPosition(Nextx * 2, Nexty);
                Console.Write(' ');
                GameBase.Area[Nexty, GameBase.Ballposex] = ' ';
                GameBase.Movey = -GameBase.Movey;
                Nexty += GameBase.Movey;
                Nextx += GameBase.Movex;
                GameBase.Score += 500;
            }
            if (GameBase.Area[GameBase.Ballposey, Nextx] == GameBase.Block)
            {
                Console.SetCursorPosition(Nextx * 2, GameBase.Ballposey);
                Console.Write(' ');
                GameBase.Area[GameBase.Ballposey, Nextx] = ' ';
                GameBase.Movex = -GameBase.Movex;
                Nexty += GameBase.Movey;
                Nextx += GameBase.Movex;
                GameBase.Score += 500;
            }
            if (GameBase.Area[Nexty, Nextx] == GameBase.Platform)
            {
                GameBase.Movey = -GameBase.Movey;
                Nexty += GameBase.Movey;
                Nextx += GameBase.Movex;
            }
        }
        public static void BlockChecks(ref int Nexty, ref int Nextx)
        {
            if (GameBase.Area[Nexty, GameBase.Ballposex] == GameBase.Block && GameBase.Area[Nexty, Nextx] == GameBase.Block && GameBase.Area[GameBase.Ballposey, Nextx] == GameBase.Block)
            {

                GameBase.Area[Nexty, GameBase.Ballposex] = ' ';
                GameBase.Area[Nexty, Nextx] = ' ';
                GameBase.Area[GameBase.Ballposey, Nextx] = ' ';
                Console.SetCursorPosition(Nextx * 2, Nexty);
                Console.Write(' ');
                Console.SetCursorPosition(GameBase.Ballposex * 2, Nexty);
                Console.Write(' ');
                Console.SetCursorPosition(Nextx * 2, GameBase.Ballposey);
                Console.Write(' ');
                GameBase.Score += 1500;
                GameBase.Movex = -GameBase.Movex;
                GameBase.Movey = -GameBase.Movey;
                Nextx = GameBase.Ballposex;
                Nexty = GameBase.Ballposey;
            }
            else
            {
                if (GameBase.Area[Nexty, GameBase.Ballposex] == GameBase.Block)
                {
                    Console.SetCursorPosition(GameBase.Ballposex * 2, Nexty);
                    Console.Write(' ');
                    GameBase.Score += 500;
                    GameBase.Area[Nexty, GameBase.Ballposex] = ' ';
                    GameBase.Movey = -GameBase.Movey;
                    Nexty += GameBase.Movey;
                }
                if (GameBase.Area[Nexty, Nextx] == GameBase.Block)
                {
                    Console.SetCursorPosition(Nextx * 2, Nexty);
                    Console.Write(' ');
                    GameBase.Score += 500;
                    GameBase.Area[Nexty, Nextx] = ' ';
                    GameBase.Movex = -GameBase.Movex;
                    Nextx += GameBase.Movex;
                }
                if (GameBase.Area[GameBase.Ballposey, Nextx] == GameBase.Block)
                {
                    Console.SetCursorPosition(Nextx * 2, GameBase.Ballposey);
                    Console.Write(' ');
                    GameBase.Score += 500;
                    GameBase.Area[GameBase.Ballposey, Nextx] = ' ';
                    GameBase.Movex = -GameBase.Movex;
                    Nextx += GameBase.Movex;
                }
            }
        }
        public static void MoveBall()
        {
            while (true)
            {
                lock (GameBase.LockObj)
                {
                    DrawAndClear.ClearBall();
                    GameBase.Area[GameBase.Ballposey, GameBase.Ballposex] = ' ';
                    int Nexty = GameBase.Ballposey + GameBase.Movey;
                    int Nextx = GameBase.Ballposex + GameBase.Movex;
                    if (Nexty == GameBase.Area.GetLength(0))
                    {
                        Console.Clear();
                        Console.WriteLine("8b        d8                        88                                   \r\n Y8,    ,8P                         88                                   \r\n  Y8,  ,8P                          88                                   \r\n   \"8aa8\" ,adPPYba,  88       88    88  ,adPPYba,  ,adPPYba,  ,adPPYba,  \r\n    `88' a8\"     \"8a 88       88    88 a8\"     \"8a I8[    \"\" a8P_____88  \r\n     88  8b       d8 88       88    88 8b       d8  `\"Y8ba,  8PP\"\"\"\"\"\"\"  \r\n     88  \"8a,   ,a8\" \"8a,   ,a88    88 \"8a,   ,a8\" aa    ]8I \"8b,   ,aa  \r\n     88   `\"YbbdP\"'   `\"YbbdP'Y8    88  `\"YbbdP\"'  `\"YbbdP\"'  `\"Ybbd8\"'  \r\n");
                        Environment.Exit(0);
                    }
                    else if ((Nextx < 0 || Nextx > GameBase.Area.GetLength(1) - 1) && (Nexty < 0 || Nexty > GameBase.Area.GetLength(0) - 1))
                    {
                        GameBase.Movex = -GameBase.Movex;
                        GameBase.Movey = -GameBase.Movey;
                        Nexty += GameBase.Movey;
                        Nextx += GameBase.Movex;
                        RicohetCheck(ref Nexty, ref Nextx);
                    }
                    else if ((Nextx < 0 || Nextx > GameBase.Area.GetLength(1) - 1))
                    {
                        GameBase.Movex = -GameBase.Movex;
                        Nextx += GameBase.Movex;
                        RicohetCheck(ref Nexty, ref Nextx);
                    }
                    else if (Nexty < 0)
                    {
                        GameBase.Movey = -GameBase.Movey;
                        Nexty += GameBase.Movey;
                        RicohetCheck(ref Nexty, ref Nextx);
                    }
                    else if (GameBase.Area[Nexty, GameBase.Ballposex] == GameBase.Platform)
                    {
                        GameBase.Movey = -GameBase.Movey;
                        Nexty += 2 * GameBase.Movey;
                    }
                    else if (GameBase.Area[GameBase.Ballposey, Nextx] == GameBase.Platform)
                    {
                        GameBase.Movex = -GameBase.Movex;
                        Nextx += 2 * GameBase.Movex;
                    }
                    else if (GameBase.Area[Nexty, GameBase.Ballposex] == GameBase.Block || GameBase.Area[Nexty, Nextx] == GameBase.Block || GameBase.Area[GameBase.Ballposey, Nextx] == GameBase.Block)
                    {
                        BlockChecks(ref Nexty, ref Nextx);
                    }
                    GameBase.Ballposex = Nextx;
                    GameBase.Ballposey = Nexty;
                    GameBase.Area[GameBase.Ballposey, GameBase.Ballposex] = GameBase.Ball;
                    DrawAndClear.Drawball();
                    Console.SetCursorPosition(0, GameBase.Area.GetLength(0));
                    Console.Write("                                                                ");
                    Console.SetCursorPosition(0, GameBase.Area.GetLength(0));
                    Console.Write("Your score is : " + GameBase.Score);
                }
                bool IsWin = CheckWin();
                if (IsWin)
                {
                    if (!GameBase.Level1Completed)
                    {
                        GameBase.Level1Completed = true;
                        Console.Clear();
                        Console.WriteLine("Level 1 Complete:");
                        Console.WriteLine("Open game again and start game, there will be new level");
                        LevelsCompletedSaves.SaveLevelComplete();
                        Environment.Exit(0);
                    }
                    else if (!GameBase.Level2Completed)
                    {
                        GameBase.Level2Completed = true;
                        Console.Clear();
                        Console.WriteLine("Level 2 Complete:");
                        Console.WriteLine("Open game again and start game, there will be new level");
                        LevelsCompletedSaves.SaveLevelComplete();
                        Environment.Exit(0);
                    }
                    else if (!GameBase.Level3Completed)
                    {
                        GameBase.Level3Completed = true;
                        Console.Clear();
                        Console.WriteLine("Level 3 Complete:");
                        Console.WriteLine("Open game again and see, what will happen");
                        LevelsCompletedSaves.SaveLevelComplete();
                        Environment.Exit(0);
                    }
                }

                if (GameBase.Difficulty == 1)
                {
                    Thread.Sleep(500);
                }
                else if (GameBase.Difficulty == 2)
                {
                    Thread.Sleep(250);
                }
                else if (GameBase.Difficulty == 3)
                {
                    Thread.Sleep(100);
                }
            }
        }
        public static bool CheckWin()
        {
            for (int i = 0; i < GameBase.Area.GetLength(0); i++)
            {
                for (int j = 0; j < GameBase.Area.GetLength(1); j++)
                {
                    if(GameBase.Area[i, j] == GameBase.Block)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
