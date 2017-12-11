using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;       //Allows us to use Lists. 
using UnityEngine.SceneManagement;
using NUnit.Framework.Constraints;


public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 0.5f;
    public static GameManager instance = null;

    private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
    public GameObject levelImage;
    public Text levelText;
    private int level = 0;                                
    private bool doingSetup = false;
    private DarkTileManager darkTileBoardScript;
    private bool isThereBoss = false;
    private bool areThereObstacles = true;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);    

        DontDestroyOnLoad(gameObject);

   
        boardScript = GetComponent<BoardManager>();
        darkTileBoardScript = GetComponent<DarkTileManager>();
        //InitGame();
    }

   
    //Initializes the game for each level.
    void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("RoomImage");
        levelText = GameObject.Find("RoomText").GetComponent<Text>();
        levelText.text = "Room " + level.ToString();
        levelImage.SetActive(true);


        Invoke("HideLevelImage", levelStartDelay);

        //Call the SetupScene function of the BoardManager script, pass it current level number.
        if (level % 2 == 1)
        {
            boardScript.SetupScene(level, areThereObstacles, isThereBoss);
        }
        else
        {
            darkTileBoardScript.SetupScene(level, areThereObstacles, isThereBoss);

        }
    }

    void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;

    }

    //This is called each time a scene is loaded.
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("I'm in: OnLevelFinishLoading!");
        //Add one to our level number.
        level++;
        //Call InitGame to initialize our level.
        InitGame();
    }
    void OnEnable()
    {
        //Tell our ‘OnLevelFinishedLoading’ function to
        //start listening for a scene change event as soon as
        //    this script is enabled.
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our ‘OnLevelFinishedLoading’ function to stop
        //listening for a scene change event as soon as this
        //    script is disabled.
        //Remember to always have an unsubscription for every
        //    delegate you subscribe to!
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    public void GameOver()
    {
        levelText.text = "You lived " + level + " rooms.";
        levelImage.SetActive(true);
        enabled = false;
    }
}