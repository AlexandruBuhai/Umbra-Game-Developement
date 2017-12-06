using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb2d;
    public float speed = 4.5f;
    public int count;
    private Animator anim;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        count = 0;
        anim = GetComponent<Animator>();
        rb2d.freezeRotation = true;

    }

    void FixedUpdate()
    {
//        float moveHoizontal = Input.GetAxis("Horizontal");
//        float moveVertical = Input.GetAxis("Vertical");
//        Vector2 movement = new Vector2(moveHoizontal, moveVertical);
//        rb2d.AddForce(movement* speed);

    }
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f) 
        {
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0f, 0f));

        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * speed * Time.deltaTime, 0f));        
        }
//        anim.SetFloat("WalkUp", Input.GetAxisRaw("Vertical"));

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
    }

}
