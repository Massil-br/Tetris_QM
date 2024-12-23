using System;
using Raylib_cs;


namespace Tetris_QMJ.src.Core{
    public class Game{
        const int width = 800;
        const int height = 600;
        const int gridColumns = 10;
        const int gridRows = 20;
        static  Grid grid = new(gridRows,gridColumns);
        public static void InitWindow(){
            
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
            Raylib.InitWindow(width ,height,"Tetris");
            Raylib.SetTargetFPS(60);
            Font MainMenuFont = Raylib.LoadFont("/assets/font/Team 401.ttf");
            //this while loop is called every frame
            while(!Raylib.WindowShouldClose()){
                int windowHeight = Raylib.GetRenderHeight();
                int windowWidth = Raylib.GetRenderWidth();
                //MainMenu.PrintMainMenu(windowWidth,windowHeight, MainMenuFont);
                GameLoop(windowHeight,windowWidth, grid);
                
            }
            Raylib.CloseWindow();
        }

        public static void GameLoop(int windowHeight, int windowWidth, Grid grid){
            int cellSize = Math.Min(windowWidth / (gridColumns + 2), windowHeight / (gridRows + 2));
            int offsetX = (windowWidth - (gridColumns * cellSize)) / 2;
            int offsetY = (windowHeight - (gridRows * cellSize)) / 2;

            Entities.Piece randomPiece = Entities.PieceFactory.GenerateRandomPiece();
            grid.AddPiece(randomPiece);
            grid.PrintGrid(gridRows,gridColumns,offsetX,offsetY, cellSize);    

            
        }

                   
    }
}

