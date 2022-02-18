using UnityEngine;
using System;
using System.Collections.Generic;


public enum MPRESPONSE {INACTIVE,FULL,AVAILABLE}
public class MonsterPoint : NVComponent {

    public static List<MonsterPoint> points=null;

    public int capacity;

    public Motives motives;

    protected List<int> _occupants;
    public List<int> occupants {get{return _occupants;}}

    public Dictionary<MONMOV,MPApp> apps;

    public int maxCapacity;

    int mpindex;

    protected int[] lastIndx;

    protected bool _active;

    public bool startActive;
    public bool isMonster;
    public bool active {get{return _active;}}
    public void Activate(){
        _active=true;
    }
    public void Deactivate(){
        _active=false;
    }
    void Awake(){
        if(points == null){
            points = new List<MonsterPoint>();
        }
        _occupants = new List<int>();
        mpindex = points.Count;
        points.Add(this);
    }

    protected override void NVUpdate()
    {
        ChUpdate();
    }

    protected virtual void ChUpdate(){}
    protected virtual void Init(){
    }

    

    public virtual MPRESPONSE Availability(int mouseIndex){
        if(!_active)
            return MPRESPONSE.INACTIVE;
        if(occupants.Count >= maxCapacity)
            return MPRESPONSE.FULL;
        return MPRESPONSE.AVAILABLE;
    }

    public void Engage(Monster m, MONMOV goal){
        if(!occupants.Contains(m.index))
            occupants.Add(m.index);
        m.SetCallback(this);
        OnEngage(m, goal);
    }

    protected virtual void OnEngage(Monster m, MONMOV goal){     
    }

    public virtual void Access(Monster m){}
    public void Disengage(Monster m){
        if(occupants.Contains(m.index))
            occupants.Remove(m.index);
        OnDisengage(m);
    }

    
    protected virtual void OnDisengage(Monster m){}

    [Serializable]
    public class MPApp{
        public float restoreRate;
    }
}