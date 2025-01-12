using Raylib_cs;
using System.Numerics;
using Tetris_QMJ.src.Entities;
using System.Collections.Generic;
using Tetris_QMJ.src.Core;
using System;

namespace Tetris_QMJ.src.Interfaces
{
    public class GameOver
    {   
        static List<Particle> particles = new List<Particle>();
        static Texture2D restartButtonTexture;
        static Texture2D mainMenuButtonTexture;

        public static void InitButtonTextures()
        {
            // Load button textures
            restartButtonTexture = Raylib.LoadTexture("assets/Buttons/RestartButton.png");
            mainMenuButtonTexture = Raylib.LoadTexture("assets/Buttons/MainMenuButton.png");
        }

        public static int PrintGameOver(int screenWidth, int screenHeight, Font font, int score)
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            DrawParticlesBackground(screenWidth, screenHeight);

            // Draw "Game Over" text
            string gameOverText = "GAME OVER";
            Vector2 textSize = Raylib.MeasureTextEx(font, gameOverText, 50, 1);
            Raylib.DrawTextEx(font, gameOverText, new Vector2((screenWidth - textSize.X) / 2, screenHeight / 4), 50, 1, Color.White);

            // Draw score
            string scoreText = $"Score {score}";
            Vector2 scoreSize = Raylib.MeasureTextEx(font, scoreText, 30, 1);
            Raylib.DrawTextEx(font, scoreText, new Vector2((screenWidth - scoreSize.X) / 2, screenHeight / 4 + textSize.Y + 20), 30, 1, Color.White);

            Raylib.EndDrawing();

            if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            {
                return 2;
            }
            return 5;
        }

        public static void DrawParticlesBackground(int screenWidth, int screenHeight)
        {
            // variable et condition pour ne pas avoir trop de particules par rapport a la taille de l'écran
            int screenPerimetre = screenHeight * screenWidth;
            if (particles.Count >= screenPerimetre / 20000)
            {
                particles.RemoveAt(Raylib.GetRandomValue(0, particles.Count - 1)); // Retirer une particule aléatoire
            }
            // Ajout de nouvelles particules 
            if (Raylib.GetRandomValue(0, 10) < 1)
            {
                particles.Add(new Particle(Raylib.GetRandomValue(0, screenWidth), Raylib.GetRandomValue(0, screenHeight)));
            }

            foreach (var particle in particles)
            {
                particle.Update(screenWidth, screenHeight);
                particle.Draw();
            }
        }
    }
}