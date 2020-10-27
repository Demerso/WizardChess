using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class Pieces : MonoBehaviour
{
    public (int,int) location;
    public List<(int, int)> moves = new List<(int, int)>();
    public NavMeshAgent agent;
    private void Start()
    {
        setMoves();
        agent = GetComponent<NavMeshAgent>();

    }


    private void setMoves()
    {
        for(int i = 0; i < 3; i++)
        {
            moves.Add((location.Item1 + i, location.Item2 + i));
        }
    }

    //find a way to change location, maybe change to tile
    public void move(Tile t)
    {
        agent.SetDestination(t.transform.position);
    }

}
