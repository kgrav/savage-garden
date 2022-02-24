using System;
using UnityEngine;


public class MultiFireEffect : Effect{

    public int fxKey, times;

    public override void Trigger( Vector3 pos, Vector3 dir){
        for(int i = 0; i < times; ++i){
            EffectsTable.TriggerEffect(fxKey, pos, dir);
        }
    }
}