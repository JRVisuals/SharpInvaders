using System.Collections.Generic;
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
        public const int PLAYER_START_LIVES = 3;
        public const int PLAYER_BULLETMAX = 5;
        public const float PLAYER_ACCEL_X = 2f;
        public const float PLAYER_MAXVEL_X = 450f;
        public const float PLAYER_FRICMULT_X = 10f;
        public const float PLAYER_BULLINIT_Y = (float)(-GAME_HEIGHT * 0.5);
        public const double PLAYER_BULLETDELAY = 0.15;
        public const int PLAYER_OFFSET_Y = 18;
        public const int PLAYER_DEAD_MS = 750;
        public const int PLAYER_RESPAWN_SEC = 2;

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
        public const int ENEMY_MAXY = 285;

        public const float ENEMY_SPEEDX = 0.5f;
        public const float ENEMY_SPEEDX_MAX = 0.5f;

        // Audio
        public const float VOLUME_GLOBAL = 0.5f;
    }

    public static class Assets
    {
        // This constant replaces the tpSpriteSheet.txt file
        // I ran into pathing issues in SpriteSheetLoader.cs and rather than using
        // file streams just went with a traditional content load for the image
        // and the array blelow (which is what was meant to be returned when loading the .txt file)
        public static readonly string[] SPRITESHEET_DATA = new string[] {
"EnemyEyes/idle/0;0;35;136;31;32;32;32;0;0",
"EnemyEyes/idle/1;0;1;1;32;32;32;32;0;0",
"EnemyEyes/idle/2;0;33;203;30;31;32;32;0;-0.03225806451612903",
"EnemyEyes/idle/3;0;97;263;29;29;32;32;-0.034482758620689655;-0.06896551724137931",
"EnemyEyes/idle/4;0;1;205;30;31;32;32;0;0",
"EnemyEyes/pop/0;0;68;136;31;31;32;32;-0.03225806451612903;0",
"EnemyEyes/pop/1;0;35;1;32;32;32;32;0;0",
"EnemyEyes/pop/2;0;69;1;32;32;32;32;0;0",
"EnemyEyes/pop/3;0;1;35;32;32;32;32;0;0",
"EnemyEyes/pop/4;0;33;269;32;28;32;32;0;-0.14285714285714285",
"EnemyEyes/pop/5;0;101;170;25;29;32;32;-0.08;-0.10344827586206896",
"EnemyPinks/idle/0;0;1;137;31;32;32;32;0;0",
"EnemyPinks/idle/1;0;35;35;32;32;32;32;0;0",
"EnemyPinks/idle/2;0;33;236;30;31;32;32;0;-0.03225806451612903",
"EnemyPinks/idle/3;0;1;271;29;29;32;32;-0.034482758620689655;-0.06896551724137931",
"EnemyPinks/idle/4;0;1;238;30;31;32;32;0;0",
"EnemyPinks/pop/0;0;68;169;31;31;32;32;-0.03225806451612903;0",
"EnemyPinks/pop/1;0;69;35;32;32;32;32;0;0",
"EnemyPinks/pop/2;0;1;69;32;32;32;32;0;0",
"EnemyPinks/pop/3;0;35;69;32;32;32;32;0;0",
"EnemyPinks/pop/4;0;67;294;32;28;32;32;0;-0.14285714285714285",
"EnemyPinks/pop/5;0;101;201;25;29;32;32;-0.08;-0.10344827586206896",
"EnemySquid/idle/0;0;65;236;30;31;32;32;-0.03333333333333333;-0.03225806451612903",
"EnemySquid/idle/1;0;67;202;30;32;32;32;-0.03333333333333333;0",
"EnemySquid/idle/2;0;101;136;26;32;32;32;-0.11538461538461539;0",
"EnemySquid/idle/3;0;1;171;30;32;32;32;-0.03333333333333333;0",
"EnemySquid/idle/4;0;35;103;32;31;32;32;0;-0.03225806451612903",
"EnemySquid/pop/0;0;34;170;31;31;32;32;-0.03225806451612903;-0.03225806451612903",
"EnemySquid/pop/1;0;69;103;32;31;32;32;0;-0.03225806451612903",
"EnemySquid/pop/2;0;69;69;32;32;32;32;0;0",
"EnemySquid/pop/3;0;1;103;32;32;32;32;0;0",
"EnemySquid/pop/4;0;32;299;32;28;32;32;0;-0.14285714285714285",
"EnemySquid/pop/5;0;99;232;25;29;32;32;-0.08;-0.10344827586206896",
        };

    }
}