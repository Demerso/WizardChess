using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class Pieces : MonoBehaviour
{
    [SerializeField] private Outline outline;
    [SerializeField] protected Animator animator;
    
    public (int, int) Loc;
    public Game.Team team;
    public bool hasMoved = false;
    
    private Collider[] _colliders;
    private Rigidbody[] _rigidbodies;
    private NavMeshAgent _agent;
    public abstract int Value { get; }

    protected UnityEvent ActionFinished;
    protected Quaternion DefaultDirection;
    
    
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _colliders = GetComponentsInChildren<Collider>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        DefaultDirection = team == Game.Team.Light? 
            new Quaternion(0,0,0,1) : 
            new Quaternion(0,1,0,0);
        transform.rotation = DefaultDirection;
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

    private IEnumerator _move(UnityEvent finished, Tile tile)
    {
        hasMoved = true;
        _agent.SetDestination(tile.transform.position);
        animator.SetBool("Walking", true);
        yield return null;
        if (tile.piece != null && tile.piece.team != team)
        {
            tile.piece.SetRagdoll(true);
            //Attack();
        }
        Loc = tile.Location;
        tile.piece = this;
        yield return new WaitUntil(_notMoving);
        animator.SetBool("Walking", false);
        finished.Invoke();
        StartCoroutine(ResetRotation());
    }
    
    private bool _notMoving()
    {
        return _agent.pathStatus == NavMeshPathStatus.PathComplete && _agent.remainingDistance < 0.1f;
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
            coll.gameObject.layer = ragdoll ? 0 : 2;
        }
        foreach (var body in _rigidbodies)
        {
            body.isKinematic = !ragdoll;
        }
        GetComponentInChildren<Animator>().enabled = !ragdoll;
    }

    public abstract void SetSelected(bool selected);

    private IEnumerator ResetRotation()
    {
        while (Mathf.Abs(Quaternion.Angle(transform.rotation, DefaultDirection)) > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                DefaultDirection, 
                0.1f);
            yield return null;
        }
    }
}
