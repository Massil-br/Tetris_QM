using System.Numerics;
using Raylib_cs;

namespace Tetris_QMJ.src.Entities{
    class Particle{
        public Vector2 Position;
        public Vector2 Velocity;
        public Color Color;
        private static System.Random rand = new System.Random();
        private float alpha = 255; // Alpha pour simuler la traînée

        public Particle(float x, float y){
            Position = new Vector2(x, y);
            Velocity = new Vector2(Raylib.GetRandomValue(-50, 50), Raylib.GetRandomValue(-50, 50));
            Color = new Color((byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)255);
        }

        // Met à jour la position et l'alpha de la particule
        public void Update(int screenWidth, int screenHeight){
            Position += Velocity * 0.1f; // Déplacement de la particule
            alpha -= 0.5f; // Réduit l'alpha pour la traînée

            // Vérifie si la particule sort de l'écran
            if (Position.X < 0 || Position.X > screenWidth || Position.Y < 0 || Position.Y > screenHeight || alpha <= 0){
                Position = new Vector2(Raylib.GetRandomValue(0, screenWidth), Raylib.GetRandomValue(0, screenHeight));
                alpha = 255; // Réinitialiser l'alpha pour la prochaine apparition
            }
        }

        public void Draw(){
            Color particleColor = new Color(Color.R, Color.G, Color.B, (byte)alpha); // Ajuste l'alpha pour simuler la traînée
            Raylib.DrawCircleV(Position, 5, particleColor); 
        }
    }
}