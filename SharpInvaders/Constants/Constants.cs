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
        public const int ENEMY_MAXY = 592;

        public const float ENEMY_SPEEDX = 0.5f;
        public const float ENEMY_SPEEDX_MAX = 0.5f;

        // Enemy Saucer
        public const int ENEMYSAUCER_STARTY = 80;
        public const float ENEMYSAUCER_SPEEDX = 0.5f;
        public const float ENEMYSAUCER_SPEEDX_MAX = 3.0f;

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
"EnemyBullet/idle/0;0;299;1;6;13;6;13;0;0",
"EnemyBullet/idle/1;0;299;16;6;13;6;13;0;0",
"EnemyEyes/idle/0;0;233;32;31;32;32;32;0;0",
"EnemyEyes/idle/1;0;199;32;32;32;32;32;0;0",
"EnemyEyes/idle/2;0;233;99;30;31;32;32;0;-0.03225806451612903",
"EnemyEyes/idle/3;0;199;132;29;29;32;32;-0.034482758620689655;-0.06896551724137931",
"EnemyEyes/idle/4;0;230;162;30;31;32;32;0;0",
"EnemyEyes/pop/0;0;266;31;31;31;32;32;-0.03225806451612903;0",
"EnemyEyes/pop/1;0;67;163;32;32;32;32;0;0",
"EnemyEyes/pop/2;0;133;95;32;32;32;32;0;0",
"EnemyEyes/pop/3;0;67;197;32;32;32;32;0;0",
"EnemyEyes/pop/4;0;265;1;32;28;32;32;0;-0.14285714285714285",
"EnemyEyes/pop/5;0;294;162;25;29;32;32;-0.08;-0.10344827586206896",
"EnemyPinks/idle/0;0;135;163;31;32;32;32;0;0",
"EnemyPinks/idle/1;0;133;129;32;32;32;32;0;0",
"EnemyPinks/idle/2;0;203;197;30;31;32;32;0;-0.03225806451612903",
"EnemyPinks/idle/3;0;266;97;29;29;32;32;-0.034482758620689655;-0.06896551724137931",
"EnemyPinks/idle/4;0;235;195;30;31;32;32;0;0",
"EnemyPinks/pop/0;0;233;66;31;31;32;32;-0.03225806451612903;0",
"EnemyPinks/pop/1;0;101;163;32;32;32;32;0;0",
"EnemyPinks/pop/2;0;101;197;32;32;32;32;0;0",
"EnemyPinks/pop/3;0;135;197;32;32;32;32;0;0",
"EnemyPinks/pop/4;0;230;132;32;28;32;32;0;-0.14285714285714285",
"EnemyPinks/pop/5;0;294;193;25;29;32;32;-0.08;-0.10344827586206896",
"EnemySaucer/idle/0;0;67;101;64;29;64;32;0;-0.034482758620689655",
"EnemySaucer/idle/1;0;133;35;64;29;64;32;0;-0.034482758620689655",
"EnemySaucer/idle/2;0;199;1;64;29;64;32;0;-0.034482758620689655",
"EnemySaucer/idle/3;0;67;132;64;29;64;32;0;-0.034482758620689655",
"EnemySaucer/pop/0;0;133;66;63;27;64;32;0;-0.07407407407407407",
"EnemySaucer/pop/1;0;1;199;64;30;64;32;0;0",
"EnemySaucer/pop/2;0;67;67;64;32;64;32;0;0",
"EnemySaucer/pop/3;0;133;1;64;32;64;32;0;0",
"EnemySquid/idle/0;0;262;162;30;31;32;32;-0.03333333333333333;-0.03225806451612903",
"EnemySquid/idle/1;0;167;95;30;32;32;32;-0.03333333333333333;0",
"EnemySquid/idle/2;0;202;163;26;32;32;32;-0.11538461538461539;0",
"EnemySquid/idle/3;0;167;129;30;32;32;32;-0.03333333333333333;0",
"EnemySquid/idle/4;0;199;66;32;31;32;32;0;-0.03225806451612903",
"EnemySquid/pop/0;0;266;64;31;31;32;32;-0.03225806451612903;-0.03225806451612903",
"EnemySquid/pop/1;0;199;99;32;31;32;32;0;-0.03225806451612903",
"EnemySquid/pop/2;0;168;163;32;32;32;32;0;0",
"EnemySquid/pop/3;0;169;197;32;32;32;32;0;0",
"EnemySquid/pop/4;0;264;132;32;28;32;32;0;-0.14285714285714285",
"EnemySquid/pop/5;0;267;195;25;29;32;32;-0.08;-0.10344827586206896",
"Player/fire/0;0;1;1;64;64;64;64;0;0",
"Player/fire/1;0;1;67;64;64;64;64;0;0",
"Player/idle/0;0;67;1;64;64;64;64;0;0",
"Player/idle/1;0;1;133;64;64;64;64;0;0"
        };

    }
}