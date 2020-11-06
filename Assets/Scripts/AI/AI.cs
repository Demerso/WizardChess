using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    // Start is called before the first frame update
    protected List<Pieces> pieces = new List<Pieces>();
    //public Tile[,] tiles = new Tile[8, 8];
    public Board board;
    public bool playing = false;
    protected Pieces current;
    protected List<(int, int)> moves;
    public (int, int) selectm;
    public Pieces x;
    protected void Start()
    {

        x = board._tiles[0, 6].piece;
    }
    public void Addpiece(Pieces p)
    {

        pieces.Add(p);
    }


    public void Play()
    {

        selectMove();
        playing = false;
        if (current == null)
        {
            Debug.Log("bruh");
        }
        if (board == null)
        {
            Debug.Log("bruh");
        }
        if (board._tiles == null)
        {
            Debug.Log("bruh");
        }

    }
    public virtual void selectMove()
    {

    }
}
