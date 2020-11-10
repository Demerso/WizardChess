using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Knight : Pieces
{
    [SerializeField] private KnightAnimationHelper animationHelper;
    
    public override int Value => 30;
    
    private const float AttackStopDistance = 7;

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
        hasMoved = true;
        if (tile.piece != null && tile.piece.team != team)
        {
            agent.stoppingDistance = AttackStopDistance;
            agent.SetDestination(tile.transform.position);
            yield return null;
            animator.SetBool("Walking", true);
            yield return new WaitUntil(_notMoving);
            var dir = tile.piece.transform.position - transform.position;
            var toRotation = Quaternion.LookRotation(dir, Vector3.up);
            StartCoroutine(SetRotation(toRotation));
            yield return new WaitUntil(() => IsRotated(toRotation));
            animator.SetTrigger("Attack");
            yield return new WaitUntil(() => animationHelper.AttackHasHit);
            StartCoroutine(tile.piece.Die());
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));
        }
        agent.stoppingDistance = 0;
        agent.SetDestination(tile.transform.position);
        yield return null;
        animator.SetBool("Walking", true);
        Loc = tile.Location;
        tile.piece = this;
        yield return new WaitUntil(_notMoving);
        animator.SetBool("Walking", false);
        finished.Invoke();
        StartCoroutine(SetRotation(DefaultDirection));
    }

    public override void SetSelected(bool selected)
    {
        if (selected) animator.SetTrigger("Selected");
    }
}
