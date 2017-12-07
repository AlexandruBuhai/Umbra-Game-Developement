using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;       //Allows us to use Lists. 
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public static GameManager instance = null;

    private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
    private GameObject levelImage;
    public Text levelText;
    private int level = 1;                                
    private bool doingSetup = false;



    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);    

        DontDestroyOnLoad(gameObject);

   
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

   
    //Initializes the game for each level.
    void InitGame()
    {
        doingSetup = true;

        //levelImage = GameObject.Find("LevelImage");
        //levelText = GameObject.Find("LevelText").GetComponent<Text>();
        //levelText.text = "Room " + level.ToString();
        //levelImage.SetActive(true);


        //Invoke("OnLevelFinishedLoading", levelStartDelay);

        //Call the SetupScene function of the BoardManager script, pass it current level number.
        boardScript.SetupScene(level);
    }

    //This is called each time a scene is loaded.
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
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