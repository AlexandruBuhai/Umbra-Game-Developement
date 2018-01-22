using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.SocialPlatforms;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb2d;
    public float moveSpeed = 5; //movespeed of the player
    private float currentMoveSpeed; // how fast you currently should be moving
    public float diagonalMoveModifier; // percentage of lost speed in diagonal

    public GameObject pickUp1;
    public GameObject pickUp2;
    public GameObject pickUp3;
    public DialogueManager dm;

    public Animator anim; 
    private Rigidbody2D myRigidbody;
    private bool hasDashed; //true if the dash should be on cooldown 
    public float restartLevelDelay = 3f;  
    public float waitForDashInterval = 1f;
    public Text healthRemaining; // Health text up top
    public Text Speed; // Speed property
    public Text availableDash;
    public Text attackDamage;

    public float health;
    public int attackDmg;
    private bool attacking = false;
    private float attackTimer = 0;
    private float attackTimer2 = 0;
    private float attackCd2 = 0.8f;
    private float attackCd = 0.3f;
    public Collider2D attackTrigger;
    private int xDir, yDir;
    private bool notDead = true;
    public bool hasAttacked = false; //for birds
    public int exitPosition;
    public int wasDead;
    private GameObject[] enemiesAlive; 
    public GameManager gameMan;
    public float MaxHP;
    private bool canMove;

    //Sounds
    public AudioClip attackSound;
    public AudioClip moveSound;
    public AudioClip moveSoundSand;
    public AudioClip hitSound;
    public AudioClip hitSound2;
    public AudioClip deadSound;
    public AudioSource source;
    private float volLowRange = 0.5f;
    private float volHighRange = 1.0f;
    private GameObject skull;
    public string clip;

    void Awake()
    {
        anim = GetComponent<Animator>(); // get the animator of the player
        attackTrigger.enabled = false;
        health = 50;
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        canMove = true;
        availableDash.text = "Dash charged";

        exitPosition = 0;
        clip = "";
        myRigidbody = GetComponent<Rigidbody2D>();
        healthRemaining.text = (health / 10f).ToString();
        wasDead = gameMan.GetDead();
        attackDmg = 5;
        xDir = 0;
        yDir = -1;
        dm = FindObjectOfType<DialogueManager>();
        pickUp1 = GameObject.FindGameObjectWithTag("Health");
        pickUp2 = GameObject.FindGameObjectWithTag("Speed");
        pickUp3 = GameObject.FindGameObjectWithTag("Damage");
        skull = GameObject.FindWithTag("Skull");
    }

    void Update() {
        MaxHP = Mathf.Max(MaxHP, health);
        Speed.text = "Speed: " + moveSpeed.ToString();
        attackDamage.text = "Damage: " + attackDmg.ToString();

        healthRemaining.text = "Health: "  + (health / 10f).ToString();
        if (canMove)
        {
            if (dm.dialogueActive)
            {
                myRigidbody.velocity = Vector2.zero;
                return;
            }
            if (Input.GetKey(KeyCode.LeftArrow) && hasAttacked == false)
            {
                attackTrigger.offset = new Vector2(-0.4f, 0.7f);
                xDir = -1;
                yDir = 0;
                AttackNow();
            }
            else if (Input.GetKey(KeyCode.UpArrow) && hasAttacked == false)
            {
                attackTrigger.offset = new Vector2(0, 1.3f);
                xDir = 0;
                yDir = 1;
                AttackNow();
            }
            else if (Input.GetKey(KeyCode.RightArrow) && hasAttacked == false )
            {
                attackTrigger.offset = new Vector2(0.4f, 0.7f);
                xDir = 1;
                yDir = 0;
                AttackNow();
            }
            else if (Input.GetKey(KeyCode.DownArrow) && hasAttacked == false)
            {
                attackTrigger.offset = new Vector2(0, 0);
                xDir = 0;
                yDir = -1;
                AttackNow();
            }

            anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal")); // we get the type of animation on Horizontal
            anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical")); 
           
            if (Input.GetAxisRaw("Horizontal") > 0.3f || Input.GetAxisRaw("Horizontal") < -0.3f)
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) && hasDashed == false)
                {
                    Debug.Log("Shift was pressed");
                    hasDashed = true;
                    availableDash.text = "Dash not ready";

                    myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * currentMoveSpeed * 20, myRigidbody.velocity.y);
                    StartCoroutine(this.WaitForDash());
                }
                else
                {

                    if (skull == null && source.isPlaying == false)
                    {
                        clip = "move";
                        float vol = Random.Range(volLowRange - 0.4f, volHighRange - 0.4f);
                        source.PlayOneShot(moveSound, vol);
                    }
                    else
                    {
                        if (source.isPlaying == false)
                        {      
                            clip = "move";
                            source.PlayOneShot(moveSoundSand, 0.05f);
                        }

                    }
                    myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * currentMoveSpeed, myRigidbody.velocity.y);
                }
            }
            if (Input.GetAxisRaw("Vertical") > 0.3f || Input.GetAxisRaw("Vertical") < -0.3f)
            {
                if (Input.GetKeyDown(KeyCode.LeftControl) && hasDashed == false)
                {
                    hasDashed = true;
                    Debug.Log("Shift was pressed");
                    availableDash.text = "Dash not ready";
                    myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, Input.GetAxisRaw("Vertical") * currentMoveSpeed * 20);
                    StartCoroutine(this.WaitForDash());
                }
                else
                {
                    myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, Input.GetAxisRaw("Vertical") * currentMoveSpeed);

                    if (skull == null && source.isPlaying == false)
                    {
                        clip = "move";
                        float vol = Random.Range(volLowRange - 0.4f, volHighRange - 0.4f);
                        source.PlayOneShot(moveSound, vol);
                    }
                    else
                    {
                        if (source.isPlaying == false)
                        {    
                            clip = "move";
                            source.PlayOneShot(moveSoundSand, 0.05f);
                        }
                    }
                } 
            }

            if (Input.GetAxisRaw("Horizontal") < 0.3f && Input.GetAxisRaw("Horizontal") > -0.3f)
            {
                myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
                if (Input.GetAxisRaw("Vertical") < 0.3f && Input.GetAxisRaw("Vertical") > -0.3f && (clip != "attack"))
                {
                    source.Stop();
                }
                
            }
            if (Input.GetAxisRaw("Vertical") < 0.3f && Input.GetAxisRaw("Vertical") > -0.3f)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0f);
            }
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.3f && Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.3f)
            {
                currentMoveSpeed = moveSpeed * diagonalMoveModifier;
            }
            else
            {
                currentMoveSpeed = moveSpeed;
            }

        }

        if (health <= 0 && notDead) { // reload the scene if you die 
            
            anim.SetBool("isDead", true);
            wasDead = 1;
            canMove = false;
            myRigidbody.velocity = new Vector2(0f, 0f);
            StartCoroutine(waitForDeath());
            notDead = false;
        }
        if (attacking)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                attacking = false;
                attackTrigger.enabled = false;
            }
        }

        if( hasAttacked )
        {
            if (attackTimer2 > 0)
            {
                attackTimer2 -= Time.deltaTime;
            }
            else
            { 
                hasAttacked = false;
            }
        }
        
    }
    private void AttackNow()
    {
        if (!attacking)
        {
            attacking = true;
            float vol = Random.Range(volLowRange + 0.2f, volHighRange);
            source.Stop();
            source.PlayOneShot(attackSound, vol);
            clip = "attack";
            attackTimer = attackCd;
            attackTimer2 = attackCd2;
            attackTrigger.enabled = true;
            hasAttacked = true;

            if (xDir == 1 && yDir == 0)
            {
                anim.SetTrigger("isAttackingRight");
            }
            else if (xDir == 0 && yDir == 1)
            {
                anim.SetTrigger("isAttackingBack");
            }
            else if (xDir == -1 && yDir == 0)
            {
                anim.SetTrigger("isAttackingLeft");
            }
            else if (xDir == 0 && yDir == -1)
            {
                anim.SetTrigger("isAttackingFront");
            }
        }
         
    }
    public float GetSpeed()
    {
        return this.moveSpeed;
    }

    public void SetSpeed( float sp )
    {
        moveSpeed = sp;
        Speed.text = "Speed: " + moveSpeed.ToString();
    }
    public float GetHealth()
    {
        return health;
    }

    public void SetHealth( float hp )
    {
        health = hp;
        healthRemaining.text = "Health: "  + (hp / 10f).ToString();
        Debug.Log(healthRemaining.text.ToString());
        StartCoroutine(WaitForSec());
    }
    public IEnumerator waitForDeath()
    {
        yield return new WaitForSeconds(2f);

        Invoke("Restart", restartLevelDelay+3);
    }
    public IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Second time" + healthRemaining.text.ToString());

    }
    public IEnumerator WaitForDash()
    {
        yield return new WaitForSeconds(waitForDashInterval);
        hasDashed = false;
        availableDash.text = "Dash charged";

    }
    private void OnTriggerEnter2D (Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Exit1")
        {
            enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemiesAlive.Length == 0)
            {
                myRigidbody.velocity = new Vector2(0f, 0f);
                Debug.Log("I'm at the exit1!");
                exitPosition = 1;
                //gameMan.whatExit(1);
                Invoke("Restart", restartLevelDelay);
                //TODO > Disable the player object since level is over.
                enabled = false;
            }
        }
        else
        if (other.tag == "Exit2")
        {
            enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemiesAlive.Length == 0)
            {
                myRigidbody.velocity = new Vector2(0f, 0f);
                Debug.Log("I'm at the exit2!");
                exitPosition = 2;
                //gameMan.whatExit(2);
                Invoke("Restart", restartLevelDelay);
                enabled = false;
            }
        }
        else
        if (other.tag == "Exit3")
        {
            enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemiesAlive.Length == 0)
            {
                myRigidbody.velocity = new Vector2(0f, 0f);
                Debug.Log("I'm at the exit3!");
                exitPosition = 3;
                //gameMan.whatExit(3);
                Invoke("Restart", restartLevelDelay);
                enabled = false;
            }
        }
        else
        if (other.tag == "Exit4")
        {
            enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemiesAlive.Length == 0)
            {
                myRigidbody.velocity = new Vector2(0f, 0f);
                Debug.Log("I'm at the exit4!");
                exitPosition = 4;
                //gameMan.whatExit(4);
                Invoke("Restart", restartLevelDelay);
                enabled = false;
            }
        }
        
        if (other.tag == "Speed")
        {
            myRigidbody.velocity = new Vector2(0f, 0f);
            Debug.Log("I'm on the Speed Pick Up!");
            moveSpeed += 3;
            pickUp1.SetActive(false);
            pickUp2.SetActive(false);
            pickUp3.SetActive(false);
        }
        else
        if (other.tag == "Health")
        {
            myRigidbody.velocity = new Vector2(0f, 0f);
            Debug.Log("I'm on the Health Pick Up!");
            health += 30;
            pickUp1.SetActive(false);
            pickUp2.SetActive(false);
            pickUp3.SetActive(false);
        }
        else
        if (other.tag == "Damage")
        {
            myRigidbody.velocity = new Vector2(0f, 0f);
            Debug.Log("I'm on the DAmage Pick Up!");
            attackDmg += 5;
            pickUp1.SetActive(false);
            pickUp2.SetActive(false);
            pickUp3.SetActive(false);
        }
    }
        
    public int GetExit()
    {
        return exitPosition;
    }

    public int WasDead()
    {
        return wasDead;
    }
    public float GetMaxHP()
    {
        return MaxHP;
    }

    private void Restart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
