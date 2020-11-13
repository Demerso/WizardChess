using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Queen : Pieces
{
    [SerializeField] private QueenAnimationHelper animationHelper;
    [SerializeField] private GameObject explosion;

    private const float AttackDuration = 1.9f;
    
    public override int Value => 90;

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
        SoundPlayer.sp.Play("QueenDie");
        SetRagdoll(true);
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }

    protected override IEnumerator _move(UnityEvent finished, Tile tile)
    {
        hasMoved = true;
        if (tile.piece != null && tile.piece.team != team)
        {
            var dir = tile.piece.transform.position - transform.position;
            var toRotation = Quaternion.LookRotation(dir, Vector3.up);
            StartCoroutine(SetRotation(toRotation));
            yield return new WaitUntil(() => IsRotated(toRotation));
            animator.SetTrigger("Attack");
            yield return new WaitUntil(() => animationHelper.ShouldCastAttack);
            var exp = Instantiate(explosion, 
                tile.transform.position + Vector3.up * 3f, Quaternion.identity);
            StartCoroutine(tile.piece.Die());
            yield return new WaitForSeconds(AttackDuration);
            Destroy(exp);
        }
        Loc = tile.Location;
        tile.piece = this;
        agent.SetDestination(tile.transform.position);
        animator.SetBool("Walking", true);
        yield return null;
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
