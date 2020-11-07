using System.Collections.Generic;
using UnityEngine.Events;

public class King : Pieces
{
    public override int Value => 10000;

    public override IEnumerable<(int, int)> GetMoves(Tile[,] tiles)
    {
        var (x, y) = Loc;
        var moves = new List<(int, int)>();

        for (var i = -1; i <= 1; i++)
        {
            for (var j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                if (!ValidCoord((x + i, y + j))) continue;
                if (tiles[x + i, y + j].piece == null) { moves.Add((x + i, y + j)); }
                else if (tiles[x + i, y + j].piece.team != team) moves.Add((x + i, y + j));
            }
        }

        return moves;
    }

    protected override UnityEvent Attack()
    {
        return ActionFinished;
    }

    public override void SetSelected(bool selected)
    {
        animator.SetBool("Selected", selected);
    }
}
