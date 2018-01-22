using UnityEngine;
using System.Collections;

public class MoleAttackAction : GOAPAction {

	private bool attacked = false;

    public MoleAttackAction(){
		addEffect ("damagePlayer", true);
		cost = 100f;
	}

	public override void reset() {
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
		return target != null;
	}

	public override bool perform(GameObject agent){
        Mole currMole = agent.GetComponent<Mole> ();
        if (currMole.stamina >= (cost))
        {
            if (currMole.player.health > 0)
            {
                currMole.animator.SetTrigger("MoleAttackLeft");

                int damage = currMole.strength;
           
                if (currMole.health > 0)
                {                
                    currMole.player.clip = "attack";
                    currMole.player.anim.SetTrigger("isHit");
                    currMole.player.source.Stop();
                    currMole.player.source.PlayOneShot(currMole.player.hitSound);
                    currMole.player.health -= damage;

                }

                if (currMole.source.isPlaying == false)
                {
                    currMole.source.PlayOneShot(currMole.attackSound, 0.6f);
                }
                currMole.stamina -= cost;


                attacked = true;
               
            }
            return true;
        }
        else
        {
            return false;
        }
	}
}
