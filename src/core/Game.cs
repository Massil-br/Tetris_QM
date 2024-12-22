using System;
using System.Numerics;
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
            Font TitleFont = Raylib.LoadFont("assets/font/Team 401.ttf");
            
            

            




            //this while loop is called every frame
            while(!Raylib.WindowShouldClose()){
                int windowHeight = Raylib.GetRenderHeight();
                int windowWidth = Raylib.GetRenderWidth();
                GameLoop(windowHeight,windowWidth, grid);
                
            }
            Raylib.CloseWindow();
        }

        public static void GameLoop(int windowHeight, int windowWidth, Grid grid){
            grid.GridArray[5, 3] = 1;
            grid.GridArray[10, 4] = 1;
            grid.GridArray[15, 6] = 1;
            int cellSize = Math.Min(windowWidth / (gridColumns + 2), windowHeight / (gridRows + 2));

            int offsetX = (windowWidth - (gridColumns * cellSize)) / 2;
            int offsetY = (windowHeight - (gridRows * cellSize)) / 2;


            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            for (int row = 0; row < gridRows; row++){
                for (int col = 0; col < gridColumns; col++){
                    if (grid.GridArray[row, col] != 0){
                        int x = offsetX + (col * cellSize);
                        int y = offsetY + (row * cellSize);
                        Raylib.DrawRectangle(x, y, cellSize, cellSize, Color.White);
                    }
                    int borderX = offsetX + (col * cellSize);
                    int borderY = offsetY + (row * cellSize);
                    Raylib.DrawRectangleLines(borderX, borderY, cellSize, cellSize, Color.Gray);
                }
            }
            Raylib.EndDrawing();
        }

                   
    }
}

