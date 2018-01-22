using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bird : Enemy {
    
    public AudioClip attackSound;
    public AudioClip deadSound;
    // Use this for initialization
    void Start () {     
        health = 10;
        speed = 30;
        strength = 10;
        regenRate = 3f;
        stamina = 500f;
        maxStamina = 500f;

       // minDist = 2f;

        setSpeed (speed);
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator> ();
        rigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.Find ("Player").GetComponent<PlayerController>();
        boxCollider = GetComponent<BoxCollider2D>();


    }

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
                source.PlayOneShot(deadSound, 0.5f);
            }
            rigidBody.velocity = Vector3.zero;      

        }
    }



}

