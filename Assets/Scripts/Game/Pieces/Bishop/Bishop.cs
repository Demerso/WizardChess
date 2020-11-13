using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bishop : Pieces
{

    [SerializeField] private BishopAnimationHelper animationHelper;

    private const float AttackStopDistance = 10;
    
    public override int Value => 30;

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

    public override IEnumerator Die()
    {
        SoundPlayer.sp.Play("BishopDie");
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
