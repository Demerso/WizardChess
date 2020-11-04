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
    [SerializeField] private Outline outline;
    
    public (int, int) Loc;
    
    private Collider[] _colliders;
    private Rigidbody[] _rigidbodies;

    private NavMeshAgent _agent;

    public Game.Team team;

    public bool hasMoved = false;

    private bool _moving = false;

    protected UnityEvent ActionFinished;
    
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _colliders = GetComponentsInChildren<Collider>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        SetRagdoll(false);
    }

    public abstract IEnumerable<(int, int)> GetMoves(Tile[,] tiles);

    protected abstract UnityEvent Attack();


    //find a way to change location, maybe change to tile
    public UnityEvent Move(Tile tile)
    {
        hasMoved = true;
        Loc = tile.Location;
        tile.piece = this;
        _agent.SetDestination(tile.transform.position);
        _moving = true;
        ActionFinished = new UnityEvent();
        if (tile.piece != null && tile.piece.team != team) Attack();
        return ActionFinished;
    }

    public void Update()
    {
        if (_moving && _agent.pathStatus == NavMeshPathStatus.PathComplete && _agent.remainingDistance < 0.1f)
        {
            _moving = false;
            ActionFinished.Invoke();
        }
    }

    public void SetTeam(Game.Team t)
    {
        team = t;
        if (outline != null) outline.OutlineColor = t == Game.Team.Light ? Color.white : Color.black;
    }

    protected static bool ValidCoord((int, int) coord)
    {
        var (x, y) = coord;
        return x >= 0 && x < 8 && y >= 0 && y < 8;
    }
    
    protected void SetRagdoll(bool ragdoll)
    {
        _agent.enabled = !ragdoll;
        foreach (var coll in _colliders)
        {
            coll.isTrigger = !ragdoll;
        }
        foreach (var body in _rigidbodies)
        {
            body.isKinematic = !ragdoll;
        }
        GetComponentInChildren<Animator>().enabled = !ragdoll;
    }
    
}
