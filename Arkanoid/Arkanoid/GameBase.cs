using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace Arkanoid
{
    
    
    public class GameBase
    {
        public char[,] Area = new char[30,21];
        public char Block = '■';
        public char Platform = '▀';
        public char Ball = '●';
        public int Ballposey = 0;
        public int Ballposex = 0;
        public int Platposey = 0;
        public int Platposex = 0;
        public int Movex = -1;
        public int Movey = -1;
        public int Difficulty = 1;
        public object LockObj = new object();
        public string SavePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Saves", "save.json");
        public string LevelPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Levels", "levels.json");
        public bool LoadedGame = false;
        public int Score = 0;
        public int ScorePerBlock = 500;
        public bool Level1Completed = true;
        public bool Level2Completed = false;
        public bool Level3Completed = false;
        public class SaveData
        {
            public List<string> SavedArea { get; set; }
            public int BallX { get; set; }
            public int BallY { get; set; }
            public int PlatX { get; set; }
            public int PlatY { get; set; }
            public int Difficulty { get; set; }
            public int movey { get; set; }
            public int movex { get; set; }
            public int score { get; set; }
        }
        public class CompletedLevelSaves
        {
            public bool Level1Complete { get; set; }
            public bool Level2Complete { get; set; }
            public bool Level3Complete { get; set; }
        }
        void SpawnPlat()
        {
            Platposex = Area.GetLength(1) / 2;
            Platposey = Area.GetLength(0) - 1;
            for (int i = Platposex-1; i <= Platposex+1; i++)
            {
                Area[Platposey, i] = Platform;
            }
        }
        

        void ShowArea()
        {
            lock (LockObj)
            {
                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < Area.GetLength(0); i++)
                {
                    for (int j = 0; j < Area.GetLength(1); j++)
                    {
                        Console.Write(Area[i,j] +" ");
                    }
                    Console.Write("\n");
                }
                Console.WriteLine("Your score is: " + Score);
                    Console.WriteLine("Saves on F5");
            }
        }
        void ClearBall()
        {
            Console.SetCursorPosition(Ballposex*2, Ballposey);
            Console.Write(' ');
        }
        void Drawball()
        {
                Console.SetCursorPosition(Ballposex * 2, Ballposey);
                Console.Write($"{Ball}");
        }
        void DrawPlatform()
        {
            for (int i = 0; i < Area.GetLength(1); i++)
            {
                Console.SetCursorPosition(i * 2, Area.GetLength(0) - 1);
                Console.Write(Area[Area.GetLength(0) - 1, i]);
            }
        }
        void RicohetCheck(ref int Nexty, ref int Nextx)
        {
            if (Area[Nexty, Ballposex] == Block)
            {
                if(Area[Nexty, Nextx] == Block)
                {
                    Console.SetCursorPosition(Nextx * 2, Nexty);
                    Console.Write(' ');
                    Area[Nexty, Nextx] = ' ';
                    Score += 500;
                }
                Console.SetCursorPosition(Nextx * 2, Nexty);
                Console.Write(' ');
                Area[Nexty, Ballposex] = ' ';
                Movey = -Movey;
                Nexty += Movey;
                Nextx += Movex;
                Score += 500;
            }
            if(Area[Ballposey, Nextx] == Block)
            {
                Console.SetCursorPosition(Nextx * 2, Ballposey);
                Console.Write(' ');
                Area[Ballposey, Nextx] = ' ';
                Movex = -Movex;
                Nexty += Movey;
                Nextx += Movex;
                Score += 500;
            }
            if (Area[Nexty, Nextx] == Platform)
            {
                Movey = -Movey;
                Nexty += Movey;
                Nextx += Movex;
            }
        }
        void BlockChecks(ref int Nexty, ref int Nextx)
        {
            if (Area[Nexty, Ballposex] == Block && Area[Nexty, Nextx] == Block && Area[Ballposey, Nextx] == Block)
            {

                Area[Nexty, Ballposex] = ' ';
                Area[Nexty, Nextx] = ' ';
                Area[Ballposey, Nextx] = ' ';
                Console.SetCursorPosition(Nextx * 2, Nexty);
                Console.Write(' ');
                Console.SetCursorPosition(Ballposex * 2, Nexty);
                Console.Write(' ');
                Console.SetCursorPosition(Nextx * 2, Ballposey);
                Console.Write(' ');
                Score += 1500;
                Movex = -Movex;
                Movey = -Movey;
                Nextx = Ballposex;
                Nexty = Ballposey;
            }
            else
            {
                if (Area[Nexty, Ballposex] == Block)
                {
                    Console.SetCursorPosition(Ballposex * 2, Nexty);
                    Console.Write(' ');
                    Score += 500;
                    Area[Nexty, Ballposex] = ' ';
                    Movey = -Movey;
                    Nexty += Movey;
                }
                if (Area[Nexty, Nextx] == Block)
                {
                    Console.SetCursorPosition(Nextx * 2, Nexty);
                    Console.Write(' ');
                    Score += 500;
                    Area[Nexty, Nextx] = ' ';
                    Movex = -Movex;
                    Nextx += Movex;
                }
                if (Area[Ballposey, Nextx] == Block)
                {
                    Console.SetCursorPosition(Nextx * 2, Ballposey);
                    Console.Write(' ');
                    Score += 500;
                    Area[Ballposey, Nextx] = ' ';
                    Movex = -Movex;
                    Nextx += Movex;
                }
            }
        }
        bool CheckWin()
        {
            for(int i = 0;i < Area.GetLength(0); i++)
            {
                for(int j = 0; j < Area.GetLength(1);j++)
                {
                    if (Area[i,j] == Block)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        void MoveBall()
        {
            while (true)
            {
                lock (LockObj)
                {
                    ClearBall();
                    Area[Ballposey, Ballposex] = ' ';
                    int Nexty = Ballposey + Movey;
                    int Nextx = Ballposex + Movex;
                    if (Nexty == Area.GetLength(0))
                    {
                        Console.Clear();
                        Console.WriteLine("8b        d8                        88                                   \r\n Y8,    ,8P                         88                                   \r\n  Y8,  ,8P                          88                                   \r\n   \"8aa8\" ,adPPYba,  88       88    88  ,adPPYba,  ,adPPYba,  ,adPPYba,  \r\n    `88' a8\"     \"8a 88       88    88 a8\"     \"8a I8[    \"\" a8P_____88  \r\n     88  8b       d8 88       88    88 8b       d8  `\"Y8ba,  8PP\"\"\"\"\"\"\"  \r\n     88  \"8a,   ,a8\" \"8a,   ,a88    88 \"8a,   ,a8\" aa    ]8I \"8b,   ,aa  \r\n     88   `\"YbbdP\"'   `\"YbbdP'Y8    88  `\"YbbdP\"'  `\"YbbdP\"'  `\"Ybbd8\"'  \r\n");
                        Environment.Exit(0);
                    }
                    else if ((Nextx < 0 || Nextx > Area.GetLength(1) - 1) && (Nexty < 0 || Nexty > Area.GetLength(0)-1))
                    {
                        Movex = -Movex;
                        Movey = -Movey;
                        Nexty += Movey;
                        Nextx += Movex;
                        RicohetCheck(ref Nexty,ref Nextx);
                    }
                    else if((Nextx < 0 || Nextx > Area.GetLength(1) - 1))
                    {
                        Movex = -Movex;
                        Nextx += Movex;
                        RicohetCheck(ref Nexty, ref Nextx);
                    }
                    else if (Nexty < 0)
                    {
                        Movey = -Movey;
                        Nexty += Movey;
                        RicohetCheck(ref Nexty, ref Nextx);
                    }
                    else if (Area[Nexty,Ballposex] == Platform)
                    {
                        Movey = - Movey;
                        Nexty += 2*Movey;
                    }
                    else if (Area[Ballposey, Nextx] == Platform)
                    {
                        Movex = -Movex;
                        Nextx += 2 * Movex;
                    }
                    else if (Area[Nexty, Ballposex] == Block || Area[Nexty, Nextx] == Block || Area[Ballposey, Nextx] == Block)
                    {
                        BlockChecks(ref Nexty, ref Nextx);
                    }
                    Ballposex = Nextx;
                    Ballposey = Nexty;
                    Area[Ballposey, Ballposex] = Ball;
                    Drawball();
                    Console.SetCursorPosition(0, Area.GetLength(0));
                    Console.Write("                                                                ");
                    Console.SetCursorPosition(0, Area.GetLength(0));
                    Console.Write("Your score is : " + Score);
                }
                bool IsWin = CheckWin();
                if (IsWin)
                {
                    if (!Level1Completed)
                    {
                        Level1Completed = true;
                        Console.Clear();
                        Console.WriteLine("Level 1 Complete:");
                        Console.WriteLine("Open game again and start game, there will be new level");
                        SaveLevelComplete();
                        Environment.Exit(0);
                    }
                    else if (!Level2Completed)
                    {
                        Level2Completed = true;
                        Console.Clear();
                        Console.WriteLine("Level 2 Complete:");
                        Console.WriteLine("Open game again and start game, there will be new level");
                        SaveLevelComplete();
                        Environment.Exit(0);
                    }
                    else if (!Level3Completed)
                    {
                        Level3Completed = true;
                        Console.Clear();
                        Console.WriteLine("Level 3 Complete:");
                        Console.WriteLine("Open game again and see, what will happen");
                        SaveLevelComplete();
                        Environment.Exit(0);
                    }
                }

                if (Difficulty == 1)
                {
                    Thread.Sleep(500);
                }
                else if (Difficulty == 2)
                {
                    Thread.Sleep(250);
                }
                else if (Difficulty == 3)
                {
                    Thread.Sleep(10);
                }
            }
        }
        void PlatrformController()
        {
            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.F5:
                        SaveGame();
                        break;
                    case ConsoleKey.A:
                        MovePlatpose(true, keyInfo);
                        break;
                    case ConsoleKey.LeftArrow:
                        MovePlatpose(true, keyInfo);
                        break;
                    case ConsoleKey.RightArrow:
                        MovePlatpose(false, keyInfo);
                        break;
                    case ConsoleKey.D:
                        MovePlatpose(false, keyInfo);
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

        void SaveLevelComplete()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LevelPath));
            var completedLevelSaves = new CompletedLevelSaves
            {
                Level1Complete = Level1Completed,
                Level2Complete = Level2Completed,
                Level3Complete = Level1Completed,
            };

            string json = JsonSerializer.Serialize(completedLevelSaves);
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(LevelPath), "levels.json"), json);
        }
        void LoadLevelComplete()
        {
            if (File.Exists(LevelPath))
            {
                string json = File.ReadAllText(LevelPath);
                var saveDataLevels = JsonSerializer.Deserialize<CompletedLevelSaves>(json);
                Level1Completed = saveDataLevels.Level1Complete;
                Level2Completed = saveDataLevels.Level2Complete;
                Level3Completed = saveDataLevels.Level3Complete;
            }
        }
        void DeleteLevelComplete()
        {
            File.Delete(LevelPath);
        }
        void SaveGame()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
            List<string> areaInList = new List<string>();
            for (int i = 0; i < Area.GetLength(0); i++)
            {
                string line = "";
                for (int j = 0; j < Area.GetLength(1); j++)
                {
                    line += Area[i, j];
                }
                areaInList.Add(line);
            }
            var saveData = new SaveData()
            {
                SavedArea = areaInList,
                BallX = Ballposex,
                BallY = Ballposey,
                PlatX = Platposex,
                PlatY = Platposey,
                Difficulty = Difficulty,
                movex = Movex,
                movey = Movey,
                score = Score,
            };

            string json = JsonSerializer.Serialize(saveData);
            File.WriteAllText(SavePath, json);
            Environment.Exit(0);
        }
        void LoadGame()
        {
            Console.Clear();
            if (File.Exists(SavePath))
            {
                string json = File.ReadAllText(SavePath);
                var saveData = JsonSerializer.Deserialize<SaveData>(json);
                int lines = saveData.SavedArea.Count;
                int colums = saveData.SavedArea[0].Length;
                char[,] area1 = new char[lines, colums];

                for (int i = 0; i < lines; i++)
                {
                    for (int j = 0; j < colums; j++)
                    {
                        area1[i, j] = saveData.SavedArea[i][j];
                    }
                }
                Area = area1;
                Ballposex = saveData.BallX;
                Ballposey = saveData.BallY;
                Platposex = saveData.PlatX;
                Platposey = saveData.PlatY;
                Difficulty = saveData.Difficulty;
                Movex = saveData.movex;
                Movey = saveData.movey;
                Score = saveData.score;
                LoadedGame = true;
            }
            else
            {
                Console.WriteLine("No saves");
                Console.ReadLine();
                Console.Clear();
            }
        }

        void MovePlatpose<T>(bool moveLeft, T key)
        {
            lock (LockObj)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Platposex + i < Area.GetLength(1))
                    {
                        Area[Platposey, Platposex + i] = ' ';
                    }
                }
                if (moveLeft && Platposex > 0)
                {
                    Platposex--;
                }
                else if (!moveLeft && Platposex + 2 < Area.GetLength(1) - 1)
                {
                    Platposex++;
                }
                for (int i = 0; i < 3; i++)
                {
                    if (Platposex + i < Area.GetLength(1))
                    {
                        Area[Platposey, Platposex + i] = Platform;
                    }
                }
                DrawPlatform();
            }
            key = default(T);
        }
        void Menu()
        {
            bool choiseconfirmed = false;
            while (!choiseconfirmed)
            {
                Console.Clear();
                LoadedGame = false;
                Console.WriteLine("ARKANOID");
                Console.WriteLine("1 - START");
                Console.WriteLine("2 - LOAD SAVES");
                Console.WriteLine("3 - DIFFICULTY");
                Console.WriteLine("4 - DELETE SAVES");
                Console.WriteLine("5 - EXIT");
                char choise = Console.ReadKey().KeyChar;
                if (choise == '1')
                {
                    Ballposex = Area.GetLength(1) / 2;
                    Ballposey = Area.GetLength(0) - 2;
                    if (File.Exists(LevelPath))
                    {
                        LoadLevelComplete();
                    }
                    Console.CursorVisible = false;
                    choiseconfirmed = true;
                    if (!LoadedGame)
                    {
                        if (!Level1Completed)
                        {
                            SpawnArea1();
                        }
                        else if (!Level2Completed)
                        {
                            SpawnArea2();
                        }
                        else if (!Level3Completed)
                        {
                            SpawnArea3();
                        }
                        else if (Level1Completed && Level2Completed && Level3Completed)
                        {
                            Console.Clear();
                            Console.WriteLine("You comlepete all levels!!");
                            Console.WriteLine("Press 4 in menu to reset your progress");
                            break;
                        }
                    }
                    SpawnPlat();
                    ShowArea();
                    Thread ball = new Thread(new ThreadStart(MoveBall));
                    ball.Start();
                    Thread plat = new Thread(new ThreadStart(PlatrformController));
                    plat.Start();
                }
                else if (choise == '2')
                {
                    LoadGame();
                }
                else if (choise == '3')
                {
                    Console.Clear();
                    while (true)
                    {
                        Console.WriteLine("Your difficult is: 3 (where 1 is Easy - 3 is Hard)");
                        Console.WriteLine("Do you want to change it?");
                        Console.WriteLine("1 - Yes");
                        Console.WriteLine("2 - No");
                        char choise1 = Console.ReadKey().KeyChar;
                        if (choise1 == '1')
                        {
                            while (true)
                            {
                                Console.Clear();
                                Console.WriteLine("Enter new Difficult:");
                                Console.WriteLine("1 - Easy");
                                Console.WriteLine("2 - Normal");
                                Console.WriteLine("3 - Hard");
                                char choise2 = Console.ReadKey().KeyChar;
                                if (choise2 == '1' || choise2 == '2' || choise2 == '3')
                                {
                                    Difficulty = Convert.ToInt32(choise2 - 48);
                                    LoadedGame = true;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Wrong value");
                                }
                            }
                            break;
                        }
                        else if (choise == '2')
                        {
                            break;
                        }
                    }
                    Console.Clear();
                }
                else if (choise == '5')
                {
                    Console.Clear();
                    Console.WriteLine("Closing game");
                    break;
                }
                else if(choise == '4')
                {
                    DeleteLevelComplete();
                }
            }
        }
        internal class Program
        {
            static void Main(string[] args)
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                int height = Console.LargestWindowHeight;
                int width = Console.LargestWindowWidth;
                Console.SetWindowSize(width,height);
                GameBase game = new GameBase();
                game.Menu();
            }
        }
    }
}

