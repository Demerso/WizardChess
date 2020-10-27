using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Pieces : MonoBehaviour
{
    private (int, int) _loc;
    
    public NavMeshAgent agent;
    public Game.Team team;

    private bool _moving = false;

    private UnityEvent _stoppedMoving;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public IEnumerable<(int,int)> GetMoves(Tile[,] tiles)
    {
        var (x, y) = _loc;
        var moves = new List<(int,int)>();
        // First diagonal (x+, y+)
        for (var i = 1; i < 8; i++)
        {
            if (x + i > 7 || y + i > 7) break;
            if (tiles[x + i, y + i].piece != null)
            {
                if (tiles[x + i, y + i].piece.team != team) moves.Add((x+i, y+i));
                break;
            }
            moves.Add((x+i, y+i));
        }
        // Second diagonal (x-, y+)
        for (var i = 1; i < 8; i++)
        {
            if (x - i < 0 || y + i > 7) break;
            if (tiles[x - i, y + i].piece != null)
            {
                if (tiles[x - i, y + i].piece.team != team) moves.Add((x-i, y+i));
                break;
            }
            moves.Add((x-i, y+i));
        }
        // Third diagonal (x+, y-)
        for (var i = 1; i < 8; i++)
        {
            if (x+i > 7 || y-i < 0) break;
            if (tiles[x+i, y-i].piece != null)
            {
                if (tiles[x+i, y-i].piece.team != team) moves.Add((x+i, y-i));
                break;
            }
            moves.Add((x+i, y-i));
        }
        // Fourth diagonal (x-, y-)
        for (var i = 1; i < 8; i++)
        {
            if (x-i < 0 || y-i < 0) break;
            if (tiles[x-i, y-i].piece != null)
            {
                if (tiles[x-i, y-i].piece.team != team) moves.Add((x-i, y-i));
                break;
            }
            moves.Add((x-i, y-i));
        }
        return moves;
    }
    

    //find a way to change location, maybe change to tile
    public UnityEvent Move(Tile t)
    {
        _loc = t.Location;
        agent.SetDestination(t.transform.position);
        _moving = true;
        _stoppedMoving = new UnityEvent();
        return _stoppedMoving;
    }

    public void Update()
    {
        if (_moving && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < 0.05f)
        {
            _moving = false;
            _stoppedMoving.Invoke();
        }
    }
}
