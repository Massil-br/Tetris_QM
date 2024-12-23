using Raylib_cs;
using System.Numerics;

public class MainMenu{
    public static void PrintMainMenu(int screenWidth, int screenHeight, Font Font){
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        string title = "TETRIS";
        float fontSize = screenHeight / 10.0f;
        float spacing = fontSize / 8.0f;
        float totalWidth = 0;
        foreach (char c in title){
            totalWidth += Raylib.MeasureTextEx(Font, c.ToString(), fontSize, spacing).X;
        }
        float startX = (screenWidth - totalWidth) / 2.0f;
        float startY = screenHeight / 4.0f;

        Color[] colors = { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Purple };
        Vector2 position = new Vector2(startX, startY);

        for (int i = 0; i < title.Length; i++){
            char c = title[i];
            Color color = colors[i % colors.Length];
            string letter = c.ToString();
            Raylib.DrawTextEx(Font, letter, position, fontSize, spacing, color);
            position.X += Raylib.MeasureTextEx(Font, letter, fontSize, spacing).X;
        }
        Raylib.EndDrawing();
    }
}