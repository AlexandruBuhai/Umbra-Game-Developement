using UnityEngine;
using System;
using System.Collections.Generic;      
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.

public class BoardManager : MonoBehaviour
{
    // Using Serializable allows us to embed a class with sub properties in the inspector.
    [Serializable]
    public class Count
    {
        public int minimum;             //Minimum value for our Count class.
        public int maximum;             //Maximum value for our Count class.

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }
        
    public int columns = 8;                                      
    public int rows = 8;                                           
    public GameObject exitFront;                                      
    public GameObject exitBottom;                                        
    public GameObject exitLeft;                                        
    public GameObject exitRight;                                       
    public GameObject[] floorTiles;                                   
    public GameObject[] enemyTiles;                                  
    public GameObject[] outerWallTilesRight;                             
    public GameObject[] outerWallTilesTop1;                             
    public GameObject[] outerWallTilesTop2;                          
    public GameObject[] outerWallTilesTop;                            
    public GameObject[] outerWallTilesLeft;                           
    public GameObject[] outerWallTilesBottom;
    public GameObject[] obstacles;
    public GameObject outerCornerTopLeft;
    public GameObject outerCornerTopRight;
    public GameObject outerCornerBottomLeft;
    public GameObject outerCornerBottomRight;
    public GameObject bossPriest;
   // public GameObject cineMachine; //not used
    public GameObject mapBorders;
    public GameObject pickUp1;
    public GameObject pickUp2;
    public GameObject pickUp3;
    public GameObject NPC1; 
    public GameObject NPC2; 
    public GameObject[] blood;
    public GameObject[] books;
    public GameObject[] water;
    private bool killedBoss;
   // public GameObject CM_vcam1;

    private Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.
    private List <Vector3> gridPositions = new List <Vector3> ();   //A list of possible locations to place tiles.

   
    //Clears our list gridPositions and prepares it to generate a new board.
    void InitialiseList ()
    {

        gridPositions.Clear ();
        for(int x = 1; x < columns-1; x++)
        {
            //Within each column, loop through y axis (rows).
            for(int y = 1; y < rows-3; y++)
            {
                gridPositions.Add (new Vector3(x, y, 0f));
            }
        }
    }
        
    void BoardSetup ()
    {

        //Instantiate Board and set boardHolder to its transform.
       // Instantiate(CM_vcam1);
        boardHolder = new GameObject ("Board").transform;
        //mapBorders = new GameObject();
        //mapBorders.transform.position = new Vector3(0, 0, 0);
       // mapBorders.transform.localScale = new Vector3(1, 1, 0);
        Debug.Log(columns.ToString() + " " + rows.ToString() + " "); 
        BoxCollider2D mapBorderCollider = mapBorders.GetComponentInChildren<BoxCollider2D>();
        mapBorderCollider.size = new Vector3(columns+2, rows+2);
        mapBorderCollider.offset = new Vector3(columns/2+0.5f, rows/2+0.5f);
        
        //Instantiate(mapBorders);
       
        //GameObject inst = Instantiate(leftWallCollider, new Vector3(-1, -1, 0f), Quaternion.identity) as GameObject;
        //inst.transform.SetParent(boardHolder);

        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for(int x = -1; x < columns + 1; x++)
        {
            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for(int y = -1; y < rows + 1; y++)
            {
                //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                GameObject toInstantiate = floorTiles[Random.Range(0,floorTiles.Length)];

                //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                if (x == -1 && y == -1)
                {
                    toInstantiate = outerCornerBottomLeft;

                }
                else if (x == columns && y == -1)
                {
                    toInstantiate = outerCornerBottomRight;

                }
                else if (x == columns && y == rows)
                {
                    toInstantiate = outerCornerTopRight;
                }
                else if (x == -1 && y == rows)
                {
                    toInstantiate = outerCornerTopLeft;

                }
                else if (x == -1)
                {
                    toInstantiate = outerWallTilesLeft[Random.Range(0, outerWallTilesLeft.Length)];
                }
                else if (x == columns)
                {
                    toInstantiate = outerWallTilesRight[Random.Range(0, outerWallTilesRight.Length)];
                }
                else if (y == -1)
                {
                    toInstantiate = outerWallTilesBottom[Random.Range(0, outerWallTilesBottom.Length)];
                }
                else if (y == rows)
                {
                    toInstantiate = outerWallTilesTop[Random.Range(0, outerWallTilesTop.Length)];
                }
                else if (y == rows - 1)
                {
                    if (x != -1 || x != columns)
                        toInstantiate = outerWallTilesTop1[Random.Range(0, outerWallTilesTop1.Length)];
                }
                else if (y == rows - 2)
                {
                    if (x != -1 || x != columns)
                        toInstantiate = outerWallTilesTop2[Random.Range(0, outerWallTilesTop2.Length)];
                }

                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance =
                    Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;


                if (toInstantiate == outerCornerBottomLeft)
                {
                    BoxCollider2D leftWallCollider = instance.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
                    leftWallCollider.size = new Vector3(1.1f, columns*2+2);

                   
                }
                if (toInstantiate == outerCornerBottomRight)
                {                    
                    BoxCollider2D bottomWallCollider = instance.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
                    bottomWallCollider.size = new Vector3(rows*2+0.5f, 1.5f);
                }
                if (toInstantiate == outerCornerTopRight)
                {
                   

                    BoxCollider2D rightWallCollider = instance.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
                    rightWallCollider.size = new Vector3(1.1f, columns*2 + 2);

                }
                if (toInstantiate == outerCornerTopLeft)
                {
                    BoxCollider2D topWallCollider = instance.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
                    topWallCollider.size = new Vector3(rows*2+2, 3.5f);
                }
               

               
                instance.transform.SetParent (boardHolder);
            }
        }
    }


    Vector3 RandomPosition ()
    {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        int randomIndex = Random.Range (0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
       

        gridPositions.RemoveAt (randomIndex);
        return randomPosition;
    }


    //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
    {
        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range (minimum, maximum+1);

        for(int i = 0; i < objectCount; i++)
        {
            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();

//            for(int j = 0; j < 10; ++j)
//            {
//                
//                if (tileArray == obstacles)
//                {
//                    if (randomPosition.y == columns || randomPosition.y == columns - 1)
//                    {
//                        randomPosition = RandomPosition();
//                    }
//                    else
//                        break;
//                }
//            }
          
            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
            if (tileChoice.tag == "water")
            {
                Debug.Log("instantiating water");
                tileChoice.transform.rotation *= Quaternion.Euler(0, 0, 180f);
            }
        }
       
    }

    public void SetupScene (int level, bool areThereObstacles, bool isThereBoss, bool killB)
    {
        //Creates the outer walls and floor.
        BoardSetup ();
        InitialiseList ();
        killedBoss = killB;
        NPC1 = GameObject.FindGameObjectWithTag("NPC1");
        NPC1.SetActive(false);
        NPC2 = GameObject.FindGameObjectWithTag("NPC2");
        NPC2.SetActive(false);

        //Determine number of enemies based on current level number, based on a logarithmic progression
        int enemyCount = (int)Mathf.Log(level*8, 2f);

        //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.

        Instantiate (pickUp1, new Vector3 (columns/2  - 0.5f - 2.5f, rows/2 - 0.5f, 0f), Quaternion.identity);
        Instantiate (pickUp2, new Vector3 (columns/2 - 0.5f, rows/2 - 0.5f, 0f), Quaternion.identity);
        Instantiate (pickUp3, new Vector3 (columns/2 - 0.5f + 2.5f, rows/2 - 0.5f, 0f), Quaternion.identity);

        if (isThereBoss)
        {
            Instantiate(bossPriest, new Vector3(columns / 2 - 0.5f, rows - 5, 0f), Quaternion.identity);
            LayoutObjectAtRandom(water, 6, 16);    
        }
        else
        {
            Debug.Log("Level1 " + killedBoss.ToString());
            if (level != 1)
            {
                LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount); 
            }
            else if (level == 1)
            {
                NPC1.SetActive(true);
                Debug.Log("Kill boss status: " + killedBoss.ToString());
                if (killedBoss)
                {

                    Debug.Log("I'm trying to create the mole!");
                    NPC2.SetActive(true);
                }
            }

            LayoutObjectAtRandom(obstacles, 4, 12);    
            LayoutObjectAtRandom(blood, 2, 5);        
            LayoutObjectAtRandom(books, 0, 4);        

        }

      
        //Instantiate the exit tile in the upper right hand corner of our game board
        Instantiate (exitFront, new Vector3 (columns/2  - 0.5f, rows - 1.5f, 0f), Quaternion.identity);
        Instantiate (exitBottom, new Vector3 (columns/2 - 0.5f, 0, 0f), Quaternion.identity);
        Instantiate (exitRight, new Vector3 (columns-1,rows/2-0.5f , 0f), Quaternion.identity);
        Instantiate (exitLeft, new Vector3 (0, rows/2-0.5f, 0f), Quaternion.identity);
    }
}
