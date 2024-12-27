using System;
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

        // The InitWindow() function first calls all functions that initialize different variables needed for the program
        // and then contains the main game loop
        public static void InitWindow()
        {
            
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
            Raylib.InitWindow(width, height, "Tetris");
            Raylib.InitAudioDevice();
            AudioGame.InitAudioGame();
            Raylib.SetTargetFPS(165);
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
                }
                // GAME
                else if (EntryCode == 1) 
                {   
                    AudioGame.PlaySound(AudioGame.soundButtonMenu);
                    grid = new Grid(gridRows,gridColumns);
                    EntryCode = GameLoop(grid);
                }
                // PAUSE
                else if (EntryCode == 2) 
                {
                    // Option window logic, if you have any.
                    AudioGame.PlaySound(AudioGame.soundButtonMenu);
                    Console.WriteLine("PAUUUUSE");
                    EntryCode = 0;  
                }
                // CLOSE WINDOW
                else if (EntryCode == 99)
                {   
                    AudioGame.PlaySound(AudioGame.soundButtonMenu);
                    Raylib.CloseWindow();
                    Environment.Exit(0);
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
                moveHandler.HandleInput(deltaTime);
                rotateHandler.HandleInput();

                if (Raylib.IsKeyPressed(KeyboardKey.Escape))
                {
                    return 2;
                }
            }
            return 99;
        }          
    }
}

