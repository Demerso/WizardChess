using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class Pieces : MonoBehaviour
{
    protected (int, int) Loc;
    
    protected NavMeshAgent agent;
    public Game.Team team;
    public bool hasMoved = false;

    private bool _moving = false;

    protected UnityEvent _actionFinished;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public abstract IEnumerable<(int, int)> GetMoves(Tile[,] tiles);

    protected abstract UnityEvent Attack();


    //find a way to change location, maybe change to tile
    public UnityEvent Move(Tile tile)
    {
        hasMoved = true;
        Loc = tile.Location;
        agent.SetDestination(tile.transform.position);
        _moving = true;
        _actionFinished = new UnityEvent();
        if (tile.piece != null && tile.piece.team != team) Attack();
        return _actionFinished;
    }

    public void Update()
    {
        if (_moving && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < 0.05f)
        {
            _moving = false;
            _actionFinished.Invoke();
        }
    }

    protected static bool ValidCoord((int, int) coord)
    {
        var (x, y) = coord;
        return x >= 0 && x < 8 && y >= 0 && y < 8;
    }
}
