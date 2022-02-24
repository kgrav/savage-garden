using UnityEngine;
using System;
using System.Collections.Generic;


public enum MPSTATE {INACTIVE,FULL,AVAILABLE,BADTYPE}


public enum MPCON {ALIVE,DEAD}
public class MonsterPoint : NVComponent {

    public static List<MonsterPoint> points=null;
    public int capacity;


    protected List<int> _occupants;
    public List<int> occupants {get{return _occupants;}}

    public MPResource[] resources;
    
    public Affinity affinityConfig;

    public Affinity affinity {get{return mindex > -1 ? Monsters.GetMonster(mindex).affinity : affinityConfig;}}
    public Dictionary<MONAPP,MPResource> apps;

    public int maxCapacity;

    public int mpindex {get; private set;}

    int _mindex=-1;
    public int mindex {get{return _mindex;}}

    protected int[] lastIndx;

    protected bool _active;

    public bool alive {get; private set;}
    MonsterBody body=>GetComponent<MonsterBody>();
    MPDamageBody dbody=>GetComponent<MPDamageBody>();


    public int getHP{get{return dbody ? dbody.hp : 0; }}
    public bool startActive;
    public bool active {get{return _active;}}
    public void Activate(){
        _active=true;
    }
    public void Deactivate(){
        _active=false;
    }

    public MONSTATE state {
        get{
            return mindex > -1 ? Monsters.GetMonster(body.monsterKey).state : MONSTATE.DEAD;
        }
    }

    public void Setup(int key){
        _mindex = key;
    }

    void Awake(){
        if(points == null){
            points = new List<MonsterPoint>();
        }
        _occupants = new List<int>();
        mpindex = points.Count;
        apps = new Dictionary<MONAPP, MPResource>();
        alive = body ? true : false;
        foreach(MPResource mpr in resources){
            apps.Add(mpr.key, mpr);
        }
        
        int dind = points.Count;
        points.Add(this);
        if(startActive)
            Activate();
    }

    public void OnTakeDamage(int amount, float impulse){

    }

    protected override void NVUpdate()
    {
        ChUpdate();
    }

    protected virtual void ChUpdate(){}
    protected virtual void Init(){
    }

    

    public MPSTATE Availability(MONAPP accessType){
        if(!_active)
            return MPSTATE.INACTIVE;
        if(!apps.ContainsKey(accessType) || apps[accessType].totalAmount < 0)
            return MPSTATE.BADTYPE;
        if(occupants.Count >= maxCapacity)
            return MPSTATE.FULL;
        return MPSTATE.AVAILABLE;
    }

    public void Engage(Monster m, MONAPP goal){
        print("engage from " + m.index + "; " + goal);
        if(!occupants.Contains(m.index))
            occupants.Add(m.index);
        m.SetCallback(this);
    }



    public void Access(Monster m, MONAPP goal){
        MPTag.ResolveTags(this, m.body.monsterPoint,apps[goal].onAccess);
        m.RestoreAppetite(goal, apps[goal].accessRate*Time.deltaTime);
        if(apps[goal].finite){
            apps[goal].totalAmount -= apps[goal].accessRate*Time.deltaTime;
            if(apps[goal].totalAmount <= 0){
                MPTag.ResolveTags(this, m.body.monsterPoint,apps[goal].onDeplete);
                return;
            }
        }
        if(mindex > -1){
            Monsters.GetMonster(mindex).OnMPAccess(goal, m);
        }
    }

    public virtual void OnAccess(Monster m, MONAPP goal){
        foreach(MPTag t in apps[goal].onAccess){
            List<MonsterPoint> mps = new List<MonsterPoint>();
            mps.Add(m.body.monsterPoint);
            t.Resolve(this,mps);
        }
    }

    public virtual void OnDeplete(Monster m, MONAPP goal){
        foreach(MPTag t in apps[goal].onAccess){
            List<MonsterPoint> mps = new List<MonsterPoint>();
            mps.Add(m.body.monsterPoint);
            t.Resolve(this,mps);
        }
    }

    public void Disengage(Monster m){
        if(occupants.Contains(m.index))
            occupants.Remove(m.index);
        OnDisengage(m);
    }

    
    protected virtual void OnDisengage(Monster m){}

    [Serializable]
    public class MPResource{
        public MONAPP key;
        public float quality;
        public float accessRate;
        public bool finite;
        public float totalAmount;
        public MPTag[] onAccess;
        public MPTag[] onDeplete;
    }
}