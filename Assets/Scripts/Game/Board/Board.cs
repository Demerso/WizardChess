using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab = null;
    [SerializeField] private Game game;

    [SerializeField] private PieceSet pieceSet;

    private const float TileWidth = 6;
    private const float TileHeight = 0.1f;
    private const int TileMask = 1 << 8;
    
    private Vector3 _boardCorner;
    private Camera _cam;
    private Tile _selected;
    
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
        var team = Game.Team.Light;
        for (var j = 0; j < 8; j += 7)
        {
            InitPiece(pieceSet.rook, 0, j, team);
            InitPiece(pieceSet.knight, 1, j, team);
            InitPiece(pieceSet.bishop, 2, j, team);
            InitPiece(pieceSet.king, 3, j, team);
            InitPiece(pieceSet.queen, 4, j, team);
            InitPiece(pieceSet.bishop, 5, j, team);
            InitPiece(pieceSet.knight, 6, j, team);
            InitPiece(pieceSet.rook, 7, j, team);
            team = Game.Team.Dark;
        }
        for (var i = 0; i < 8; i++)
        {
            InitPiece(pieceSet.pawn, i, 1, Game.Team.Light);
            InitPiece(pieceSet.pawn, i, 6, Game.Team.Dark);
        }
    }

    private void InitPiece(GameObject obj, int x, int y, Game.Team team)
    {
        var piece = Instantiate(obj, _tiles[x, y].transform.position, Quaternion.identity).GetComponent<Pieces>();
        piece.SetTeam(team);
        _tiles[x, y].piece = piece;
        piece.Loc = (x, y);
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
                if (_selected != null && _selected.piece != null)
                    _selected.piece.SetSelected(false);
                _selected = tile;
                tile.piece.SetSelected(true);
                var moves = tile.piece.GetMoves(_tiles);
                SetTurn(tile.piece.team);
                foreach (var (x, y) in moves)
                {
                    _tiles[x, y].SetState(Tile.State.Active);
                }
                break;
            case Tile.State.Active:
                _selected.piece.SetSelected(false);
                _selected.piece.Move(tile).AddListener(game.EndTurn);
                _selected.piece = null;
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

    [Serializable]
    private struct PieceSet
    {
        public GameObject king;
        public GameObject queen;
        public GameObject bishop;
        public GameObject knight;
        public GameObject rook;
        public GameObject pawn;
    }
}
