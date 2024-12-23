using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using Tetris_QMJ.src.Entities;
namespace Tetris_QMJ.src.Interfaces{
    public class MainMenu{
        static List<Particle> particles = new List<Particle>();
        static Texture2D playButtonTexture;
        static Texture2D optionsButtonTexture;
        static Texture2D quitButtonTexture;

        public static void InitButtonTextures(){
            // Charger les textures des boutons
            playButtonTexture = Raylib.LoadTexture("assets/Buttons/PlayButton.png");
            optionsButtonTexture = Raylib.LoadTexture("assets/Buttons/OptionsButton.png");
            quitButtonTexture = Raylib.LoadTexture("assets/Buttons/QuitButton.png");
        }
        static int EntryCode = 0;

        public static int PrintMainMenu(int screenWidth, int screenHeight, Font font){
            
            Raylib.BeginDrawing();
            DrawParticlesBackground(screenWidth, screenHeight); // Dessiner les particules avec traînée
            PrintTetrisOnTop(screenWidth, screenHeight, font);
            EntryCode = DrawButtons(screenWidth, screenHeight);
            Raylib.EndDrawing();
            return EntryCode;
        }
        static void DrawParticlesBackground(int screenWidth, int screenHeight){
            // variable et condition pour ne pas avoir trop de particules par rapport a la taille de l'écran
            int screenPerimetre = screenHeight*screenWidth;
            if (particles.Count >= screenPerimetre/20000){
                particles.RemoveAt(Raylib.GetRandomValue(0, particles.Count - 1)); // Retirer une particule aléatoire
            }
            // Ajout de nouvelles particules 
            if (Raylib.GetRandomValue(0, 10) < 1 ) {
                particles.Add(new Particle(Raylib.GetRandomValue(0, screenWidth), Raylib.GetRandomValue(0, screenHeight)));
            }
            
            foreach (var particle in particles){
                particle.Update(screenWidth, screenHeight); 
                particle.Draw();
            }
        }


        // Fonction pour afficher le texte "TETRIS" en haut de l'écran
        static void PrintTetrisOnTop(int screenWidth, int screenHeight, Font font){
            Raylib.ClearBackground(Color.Black);
            string title = "TETRIS";
            float fontSize = screenHeight / 15.0f;
            float spacing = fontSize / 8.0f;
            float totalWidth = 0;

            // Calcul de la largeur totale du texte
            foreach (char c in title){
                totalWidth += Raylib.MeasureTextEx(font, c.ToString(), fontSize, spacing).X;
            }

            // Position pour centrer le texte
            float startX = (screenWidth - totalWidth) / 2.0f;
            float startY = screenHeight / 20.0f;

            Color[] colors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Purple };
            Vector2 position = new Vector2(startX, startY);

            // Affichage de chaque lettre avec une couleur différente
            for (int i = 0; i < title.Length; i++){
                char c = title[i];
                Color color = colors[i % colors.Length];
                string letter = c.ToString();
                Raylib.DrawTextEx(font, letter, position, fontSize, spacing, color);
                position.X += Raylib.MeasureTextEx(font, letter, fontSize, spacing).X;
            }
        }


        static int DrawButtons(int screenWidth, int screenHeight){
            float buttonWidthPlay = playButtonTexture.Width;
            float buttonHeightPlay = playButtonTexture.Height;
            float buttonWidthOptions = optionsButtonTexture.Width;
            float buttonHeightOptions = optionsButtonTexture.Height;
            float buttonWidthQuit = quitButtonTexture.Width;
            float buttonHeightQuit = quitButtonTexture.Height;

            
            float scaleFactor = 1.3f; 

            // Calcul des nouvelles dimensions pour les boutons
            float scaledButtonWidthPlay = buttonWidthPlay * scaleFactor;
            float scaledButtonHeightPlay = buttonHeightPlay * scaleFactor;
            float scaledButtonWidthOptions = buttonWidthOptions * scaleFactor;
            float scaledButtonHeightOptions = buttonHeightOptions * scaleFactor;
            float scaledButtonWidthQuit = buttonWidthQuit * scaleFactor;
            float scaledButtonHeightQuit = buttonHeightQuit * scaleFactor;

            float buttonSpacing = 10.0f;

            // Calcul de la hauteur totale des boutons avec espacement
            float totalButtonsHeight = scaledButtonHeightPlay + scaledButtonHeightOptions + scaledButtonHeightQuit + 2 * buttonSpacing;

            // Calcul de la position Y pour centrer les boutons verticalement
            float startY = (screenHeight - totalButtonsHeight) / 2;

            // Position X pour centrer les boutons horizontalement par rapport au centre de chaque texture redimensionnée
            float startXPlay = (screenWidth - scaledButtonWidthPlay) / 2;
            float startXOptions = (screenWidth - scaledButtonWidthOptions) / 2;
            float startXQuit = (screenWidth - scaledButtonWidthQuit) / 2;

            // Positions des boutons sur l'axe Y
            float playButtonY = startY;
            float optionsButtonY = playButtonY + scaledButtonHeightPlay + buttonSpacing;
            float quitButtonY = optionsButtonY + scaledButtonHeightOptions + buttonSpacing;

            
            Raylib.DrawTexturePro(playButtonTexture, 
                                new Rectangle(0, 0, buttonWidthPlay, buttonHeightPlay), 
                                new Rectangle(startXPlay, playButtonY, scaledButtonWidthPlay, scaledButtonHeightPlay), 
                                Vector2.Zero, 0.0f, Color.White);

            Raylib.DrawTexturePro(optionsButtonTexture, 
                                new Rectangle(0, 0, buttonWidthOptions, buttonHeightOptions), 
                                new Rectangle(startXOptions, optionsButtonY, scaledButtonWidthOptions, scaledButtonHeightOptions), 
                                Vector2.Zero, 0.0f, Color.White);

            Raylib.DrawTexturePro(quitButtonTexture, 
                                new Rectangle(0, 0, buttonWidthQuit, buttonHeightQuit), 
                                new Rectangle(startXQuit, quitButtonY, scaledButtonWidthQuit, scaledButtonHeightQuit), 
                                Vector2.Zero, 0.0f, Color.White);

            Vector2 mousePosition = Raylib.GetMousePosition();
            if (Raylib.CheckCollisionPointRec(mousePosition, new Rectangle(startXPlay, playButtonY, scaledButtonWidthPlay, scaledButtonHeightPlay)) && Raylib.IsMouseButtonPressed(MouseButton.Left)){
                return 1; // code 1 = gameloop dans la main loop
            }
            else if (Raylib.CheckCollisionPointRec(mousePosition, new Rectangle(startXOptions, optionsButtonY, scaledButtonWidthOptions, scaledButtonHeightOptions)) && Raylib.IsMouseButtonPressed(MouseButton.Left)){
                return 2; // code 2 menu options dans la main loop
            }
            else if (Raylib.CheckCollisionPointRec(mousePosition, new Rectangle(startXQuit, quitButtonY, scaledButtonWidthQuit, scaledButtonHeightQuit)) && Raylib.IsMouseButtonPressed(MouseButton.Left)){
                return 99; // code 99 = quitter le jeu dans la main loop
            }else {
                return 0; // code 0 = main menu donc relance la loop au dessus
            }
        }





    }
}
