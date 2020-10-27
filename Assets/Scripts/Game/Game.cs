using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool playerTurn = false;
    RaycastHit hit;
    Ray ray;
    void Start()
    {
        playerTurn = true;
    }

    private void Update()
    {

    }


}
