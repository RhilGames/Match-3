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

    public float gemSpeed;

    public MatchFinder matchFind;

    private void Awake()
    {
        matchFind = FindObjectOfType<MatchFinder>();
    }

        // Start is called before the first frame update
    void Start()
    {
        allGems = new Gem[width, height];
        Setup();
    }

    void Update()
    {
        matchFind.FindAllMatches();
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

                int iterations = 0;
                while(MatchesAt(new Vector2Int(x,y), gems[gemToUse]) && iterations < 100)
                {
                    gemToUse = Random.Range(0, gems.Length);
                    iterations++;
                }

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

    bool MatchesAt(Vector2Int posToCheck, Gem gemToCheck)
    {
        if(posToCheck.x > 1)
        {
            if (allGems[posToCheck.x - 1, posToCheck.y].type == gemToCheck.type && allGems[posToCheck.x - 2, posToCheck.y].type == gemToCheck.type)
            {
                return true;
            }
        }
        if (posToCheck.y > 1)
        {
            if (allGems[posToCheck.x, posToCheck.y - 1].type == gemToCheck.type && allGems[posToCheck.x, posToCheck.y - 2].type == gemToCheck.type)
            {
                return true;
            }
        }

        return false;
    }

    private void DestoryMatchedGemAt(Vector2Int pos)
    {
        if(allGems[pos.x, pos.y] != null)
        {
            if(allGems[pos.x, pos.y].isMatched)
            {
                Destroy(allGems[pos.x,pos.y].gameObject);
                allGems[pos.x, pos.y] = null;
            }
        }
    }

    public void DestroyMatches()
    {
        for(int i = 0; i < matchFind.currentMatches.Count; i++)
        {
            if(matchFind.currentMatches[i] != null)
            {
                DestoryMatchedGemAt(matchFind.currentMatches[i].posIndex);
            }
        }

        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo()
    {
        yield return new WaitForSeconds(.2f);

        int nullCounter = 0;

        for (int x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if(allGems[x,y] == null)
                {
                    nullCounter++;
                }
                else if(nullCounter > 0)
                {
                    allGems[x, y].posIndex.y -= nullCounter;
                    allGems[x, y- nullCounter] = allGems[x, y];
                    allGems[x, y] = null;
                }

            }

            nullCounter = 0;
        }

        StartCoroutine(FillBoardCo());
    }

    private IEnumerator FillBoardCo()
    {
        yield return new WaitForSeconds(.5f);

        RefillBoard();

        yield return new WaitForSeconds(.5f);

        matchFind.FindAllMatches();

        if(matchFind.currentMatches.Count > 0)
        {
            yield return new WaitForSeconds(1.5f);
            DestroyMatches();
        }

    }

    private void RefillBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (allGems[x, y] == null)
                {
                    int gemToUse = Random.Range(0, gems.Length);

                    SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
                }
            }
        }

    }
}
