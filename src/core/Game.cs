using System;
using System.Numerics;
using Raylib_cs;


namespace Tetris_QMJ.src.Core{
    public class Game{
        public static void InitWindow(){
            int width = 1000;
            int heigth = 1000;
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
            Raylib.InitWindow(width ,heigth,"Tetris");
            
            Raylib.SetTargetFPS(60);
            Font TitleFont = Raylib.LoadFont("assets/font/Team 401.ttf");
            while(!Raylib.WindowShouldClose()){
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                
                Raylib.DrawTextEx(TitleFont, "TETRIS", new Vector2((Raylib.GetRenderWidth()/2)-35,50), 40, 2, Color.Red);
                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }
                   
    }
}

