namespace SharpInvaders
{
    public class Constants
    {

        public const int GAME_WIDTH = 740;
        public const int GAME_HEIGHT = 900;

        public const float PLAYER_ACCEL_X = 2f;
        public const float PLAYER_MAXVEL_X = 450f;
        public const float PLAYER_FRICMULT_X = 7f;

        public const float PLAYER_BULLINIT_Y = (float)(-GAME_HEIGHT * 0.75);
        public const double PLAYER_BULLETDELAY = 0.15;
        public const int PLAYER_BULLETMAX = 10;
    }


}