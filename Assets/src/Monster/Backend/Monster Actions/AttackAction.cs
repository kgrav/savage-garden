using UnityEngine;
using System;

public class AttackAction : MAction {
    MonsterPart attack;
    MonsterPoint target;
    float waitTime;
    public AttackAction(MonsterPoint target,MonsterPart attack, float priority, MONAPP reason, Monster self){
        this.attack=attack;
        this.target=target;
        this.self=self;
        this.reason=reason;
        this.priority=priority;
        this.waitTime = attack.attackTime;
    }
    public override void DoAction()
    {
        waitTime -= Time.deltaTime;
        
    }

    public override void EndAction()
    {
        if(reason==MONAPP.HUNGER){
            if(target.getHP>0){
                GotoMPAction gtact = new GotoMPAction(self,reason,target,priority*Monsters.monsters.PriorityDecay);
                gtact.AddToEnd(new AttackAction(target,attack,priority*Monsters.monsters.PriorityDecay,reason,self));
                next = gtact; 
                if(self.body.actionMsg)
                Monsters.print(self.index + ": didn't kill, chaining attack");
            }
            else{
                GotoMPAction gtact = new GotoMPAction(self,reason,target,priority);
                gtact.AddToEnd(new MPAccessAction(self,target,reason,priority));
                if(self.body.actionMsg)
                Monsters.print(self.index + ": killed, chaining feed");
            }
        }
    }

    public override bool EndCondition()
    {
        return waitTime <=0;
    }

    protected override void StartAction()
    {
        waitTime=attack.attackTime;
        attack.TriggerAttack();
    }
}