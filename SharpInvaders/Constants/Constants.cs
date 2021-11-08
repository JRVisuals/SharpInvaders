
namespace SharpInvaders.Constants
{
    public static class Global
    {

        // Game Environment
        public const int GAME_WIDTH = 600;
        public const int GAME_HEIGHT = 800;

        public const bool USE_FIXED_STEP = true;
        public const double FIXED_STEP_MS = 10; // 10 is good and fast
        public const bool DEBUG = false;

        // Player
        public const float PLAYER_ACCEL_X = 2f;
        public const float PLAYER_MAXVEL_X = 450f;
        public const float PLAYER_FRICMULT_X = 7f;
        public const float PLAYER_BULLINIT_Y = (float)(-GAME_HEIGHT * 0.5);
        public const double PLAYER_BULLETDELAY = 0.15;
        public const int PLAYER_BULLETMAX = 5;
        public const int PLAYER_OFFSET_Y = 18;
        public const int PLAYER_RESPAWN_SEC = 3;

        // Bunkers
        public const int BUNKERS_TOTAL = 3;

        // Enemies
        public const int ENEMY_STARTY = 160;
        public const int ENEMY_COLS = 14;
        public const int ENEMY_ROWS = 5;
        public const int ENEMY_ROWGAP = 40;

        public const float ENEMY_BULLINIT_Y = (float)(GAME_HEIGHT * 0.25);
        public const int ENEMY_BULLETMAX = 5;
        public const double ENEMY_BULLETDELAY = 5.0;

        public const int ENEMY_DROPY = 8;
        public const int ENEMY_MAXY = 250;

        public const float ENEMY_SPEEDX = 0.5f;
        public const float ENEMY_SPEEDX_MAX = 0.5f;

        // Audio
        public const float VOLUME_GLOBAL = 0.5f;
    }

}