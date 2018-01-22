using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mole : Enemy {

    //private GameManager instance;
	// Use this for initialization
    public AudioClip attackSound;
    public AudioClip deadSound;
	void Start () {
        
		stamina = 100f;
		health = 10;
		speed = 7;
		strength = 10;
		regenRate = .5f;
		maxStamina = 100f;

		terminalSpeed = speed / 10;
		initialSpeed = (speed / 10) / 2;
		acceleration = (speed / 10) / 4;

        source = GetComponent<AudioSource>();
		animator = GetComponent<Animator> ();
        rigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.Find ("Player").GetComponent<PlayerController>();
        boxCollider = GetComponent<BoxCollider2D>();

	}

//    void Update()
//    {
//        if (health <= 0)
//        {
//            Debug.Log("I am dead");
//            Destroy(this);
//        }
//    }

	public override void passiveRegen(){
		stamina += regenRate;
	}

	public override HashSet<KeyValuePair<string, object>> createGoalState(){
		HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>> ();
		goal.Add (new KeyValuePair<string, object> ("damagePlayer", true));
		goal.Add (new KeyValuePair<string, object> ("stayAlive", true));
		return goal;
	}
    public void Damage(int damage)
    {
        Debug.Log("I take as a Mole!");
        health -= damage;
        animator.SetTrigger("MoleHit");
        if (health <= 0)
        {
            // animator.SetBool("isDead", true);
            speed = 0;
            strength = 0;
            regenRate = 0;
            boxCollider.enabled = false;
            animator.SetBool("MoleDead", true);
            gameObject.tag = "DeadEnemy";
            if (source.isPlaying == false)
            {
                source.PlayOneShot(deadSound, 0.8f);
            }
            rigidBody.velocity = Vector3.zero;      
//            StartCoroutine(WaitForDeath());
        }
    }

//    IEnumerator WaitForDeath()
//    {
//        rigidBody.velocity = Vector3.zero;
//        yield return new WaitForSeconds(0.4f);
//       // Destroy(gameObject);
//    }
}
