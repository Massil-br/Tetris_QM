using Raylib_cs;

namespace Tetris_QMJ.src.Audio
{
    public class AudioGame
    {   
        // Useful variable for game music

        public static float volume = 0.5f;
        private static bool isMusicPlaying = false;

        // Variables for each music/sound in the game --> initialized in the InitAudioGame() function

        public static Music musicBackgroundMainMenu1;
        public static Music musicBackgroundMainMenu2;
        public static Sound musicStartButton;
        public static Sound soundPieceRotate;
        public static Sound soundPieceMove;
        public static Sound soundButtonMenu;
        public static Sound soundClearLineGrid;
        public static Music musicBackgroundInGame;

        public static void InitAudioGame()
        {
            musicBackgroundMainMenu1 = Raylib.LoadMusicStream("assets/Audio/musicBackgroundMainMenu1.mp3");
            musicBackgroundMainMenu2 = Raylib.LoadMusicStream("assets/Audio/musicBackgroundMainMenu2.mp3");
            musicStartButton = Raylib.LoadSound("assets/Audio/soundButtonClick.wav");
            soundPieceRotate = Raylib.LoadSound("assets/Audio/soundRotate.wav");
            soundPieceMove = Raylib.LoadSound("assets/Audio/pieceMove.wav");
            soundButtonMenu = Raylib.LoadSound("assets/Audio/soundButtonClick.wav");
            soundClearLineGrid = Raylib.LoadSound("assets/Audio/soundSuccess.wav"); 
        }

        // PlayMusicStream() function streams music continuously; if called while music is already playing, it replaces the current music

        public static void PlayMusicStream(Music music)
        {
            if (!isMusicPlaying)
            {
                Raylib.PlayMusicStream(music);
                Raylib.SetMusicVolume(music, volume);
                isMusicPlaying = true;
            }
            Raylib.UpdateMusicStream(music);
        }

        // PlaySound() function plays a given sound

        public static void PlaySound(Sound sound)
        {   
            Raylib.SetSoundVolume(sound, volume);
            Raylib.PlaySound(sound);
        }

        // UnloadAudioResources() function deinitializes the variables corresponding to the game's music/sound 
        public static void UnloadAudioResources()
        {
            Raylib.UnloadMusicStream(musicBackgroundMainMenu1);
            Raylib.UnloadMusicStream(musicBackgroundMainMenu2);
            Raylib.UnloadSound(musicStartButton);
            Raylib.UnloadSound(soundPieceRotate);
            Raylib.UnloadSound(soundPieceMove);
            Raylib.UnloadSound(soundButtonMenu);
            Raylib.UnloadSound(soundClearLineGrid);
            Raylib.UnloadMusicStream(musicBackgroundInGame);
        }
    }
}