using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreafericitulAttack2 : GOAPAction {

    private bool attacked = false;

    public PreafericitulAttack2(){
        addEffect ("damagePlayer", true);
        cost = 150f;
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
        Preafericitul currBoss = agent.GetComponent<Preafericitul> ();
        if (currBoss.stamina >= (cost)) {
            int damage = currBoss.strength;
          
            StartCoroutine(WaitForAnimation(currBoss));
            currBoss.player.clip = "attack";
            currBoss.player.health -= damage;
            currBoss.player.source.PlayOneShot(currBoss.player.hitSound);

            if(currBoss.health != 0)
                currBoss.player.anim.SetTrigger("isHit");
            currBoss.stamina -= cost;
            if (currBoss.source.isPlaying == false)
            {
                currBoss.source.PlayOneShot(currBoss.attackSound, 0.08f);
            }

            attacked = true;

            return true;
        } else {
            return false;
        }
      
    }

    IEnumerator WaitForAnimation(Preafericitul currBoss)
    {
       
        currBoss.animator.SetBool("isAttacking", true);
        //currBoss.animator.SetTrigger("isAtt");
        currBoss.regenRate = 0;
        yield return new WaitForSeconds(1f);
        currBoss.animator.SetBool("isAttacking", false);
        currBoss.regenRate = 0.7f;
    }
}
