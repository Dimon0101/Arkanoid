using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid.Mechanics
{
    public static class ControllSettings
    {
        public static void PlatrformController()
        {
            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.F5:
                        InGameSaves.SaveGame();
                        break;
                    case ConsoleKey.A:
                        Platform.MovePlatpose(true, keyInfo);
                        break;
                    case ConsoleKey.LeftArrow:
                        Platform.MovePlatpose(true, keyInfo);
                        break;
                    case ConsoleKey.RightArrow:
                        Platform.MovePlatpose(false, keyInfo);
                        break;
                    case ConsoleKey.D:
                        Platform.MovePlatpose(false, keyInfo);
                        break;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine("8b        d8                                                            88           \r\n Y8,    ,8P                                                             88           \r\n  Y8,  ,8P                                                              88           \r\n   \"8aa8\" ,adPPYba,  88       88    8b,dPPYba,   ,adPPYba,   ,adPPYba,  88,dPPYba,   \r\n    `88' a8\"     \"8a 88       88    88P'   `\"8a a8\"     \"8a a8\"     \"8a 88P'    \"8a  \r\n     88  8b       d8 88       88    88       88 8b       d8 8b       d8 88       d8  \r\n     88  \"8a,   ,a8\" \"8a,   ,a88    88       88 \"8a,   ,a8\" \"8a,   ,a8\" 88b,   ,a8\"  \r\n     88   `\"YbbdP\"'   `\"YbbdP'Y8    88       88  `\"YbbdP\"'   `\"YbbdP\"'  8Y\"Ybbd8\"'   \r\n");
                        Environment.Exit(0);
                        return;
                }
            }
        }
    }
}
