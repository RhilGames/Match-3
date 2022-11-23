using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    //***************Setting up Game Board******************
    
    //creating variable for width and height
    public int width;
    public int height;
    
    //creating a reference to tile prefab
    public GameObject bgTilePrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    //Creating a grid and filling it with the prefab tile
    private void Setup()
    {
        for(int x = 0; x < width; x++)
        {
            for(var y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                GameObject bgTile = Instantiate(bgTilePrefab, pos, Quaternion.identity);
                //puts the tiles underneath the board gameobject to keep a clean look
                bgTile.transform.parent = transform;
                //listing each tile by which position they hold in coordinates
                bgTile.name = "BG Tile - " + x + ", " + y;
            }
        }
    }
}
