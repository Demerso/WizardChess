using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class Pieces : MonoBehaviour
{
    [SerializeField] private Outline outline;
    [SerializeField] protected Animator animator;
    [SerializeField] protected NavMeshAgent agent;

    public (int, int) Loc;
    public Game game;
    public Game.Team team;
    public bool hasMoved = false;
    
    private Collider[] _colliders;
    private Rigidbody[] _rigidbodies;
    public abstract int Value { get; }

    protected UnityEvent ActionFinished;
    
    private Quaternion _defaultDirection;
    
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _colliders = GetComponentsInChildren<Collider>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _defaultDirection = team == Game.Team.Light? 
            new Quaternion(0,0,0,1) : 
            new Quaternion(0,1,0,0);
        transform.rotation = _defaultDirection;
        SetRagdoll(false);
    }

    public abstract IEnumerable<(int, int)> GetMoves(Tile[,] tiles);

    protected abstract UnityEvent Attack();


    public UnityEvent Move(Tile tile)
    {
        ActionFinished = new UnityEvent();
        StartCoroutine(_move(ActionFinished, tile));
        return ActionFinished;
    }

    public abstract IEnumerator Die();

    protected abstract IEnumerator _move(UnityEvent finished, Tile tile);

    protected bool _notMoving()
    {
        return agent.pathStatus == NavMeshPathStatus.PathComplete && 
               agent.remainingDistance - agent.stoppingDistance < 0.1f;
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
        agent.enabled = !ragdoll;
        foreach (var coll in _colliders)
        {
            coll.isTrigger = !ragdoll;
            coll.gameObject.layer = ragdoll ? 0 : 2;
        }
        foreach (var body in _rigidbodies)
        {
            body.isKinematic = !ragdoll;
        }
        GetComponentInChildren<Animator>().enabled = !ragdoll;
    }

    public abstract void SetSelected(bool selected);

    protected IEnumerator ResetRotation()
    {
        while (Mathf.Abs(Quaternion.Angle(transform.rotation, _defaultDirection)) > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                _defaultDirection, 
                0.1f);
            yield return null;
        }
    }

    public void Win()
    {
        animator.SetTrigger("Win");
    }
    
    public void Defeat()
    {
        animator.SetTrigger("Defeat");
    }
}
