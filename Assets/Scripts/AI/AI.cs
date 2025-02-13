﻿using System;
using System.Collections;

public abstract class AI
{
    protected readonly Board Board;
    protected readonly Game Game;
    

    protected AI() {}
    
    protected AI(Game game, Board board)
    {
        Game = game;
        Board = board;
    }

    public void Move((Pieces piece, int x, int y) move)
    {
        var (piece, x, y) = move;
        var (px, py) = piece.Loc;
        piece.Move(Board.Tiles[x, y]).AddListener(Game.EndTurn);
        Board.Tiles[px, py].piece = null;
    }
    
    public abstract IEnumerator SelectMove(Action<(Pieces, int, int)> action);
    
}
