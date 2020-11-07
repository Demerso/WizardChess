using System.Collections.Generic;
using UnityEngine.Events;

public class Bishop : Pieces
{
    public override int Value
    {
        get => 30 * (team == Game.Team.Light ? 1 : -1);
    }
    
    public override IEnumerable<(int, int)> GetMoves(Tile[,] tiles)
    {
        var (x, y) = Loc;
        var moves = new List<(int, int)>();
        // First diagonal (x+, y+)
        for (var i = 1; i < 8; i++)
        {
            if (!ValidCoord((x + i, y + i))) break;
            if (tiles[x + i, y + i].piece != null)
            {
                if (tiles[x + i, y + i].piece.team != team) moves.Add((x + i, y + i));
                break;
            }
            moves.Add((x + i, y + i));
        }
        // Second diagonal (x-, y+)
        for (var i = 1; i < 8; i++)
        {
            if (!ValidCoord((x - i, y + i))) break;
            if (tiles[x - i, y + i].piece != null)
            {
                if (tiles[x - i, y + i].piece.team != team) moves.Add((x - i, y + i));
                break;
            }
            moves.Add((x - i, y + i));
        }
        // Third diagonal (x+, y-)
        for (var i = 1; i < 8; i++)
        {
            if (!ValidCoord((x + i, y - i))) break;
            if (tiles[x + i, y - i].piece != null)
            {
                if (tiles[x + i, y - i].piece.team != team) moves.Add((x + i, y - i));
                break;
            }
            moves.Add((x + i, y - i));
        }
        // Fourth diagonal (x-, y-)
        for (var i = 1; i < 8; i++)
        {
            if (!ValidCoord((x - i, y - i))) break;
            if (tiles[x - i, y - i].piece != null)
            {
                if (tiles[x - i, y - i].piece.team != team) moves.Add((x - i, y - i));
                break;
            }
            moves.Add((x - i, y - i));
        }
        return moves;
    }

    protected override UnityEvent Attack()
    {
        return ActionFinished;
    }

    public override void SetSelected(bool selected)
    {
        if (selected) animator.SetTrigger("Selected");
    }
}
