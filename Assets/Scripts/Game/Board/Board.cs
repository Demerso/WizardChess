using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    //THIS BOARD ONLY FOR PLAYER VS AI COULD CHANGE TO PLAYER PLAY
    [SerializeField] private GameObject tilePrefab = null;

    private const float TileWidth = 6;
    private const float TileHeight = 0.1f;
    private Vector3 _boardCorner;

    public Camera cam;
    //public NavMeshAgent agent;
    public static Tile tileHover;
    public Tile selected;
    public Pieces temp;
    private readonly Tile[,] _tiles = new Tile[8, 8];
    //board x position is horizontal
    private void Start()
    {
        _boardCorner = transform.position;
        // Create a tile for every tile on the board
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                var inst = Instantiate(
                    tilePrefab,
                     _boardCorner + new Vector3(
                        i * TileWidth + TileWidth / 2,
                        TileHeight / 2,
                        j * TileWidth + TileWidth / 2
                    ),
                    Quaternion.identity,
                    transform);
                _tiles[i, j] = inst.GetComponent<Tile>();
            }
        }
        Init();
        _tiles[0, 0].piece = temp;

    }

    void Init()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                _tiles[j, i].SetInit(true);
                //dont need to make ai side active, 
                //_tiles[j, 8 - i - 1].SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (Game.playerTurn)
        {
            //selecting a tile
            if (Input.GetMouseButtonDown(0) && selected == null && tileHover!= null)
            {
                //set Tile clicked to Active, will disable hover for all tile not active                //set all move for the piece active
                //set all move Tile for the piece active

                tileHover.SetActive(true);
                selected = tileHover;
                Debug.Log(selected.piece.moves.Count);
                for (int i = 0; i < selected.piece.moves.Count; i++)
                {
                    _tiles[selected.piece.moves[i].Item1, selected.piece.moves[i].Item1].SetInit(true);
                    _tiles[selected.piece.moves[i].Item1, selected.piece.moves[i].Item1].SetActive(true);
                }
            }
            //unselect a selected tile
            else if(Input.GetMouseButtonDown(0) && selected !=null&& tileHover == null)
            {
                //Didnt click on active Tile,
                for (int i = 0; i < selected.piece.moves.Count; i++)
                {
                    _tiles[selected.piece.moves[i].Item1, selected.piece.moves[i].Item1].SetActive(false);
                }
                selected.SetActive(false);
                selected = null;


            }
            //Select Destination Tile
            else if (Input.GetMouseButtonDown(0) && selected !=null && tileHover != null)
            {
             
                tileHover.piece = selected.piece;

                tileHover.piece.move(tileHover);
                //set all pieces that were back to 
                for (int i = 0; i < selected.piece.moves.Count; i++)
                {
                    _tiles[selected.piece.moves[i].Item1, selected.piece.moves[i].Item1].SetActive(false);
                    
                }
                selected.piece = null;
                selected.SetActive(false);
                selected.SetInit(false);
                selected = null;

                
        
            }


        }

    }
}
