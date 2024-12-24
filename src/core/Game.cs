using System;
using Raylib_cs;
using Tetris_QMJ.src.Audio;
using Tetris_QMJ.src.Interfaces;

namespace Tetris_QMJ.src.Core{
    public class Game{
        const int width = 800;
        const int height = 600;
        const int gridColumns = 10;
        const int gridRows = 20;
        static  Grid grid = new(gridRows,gridColumns);
        private static bool isPlaying = false;
        public static void InitWindow()
        {
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
            Raylib.InitWindow(width, height, "Tetris");
            Raylib.InitAudioDevice();
            AudioGame.InitAudioGame();
            Raylib.SetTargetFPS(165);
            Font MainMenuFont = Raylib.LoadFont("assets/font/Team 401.ttf");
            MainMenu.InitButtonTextures();
            
            int EntryCode = 0;
            while (!Raylib.WindowShouldClose())
            {
                int windowHeight = Raylib.GetRenderHeight();
                int windowWidth = Raylib.GetRenderWidth();
                if (EntryCode == 0){
                    EntryCode = MainMenu.PrintMainMenu(windowWidth, windowHeight, MainMenuFont);
                    AudioGame.PlayMusic(AudioGame.musicBackgroundMainMenu1);
                }
                else if (EntryCode == 1){    
                    GameLoop(windowHeight, windowWidth, grid);
                    isPlaying = false;
                }
                else if (EntryCode == 2){
                        // Option window logic, if you have any.
                }else if (EntryCode == 99){
                        Raylib.CloseWindow();
                        Environment.Exit(0);
                }
 
            }
            AudioGame.UnloadAudioResources();
            Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
        }

        public static void GameLoop(int windowHeight, int windowWidth, Grid grid){
            int cellSize = Math.Min(windowWidth / (gridColumns + 2), windowHeight / (gridRows + 2));
            int offsetX = (windowWidth - (gridColumns * cellSize)) / 2;
            int offsetY = (windowHeight - (gridRows * cellSize)) / 2;
            
            // Génère une nouvelle pièce aléatoire
            Entities.Piece randomPiece = Entities.PieceFactory.GenerateRandomPiece(1);
            grid.AddPiece(randomPiece);
            grid.SetActivePiece(randomPiece);

            Rotation rotateHandler = new Rotation(grid);
            Move moveHandler = new Move(grid);
            moveHandler.SetPiece(randomPiece);

            // Rotation rotateHandler = new Rotation(grid);

            // La boucle de jeu continue tant que la fenêtre n'est pas fermée
            while (!Raylib.WindowShouldClose()){
                Raylib.BeginDrawing();  // Démarre la phase de dessin
                Raylib.ClearBackground(Color.Black);  // Efface l'écran en noir

                // Calcul du deltaTime (temps écoulé depuis le dernier frame)
                float deltaTime = Raylib.GetFrameTime();

                // Met à jour le timer et déplace la pièce si nécessaire
                moveHandler.UpdateTimer(deltaTime);

                // Dessine la grille et la pièce
                grid.PrintGrid(gridRows, gridColumns, offsetX, offsetY, cellSize);
                moveHandler.HandleInput();
                rotateHandler.HandleInput();
                
            }
        }          
    }
}

