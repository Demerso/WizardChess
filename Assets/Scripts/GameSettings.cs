using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{

    public static PlayerType Player1;
    public static PlayerType Player2;
    
    public enum PlayerType
    {
        Player, EasyAI, HardAI
    }
}
