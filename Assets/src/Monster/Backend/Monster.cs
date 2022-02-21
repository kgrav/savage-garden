using UnityEngine;
using System;
using System.Collections.Generic;

public enum MONAPP{NONE=-1,BOREDOM=0,HUNGER=1,LONELY=2,HORNY=3}
public enum MONSTATE{NONE=0,ACTION=1}
public class Monster {

    

    MAction BuildAction(Monster m, List<int> orderedAppetites){
        MAction r = null;
        for(int i = 0; i < orderedAppetites.Count; ++i){
            if(r!=null)
                break;
            switch((MONAPP)orderedAppetites[i]){
                case MONAPP.NONE:
                    r = null;
                break;
                case MONAPP.BOREDOM:
                    r = BuildBoredomAction();
                break;
                case MONAPP.HUNGER:
                    r = BuildHungerAction();
                break;
            }
        }
        return r==null ? BuildWanderAction() : r;
    }

    MAction BuildBoredomAction(){
        return BuildWanderAction();
    }

    MAction BuildWanderAction(){
        Vector3 wanderpt = Monsters.GetWanderPoint();
        return new ApproachAction(this, MONAPP.BOREDOM,wanderpt);
    }

    MAction BuildHungerAction(){
        MAction r = null;
        MonsterPoint bestPoint = GetBestMonsterPoint(MONAPP.HUNGER);
        if(bestPoint!=null){
            r = new ApproachAction(this,MONAPP.HUNGER,bestPoint.tform.position);
            r.AddToEnd(new MPAccessAction(this, bestPoint, MONAPP.HUNGER));
        }
        return r;
    }


    public MonsterPoint GetBestMonsterPoint(MONAPP goal){
        mpvec best = new mpvec(null,float.MaxValue);
        foreach(MonsterPoint mp in body.sense.GetPoints()){
            if(MPSTATE.AVAILABLE == mp.Availability(goal)){
                float pdistance = Vector3.Distance(body.tform.position, mp.tform.position);
                float qdist = mp.apps[goal].quality*Monsters.QualityScale;
                float prefDist = Vector2.Distance(Vector2.zero, new Vector2(pdistance,qdist));
                if(best.pt==null || prefDist < best.prefDist)
                    best=new mpvec(mp, prefDist);
            }
        }   
        Monsters.print(best.pt);
        return best.pt;
    }

    class mpvec{
        public MonsterPoint pt;
        public float prefDist;

        public mpvec(MonsterPoint pt, float prefDist){
            this.pt=pt;
            this.prefDist=prefDist;
        }
    }
    


    public List<AppetiteBarData> GetAppetiteData(){
        List<AppetiteBarData> r = new List<AppetiteBarData>();
        foreach(MonsterAppetite a in appetites){
            r.Add(a.GetDisplayData());
        }
        return r;
    }

    public List<AffinityData> GetAffinityData(){
        List<AffinityData> r = new List<AffinityData>();
        r.Add(new AffinityData("S",affinity.sloth/20, Color.cyan));
        r.Add(new AffinityData("G",affinity.gluttony/20,Color.green));
        r.Add(new AffinityData("W",affinity.wrath/20, Color.red));
        return r;
    }

    public MonsterPoint callback {get;private set;}

    public MonsterBody body {get;private set;}

    public MonsterAppetite[] appetites {get;private set;}
    List<MonsterAppetite> appetitesView;
    public int index {get;private set;}

    public Affinity affinity {get;private set;}

    public bool alive;

    public MONSTATE state {get;private set;}

    float rtime;
    public void SetCallback(MonsterPoint monsterPoint){
        callback=monsterPoint;
    }

    public MAction action {get; private set;}


    public Monster(MonsterPreset preset, int index){
        this.index=index;
        affinity = preset.affinity;
        appetites = new MonsterAppetite[2];
        appetites[(int)MONAPP.BOREDOM] = new MonsterAppetite(preset.boredom, 20-preset.affinity.sloth);
        appetites[(int)MONAPP.HUNGER] = new MonsterAppetite(preset.hunger,20-preset.affinity.gluttony);
        body=GameObject.Instantiate(preset.bodyPreset, Monsters.GetWanderPoint(), Quaternion.identity).GetComponent<MonsterBody>();
        body.Setup(this);
        rtime=0.1f;
        appetitesView=new List<MonsterAppetite>();
    }

    

    List<int> orderMotives(){
            List<MonsterAppetite> e1 = new List<MonsterAppetite>();
            foreach(MonsterAppetite a in appetites){
                if(a.low && !e1.Contains(a)){
                    e1.Add(a);
                }
            }
            return orderMotivesStep(new List<int>(), e1);
        }

        List<int> orderMotivesStep(List<int> running, List<MonsterAppetite> total){
            if(total.Count<=0)
                return running;
            MonsterAppetite highest = null;
            float maxPriority = float.MinValue;
            foreach(MonsterAppetite a in total){
                if(a.priority > maxPriority){
                    highest = a;
                    maxPriority = a.priority;
                }
            }
            running.Add((int)highest.movKey);
            total.Remove(highest);
            return orderMotivesStep(running, total);
        }


    public void Update(){ 
        rtime -= Time.deltaTime;
        if(rtime <= 0){
            rstep();
            rtime=Monsters.rtime;
        }
        string appetitesMsg = "appetites: ";
        foreach(MonsterAppetite a in appetites){
            
            a.Update();
            appetitesMsg += a.movKey + ": " + a.value + ", ";
            
        }
        if(body.appetitesMsg)
            Monsters.print(appetitesMsg);
        state=MONSTATE.NONE;
        if(action != null){
            state=MONSTATE.ACTION;
            if(action.EndCondition()){
                action.EndAction();
                action=action.next;
                state=action == null ? MONSTATE.NONE : MONSTATE.ACTION;
            }
            else{
                if(!action.init){
                    action.InitAction();
                }
                action.DoAction();
            }
        }
    }

    public void RestoreAppetite(MONAPP appetite, float amount){
        appetites[(int)appetite].Restore(amount);
    }

    public bool AppetiteIsHigh(MONAPP appetite){
        return appetites[(int)appetite].high;
    }

    public void SetDecreasePause(MONAPP appetite, bool pause){
        appetites[(int)appetite].pause=pause;
    }

    //to-do: motive evaluation function
    void rstep(){
        if(state==MONSTATE.NONE){
            Monsters.print(index + ": entering rstep with state NONE");

            List<int> motivesView = orderMotives();
            string chainmsg = index + ": low appetites: ";
            foreach(int i in motivesView){
                chainmsg += (MONAPP)i + ", ";
            }
            if(body.motiveOrderMsg)
            Monsters.print(chainmsg);
            action = BuildAction(this,motivesView);
            if(action!=null){
                chainmsg = index + ": received action of type " + action.GetType();
                MAction a1 =action.next;
                while(a1!= null){
                    chainmsg += "=> " + a1.GetType();
                    a1=a1.next;
                }
                if(body.actionMsg)
                Monsters.print(chainmsg);
            }
            else
            {Monsters.print("could not build action");}
        }
    }


    //access types:
    //BOREDOM - trying to socialize
    //HUNGER - trying to eat alive
    public void OnMPAccess(MONAPP accessType, Monster accessor){
        
    }
}