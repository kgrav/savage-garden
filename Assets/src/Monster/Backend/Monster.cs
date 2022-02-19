using UnityEngine;
using System;
using System.Collections.Generic;

public enum MONMOV{NONE=-1,BOREDOM=0,HUNGER=1,LONELY=2,HORNY=3}
public enum MONSTATE{NONE=0,ACTION=1}
public class Monster {

    MAction BuildAction(Monster m, MONMOV reason){
        switch(reason){
            case MONMOV.NONE:
                return null;
            break;
            case MONMOV.BOREDOM:
                return BuildBoredomAction();
            break;
            case MONMOV.HUNGER:
                return BuildHungerAction();
        }
        return null;
    }

    MAction BuildBoredomAction(){
        Vector3 wanderpt = Monsters.GetWanderPoint();
        GameObject.Instantiate(Monsters.monsters.marker, wanderpt,Quaternion.identity);
        return new ApproachAction(this, MONMOV.BOREDOM,wanderpt);
    }

    MAction BuildHungerAction(){
        List<MonsterPoint> options = new List<MonsterPoint>();
        return null;
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
        appetites = new MonsterAppetite[1];
        appetites[(int)MONMOV.BOREDOM] = new MonsterAppetite(preset.boredom, preset.affinity.sloth);
        body=GameObject.Instantiate(preset.bodyPreset, Monsters.GetWanderPoint(), Quaternion.identity).GetComponent<MonsterBody>();
        body.Setup(this);
        rtime=Monsters.rtime;
        appetitesView=new List<MonsterAppetite>();
    }

    

    List<int> orderMotives(){
            List<MonsterAppetite> e1 = new List<MonsterAppetite>();
            foreach(MonsterAppetite a in appetites){
                if(a.low){
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
        foreach(MonsterAppetite a in appetites){
            a.Update();
        }
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

    public void RestoreAppetite(MONMOV appetite, float amount){
        appetites[(int)appetite].Restore(amount);
    }

    public bool AppetiteIsHigh(MONMOV appetite){
        return appetites[(int)appetite].high;
    }

    //to-do: motive evaluation function
    void rstep(){
        if(state==MONSTATE.NONE){
            Monsters.print(index + ": entering rstep with state NONE");

            List<int> motivesView = orderMotives();
            MONMOV newActionGoal = MONMOV.BOREDOM;
            action = BuildAction(this,newActionGoal);
            Monsters.print(index + ": received action of type " + action.GetType());
        }
    }


    //access types:
    //BOREDOM - trying to socialize
    //HUNGER - trying to eat alive
    public void OnMPEngage(MONMOV accessType, Monster accessor){
        
    }
}