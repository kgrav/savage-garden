using System;
using UnityEngine;

public class MultiEffect : Effect {
    bool childrenInit;

    Effect[] _children;
    Effect[] children { get{
        if(!childrenInit){
            _children = GetComponentsInChildren<Effect>();
            childrenInit=true;
        }
        return _children;
    }}
    
    public override void Trigger(Vector3 position, Vector3 direction){
        foreach(Effect e in children){
            if(e.GetHashCode()!=this.GetHashCode())
            e.Trigger(position, direction);
        }
    }
}