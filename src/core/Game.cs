using System;
using Raylib_cs;
using Tetris_QMJ.src.Interfaces;

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
            Font MainMenuFont = Raylib.LoadFont("assets/font/Team 401.ttf");
            MainMenu.InitButtonTextures();
            //this while loop is called every frame
            int EntryCode = 0;
            while(!Raylib.WindowShouldClose()){

                int windowHeight = Raylib.GetRenderHeight();
                int windowWidth = Raylib.GetRenderWidth();
                if(EntryCode == 0){
                    EntryCode = MainMenu.PrintMainMenu(windowWidth,windowHeight, MainMenuFont);
                }else if (EntryCode == 1){
                    GameLoop(windowHeight,windowWidth, grid);
                }else if (EntryCode == 2){
                    //optionWindowCode en dessous
                }else if (EntryCode == 99){
                    Raylib.CloseWindow();
                    Environment.Exit(0);
                } 
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

