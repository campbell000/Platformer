using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformerGame.GameStates
{
    public enum GameState
    {
        //Default state
        PLAY, 
        
        //State where nothing should be happening except for listening to an unpause command.
        //EFFECT: NOTHING MOVES, NOTHING IS UPDATED
        PAUSED, 
        
        //A State where the user can interact with a menu
        MENU, 
        
        //A state where the user can see text on the screen, and occassionally make dialog choices.
        DIALOG
    }
}
