using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    [SerializeField] private GameObject tilePrefab = null;

    private const float TileWidth = 6;
    private const float TileHeight = 0.1f;
    private Vector3 _boardCorner;
    
    private readonly Tile[,] _tiles = new Tile[8, 8];

    private void Start()
    {
        _boardCorner = transform.position;
        // Create a tile for every tile on the board
        for (var i = 0; i < 8; i++) {
            for (var j = 0; j < 8; j++)
            {
                var inst = Instantiate(
                    tilePrefab,
                     _boardCorner + new Vector3(
                        i * TileWidth + TileWidth / 2,
                        TileHeight/2,
                        j * TileWidth + TileWidth / 2
                    ),
                    Quaternion.identity,
                    transform);
                _tiles[i, j] = inst.GetComponent<Tile>();
            }
        }
    }
    
}
