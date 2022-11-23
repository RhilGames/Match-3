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

    //creating an array of gems
    public Gem[] gems;
    //store x and y positions
    public Gem[,] allGems;

        // Start is called before the first frame update
    void Start()
    {
        allGems = new Gem[width, height];
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

                //creating a random number for gem to use
                int gemToUse = Random.Range(0, gems.Length);

                //sends random number and current position to spawn a gem in that location
                SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
            }
        }
    }

    //Putting different gems into locations
    private void SpawnGem(Vector2Int pos, Gem gemToSpawn)
    {
        Gem gem = Instantiate(gemToSpawn, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
        gem.transform.parent = transform;
        //listing each gems position
        gem.name = "Gem = " + pos.x + ", " + pos.y;
        allGems[pos.x, pos.y] = gem;

        gem.SetUpGem(pos, this);
    }
}