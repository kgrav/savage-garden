using UnityEngine;
using System;


public enum MACT {APPROACH,FACE,MPACCESS}
public enum MTAG {ALIVE}
public abstract class MAction {

    public MAction next {get; set;}
    public Monster self {get; set;}
    public MONAPP reason {get; protected set;}

    public bool init {get; protected set;}
    public abstract bool EndCondition();

    protected abstract void StartAction();
    public abstract void DoAction();

    public abstract void EndAction();

    public void InitAction(){
        if(!init){
            init=true;
            StartAction();
        }
    }

    public void AddToEnd(MAction action){
        if(next==null)
            next = action;
        else
            next.AddToEnd(action);
    }

    public int tails {get{if(next == null) return 0; return next.tails+1;}}
}