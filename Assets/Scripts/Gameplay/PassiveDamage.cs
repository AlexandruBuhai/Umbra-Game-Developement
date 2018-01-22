using UnityEngine;
using System.Collections;

public class PassiveDamage : MonoBehaviour {

	private float timer; 
	public float lowerLife;
	public int damage;

	// Use this for initialization
	void Start () {
		//change later for enemies/player
		timer = 0.0f;
		lowerLife = .75f;
		damage = 5;
       // transform.rotation = Quaternion.AngleAxis(180.0f, transform.forward) * transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D collision){
		timer += Time.deltaTime;
		if (collision.gameObject.name == "Player") {
			if (timer > lowerLife) {
				timer = 0;
                GameObject.Find ("Player").GetComponent<PlayerController> ().health -= damage;
                //GameObject.Find("Player").GetComponent<PlayerController>().source.PlayOneShot( GameObject.Find("Player").GetComponent<PlayerController>().hitSound);
                GameObject.Find("Player").GetComponent<PlayerController>().anim.SetTrigger("isHit");
			}
		}
	}
}
