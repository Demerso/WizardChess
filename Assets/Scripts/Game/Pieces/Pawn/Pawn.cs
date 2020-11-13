using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pawn : Pieces
{
    [SerializeField] private PawnAnimationHelper animationHelper;
    
    public override int Value => 10;

    private const float AttackStopDistance = 5;

    public override IEnumerable<(int, int)> GetMoves(Tile[,] tiles)
    {
        var (x, y) = Loc;
        var moves = new List<(int, int)>();
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
        if (ValidCoord((x + 1, y + dir)) && tiles[x + 1, y + dir].piece != null && tiles[x + 1, y + dir].piece.team != team)
            moves.Add((x + 1, y + dir));
        if (ValidCoord((x - 1, y + dir)) && tiles[x - 1, y + dir].piece != null && tiles[x - 1, y + dir].piece.team != team)
            moves.Add((x - 1, y + dir));
        return moves;
    }

    protected override UnityEvent Attack()
    {
        return ActionFinished;
    }

    public override IEnumerator Die()
    {
        SoundPlayer.sp.Play("PawnDie");
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
        animator.SetBool("Selected", selected);
    }
}
