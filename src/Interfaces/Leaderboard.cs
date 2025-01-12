using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Raylib_cs;

namespace Tetris_QMJ.src.Core
{
    public class Leaderboard
    {
        private const string LeaderboardFilePath = "./src/Interfaces/leaderboard.txt";
        private List<(string Username, int Score)> scores;

        public Leaderboard()
        {
            scores = new List<(string Username, int Score)>();
            LoadScores();
        }

        //ajoute un score au leaderboard et le sauvegarde dans le fichier leaderboard.txt
        public void AddScore(string username, int score)
        {
            scores.Add((username, score));
            scores.Sort((a, b) => b.Score.CompareTo(a.Score));  // Sort descending
            if (scores.Count > 5)
            {
                scores.RemoveAt(5);  // Keep only top 5
            }
            SaveScores();
        }

        //charge les scores depuis le fichier leaderboard.txt
        private void LoadScores()
        {
            if (File.Exists(LeaderboardFilePath))
            {
                var lines = File.ReadAllLines(LeaderboardFilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                    {
                        scores.Add((parts[0], score));
                    }
                }
                scores.Sort((a, b) => b.Score.CompareTo(a.Score));  // Sort descending
            }
        }

        //sauvegarde les scores dans le fichier leaderboard.txt
        private void SaveScores()
        {
            var lines = new List<string>();
            foreach (var score in scores)
            {
                lines.Add($"{score.Username}:{score.Score}");
            }
            File.WriteAllLines(LeaderboardFilePath, lines);
        }

        //affiche le leaderboard
        public void Display(int screenWidth, int screenHeight, Font font)
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawText("Leaderboard", screenWidth / 2 - 100, 50, 40, Color.White);
            

            for (int i = 0; i < scores.Count; i++)
            {
                var score = scores[i];
                Raylib.DrawTextEx(font, $"{i + 1}. {score.Username}: {score.Score}", 
                                  new Vector2(screenWidth / 2 - 100, 100 + i * 30), 
                                  20, 1.0f, Color.White);
            }
            
            Raylib.EndDrawing();
        }

        //retourne les 5 meilleurs scores
        public List<string> GetTopScores()
        {
            List<string> topScores = new List<string>();

            // Retrieve the top 5 scores
            foreach (var score in scores.Take(5))
            {
                topScores.Add($"{score.Username} - {score.Score}");
            }

            return topScores;
        }
    }
}