using System.Collections.Generic;
using UnityEngine.Events;

public class Knight : Pieces
{
    private new void Start()
    {
        base.Start();
        if (team == Game.Team.Light)
        {
            value = 30;
        }
        else
        {
            value = -30;
        }
    }
    private static readonly (int, int)[] DefaultMoves =
    {
        (2, 1), (1, 2), (-1, 2), (-2, 1),
        (-2, -1), (-1, -2), (1, -2), (2, -1)
    };

    public override IEnumerable<(int, int)> GetMoves(Tile[,] tiles)
    {
        var (x, y) = Loc;
        var moves = new List<(int, int)>();

        foreach (var (i, j) in DefaultMoves)
        {
            if (!ValidCoord((x + i, y + j))) continue;
            if (tiles[x + i, y + j].piece == null) moves.Add((x + i, y + j));
            else if (tiles[x + i, y + j].piece.team != team) moves.Add((x + i, y + j));
        }

        return moves;
    }

    protected override UnityEvent Attack()
    {
        return ActionFinished;
    }
}
