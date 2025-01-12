using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using Tetris_QMJ.src.Entities;

namespace Tetris_QMJ.src.Interfaces
{
    public class MainMenu
    {
        static List<Particle> particles = new List<Particle>();
        static Texture2D playButtonTexture; 
        static Texture2D leaderButtonTexture;
        static Texture2D optionsButtonTexture;
        static Texture2D quitButtonTexture;
        public static string username = "";

        public static void InitButtonTextures()
        {
            // Charger les textures des boutons
            playButtonTexture = Raylib.LoadTexture("assets/Buttons/PlayButton.png");
            leaderButtonTexture = Raylib.LoadTexture("assets/Buttons/Leaderboard.png");
            optionsButtonTexture = Raylib.LoadTexture("assets/Buttons/OptionsButton.png");
            quitButtonTexture = Raylib.LoadTexture("assets/Buttons/QuitButton.png");
        }

        static int EntryCode = 0;

        public static int PrintMainMenu(int screenWidth, int screenHeight, Font font)
        {
            Raylib.BeginDrawing();
            DrawParticlesBackground(screenWidth, screenHeight);
            PrintTetrisOnTop(screenWidth, screenHeight, font);
            EntryCode = DrawButtons(screenWidth, screenHeight);
            Raylib.EndDrawing();

            if (EntryCode == 1)
            {
                username = GetUsername(screenWidth, screenHeight, font);
                if (!string.IsNullOrEmpty(username))
                {
                    return 1;
                }
                else
                {
                    EntryCode = 0;
                }
            }
            return EntryCode;
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

        // Fonction pour afficher le texte "TETRIS" en haut de l'écran
        static void PrintTetrisOnTop(int screenWidth, int screenHeight, Font font)
        {
            Raylib.ClearBackground(Color.Black);
            string title = "TETRIS";
            float fontSize = screenHeight / 15.0f;
            float spacing = fontSize / 8.0f;
            float totalWidth = 0;

            // Calcul de la largeur totale du texte
            foreach (char c in title)
            {
                totalWidth += Raylib.MeasureTextEx(font, c.ToString(), fontSize, spacing).X;
            }

            // Position pour centrer le texte
            float startX = (screenWidth - totalWidth) / 2.0f;
            float startY = screenHeight / 20.0f;

            Color[] colors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Purple };
            Vector2 position = new Vector2(startX, startY);

            // Affichage de chaque lettre avec une couleur différente
            for (int i = 0; i < title.Length; i++)
            {
                char c = title[i];
                Color color = colors[i % colors.Length];
                string letter = c.ToString();
                Raylib.DrawTextEx(font, letter, position, fontSize, spacing, color);
                position.X += Raylib.MeasureTextEx(font, letter, fontSize, spacing).X;
            }
        }

        static int DrawButtons(int screenWidth, int screenHeight)
        {
            float buttonWidthPlay = playButtonTexture.Width;
            float buttonHeightPlay = playButtonTexture.Height;
            float buttonWidthLeader = leaderButtonTexture.Width;
            float buttonHeightLeader = leaderButtonTexture.Height;
            float buttonWidthOptions = optionsButtonTexture.Width;
            float buttonHeightOptions = optionsButtonTexture.Height;
            float buttonWidthQuit = quitButtonTexture.Width;
            float buttonHeightQuit = quitButtonTexture.Height;

            float scaleFactor = 1.3f;

            // Calcul des nouvelles dimensions pour les boutons
            float scaledButtonWidthPlay = buttonWidthPlay * scaleFactor;
            float scaledButtonHeightPlay = buttonHeightPlay * scaleFactor;
            float scaledButtonWidthLeader = buttonWidthLeader * scaleFactor;
            float scaledButtonHeightLeader = buttonHeightLeader * scaleFactor;
            float scaledButtonWidthOptions = buttonWidthOptions * scaleFactor;
            float scaledButtonHeightOptions = buttonHeightOptions * scaleFactor;
            float scaledButtonWidthQuit = buttonWidthQuit * scaleFactor;
            float scaledButtonHeightQuit = buttonHeightQuit * scaleFactor;

            float buttonSpacing = 10.0f;

            // Calcul de la hauteur totale des boutons avec espacement
            float totalButtonsHeight = scaledButtonHeightPlay + scaledButtonHeightLeader + scaledButtonHeightOptions + scaledButtonHeightQuit + 3 * buttonSpacing;

            // Calcul de la position Y pour centrer les boutons verticalement
            float startY = (screenHeight - totalButtonsHeight) / 2;

            // Position X pour centrer les boutons horizontalement par rapport au centre de chaque texture redimensionnée
            float startXPlay = (screenWidth - scaledButtonWidthPlay) / 2;
            float startXLeader = (screenWidth - scaledButtonWidthLeader) / 2;
            float startXOptions = (screenWidth - scaledButtonWidthOptions) / 2;
            float startXQuit = (screenWidth - scaledButtonWidthQuit) / 2;

            // Positions des boutons sur l'axe Y
            float playButtonY = startY;
            float leaderButtonY = playButtonY + scaledButtonHeightPlay + buttonSpacing;
            float optionsButtonY = leaderButtonY + scaledButtonHeightLeader + buttonSpacing;
            float quitButtonY = optionsButtonY + scaledButtonHeightOptions + buttonSpacing;

            //bouton Play
            Raylib.DrawTexturePro(playButtonTexture,
                                new Rectangle(0, 0, buttonWidthPlay, buttonHeightPlay),
                                new Rectangle(startXPlay, playButtonY, scaledButtonWidthPlay, scaledButtonHeightPlay),
                                Vector2.Zero, 0.0f, Color.White);

            //bouton Leaderboard
            Raylib.DrawTexturePro(leaderButtonTexture,
                                new Rectangle(0, 0, buttonWidthLeader, buttonHeightLeader),
                                new Rectangle(startXLeader, leaderButtonY, scaledButtonWidthLeader, scaledButtonHeightLeader),
                                Vector2.Zero, 0.0f, Color.White);
            //bouton Options
            Raylib.DrawTexturePro(optionsButtonTexture,
                                new Rectangle(0, 0, buttonWidthOptions, buttonHeightOptions),
                                new Rectangle(startXOptions, optionsButtonY, scaledButtonWidthOptions, scaledButtonHeightOptions),
                                Vector2.Zero, 0.0f, Color.White);
            //bouton Quit
            Raylib.DrawTexturePro(quitButtonTexture,
                                new Rectangle(0, 0, buttonWidthQuit, buttonHeightQuit),
                                new Rectangle(startXQuit, quitButtonY, scaledButtonWidthQuit, scaledButtonHeightQuit),
                                Vector2.Zero, 0.0f, Color.White);

            Vector2 mousePosition = Raylib.GetMousePosition();
            if (Raylib.CheckCollisionPointRec(mousePosition, new Rectangle(startXPlay, playButtonY, scaledButtonWidthPlay, scaledButtonHeightPlay)) && Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                return 1; // code 1 = gameloop dans la main loop
            }
            else if (Raylib.CheckCollisionPointRec(mousePosition, new Rectangle(startXLeader, leaderButtonY, scaledButtonWidthLeader, scaledButtonHeightLeader)) && Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                return 4; // code 4 = leaderboard dans la main loop
            }
            else if (Raylib.CheckCollisionPointRec(mousePosition, new Rectangle(startXOptions, optionsButtonY, scaledButtonWidthOptions, scaledButtonHeightOptions)) && Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                return 3; // code 3 menu options dans la main loop
            }
            else if (Raylib.CheckCollisionPointRec(mousePosition, new Rectangle(startXQuit, quitButtonY, scaledButtonWidthQuit, scaledButtonHeightQuit)) && Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                return 99; // code 99 = quitter le jeu dans la main loop
            }
            else
            {
                return 0; // code 0 = main menu donc relance la loop au dessus
            }
        }

        //demande et recupere le nom d'utilisateur du joueur
        public static string GetUsername(int screenWidth, int screenHeight, Font font)
        {
            bool enterPressed = false;

            //boucle qui permet de recuperer le nom d'utilisateur choisi par le joueur
            while (!enterPressed && !Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                Raylib.DrawText("Enter your username and press enter twice:", screenWidth / 2 - 220, screenHeight / 2 - 50, 20, Color.White);
                Raylib.DrawText(username, screenWidth / 2 - 100, screenHeight / 2, 20, Color.White);

                Raylib.EndDrawing();

                // Récupérer la touche pressée par l'utilisateur
                int key = Raylib.GetKeyPressed();
                if (key > 0)
                {
                    if (key == (int)KeyboardKey.Backspace && username.Length > 0)
                    {
                        // permet de supprimer un caractere
                        username = username.Substring(0, username.Length - 1);
                    }
                    else if (key == (int)KeyboardKey.Enter)
                    {
                        // permet de valider le nom d'utilisateur
                        enterPressed = true;
                    }
                    // limite la taille du nom d'utilisateur a 16 caracteres
                    else if (key >= 32 && key <= 126 && username.Length < 16)
                    {
                        username += MapAzertyKey((char)key);
                    }
                }
            }
            return username;
        }


        //func qui permet au jeu de fonctionner en azerty au lieu d etre en qwerty
        public static char MapAzertyKey(char key)
        {
            // Dictionnaire pour mapper les caractères AZERTY vers leur équivalent
            Dictionary<char, char> azertyMap = new Dictionary<char, char>
            {
                { 'q', 'a' }, { 'a', 'q' },
                { 'z', 'w' }, { 'w', 'z' },
                { 'm', ',' }, { ',', 'm' },
                { 'Q', 'A' }, { 'A', 'Q' },
                { 'Z', 'W' }, { 'W', 'Z' },
                { 'M', '?' }, { '?', 'M' },
                { '&', '1' }, { 'é', '2' }, { '"', '3' }, { '\'', '4' },
                { '(', '5' }, { '-', '6' }, { 'è', '7' }, { '_', '8' },
                { 'ç', '9' }, { 'à', '0' }
            };

            // Vérifier si la touche existe dans le mappage
            if (azertyMap.ContainsKey(key))
            {
                return azertyMap[key];
            }

            // Si aucune correspondance, retourner la touche originale
            return key;
        }
    }
}