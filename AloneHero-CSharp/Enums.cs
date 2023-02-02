using System;
using System.Collections.Generic;
using System.Text;

namespace AloneHero_CSharp
{
    enum Directions
    {
        RIGHT,
        LEFT
    };
    enum States
    {
        RUN,
        HIT,
        DAMAGE,
        IDLE,
        JUMP,
        FALL,
        DEATH
    };

    enum Codes
    {
        RUN_C,
        DAMAGE_C,
        IDLE_C,
        FALL_C,
        JUMP_C,
        HIT_C,
        CHANGE_X,
        ENEMY_BARIER,
        IMPROVE_STATS,
        HEALTH_UP,
        SPEED_UP,
        COIN_UP,
        COIN_DOWN,
        BLEED_C,
        HEALTH_UNITS,
        SPEED_UNITS,
        COIN_UNITS,
        END_GAME, 
        NEXT_LEVEL,
        PLAYER_STATS_MOVE
    };

    enum AddStates
    {
        NONE,
        BLEED
    };


    class Enums
    {
        
    }
}
