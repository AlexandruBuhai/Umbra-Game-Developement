using UnityEngine;
using System.Collections;

public class BirdAttackAction : GOAPAction {

    private bool attacked = false;

    public BirdAttackAction(){
        //addPrecondition ("playerDefending", true);
        addEffect ("damagePlayer", true);
        cost = 300f;
    }

    public override void reset(){
        attacked = false;
        target = null;
    }

    public override bool isDone(){
        return attacked;
    }

    public override bool requiresInRange(){
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent){
        target = GameObject.Find ("Player");
        Bird currA = agent.GetComponent<Bird> ();

        if (target != null && target.GetComponent<PlayerController>().hasAttacked && 
            currA.stamina >= (500 - cost) ) { 
            return true;
        } else {
            return false;
        }
    }

    public override bool perform(GameObject agent){
        Bird currA = agent.GetComponent<Bird> ();
        if (currA.stamina >= cost)
        {

            if (currA.player.health > 0)
            {

                Debug.Log("I am attacking");
                currA.stamina -= cost;
                currA.animator.SetTrigger("MoleAttackLeft");
           
                int damage = currA.strength;
                if (currA.health > 0)
                {                
                    currA.player.clip = "attack";
                    currA.player.anim.SetTrigger("isHit");
                    currA.player.source.Stop();
                    currA.player.source.PlayOneShot(currA.player.hitSound);
                    currA.player.health -= damage;

                }
                
                if (currA.source.isPlaying == false)
                {
                    currA.source.PlayOneShot(currA.attackSound, 0.2f);
                }
                attacked = true;

            }
            return true;
        }
        else
            return false;
      
    }
}

