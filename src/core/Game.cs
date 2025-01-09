using System;
using System.Linq;
using System.Linq.Expressions;
using Raylib_cs;
using Tetris_QMJ.src.Audio;
using Tetris_QMJ.src.Interfaces;


namespace Tetris_QMJ.src.Core{
    public class Game{

        // Initializes the window size at the start of the program

        const int width = 800;
        const int height = 600;

         // Initializes the 10*20 game grid

        const int gridColumns = 10;
        const int gridRows = 20;
        static  Grid grid = new(gridRows,gridColumns);
        static Options options = new Options();
        

        // The InitWindow() function first calls all functions that initialize different variables needed for the program
        // and then contains the main game loop
        public static void InitWindow()
        {
            
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow); // pour permettre le resize
            Raylib.InitWindow(width, height, "Tetris"); // init de la fenêtre
            Raylib.InitAudioDevice();
            AudioGame.InitAudioGame();
            Raylib.SetTargetFPS(165); // fps de la fenetre 
            Font MainMenuFont = Raylib.LoadFont("assets/font/Team 401.ttf");
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
                    if (EntryCode == 1){
                        grid = new Grid(gridRows,gridColumns);
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
                    Console.WriteLine("PAUUUUSE");
                    EntryCode = 0;  
                }
                else if (EntryCode == 3){
                    //OPTIONS MENU;
                    Console.WriteLine("option menuuuuu");
                    EntryCode = ShowOptionsMenu(options);
                    options.SaveKey();
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

        public static int GameLoop( Grid grid){
            int windowHeight; 
            int windowWidth ;
            int cellSize ;
            int offsetX ;
            int offsetY; 
            
            // Generates a new random piece
            Entities.Piece randomPiece = Entities.PieceFactory.GenerateRandomPiece(1);
            grid.AddPiece(randomPiece);
            grid.SetActivePiece(randomPiece);

            Rotation rotateHandler = new Rotation(grid);
            Move moveHandler = new Move(grid);
            moveHandler.SetPiece(randomPiece);
            
            Timer timer = new Timer();
            Font font = Raylib.LoadFont("assets/font/College Squad Regular.ttf");

            // The game loop continues as long as the window is not closed
            while (!Raylib.WindowShouldClose())
            {
                windowHeight= Raylib.GetRenderHeight();
                windowWidth = Raylib.GetRenderWidth();
                cellSize = Math.Min(windowWidth / (gridColumns + 2), windowHeight / (gridRows + 2));
                offsetX = (windowWidth - (gridColumns * cellSize)) / 2;
                offsetY = (windowHeight - (gridRows * cellSize)) / 2;
                AudioGame.PlayMusicStream(AudioGame.musicBackgroundMainMenu1);
                Raylib.BeginDrawing();  // Starts the drawing phase
                Raylib.ClearBackground(Color.Black);

                // Calculates deltaTime (time elapsed since the last frame)
                float deltaTime = Raylib.GetFrameTime();

                // Updates the timer and moves the piece if necessary
                moveHandler.UpdateTimer(deltaTime);

                // Updates and displays the timer
                timer.UpdateTimer();
                timer.ShowTime(10, 10, font, 40, Color.White);

                // Handles new pieces
                if (!grid.GetPiece().IsActive) 
                {
                    Entities.Piece newPiece = Entities.PieceFactory.GenerateRandomPiece(1);
                    grid.AddPiece(newPiece);
                    grid.SetActivePiece(newPiece);
                    moveHandler.SetPiece(newPiece);
                }

                // Draws the grid and the piece
                grid.PrintGrid(gridRows, gridColumns, offsetX, offsetY, cellSize);
                MainMenu.DrawParticlesBackground(windowWidth, windowHeight);
                moveHandler.HandleInput(deltaTime);
                rotateHandler.HandleInput();

                if (Raylib.IsKeyPressed(KeyboardKey.Escape))
                {
                    return 2;
                }
            }
            return 99;
        }

        public static int ShowOptionsMenu(Options options)
        {
            // Liste des actions pour lesquelles on peut configurer les touches
            var actions = options.KeyBindings.Keys.ToList();
            int selectedIndex = 0; // Indice de l'action actuellement sélectionnée
            bool inOptionsMenu = true; // Contrôle pour rester ou quitter le menu des options

            while (inOptionsMenu && !Raylib.WindowShouldClose())
            {
                // Détection des touches pour naviguer dans le menu
                if (Raylib.IsKeyPressed(KeyboardKey.Down))
                {
                    selectedIndex = (selectedIndex + 1) % actions.Count;
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.Up))
                {
                    selectedIndex = (selectedIndex - 1 + actions.Count) % actions.Count;
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                {
                    // Permet de modifier la touche pour l'action sélectionnée
                    string action = actions[selectedIndex];
                    SetNewKeyForAction(options, action);
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.Escape))
                {
                    // Quitte le menu des options
                    inOptionsMenu = false;
                    return 0;
                }

                // Affichage du menu des options
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.DarkGray);

                Raylib.DrawText("Options Menu - Press ESCAPE to Exit", 10, 10, 20, Color.White);

                // Affiche chaque action et met en surbrillance celle sélectionnée
                for (int i = 0; i < actions.Count; i++)
                {
                    Color textColor = (i == selectedIndex) ? Color.Yellow : Color.White;
                    Raylib.DrawText($"{actions[i]}: {options.KeyBindings[actions[i]]}",
                                    50, 50 + i * 30, 20, textColor);
                }

                Raylib.EndDrawing();
            }
            return 0;
        }

        private static void SetNewKeyForAction(Options options, string action)
        {
            bool waitingForKey = true;

            while (waitingForKey && !Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                Raylib.DrawText($"Press a key to set for {action}", 10, 10, 20, Color.White);
                Raylib.EndDrawing();

                // Vérifie si une touche est pressée
                foreach (KeyboardKey key in Enum.GetValues(typeof(KeyboardKey)))
                {
                    if (Raylib.IsKeyPressed(key))
                    {
                        // Met à jour la touche pour l'action spécifiée
                        options.SetKey(action, key);
                        waitingForKey = false;
                        break;
                    }
                }
            }
        }

       
    }
}

