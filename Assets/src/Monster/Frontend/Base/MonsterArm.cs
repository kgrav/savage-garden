using UnityEngine;
using System;

public class MonsterArm : MonsterPart {
    AttackHitbox hb=>GetComponentInChildren<AttackHitbox>();

    public override void TriggerAttack()
    {
        print("trigger attack");
        anim.SetTrigger("Attack");
    }

    public override MAction BuildAttackAction(MonsterPoint target, Monster self, MONAPP reason, float priority)
    {
           MAction r = new GotoMPAction(self,reason,target,priority);
           r.AddToEnd(new AttackAction(target,this,priority,reason,self));
           return r;
    }

    void ActivateHitbox(){
        hb.SetActive(true);
    } 

    void DeactivateHitbox(){
        hb.SetActive(false);
    }
}