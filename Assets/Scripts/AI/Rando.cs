using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rando : AI
{

    public Rando(Game game, Board board) : base(game, board) {}
    
    public override IEnumerator SelectMove(Action<(Pieces, int, int)> action)
    {
        yield return new WaitWhile(() => Game.UIIsOpen);
        var possibleMoves = new List<(Pieces, int, int)>();

        foreach (var tile in Board.Tiles)
        {
            if (tile.piece == null || tile.piece.team != Game.currTeam) continue;
            foreach (var (x, y) in tile.piece.GetMoves(Board.Tiles))
                possibleMoves.Add((tile.piece, x, y));
            yield return null;
        }

        action(possibleMoves[Random.Range(0, possibleMoves.Count - 1)]);
    }
    
}
