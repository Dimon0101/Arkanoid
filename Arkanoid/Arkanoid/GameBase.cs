using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using Arkanoid.Mechanics;

namespace Arkanoid
{
    public static class GameBase
    {
        public static char[,] Area = new char[30, 21];
        public static char Block = '■';
        public static char Platform = '▀';
        public static char Ball = '●';
        public static int Ballposey = 0;
        public static int Ballposex = 0;
        public static int Platposey = 0;
        public static int Platposex = 0;
        public static int Movex = -1;
        public static int Movey = -1;
        public static int Difficulty = 3;
        public static object LockObj = new object();
        public static string SavePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Saves", "save.json");
        public static string LevelPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Levels", "levels.json");
        public static bool LoadedGame = false;
        public static int Score = 0;
        public static int ScorePerBlock = 500;
        public static bool Level1Completed = true;
        public static bool Level2Completed = true;
        public static bool Level3Completed = false;
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
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu.StartMenu();
        }
    }
}

