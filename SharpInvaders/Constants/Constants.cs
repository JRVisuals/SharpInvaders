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

        // Enemy Saucer
        public const int ENEMYSAUCER_STARTY = 80;
        public const float ENEMYSAUCER_SPEEDX = 1.0f;
        public const float ENEMYSAUCER_SPEEDX_MAX = 4.0f;

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
"EnemyEyes/idle/0;0;35;131;31;32;32;32;0;0",
"EnemyEyes/idle/1;0;1;131;32;32;32;32;0;0",
"EnemyEyes/idle/2;0;169;66;30;31;32;32;0;-0.03225806451612903",
"EnemyEyes/idle/3;0;136;198;29;29;32;32;-0.034482758620689655;-0.06896551724137931",
"EnemyEyes/idle/4;0;170;99;30;31;32;32;0;0",
"EnemyEyes/pop/0;0;103;161;31;31;32;32;-0.03225806451612903;0",
"EnemyEyes/pop/1;0;67;93;32;32;32;32;0;0",
"EnemyEyes/pop/2;0;1;165;32;32;32;32;0;0",
"EnemyEyes/pop/3;0;35;165;32;32;32;32;0;0",
"EnemyEyes/pop/4;0;1;199;32;28;32;32;0;-0.14285714285714285",
"EnemyEyes/pop/5;0;198;198;25;29;32;32;-0.08;-0.10344827586206896",
"EnemyPinks/idle/0;0;167;32;31;32;32;32;0;0",
"EnemyPinks/idle/1;0;133;32;32;32;32;32;0;0",
"EnemyPinks/idle/2;0;136;165;30;31;32;32;0;-0.03225806451612903",
"EnemyPinks/idle/3;0;167;198;29;29;32;32;-0.034482758620689655;-0.06896551724137931",
"EnemyPinks/idle/4;0;169;132;30;31;32;32;0;0",
"EnemyPinks/pop/0;0;103;194;31;31;32;32;-0.03225806451612903;0",
"EnemyPinks/pop/1;0;101;93;32;32;32;32;0;0",
"EnemyPinks/pop/2;0;68;127;32;32;32;32;0;0",
"EnemyPinks/pop/3;0;102;127;32;32;32;32;0;0",
"EnemyPinks/pop/4;0;35;199;32;28;32;32;0;-0.14285714285714285",
"EnemyPinks/pop/5;0;201;133;25;29;32;32;-0.08;-0.10344827586206896",
"EnemySaucer/idle/0;0;1;69;64;29;64;32;0;-0.034482758620689655",
"EnemySaucer/idle/1;0;67;33;64;29;64;32;0;-0.034482758620689655",
"EnemySaucer/idle/2;0;133;1;64;29;64;32;0;-0.034482758620689655",
"EnemySaucer/idle/3;0;1;100;64;29;64;32;0;-0.034482758620689655",
"EnemySaucer/pop/0;0;67;64;63;27;64;32;0;-0.07407407407407407",
"EnemySaucer/pop/1;0;67;1;64;30;64;32;0;0",
"EnemySaucer/pop/2;0;1;1;64;32;64;32;0;0",
"EnemySaucer/pop/3;0;1;35;64;32;64;32;0;0",
"EnemySquid/idle/0;0;168;165;30;31;32;32;-0.03333333333333333;-0.03225806451612903",
"EnemySquid/idle/1;0;200;31;30;32;32;32;-0.03333333333333333;0",
"EnemySquid/idle/2;0;202;99;26;32;32;32;-0.11538461538461539;0",
"EnemySquid/idle/3;0;201;65;30;32;32;32;-0.03333333333333333;0",
"EnemySquid/idle/4;0;135;66;32;31;32;32;0;-0.03225806451612903",
"EnemySquid/pop/0;0;136;132;31;31;32;32;-0.03225806451612903;-0.03225806451612903",
"EnemySquid/pop/1;0;136;99;32;31;32;32;0;-0.03225806451612903",
"EnemySquid/pop/2;0;69;161;32;32;32;32;0;0",
"EnemySquid/pop/3;0;69;195;32;32;32;32;0;0",
"EnemySquid/pop/4;0;199;1;32;28;32;32;0;-0.14285714285714285",
"EnemySquid/pop/5;0;200;165;25;29;32;32;-0.08;-0.10344827586206896",
        };

    }
}