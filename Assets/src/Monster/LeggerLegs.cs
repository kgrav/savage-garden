using UnityEngine;
using System;

public class LeggerLegs : MonsterLegs {
    public float legforce;

    void WalkApex(){
        parent.AddLegForce(legforce);

    }

    public override void StartMove(){
        anim.SetBool("Walk",true);
    }

    public override void EndMove()
    {
        base.EndMove();
    }

    public override void StartSexDance()
    {
        base.StartSexDance();
    }
}   