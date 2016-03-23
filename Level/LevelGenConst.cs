using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformerGame.Level
{
    class LevelGenConst
    {
        public const int TILE_WIDTH = 64;
        public const int TILE_HEIGHT = 64;


        //Constants for reading tiles
        public const String TILE = "-";
        public const String SLOPE_UP_TILE = "/";
        public const String SLOPE_DOWN_TILE = "\\";
        public const String PLAYER = "P";
        public const String EMPTY_TILE = "0";
        public const String SLOW_SLOPE_DOWN_1 = "\\s1";
        public const String SLOW_SLOPE_DOWN_2 = "\\s2";
        public const String SLOW_SLOPE_UP_1 = "/s1";
        public const String SLOW_SLOPE_UP_2 = "/s2";
        public const String NPC = "N";
    }
}
