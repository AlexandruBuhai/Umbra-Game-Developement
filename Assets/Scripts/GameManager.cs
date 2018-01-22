using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;       //Allows us to use Lists. 
using UnityEngine.SceneManagement;
using System.Net;
using System.Runtime.Remoting.Services;
using System;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 0.5f;
    public static GameManager instance = null;

    private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
    public GameObject levelImage;
    public Text levelText;
    public GameObject levelTextObj;
    private int level = 0;  //level number                                
    //private bool doingSetup = false;
    private DarkTileManager darkTileBoardScript;
    private bool isThereBoss = false;
    private bool areThereObstacles = true;
    public GameObject backgroundSounds;
    public GameObject bossSound;
    public PlayerController player;
    public GameObject playerObj;
    private int positionExit; //1-up 2-right 3-down 4-left
    private int cnt = 0;
    private float playerSpeed;
    private float playerHealth;
    public GameObject spawnPointUp;
    public GameObject spawnPointRight;
    public GameObject spawnPointDown;
    public GameObject spawnPointLeft;
    public int newLife; //wasDead in the playerController
    private string firstPhrase;
    private string secondPhrase;
    private int prevLvl; //if there was a boss before
    private bool killBoss;
    private float MaxHP;

    private GameObject levelCount;
    private Text levelCountText;
    void Awake()
    {
        positionExit = 0;
        killBoss = false;
        prevLvl = 5;
        AudioListener.volume = 0.8F; //change this for whole volume
        //Check if instance already exists
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);   
            Destroy(backgroundSounds);
            Destroy(bossSound);

        }
      
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(backgroundSounds);
        DontDestroyOnLoad(bossSound);
        firstPhrase = "The Calamity came and changed the world forever. Once prosperous realm of Men, now desolate ruins of once great City. Underground kingdom of Cartites, mole-like creatures, now gone. High towers and temples on mountain tops belonging to proud nation of Pasari, bird-like enigmatic humanoids, torn apart and ruined.  \n(Space - Next \nEnter to skip)";
        secondPhrase = "The time ceased to exist. Did the Calamity happened years ago? OR was it just couple of weeks? In dark world without ruins, everyone keeps fighting to survive. And the shadows grow darker… \n\n(Enter to close)";  
        //DontDestroyOnLoad(player);
        boardScript = GetComponent<BoardManager>();
        darkTileBoardScript = GetComponent<DarkTileManager>();
  
    }

    void Update()
    {
        
        //Debug.Log("!!!!!! " + positionExit.ToString() + " !!!!!");

        if (levelImage != null)
        {
            if (Input.GetKey(KeyCode.Return) || cnt > 2)
            {
                
                levelText.text = "";
                levelImage.SetActive(false);
            } 
            if (Input.GetKey(KeyCode.Escape) )
            {
                Destroy(gameObject);   
                Destroy(backgroundSounds);
                Destroy(bossSound);
                SceneManager.LoadScene("menu", LoadSceneMode.Single);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (cnt == 0)
                {
                    levelText.text = firstPhrase;
                    cnt++;
                }
                else if (cnt >= 1 && cnt <= 2)
                {
                    levelText.text = secondPhrase;
                    cnt++;
                }
            }
        }

    }
    //Initializes the game for each level.
    void InitGame()
    {
        //doingSetup = true;
        //cnt = 0;

        levelImage = GameObject.FindGameObjectWithTag("RoomImage");
        levelTextObj = GameObject.FindGameObjectWithTag("RoomText"); //check if this is working
        levelText = levelTextObj.GetComponent<Text>();
        if (newLife == 1)
        {
            level = 1;
            cnt = 0;
            firstPhrase = "You are not yet released form this weak shell. Return back to life and continue in fulfilling your purpose in this dark world. \n\n(Enter to close)";
            secondPhrase = "";

            if (killBoss == true)
            {
                cnt = 0;
                firstPhrase = "When you woke up in the safe place you noticed a new figure standing nearby. It is a one of the Mole folk, without weapon and armor.  \n(Space - Next \nEnter to skip)";
                secondPhrase = " He saw you vanquishing the Mad priest and followed you since then. He seems harmless but can you trust him?  \n\n(Enter to close)";
            }
       }
        if (level == 22)
        {
            cnt = 0;
            firstPhrase = "You have discovered the truth about origin of Calamity. But is your purpose fulfilled? Is there even a purpose to keep going in this dark world of violence and death… \n\n(Enter to close)";
            secondPhrase = "";
        }
        player = FindObjectOfType<PlayerController>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        spawnPointLeft = GameObject.FindGameObjectWithTag("Sp4");
        spawnPointUp = GameObject.FindGameObjectWithTag("Sp1");
        spawnPointRight = GameObject.FindGameObjectWithTag("Sp2");
        spawnPointDown = GameObject.FindGameObjectWithTag("Sp3");
      
        Debug.Log("Here");

        if (playerObj != null)
        {
            if (positionExit == 1)
            {
                Debug.Log("Up");
                //player.tr
                playerObj.transform.position = spawnPointUp.transform.position;
            }
            else
            if (positionExit == 2)
            {
                Debug.Log("Right");
                playerObj.transform.position = spawnPointRight.transform.position;
            }
            else
            if (positionExit == 3)
            {
                Debug.Log("Down");
                        playerObj.transform.position = spawnPointDown.transform.position;
            }
            else
            if (positionExit == 4)
            {
                Debug.Log("Left");
                playerObj.transform.position = spawnPointLeft.transform.position;
            }
        }
            
       
        //Call the SetupScene function of the BoardManager script, pass it current level number.
        if (level % 3 == 0 && level % 2 != 0)
        {
            backgroundSounds.GetComponent<AudioSource>().enabled = false;
            bossSound.GetComponent<AudioSource>().enabled = true;

        }
        else
        {
            if (backgroundSounds.GetComponent<AudioSource>().enabled == false)
            {
                backgroundSounds.GetComponent<AudioSource>().enabled = true;
                bossSound.GetComponent<AudioSource>().enabled = false;

            }
        }
        if (prevLvl < level && isThereBoss)
        {
            killBoss = true;
        }

        if (level % 2 == 1)
        {
            if (level < 5)
            {
                if (backgroundSounds.GetComponent<AudioSource>().enabled == false)
                {
                    
                    backgroundSounds.GetComponent<AudioSource>().enabled = true;
                    bossSound.GetComponent<AudioSource>().enabled = false;

                }
            }
        
            isThereBoss = false;
            if (level >= 5)
            {
                int chance = Random.Range(1, 6);
                if (chance == 2 || chance == 3)
                {
                    isThereBoss = true;
                    prevLvl = level;
                    backgroundSounds.GetComponent<AudioSource>().enabled = false;
                    bossSound.GetComponent<AudioSource>().enabled = true;
                }
                else
                {
                    isThereBoss = false;
                    backgroundSounds.GetComponent<AudioSource>().enabled = true;
                    bossSound.GetComponent<AudioSource>().enabled = false;
                }
            }


            boardScript.SetupScene(level, areThereObstacles, isThereBoss, killBoss);
            player.SetSpeed(playerSpeed);
            player.SetHealth(playerHealth);
        }
        else
        {
            isThereBoss = false;
            darkTileBoardScript.SetupScene(level, areThereObstacles, isThereBoss);
            player.SetSpeed(playerSpeed);
            player.SetHealth(playerHealth);


        }
        if (level == 1 && newLife == 1)
        {
            Debug.Log("I died, and my health restored");
            player.SetSpeed(playerSpeed);
            player.SetHealth(MaxHP);
        }
        //initiate LevelCount Text
        levelCount = GameObject.FindGameObjectWithTag("LevelText");
        levelCountText = levelCount.GetComponent<Text>();
        levelCountText.text = "Level: " + level.ToString();
    }

    void HideLevelImage()
    {
        levelImage.SetActive(false);
        //doingSetup = false;
    }

   
    //This is called each time a scene is loaded.
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("I'm in: OnLevelFinishLoading!");
        //Add one to our level number.
        level++;
        playerSpeed = player.GetSpeed();
        playerHealth = player.GetHealth();
        positionExit = player.GetExit();
        newLife = player.WasDead();
        MaxHP = Mathf.Max(MaxHP, player.GetMaxHP());
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
    public void Start()
    {
        Debug.Log("I am at start at gameManager");
        positionExit = 0;
        playerObj = GameObject.FindGameObjectWithTag("Player");

    }

    public int GetDead()
    {
        return newLife;
    }

    public void GameOver()
    {
        levelText.text = "You lived " + level + " rooms.";
       // levelImage.SetActive(true);
        enabled = false;
    }

    public int GetLevelCount()
    {
        return prevLvl;
    }
}