
namespace SharpInvaders.Constants
{
    public static class Global
    {

        // Game Environment
        public const int GAME_WIDTH = 600;
        public const int GAME_HEIGHT = 800;

        public const bool USE_FIXED_STEP = false;
        public const double FIXED_STEP_MS = 10;
        public const bool DEBUG = false;

        // Player
        public const float PLAYER_ACCEL_X = 2f;
        public const float PLAYER_MAXVEL_X = 450f;
        public const float PLAYER_FRICMULT_X = 7f;
        public const float PLAYER_BULLINIT_Y = (float)(-GAME_HEIGHT * 0.5);
        public const double PLAYER_BULLETDELAY = 0.25;
        public const int PLAYER_BULLETMAX = 3;

        // Bunkers
        public const int BUNKERS_TOTAL = 3;

        // Audio
        public const float VOLUME_GLOBAL = 0.5f;
    }

}