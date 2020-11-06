using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class Pieces : MonoBehaviour
{
    public (int, int) Loc;

    private NavMeshAgent _agent;
    public Game.Team team;
    public bool hasMoved = false;
    public int value;
    private bool _moving = false;

    protected UnityEvent ActionFinished;

    protected void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public abstract IEnumerable<(int, int)> GetMoves(Tile[,] tiles);

    protected abstract UnityEvent Attack();


    public UnityEvent Move(Tile tile)
    {
        hasMoved = true;
        Loc = tile.Location;

        _agent.SetDestination(tile.transform.position);
        _moving = true;
        ActionFinished = new UnityEvent();
        if (tile.piece != null && tile.piece.team != team) Attack();
        tile.piece = this;
        return ActionFinished;
    }
    public virtual void GhostMove(Tile tile)
    {

        Loc = tile.Location;
        tile.piece = this;
        // hasMoved = true;


    }


    public void Update()
    {
        if (_moving && _agent.pathStatus == NavMeshPathStatus.PathComplete && _agent.remainingDistance < 0.1f)
        {
            _moving = false;
            ActionFinished.Invoke();
        }
    }



    protected static bool ValidCoord((int, int) coord)
    {
        var (x, y) = coord;
        return x >= 0 && x < 8 && y >= 0 && y < 8;
    }
}
