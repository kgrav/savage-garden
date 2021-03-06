using UnityEngine;
using System;
using System.Collections.Generic;

public enum MONAPP{NONE=-1,BOREDOM=0,HUNGER=1,STAMINA=2,LONELY=3,HORNY=4}
public enum MONSTATE{NONE=0,ACTION=1,DEAD=2}
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
        return new ApproachAction(this, MONAPP.BOREDOM,wanderpt,100);
    }

    MAction BuildHungerAction(){
        MAction r = null;
        mpvec bestPoint = GetBestMonsterPoint(MONAPP.HUNGER);
        if(bestPoint.pt!=null){
            r = new GotoMPAction(this,MONAPP.HUNGER,bestPoint.pt,bestPoint.prefDist);
            if(bestPoint.pt.state == MONSTATE.DEAD){
                
                r.AddToEnd(new MPAccessAction(this, bestPoint.pt, MONAPP.HUNGER,bestPoint.prefDist));
            }
            else
            {
                MonsterPart attackpart = body.GetBestAttack();
                if(body.actionMsg)
                    Monsters.print(index + ": " + "attacking with" + attackpart);
                if(attackpart){
                    r.AddToEnd(new AttackAction(bestPoint.pt, attackpart,bestPoint.prefDist,MONAPP.HUNGER, this));
                }
            }
        }
        return r;
    }




    mpvec GetBestMonsterPoint(MONAPP goal){
        mpvec best = new mpvec(null,float.MaxValue);
        float qualityThreshold = (20-affinity.pride);
        foreach(MonsterPoint mp in body.sense.GetPoints()){
            if(MPSTATE.AVAILABLE == mp.Availability(goal) && mp.mindex != index){
                float pdistance = Vector3.Distance(body.tform.position, mp.tform.position);
                float quality = GetQuality(mp,goal);
                if(body.actionMsg)
                Monsters.print(index + ": " + mp.mindex + " quality: " + quality + " threshold: " + (qualityThreshold + appetites[(int)goal].criticality));
                if(quality < qualityThreshold + appetites[(int)goal].criticality){
                float qdist = mp.apps[goal].quality*Monsters.QualityScale;
                float prefDist = Vector2.Distance(Vector2.zero, new Vector2(pdistance,qdist));
                if(best.pt==null || prefDist < best.prefDist)
                    best=new mpvec(mp, prefDist);
                }
            }
        }
        return best;
    }


    public float GetQuality(MonsterPoint mp,MONAPP goal){
        float r = mp.apps[goal].quality;
        switch(goal){
            case MONAPP.HUNGER:
                r=GetHungerQuality(mp);
            break;
        }
        return r;
    }

    float GetHungerQuality(MonsterPoint mp){
        float r = mp.apps[MONAPP.HUNGER].quality;
        r+= mp.state == MONSTATE.DEAD ? 0 : (20-affinity.sloth)/20;
        r+= mp.affinity.wrath < affinity.wrath ? 0 : (20-affinity.pride)/20;
        r+= mp.getHP;
        return r;
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
        appetites = new MonsterAppetite[3];
        appetites[(int)MONAPP.BOREDOM] = new MonsterAppetite(preset.boredom, 20-preset.affinity.sloth);
        appetites[(int)MONAPP.HUNGER] = new MonsterAppetite(preset.hunger,20-preset.affinity.gluttony);
        appetites[(int)MONAPP.STAMINA] = new MonsterAppetite(preset.stamina,20);
        body=GameObject.Instantiate(preset.bodyPreset, Monsters.GetWanderPoint(), Quaternion.identity).GetComponent<MonsterBody>();
        body.Setup(this);
        rtime=0.1f;
        appetitesView=new List<MonsterAppetite>();
    }

    public void KillMonster(string reason){
        Monsters.print(index + " is kill. " + reason);
        if(action!=null){
            action.EndAction();
            action=null;
        }
        state=MONSTATE.DEAD;
        body.tform.LookAt(Vector3.up,body.tform.forward);
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
                if(a.priority + a.criticality > maxPriority){
                    highest = a;
                    maxPriority = a.priority + a.criticality;
                }
            }
            running.Add((int)highest.movKey);
            total.Remove(highest);
            return orderMotivesStep(running, total);
        }


    public void Update(){ 
        if(state!=MONSTATE.DEAD){
        rtime -= Time.deltaTime;
        if(rtime <= 0){
            rstep();
            rtime=Monsters.rtime;
        }
        string appetitesMsg = "appetites: ";
        foreach(MonsterAppetite a in appetites){
            
            a.Update();
            appetitesMsg += a.movKey + ": " + a.value + ", ";
            if(a.criticality > Monsters.monsters.criticalityDeathThreshold){
                KillMonster("died of " + a.movKey);
            }   
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
            List<int> motivesView = orderMotives();
            string chainmsg = index + ": low appetites: ";
            foreach(int i in motivesView){
                chainmsg += (MONAPP)i + ", ";
            }
            if(body.motiveOrderMsg)
            Monsters.print(chainmsg);
            MAction newAction=BuildAction(this,motivesView);
            if(newAction != null){
                if(action==null || (newAction.reason!=action.reason && newAction.priority < action.priority)){
                    chainmsg = index + ": received action of type " + newAction.GetType();
                    MAction a1 =newAction.next;
                    while(a1!= null){
                        chainmsg += "=> " + a1.GetType();
                        a1=a1.next;
                    }
                    if(action!=null){
                        chainmsg += ", canceling action of type " + action.GetType() + " (" + action.reason + ") ";
                        action.EndAction();
                    }                
                    if(body.actionMsg)
                    Monsters.print(chainmsg);
                    action=newAction;
                }
                else
                {
                    if(body.actionMsg)
                    Monsters.print(index + " new action of type " + newAction.GetType() + " rejected.  reason match: "+ (newAction.reason!=action.reason)+" priority comparison (new < current) " + (newAction.priority < action.priority));
                }
            }
    }


    //access types:
    //BOREDOM - trying to socialize
    //HUNGER - trying to eat alive
    public void OnMPAccess(MONAPP accessType, Monster accessor){
        
    }
}