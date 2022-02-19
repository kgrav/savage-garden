using UnityEngine;
using System;
using System.Collections.Generic;


public enum MPSTATE {INACTIVE,FULL,AVAILABLE,BADTYPE}
public class MonsterPoint : NVComponent {

    public static List<MonsterPoint> points=null;
    public int capacity;

    public Motives motives;

    protected List<int> _occupants;
    public List<int> occupants {get{return _occupants;}}

    public MPResource[] resources;

    public Dictionary<MONMOV,MPResource> apps;

    public int maxCapacity;

    int mpindex;

    int mindex;

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
        apps = new Dictionary<MONMOV, MPResource>();
        foreach(MPResource mpr in resources){
            apps.Add(mpr.key, mpr);
        }
        int dind = points.Count;
        points.Add(this);
    }

    protected override void NVUpdate()
    {
        ChUpdate();
    }

    protected virtual void ChUpdate(){}
    protected virtual void Init(){
    }

    

    public MPSTATE Availability(MONMOV accessType){
        if(!_active)
            return MPSTATE.INACTIVE;
        if(!apps.ContainsKey(accessType) || apps[accessType].totalAmount < 0)
            return MPSTATE.BADTYPE;
        if(occupants.Count >= maxCapacity)
            return MPSTATE.FULL;
        return MPSTATE.AVAILABLE;
    }

    public void Engage(Monster m, MONMOV goal){
        if(!occupants.Contains(m.index))
            occupants.Add(m.index);
        m.SetCallback(this);
    }



    public void Access(Monster m, MONMOV goal){
        
    }
    public void Disengage(Monster m){
        if(occupants.Contains(m.index))
            occupants.Remove(m.index);
        OnDisengage(m);
    }

    
    protected virtual void OnDisengage(Monster m){}

    [Serializable]
    public class MPResource{
        public MONMOV key;
        public float restoreRate;
        public float quality;
        public float accessRate;
        public bool finite;
        public float totalAmount;
    }
}