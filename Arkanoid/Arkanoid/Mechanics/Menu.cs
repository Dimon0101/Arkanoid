using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid.Mechanics
{
    public class Menu
    {
        public void StartMenu()
        {
            bool choiseconfirmed = false;
            while (!choiseconfirmed)
            {
                Console.Clear();
                GameBase.LoadedGame = false;
                Console.WriteLine("ARKANOID");
                Console.WriteLine("1 - START");
                Console.WriteLine("2 - LOAD SAVES");
                Console.WriteLine("3 - DIFFICULTY");
                Console.WriteLine("4 - DELETE SAVES");
                Console.WriteLine("5 - EXIT");
                char choise = Console.ReadKey().KeyChar;
                if (choise == '1')
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
                else if (choise == '2')
                {
                    InGameSaves.LoadGame();
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
                                    GameBase.Difficulty = Convert.ToInt32(choise2 - 48);
                                    GameBase.LoadedGame = true;
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
                else if (choise == '4')
                {
                    LevelsCompletedSaves.DeleteLevelComplete();
                }
            }
        }
    }
}
