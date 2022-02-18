using UnityEngine;
using System;



public abstract class MAction {

    public MAction next {get; set;}
    public Monster self {get; set;}
    public MONMOV reason {get; protected set;}


    public abstract bool EndCondition();
    public abstract void DoAction();

    public abstract void EndAction();

    public void AddToEnd(MAction action){
        if(next==null)
            next = action;
        else
            next.AddToEnd(action);
    }

    public int tails {get{if(next == null) return 0; return next.tails+1;}}
}