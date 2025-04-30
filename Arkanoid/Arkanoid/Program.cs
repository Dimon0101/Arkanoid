using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace Arkanoid
{
    public class SaveData
    {
        public List<string> Area { get; set; }
        public int BallX { get; set; }
        public int BallY { get; set; }
        public int PlatX { get; set; }
        public int PlatY { get; set; }
        public int Difficulty { get; set; }
        public int movey { get; set; }
        public int movex { get; set; }
        public int score { get; set; }
    }
    class Arkanoid
    {
        char block = '■';
        char platform = '▀';
        char ball = '●';
        int ballposey = 0;
        int ballposex = 0;
        int platposey = 0;
        int platposex = 0;
        int movex = -1;
        int movey = -1;
        int difficulty = 3;
        char[,] area;
        object lockObj = new object();
        string savePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Saves", "save.json");
        bool loadedGame = false;
        int score = 0;
        public Arkanoid(char[,] area)
        {
            this.area = area;
            ballposey = area.GetLength(0) - 2;
            ballposex = area.GetLength(1) / 2;
            platposey = area.GetLength(0) - 1;
            platposex = area.GetLength(1) / 2;
        }
        public void SpawnArea()
        {
            if (!loadedGame)
            {
                for (int i = 0; i < area.GetLength(0); i++)
                {
                    for (int j = 0; j < area.GetLength(1); j++)
                    {
                        if (i >= 0 && i < 5)
                        {
                            area[i, j] = block;
                        }
                        else
                        {
                            area[i, j] = ' ';
                        }
                    }
                }
                area[ballposey, ballposex] = ball;
                for (int i = 0; i < 3; i++)
                {
                    if (platposex + i < area.GetLength(1))
                        area[platposey, platposex + i] = platform;
                }
                score = 0;
            }
        }

        public void ShowArea()
        {
            lock (lockObj)
            {
                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < area.GetLength(0); i++)
                {
                    for (int j = 0; j < area.GetLength(1); j++)
                    {
                        Console.Write(area[i,j] +" ");
                    }
                    Console.Write("\n");
                }
                Console.WriteLine("Your score is: " + score);
            }
        }
        public void ClearBall()
        {
            Console.SetCursorPosition(ballposex*2, ballposey);
            Console.Write(' ');
        }
        public void Drawball()
        {
                Console.SetCursorPosition(ballposex*2, ballposey);
                Console.Write($"{ball}");
        }
        public void RicohetCheck(ref int nexty, ref int nextx)
        {
            if (area[nexty, nextx] == block)
            {
                Console.SetCursorPosition(nextx * 2, nexty);
                Console.Write(' ');
                movey = -movey;
                nexty += movey;
                nextx += movex;
                score += 500;
            }
            if (area[nexty, nextx] == platform)
            {
                movey = -movey;
                nexty += movey;
                nextx += movex;
            }
        }
        public void MoveBall()
        {
            while (true)
            {
                lock (lockObj)
                {
                    ClearBall();
                    area[ballposey, ballposex] = ' ';
                    int nexty = ballposey + movey;
                    int nextx = ballposex + movex;
                    if (nextx < 0 || nextx > area.GetLength(1) - 1)
                    {
                        movex = -movex;
                        nextx += movex;
                        RicohetCheck(ref nexty,ref nextx);
                    }
                    else if (nexty < 0)
                    {
                        movey = -movey;
                        nexty += movey;
                        RicohetCheck(ref nexty, ref nextx);
                    }
                    else if (nexty == area.GetLength(0))
                    {
                        Console.Clear();
                        Console.WriteLine("8b        d8                        88                                   \r\n Y8,    ,8P                         88                                   \r\n  Y8,  ,8P                          88                                   \r\n   \"8aa8\" ,adPPYba,  88       88    88  ,adPPYba,  ,adPPYba,  ,adPPYba,  \r\n    `88' a8\"     \"8a 88       88    88 a8\"     \"8a I8[    \"\" a8P_____88  \r\n     88  8b       d8 88       88    88 8b       d8  `\"Y8ba,  8PP\"\"\"\"\"\"\"  \r\n     88  \"8a,   ,a8\" \"8a,   ,a88    88 \"8a,   ,a8\" aa    ]8I \"8b,   ,aa  \r\n     88   `\"YbbdP\"'   `\"YbbdP'Y8    88  `\"YbbdP\"'  `\"YbbdP\"'  `\"Ybbd8\"'  \r\n");
                        Environment.Exit(0);
                    }
                    else if (area[nexty,ballposex] == platform)
                    {
                        movey = - movey;
                        nexty += 2*movey;
                    }
                    else if (area[ballposey, nextx] == platform)
                    {
                        movex = -movex;
                        nextx += 2 * movex;
                    }
                    else if (area[nexty, nextx] == platform)
                    {
                        movey = -movey;
                        nexty += 2*movey;
                        nextx += 2*movex;
                    }
                    else if (area[nexty, ballposex] == block)
                    {
                        if (area[ballposey, nextx] == block && nexty > 4)
                        {
                            Console.SetCursorPosition(nextx * 2, ballposey);
                            Console.Write(' ');
                            score += 500;
                        }
                        if (area[nexty, nextx] == block && nexty > 4)
                        {

                            Console.SetCursorPosition(nextx * 2, nexty);
                            Console.Write(' ');
                            score += 500;
                        }
                        Console.SetCursorPosition(ballposex * 2, nexty);
                        Console.Write(' ');
                        area[nexty, ballposex] = ' ';
                        movey = -movey;
                        nexty += movey;
                        score += 500;
                    }
                    else if (area[ballposey, nextx] == block)
                    {
                        if (area[nexty, ballposex] == block && nexty > 4)
                        {
                            Console.SetCursorPosition(ballposex * 2, nexty);
                            Console.Write(' ');
                            score += 500;
                        }
                        if (area[nexty, nextx] == block && nexty > 4)
                        {

                            Console.SetCursorPosition(nextx * 2, nexty);
                            Console.Write(' ');
                            score += 500;
                        }
                        Console.SetCursorPosition(nextx * 2, ballposey);
                        Console.Write(' ');
                        area[ballposey, nextx] = ' ';
                        movex = -movex;
                        score += 500;
                    }
                    else if (area[nexty, nextx] == block)
                    {
                        if(area[nexty,ballposex] == block)
                        {
                            Console.SetCursorPosition(ballposex*2, nexty);
                            Console.Write(' ');
                            score += 500;
                        }
                        if(area[ballposey,nextx] == block)
                        {
                            Console.SetCursorPosition(nextx*2, ballposey);
                            Console.Write(' ');
                            score += 500;
                        }
                        Console.SetCursorPosition(nextx * 2, nexty);
                        Console.Write(' ');
                        area[nexty, nextx] = ' ';
                        movey = -movey;
                        score += 500;
                        nexty = ballposey + movey;
                        nextx = ballposex + movex;
                    }
                    ballposex = nextx;
                    ballposey = nexty;
                    area[ballposey, ballposex] = ball;
                    Drawball();
                    Console.SetCursorPosition(0, area.GetLength(0));
                    Console.Write("                                                                ");
                    Console.SetCursorPosition(0, area.GetLength(0));
                    Console.Write("Your score is : " + score);
                }
                if (difficulty == 1)
                {
                    Thread.Sleep(400);
                }
                else if (difficulty == 2)
                {
                    Thread.Sleep(250);
                }
                else if (difficulty == 3)
                {
                    Thread.Sleep(100);
                }
            }
        }
        public void MovePlatrform()
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

        public void SaveGame()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            var rows = new List<string>();
            for (int i = 0; i < area.GetLength(0); i++)
            {
                string row = "";
                for (int j = 0; j < area.GetLength(1); j++)
                {
                    row += area[i, j];
                }
                rows.Add(row);
            }
            var saveData = new SaveData()
            {
                Area = rows,
                BallX = ballposex,
                BallY = ballposey,
                PlatX = platposex,
                PlatY = platposey,
                Difficulty = difficulty,
                movex = movex,
                movey = movey,
                score = score,
            };

            string json = JsonSerializer.Serialize(saveData);
            File.WriteAllText(savePath, json);
            Environment.Exit(0);
        }
        public void LoadGame()
        {
            Console.Clear();
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                var saveData = JsonSerializer.Deserialize<SaveData>(json);
                int rows = saveData.Area.Count;
                int cols = saveData.Area[0].Length;
                char[,] area1 = new char[rows, cols];

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        area1[i, j] = saveData.Area[i][j];
                    }
                }
                area = area1;
                ballposex = saveData.BallX;
                ballposey = saveData.BallY;
                platposex = saveData.PlatX;
                platposey = saveData.PlatY;
                difficulty = saveData.Difficulty;
                movex = saveData.movex;
                movey = saveData.movey;
                score = saveData.score;
                loadedGame = true;
            }
            else
            {
                Console.WriteLine("No saves");
                Console.ReadLine();
                Console.Clear();
            }
        }

        public void MovePlatpose<T>(bool moveLeft, T key)
        {
            lock (lockObj)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (platposex + i < area.GetLength(1))
                    {
                        area[platposey, platposex + i] = ' ';
                    }
                }
                if (moveLeft && platposex > 0)
                {
                    platposex--;
                }
                else if (!moveLeft && platposex + 2 < area.GetLength(1) - 1)
                    platposex++;

                for (int i = 0; i < 3; i++)
                {
                    if (platposex + i < area.GetLength(1))
                        area[platposey, platposex + i] = platform;
                }
            }
            for (int i = 0; i < area.GetLength(1); i++)
            {
                Console.SetCursorPosition(i * 2, area.GetLength(0)-1);
                Console.Write(area[area.GetLength(0)-1,i]);
            }
            key = default(T);
        }
        public void Menu()
        {
            bool choiseconfirmed = false;
            while (!choiseconfirmed)
            {
                Console.CursorVisible = false;
                loadedGame = false;
                Console.WriteLine("ARKANOID");
                Console.WriteLine("1 - START");
                Console.WriteLine("2 - LOAD SAVES");
                Console.WriteLine("3 - DIFFICULTY");
                Console.WriteLine("4 - EXIT");
                char choise = Console.ReadKey().KeyChar;
                if (choise == '1')
                {
                    choiseconfirmed = true;
                    SpawnArea();
                    ShowArea();
                    Thread ball = new Thread(new ThreadStart(MoveBall));
                    ball.Start();
                    Thread platform = new Thread(new ThreadStart(MovePlatrform));
                    platform.Start();
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

                        Console.WriteLine("Difficulty of game is : " + difficulty);
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
                                    difficulty = Convert.ToInt32(choise2 - 48);
                                    loadedGame = true;
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
                else if (choise == '4')
                {
                    Console.Clear();
                    Console.WriteLine("Closing game");
                    break;
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
                Arkanoid game = new Arkanoid(new char[30, 21]);
                game.Menu();
            }
        }
    }
}

