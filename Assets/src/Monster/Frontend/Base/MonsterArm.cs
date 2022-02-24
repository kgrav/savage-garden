using UnityEngine;
using System;

public class MonsterArm : MonsterPart {
    AttackHitbox hb=>GetComponentInChildren<AttackHitbox>();

    public override void TriggerAttack()
    {
        print("trigger attack");
        anim.SetTrigger("Attack");
    }

    void ActivateHitbox(){
        hb.SetActive(true);
    } 

    void DeactivateHitbox(){
        hb.SetActive(false);
    }
}