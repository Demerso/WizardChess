using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab = null;
    [SerializeField] private Game game;

    private const float TileWidth = 6;
    private const float TileHeight = 0.1f;
    private const int TileMask = 1 << 8;
    
    private Vector3 _boardCorner;
    private Camera _cam;
    
    public Tile selected;
    public Pieces temp;
    private readonly Tile[,] _tiles = new Tile[8, 8];
    
    
    // Board x position is horizontal
    private void Start()
    {
        _cam = Camera.main;
        _boardCorner = transform.position;
        InitTiles();
        InitPieces();
    }

    private void InitTiles()
    {
        // Create a tile for every tile on the board
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                var tile = Instantiate(
                    tilePrefab,
                    _boardCorner + new Vector3(
                        i * TileWidth + TileWidth / 2,
                        TileHeight / 2,
                        j * TileWidth + TileWidth / 2
                    ),
                    Quaternion.identity,
                    transform).GetComponent<Tile>();
                tile.Location = (i, j);
                _tiles[i, j] = tile;
            }
        }
    }
    
    private void InitPieces()
    {
        temp.Move(_tiles[0,0]);
        _tiles[0, 0].piece = temp;
        _tiles[0, 0].Location = (0, 0);
    }
    
    public void SetTurn(Game.Team team)
    {
        foreach (var tile in _tiles)
        {
            if (tile.piece != null && tile.piece.team == team)
            {
                tile.SetState(Tile.State.Hidden);
            }
            else
            {
                tile.SetState(Tile.State.Inactive);
            }
        }
    }

    private void DeactivateTiles()
    {
        foreach (var tile in _tiles)
        {
            tile.SetState(Tile.State.Inactive);
        }
    }

    private void ClickTile(Tile tile)
    {
        switch (tile.state)
        {
            case Tile.State.Hidden:
                selected = tile;
                var moves = tile.piece.GetMoves(_tiles);
                SetTurn(tile.piece.team);
                foreach (var (x, y) in moves)
                {
                    _tiles[x, y].SetState(Tile.State.Active);
                }
                break;
            case Tile.State.Active:
                selected.piece.Move(tile).AddListener(game.EndTurn);
                DeactivateTiles();
                break;
            default:
                break;
        }

    }
    
    
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, TileMask))
            {
                ClickTile(hit.collider.gameObject.GetComponent<Tile>());
            }
        }
        
        
        /*
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


        }*/

    }
}
