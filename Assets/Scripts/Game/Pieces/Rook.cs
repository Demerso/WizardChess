using System.Collections.Generic;
using UnityEngine.Events;

public class Rook : Pieces
{
    
    public override int Value => 50;

    public override IEnumerable<(int, int)> GetMoves(Tile[,] tiles)
    {
        var (x, y) = Loc;
        var moves = new List<(int, int)>();
        // Straight 1
        for (var i = 1; i < 8; i++)
        {
            if (!ValidCoord((x + i, y))) break;
            if (tiles[x + i, y].piece != null)
            {
                if (tiles[x + i, y].piece.team != team) moves.Add((x + i, y));
                break;
            }
            moves.Add((x + i, y));
        }
        // Straight 2
        for (var i = 1; i < 8; i++)
        {
            if (!ValidCoord((x - i, y))) break;
            if (tiles[x - i, y].piece != null)
            {
                if (tiles[x - i, y].piece.team != team) moves.Add((x - i, y));
                break;
            }
            moves.Add((x - i, y));
        }
        // Straight 3
        for (var i = 1; i < 8; i++)
        {
            if (!ValidCoord((x, y + i))) break;
            if (tiles[x, y + i].piece != null)
            {
                if (tiles[x, y + i].piece.team != team) moves.Add((x, y + i));
                break;
            }
            moves.Add((x, y + i));
        }
        // Straight 4
        for (var i = 1; i < 8; i++)
        {
            if (!ValidCoord((x, y - i))) break;
            if (tiles[x, y - i].piece != null)
            {
                if (tiles[x, y - i].piece.team != team) moves.Add((x, y - i));
                break;
            }
            moves.Add((x, y - i));
        }
        return moves;
    }

    protected override UnityEvent Attack()
    {
        return ActionFinished;
    }

    public override void Die()
    {
        SetRagdoll(true);
    }

    public override void SetSelected(bool selected)
    {
        animator.SetTrigger("Selected");
    }
}
