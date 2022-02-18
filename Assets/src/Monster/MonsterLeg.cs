using UnityEngine;
using System;

public class MonsterLeg : MonsterPart {


    public float legforce;
    public Vector3 forceOffset;
    void WalkApex(){
        parent.AddLegForce(this);
    }


    public void SetWalkCycle(float delay){
        Invoke("StartWalkCycle",delay);
    }

    public void StopWalkCycle(){
        CancelInvoke("StartWalkCycle");
        anim.SetBool("walk",false);
    }


    void StartWalkCycle(){
        anim.SetBool("walk",true);
    }

}