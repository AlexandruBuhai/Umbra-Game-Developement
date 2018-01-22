
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Preafericitul : Enemy {

    public AudioClip attackSound;
    public AudioClip deadSound;

    void Start () {
        stamina = 300f;
        health = 120;
        speed = 10;
        strength = 10;
        regenRate = 0.9f; //0.7f
        maxStamina = 151f;
        minDist = 3;

        terminalSpeed = speed / 10;
        initialSpeed = (speed / 10) / 2;
        acceleration = (speed / 10) / 4;

        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator> ();
        polygonCollider = GetComponent<PolygonCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.Find ("Player").GetComponent<PlayerController>();

    }

    public override void passiveRegen(){
        stamina += regenRate;
    }

    public override HashSet<KeyValuePair<string, object>> createGoalState(){
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>> ();
        goal.Add (new KeyValuePair<string, object> ("damagePlayer", true));
        goal.Add (new KeyValuePair<string, object> ("stayAlive", true));
        goal.Add (new KeyValuePair<string, object> ("stayAlive", true));

        return goal;
    }

    public void Damage(int damage)
    {
        Debug.Log("I take damage!");
        health -= damage;
        stamina = -30;
        animator.SetTrigger("isHit");
        if (health <= 0)
        {
            rigidBody.velocity = Vector3.zero;
           // animator.SetBool("isDead", true);
            speed = 0;
            strength = 0;
            regenRate = 0;
            polygonCollider.enabled = false;
            boxCollider.enabled = false;
            animator.SetBool("isDead", true);
            gameObject.tag = "DeadEnemy";
            if (source.isPlaying == false)
            {
                source.PlayOneShot(deadSound, 0.2f);
            }
        }
    }
}
