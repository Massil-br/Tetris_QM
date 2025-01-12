using System;
using System.Collections.Generic;
using System.Linq;
using Raylib_cs;
using Tetris_QMJ.src.Audio;
using Tetris_QMJ.src.Entities;
using Tetris_QMJ.src.Interfaces;

namespace Tetris_QMJ.src.Core
{
    public class Game
    {
        // Initializes the window size at the start of the program
        const int width = 800;
        const int height = 600;

        // Initializes the 10*20 game grid
        const int gridColumns = 10;
        const int gridRows = 20;
        static Grid grid = new(gridRows, gridColumns);
        static Options options = new Options();
        static Leaderboard leaderboard = new Leaderboard();
        

        // The InitWindow() function first calls all functions that initialize different variables needed for the program
        // and then contains the main game loop
        public static void InitWindow()
        {
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow); // to allow resize
            Raylib.InitWindow(width, height, "Tetris"); // window init
            Raylib.InitAudioDevice();
            AudioGame.InitAudioGame();
            Raylib.SetTargetFPS(165); // window FPS 
            Font MainMenuFont = Raylib.LoadFont("assets/font/Team 401.ttf");
            Font font = Raylib.LoadFont("assets/font/College Squad Regular.ttf");
            MainMenu.InitButtonTextures();
            Raylib.SetExitKey(KeyboardKey.Null);

            // Main game loop, starts in the menu
            int EntryCode = 0;
            while (!Raylib.WindowShouldClose())
            {
                int windowHeight = Raylib.GetRenderHeight();
                int windowWidth = Raylib.GetRenderWidth();

                // MENU
                if (EntryCode == 0)
                {
                    EntryCode = MainMenu.PrintMainMenu(windowWidth, windowHeight, MainMenuFont);
                    AudioGame.PlayMusicStream(AudioGame.musicBackgroundMainMenu1);
                    if (EntryCode == 1)
                    {
                        grid = new Grid(gridRows, gridColumns);
                        MainMenu.username = MainMenu.GetUsername(windowWidth, windowHeight, MainMenuFont);
                    }
                }
                // GAME
                else if (EntryCode == 1)
                {
                    AudioGame.PlaySound(AudioGame.soundButtonMenu);
                    EntryCode = GameLoop(grid);
                }
                // PAUSE
                else if (EntryCode == 2)
                {
                    AudioGame.PlaySound(AudioGame.soundButtonMenu);
                    Console.WriteLine("PAUSE");
                    EntryCode = 0;
                }
                else if (EntryCode == 3)
                {
                    // OPTIONS MENU;
                    AudioGame.PlaySound(AudioGame.soundButtonMenu);
                    Console.WriteLine("option menu");
                    EntryCode = ShowOptionsMenu(options);
                    options.SaveKey();
                }
                else if (EntryCode == 4)
                {
                    // Leaderboard
                    AudioGame.PlayMusicStream(AudioGame.musicBackgroundMainMenu1);
                    EntryCode = leaderboard.Display(windowWidth, windowHeight, font);
                    
                }
                else if (EntryCode == 5)
                {
                    // GAME OVER
                    // AudioGame.PlayMusicStream(AudioGame.musicBackgroundGameOver);
                    EntryCode = GameOver.PrintGameOver(windowWidth, windowHeight, MainMenuFont, grid.Score);
                }

                // CLOSE WINDOW
                else if (EntryCode == 99)
                {
                    AudioGame.PlaySound(AudioGame.soundButtonMenu);
                    Raylib.CloseWindow();
                    break;
                }
            }

            // Unloads different variables
            AudioGame.UnloadAudioResources();
            Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
        }

        public static int GameLoop(Grid grid)
        {
            int windowHeight;
            int windowWidth;
            int cellSize;
            int offsetX;
            int offsetY;

            // Generates a new random piece
            Piece randomNextPiece = PieceFactory.GenerateRandomPiece(1);
            Piece firstPiece = PieceFactory.GenerateRandomPiece(1);
            grid.NextPiece = randomNextPiece;
            grid.ActivePiece = firstPiece;
            // grid.AddPiece(grid.ActivePiece);
            // grid.SetActivePiece(grid.ActivePiece);

            Rotation rotateHandler = new Rotation(grid);
            Move moveHandler = new Move(grid);
            moveHandler.SetPiece(grid.ActivePiece);

            Timer timer = new Timer();
            Font font = Raylib.LoadFont("assets/font/College Squad Regular.ttf");

            // The game loop continues as long as the window is not closed
            while (!Raylib.WindowShouldClose())
            {
                windowHeight = Raylib.GetRenderHeight();
                windowWidth = Raylib.GetRenderWidth();
                cellSize = Math.Min(windowWidth / (gridColumns + 2), windowHeight / (gridRows + 2));
                offsetX = (windowWidth - (gridColumns * cellSize)) / 2;
                offsetY = (windowHeight - (gridRows * cellSize)) / 2;

                Raylib.BeginDrawing();  // Starts the drawing phase
                Raylib.ClearBackground(Color.Black);

                // calcul le temps écoulé depuis la dernière frame
                float deltaTime = Raylib.GetFrameTime();

                // met a jour le timer pour la vitesse de la pièce et les déplacements de la pièce
                moveHandler.UpdateTimer(deltaTime);

                // met a jour et affiche le timer
                timer.UpdateTimer();
                timer.ShowTime(10, 10, font, 40, Color.White);

                // Draws the grid and the piece
                grid.PrintGrid(gridRows, gridColumns, offsetX, offsetY, cellSize, grid.NextPiece);
                MainMenu.DrawParticlesBackground(windowWidth, windowHeight);
                moveHandler.HandleInput(deltaTime);
                rotateHandler.HandleInput();

                if (grid.IsSpawnPositionFree() == false)
                {
                    leaderboard.AddScore(MainMenu.username, grid.Score);
                    MainMenu.username = "";
                    return 5;
                }

                if (Raylib.IsKeyPressed(KeyboardKey.Escape))
                {
                    return 2;
                }
            }
            return 99;
        }

        //Affiche le menu option
        public static int ShowOptionsMenu(Options options)
        {
            // liste les actions du menu option
            var actions = options.KeyBindings.Keys.ToList();
            int selectedIndex = 0;
            bool inOptionsMenu = true;

            while (inOptionsMenu && !Raylib.WindowShouldClose())
            {
                // Permet de détecter les touches du clavier pour la navigation dans le menu option
                if (Raylib.IsKeyPressed(KeyboardKey.Down))
                {
                    selectedIndex = (selectedIndex + 1) % (actions.Count + 1); // Inclure la barre de volume
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.Up))
                {
                    selectedIndex = (selectedIndex - 1 + actions.Count + 1) % (actions.Count + 1); // Inclure la barre de volume
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.Enter) && selectedIndex < actions.Count)
                {
                    // Permet de modifier la touche de l'action souhaitée
                    string action = actions[selectedIndex];
                    NewKey(options, action);
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.Escape))
                {
                    inOptionsMenu = false;
                    return 0;
                }
                else if (selectedIndex == actions.Count) // Contrôles pour la barre de volume
                {
                    if (Raylib.IsKeyPressed(KeyboardKey.Right))
                    {
                        options.Volume = Math.Min(options.Volume + 0.1f, 1.0f); // Augmenter le volume
                        AudioGame.Volume = options.Volume;
                        Console.WriteLine("++");
                        Console.WriteLine(options.Volume);
                        Console.WriteLine(AudioGame.Volume);
                        AudioGame.PlayMusicStream(AudioGame.musicBackgroundMainMenu1);
                    }
                    else if (Raylib.IsKeyPressed(KeyboardKey.Left))
                    {
                        options.Volume = Math.Max(options.Volume - 0.1f, 0.0f); // Diminuer le volume
                        AudioGame.Volume = options.Volume;
                        Console.WriteLine("--");
                        Console.WriteLine(options.Volume);
                        Console.WriteLine(AudioGame.Volume);
                        AudioGame.PlayMusicStream(AudioGame.musicBackgroundMainMenu1);
                    }
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                Raylib.DrawText("Options Menu - Press ESCAPE to Exit", 200, 10, 20, Color.White);

                //affiche les actions et les touches associées dans le menu option
                for (int i = 0; i < actions.Count; i++)
                {
                    Color textColor = (i == selectedIndex) ? Color.Yellow : Color.White;
                    Raylib.DrawText($"{actions[i]}: {options.KeyBindings[actions[i]]}",
                                    50, 50 + i * 30, 20, textColor);
                }

                Color volumeColor = (selectedIndex == actions.Count) ? Color.Yellow : Color.White;
                Raylib.DrawText("Volume:", 50, 50 + actions.Count * 30, 20, volumeColor);
                Raylib.DrawRectangle(150, 50 + actions.Count * 30, 200, 20, Color.Gray);
                Raylib.DrawRectangle(150, 50 + actions.Count * 30, (int)(200 * options.Volume), 20, Color.Green);

                MainMenu.DrawParticlesBackground(Raylib.GetRenderWidth(), Raylib.GetRenderHeight());
                Raylib.EndDrawing();
            }
            return 0;
        }

        // permet de changer la touche d'une action dans le menu option
        private static void NewKey(Options options, string action)
        {
            bool waitingForKey = true;

            //attent qu'une touche choisi soit pressée par l utilisateur
            while (waitingForKey && !Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                Raylib.DrawText($"Press a key for {action}", 260, 250, 20, Color.White);
                Raylib.EndDrawing();

                // parcours toutes les touches du clavier
                foreach (KeyboardKey key in Enum.GetValues(typeof(KeyboardKey)))
                {
                    if (Raylib.IsKeyPressed(key))
                    {
                        // met a jour la touche de l'action
                        options.SetKey(action, key);
                        waitingForKey = false;
                        break;
                    }
                }
            }
        }
    }
}
