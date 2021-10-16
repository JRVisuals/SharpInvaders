
namespace SharpInvaders.Constants
{
    public static class Global
    {

        public const int GAME_WIDTH = 600;
        public const int GAME_HEIGHT = 800;

        public const float PLAYER_ACCEL_X = 2f;
        public const float PLAYER_MAXVEL_X = 450f;
        public const float PLAYER_FRICMULT_X = 7f;

        public const float PLAYER_BULLINIT_Y = (float)(-GAME_HEIGHT * 0.75);
        public const double PLAYER_BULLETDELAY = 0.15;
        public const int PLAYER_BULLETMAX = 10;

        public const int BUNKERS_TOTAL = 3;

        public const bool DEBUG = false;


        // Audio
        public const float VOLUME_GLOBAL = 0.5f;
    }

}