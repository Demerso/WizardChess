using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pawn : Pieces
{
    public override IEnumerable<(int, int)> GetMoves(Tile[,] tiles)
    {
        var (x, y) = Loc;
        var moves = new List<(int,int)>();
        var dir = team == Game.Team.Light ? 1 : -1;
        if (ValidCoord((x, y + dir)) && tiles[x, y + dir].piece == null)
        {
            moves.Add((x, y + dir)); // Move 1 forward
            if (ValidCoord((x, y + 2 * dir)) && tiles[x, y + 2 * dir].piece == null && !hasMoved)
            {
                moves.Add((x, y + 2 * dir)); // Move 2 forward if has not moved
            }
        }
        // Move diagonal
        if (ValidCoord((x+1, y+dir)) && tiles[x+1, y+dir].piece != null) moves.Add((x+1, y+dir));
        if (ValidCoord((x-1, y+dir)) && tiles[x-1, y+dir].piece != null) moves.Add((x-1, y+dir));
        return moves;
    }

    protected override UnityEvent Attack()
    {
        return _actionFinished;
    }
}
