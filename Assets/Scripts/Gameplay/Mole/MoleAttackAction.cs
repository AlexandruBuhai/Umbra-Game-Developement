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
        if (currMole.stamina >= (cost)) {
			
			//currWolf.animator.Play ("wolfAttack");

            int damage = currMole.strength;
            if (currMole.player.isBlocking) {
                damage -= currMole.player.defense;
			}

            currMole.player.health -= damage;
            currMole.stamina -= cost;

			attacked = true;
			return true;
		} else {
			return false;
		}
	}
}
