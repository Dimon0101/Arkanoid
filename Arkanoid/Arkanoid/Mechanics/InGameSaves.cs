using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static Arkanoid.GameBase;
using static System.Formats.Asn1.AsnWriter;

namespace Arkanoid.Mechanics
{
    public static class InGameSaves
    {
        public static void SaveGame()
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
        public static void LoadGame()
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
    }
}
