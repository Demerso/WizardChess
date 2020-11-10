using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class King : Pieces
{
    [SerializeField] private KingAnimationHelper animationHelper;
    [SerializeField] private GameObject explosion;
    
    private const float AttackDuration = 1.9f;
    
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

    public override IEnumerator Die()
    {
        SetRagdoll(true);
        game.KingDied(team);
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
        animator.SetBool("Selected", selected);
    }
}
