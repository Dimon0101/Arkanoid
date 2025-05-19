using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Arkanoid.Mechanics
{
    public static class LevelsCompletedSaves
    {
        public static void SaveLevelComplete()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(GameBase.LevelPath));
            var completedLevelSaves = new GameBase.CompletedLevelSaves
            {
                Level1Complete = GameBase.Level1Completed,
                Level2Complete = GameBase.Level2Completed,
                Level3Complete = GameBase.Level1Completed,
            };

            string json = JsonSerializer.Serialize(completedLevelSaves);
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(GameBase.LevelPath), "levels.json"), json);
        }
        public static void LoadLevelComplete()
        {
            if (File.Exists(GameBase.LevelPath))
            {
                string json = File.ReadAllText(GameBase.LevelPath);
                var saveDataLevels = JsonSerializer.Deserialize<GameBase.CompletedLevelSaves>(json);
                GameBase.Level1Completed = saveDataLevels.Level1Complete;
                GameBase.Level2Completed = saveDataLevels.Level2Complete;
                GameBase.Level3Completed = saveDataLevels.Level3Complete;
            }
        }
        public static void DeleteLevelComplete()
        {
            File.Delete(GameBase.LevelPath);
        }
    }
}
