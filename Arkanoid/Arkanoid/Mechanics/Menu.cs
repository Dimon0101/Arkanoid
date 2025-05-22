using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid.Mechanics
{
    public static class Menu
    {
        public static void ChangeDifficulty()
        {
            Console.Clear();
            string[] ChooseDifficulty = { "Easy", "Normal", "Hard" };
            var CursoreCounter = 0;
            bool DifficultyChoosen = false;
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(ChooseDifficulty[0]);
            Console.ResetColor();
            Console.WriteLine(ChooseDifficulty[1]);
            Console.WriteLine(ChooseDifficulty[2]);
            while (!DifficultyChoosen)
            {
                if(Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(true);
                    Console.ResetColor();
                    switch(keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if(CursoreCounter !=0)
                            {
                                CursoreCounter--;
                                MoveCursore(ChooseDifficulty,CursoreCounter);
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if(CursoreCounter != 2)
                            {
                                CursoreCounter++;
                                MoveCursore(ChooseDifficulty, CursoreCounter);
                            }
                            break;
                        case ConsoleKey.Enter:
                            GameBase.Difficulty = CursoreCounter+1;
                            DifficultyChoosen = true;
                            break;
                    }
                }
            }
            Console.Clear();
        }
        public static void MoveCursore(string[] option, int cursorepos)
        {

            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < option.Length; i++)
            {
                if (i == cursorepos)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(option[i]);
                Console.ResetColor();
            }
        }
        public static void StartMenu()
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            int height = Console.LargestWindowHeight;
            int width = Console.LargestWindowWidth;
            Console.SetWindowSize(width, height);
            bool choiseconfirmed = false;
            var cursorepos = 0;
            Console.CursorVisible = false;
            Console.Clear();

            String[] option = { "Start Game", "Load Game", "Difficulty choose","Delete Saves", "Exit" };


            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(option[0]);
            Console.ResetColor();
            Console.WriteLine(option[1]);
            Console.WriteLine(option[2]);
            Console.WriteLine(option[3]);
            Console.WriteLine(option[4]);
            while (!choiseconfirmed)
            {
                if (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(true);
                    Console.ResetColor();
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.Escape:
                            return;
                        case ConsoleKey.DownArrow:
                            if (cursorepos != 4)
                            {
                                cursorepos++;
                                MoveCursore(option, cursorepos);
                            }
                            break;
                        case ConsoleKey.UpArrow:
                            if (cursorepos != 0)
                            {
                                cursorepos--;
                                MoveCursore(option, cursorepos);
                            }
                            break;
                        case ConsoleKey.Enter:
                            if (cursorepos == 0)
                            {
                                Ball.SpawnBall();
                                if (File.Exists(GameBase.LevelPath))
                                {
                                    LevelsCompletedSaves.LoadLevelComplete();
                                }
                                Console.CursorVisible = false;
                                choiseconfirmed = true;
                                if (!GameBase.LoadedGame)
                                {
                                    if (!GameBase.Level1Completed)
                                    {
                                        SpawnLevels.SpawnArea1();
                                    }
                                    else if (!GameBase.Level2Completed)
                                    {
                                        SpawnLevels.SpawnArea2();
                                    }
                                    else if (!GameBase.Level3Completed)
                                    {
                                        SpawnLevels.SpawnArea3();
                                    }
                                    else if (GameBase.Level1Completed && GameBase.Level2Completed && GameBase.Level3Completed)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("You comlepete all levels!!");
                                        Console.WriteLine("Press 4 in menu to reset your progress");
                                        break;
                                    }
                                }
                                Platform.SpawnPlat();
                                DrawAndClear.ShowArea();
                                Thread ball = new Thread(new ThreadStart(Ball.MoveBall));
                                ball.Start();
                                Thread plat = new Thread(new ThreadStart(ControllSettings.PlatrformController));
                                plat.Start();
                            }
                            else if (cursorepos == 1)
                            {
                                InGameSaves.LoadGame();
                            }
                            else if (cursorepos == 2)
                            {
                                ChangeDifficulty();
                                MoveCursore(option,cursorepos);
                            }
                            else if (cursorepos == 3)
                            {
                                LevelsCompletedSaves.DeleteLevelComplete();
                            }
                            else if (cursorepos == 4)
                            {
                                Console.Clear();
                                Console.WriteLine("Closing game");
                                break;
                            }
                            break;
                        case ConsoleKey.Backspace:
                            Console.Clear();
                            MoveCursore(option, cursorepos);
                            break;
                    }
                }
            }
        }
    }
}
