using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb2d;
    public float moveSpeed = 5;
    private Animator anim;
    private bool playerMoving;
    private Vector2 lastMove;
    private Rigidbody2D myRigidbody;
    private bool hasDashed;
    public float restartLevelDelay = 1f;     
    public float waitForDashInterval = 1f;

    public Text availableDash;
     
    void Start()
    {
        availableDash.text = "Dash charged";
        anim = GetComponent<Animator>(); // get the animator of the player
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {
    
        playerMoving = false;
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && hasDashed == false)
            {
                Debug.Log("Shift was pressed");
                hasDashed = true;
                availableDash.text = "Dash not ready";

                myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed*20, myRigidbody.velocity.y);
                playerMoving = true;
                lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
                StartCoroutine(this.WaitForDash());
            }
            else
            {
                myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, myRigidbody.velocity.y);
                playerMoving = true;
                lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
            }
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && hasDashed == false)
            {
                hasDashed = true;
                Debug.Log("Shift was pressed");
                availableDash.text = "Dash not ready";
                myRigidbody.velocity = new Vector2( myRigidbody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed*20);
                playerMoving = true;
                lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
                StartCoroutine(this.WaitForDash());
            }
            else
            {
                myRigidbody.velocity = new Vector2( myRigidbody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);
                playerMoving = true;
                lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
            }

          
        }
        if (Input.GetAxisRaw("Horizontal") < 0.5f && Input.GetAxisRaw("Horizontal") > -0.5f)
        {
            myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
            
        }
        if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0f);
        }
        
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
        if (other.tag == "Exit")
        {
            myRigidbody.velocity = new Vector2(0f, 0f);
            Debug.Log("I'm at the exit!");

            Invoke("Restart", restartLevelDelay);

            //TODO > Disable the player object since level is over.
            enabled = false;
        }
    }

    private void Restart ()
    {
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
       
    }
}
