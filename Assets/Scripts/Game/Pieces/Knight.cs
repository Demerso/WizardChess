using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Knight : Pieces
{
    public override int Value => 30;

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

    public override IEnumerator Die()
    {
        SetRagdoll(true);
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }

    protected override IEnumerator _move(UnityEvent finished, Tile tile)
    {
        // TODO: Make for knight
        hasMoved = true;
        agent.SetDestination(tile.transform.position);
        animator.SetBool("Walking", true);
        yield return null;
        if (tile.piece != null && tile.piece.team != team)
        {
            StartCoroutine(tile.piece.Die());
        }
        
        Loc = tile.Location;
        tile.piece = this;
        yield return new WaitUntil(_notMoving);
        animator.SetBool("Walking", false);
        finished.Invoke();
        StartCoroutine(ResetRotation());
    }

    public override void SetSelected(bool selected)
    {
        if (selected) animator.SetTrigger("Selected");
    }
}
